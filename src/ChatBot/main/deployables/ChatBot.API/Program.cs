using System.Reflection;
using System.Text;
using ChatBot.API.ExceptionHandler;
using ChatBot.API.hubs;
using ChatBot.API.Models;
using ChatBot.API.Workers;
using ChatBot.Core.Boundaries.BotMessageHandlers;
using ChatBot.Core.Boundaries.Persistence;
using ChatBot.Core.Models;
using ChatBot.Core.Services;
using ChatBot.Core.Services.Contracts;
using ChatBot.Persistence;
using ChatBot.Persistence.Repositories;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using RabbitMqMessageHandler.Settings;

namespace ChatBot.API
{
    public class Program
    {
        private const string SQL_CONNECTION_STRING = "SqlServerConnection";
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>();
            var jwtSettings = builder.Configuration.GetSection("JWTSettings").Get<JwtSettings>();
            builder.Services.AddScoped((_) => jwtSettings);
            // Add services to the container.
            AddDbContext(builder.Services, builder.Configuration);
            var rabbitMqConfiguration = builder.Configuration.GetSection("RabbitMq").Get<RabbitMqConfiguration>();
            builder.Services.AddSingleton((_) => rabbitMqConfiguration);

            builder.Services.AddControllers();
            RegisterServices(builder.Services);
            RegisterRepositories(builder.Services);

            builder.Services.AddCors(AddDefaultCors(apiSettings));


            builder.Services.AddIdentity<User, IdentityRole>(o => {
                o.User.RequireUniqueEmail = false;
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 5;
                o.Lockout.DefaultLockoutTimeSpan = new System.TimeSpan(0, 5, 0);
                o.Lockout.MaxFailedAccessAttempts = 5;
            }).AddEntityFrameworkStores<ChatDbContext>().AddDefaultTokenProviders();
            ConfigureJwt(builder.Services, jwtSettings);

            builder.Services.AddAutoMapper(typeof(Program));
   
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Chat bot",
                    Version = "v1",
                    Description = "Chat bot APO",
                    Contact = new OpenApiContact
                    {
                        Name = "Luis Javier",
                        Email = "javier.luis29@gmail.com"
                    }
                });

                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, Array.Empty<string>()}
                });

                var xmlFile = $"{typeof(Program).Assembly.GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            builder.Services.AddSignalR(opt => { opt.ClientTimeoutInterval = TimeSpan.FromMinutes(60); opt.KeepAliveInterval = TimeSpan.FromMinutes(30); }).AddJsonProtocol();

            var app = builder.Build();

            app.UseCors();
            app.ConfigureExceptionHandler();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatRoomHub>("/api/chat-room/hub");
                endpoints.MapControllers();
            });


            app.Run();
        }

        private static Action<CorsOptions> AddDefaultCors(ApiSettings apiSettings)
        {

            return options =>
            {
                options.AddDefaultPolicy(cors =>
                {
                    cors
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .AllowAnyOrigin()
                        .WithOrigins(apiSettings.ClientUrls.ToArray());
                });
            };
        }

        private static void AddDbContext( IServiceCollection services, ConfigurationManager configuration)
        {
            var migrationsAssembly = typeof(ChatDbContext).GetTypeInfo().Assembly.GetName().Name;
            string connectionString = configuration.GetConnectionString(SQL_CONNECTION_STRING);

            void contextBuilder(DbContextOptionsBuilder b) =>
                b.UseSqlServer(connectionString, sql =>
                {
                    sql.MigrationsAssembly(migrationsAssembly);
                    sql.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                });

            services.AddDbContext<ChatDbContext>(contextBuilder);
            services.AddScoped<DbContext, ChatDbContext>();
        }

        private static void RegisterServices(IServiceCollection  services)
        {
            
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenHandler, JwtTokenHandler>();
            services.AddScoped<IChatRoomService, ChatRoomService>();
            services.AddSingleton<IBotSendMessageHandler, BotSendMessage>();
            services.AddHostedService<BotMessageReceiver>();
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IChatRoomRepository, ChatRoomRepository>();

        }
        private static void ConfigureJwt(IServiceCollection services, JwtSettings jwtSettings)
        {
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtSettings.ValidIssuer,
                    ValidAudience = jwtSettings.ValidAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecurityKey))
                };
            });
        }
    }
}