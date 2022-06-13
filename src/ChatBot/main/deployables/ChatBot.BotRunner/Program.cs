using ChatBot.Core.Boundaries.BotMessageHandlers;
using RabbitMqMessageHandler;
using RabbitMqMessageHandler.Settings;

namespace ChatBot.BotRunner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            var rabbitMqConfiguration = builder.Configuration.GetSection("RabbitMq").Get<RabbitMqConfiguration>();
            builder.Services.AddSingleton((_) => rabbitMqConfiguration);

            var stockServiceConfiguration = builder.Configuration.GetSection("StockConfig").Get<StockServiceConfiguration>();
            builder.Services.AddSingleton((_) => stockServiceConfiguration);
            RegisterServices(builder.Services);
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

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<IBotSendMessageHandler, BotSendMessageHandler>();
            services.AddSingleton<IBotReceiverMessageHandler, BotReceiverMessageHandler>();
            services.AddHostedService<BotWorker>();
        }
    }
}