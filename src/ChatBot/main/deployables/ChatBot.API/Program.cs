using System.Reflection;
using ChatBot.API.ExceptionHandler;
using ChatBot.API.Models;
using ChatBot.Core.Boundaries.Persistence;
using ChatBot.Core.Models;
using ChatBot.Core.Services;
using ChatBot.Core.Services.Contracts;
using ChatBot.Persistence;
using ChatBot.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ChatBot.API
{
    public class Program
    {
        private const string SQL_CONNECTION_STRING = "SqlServerConnection";
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>();
            // Add services to the container.
            AddDbContext(builder.Services, builder.Configuration);
            RegisterServices(builder.Services);
            RegisterRepositories(builder.Services);


            builder.Services.AddCors(options =>
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
            });


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

            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            

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

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
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
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
        }
    }
}