using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using src.events.handlers;
using src.models;
using src.sender;

namespace src
{
    public class Startup
    {
        private IRabbitEndpoint _rabbitEndpoint;
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore().AddJsonFormatters();
            services.AddTransient<ISomethingHappenedHandler, SomethingHappenedHandler>();
            services.AddTransient<IRouter, Router>();
            services.AddSingleton<IRabbitEndpoint, RabbitEndpoint>();
            services.AddTransient<ISettingsRepository, SettingsRepository>();
            services.AddTransient<ISenderFactory, SenderFactory>();
        }
        public void Configure(IApplicationBuilder app, IApplicationLifetime applicationLifetime, ILoggerFactory loggerFactory, IRabbitEndpoint rabbitEndpoint)
        {
            _rabbitEndpoint = rabbitEndpoint;
            applicationLifetime.ApplicationStarted.Register(OnStarted);
            applicationLifetime.ApplicationStopping.Register(OnShutdown);
            app.UseMvc();

            if (Infokeeper.GetEnvironmentMode() == "dev")
            {
                loggerFactory.AddDebug();
            }
        }


        private void OnStarted()
        {
            _rabbitEndpoint.StartListening();
        }
        private void OnShutdown()
        {
            _rabbitEndpoint.StopListening();
        }
    }
}
