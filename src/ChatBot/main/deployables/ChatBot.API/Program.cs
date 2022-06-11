using System.Reflection;
using ChatBot.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ChatBot.API
{
    public class Program
    {
        private const string SQL_CONNECTION_STRING = "";
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            AddDbContext(builder.Services, builder.Configuration);
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            

            var app = builder.Build();

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
    }
}