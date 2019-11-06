using Autofac;
using Autofac.Extensions.DependencyInjection;
using EventBus;
using EventBus.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ordering.API.BackgroundTasks.Configuration;
using Ordering.API.BackgroundTasks.Tasks;
using Serilog;
using ServiceBus;
using System;

namespace Ordering.API.BackgroundTasks
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddCustomMvc()
                     .AddSubscriptionsManager()
                     .AddCustomSettings(Configuration)
                     .AddBackgroundTasks()
                     .AddCustomIntegrations(Configuration);

            RegisterEventBus(services);

            //create autofac based service provider
            var container = new ContainerBuilder();

            container.Populate(services);

            var builder = container.Build();

            return new AutofacServiceProvider(builder);
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog();
        }

        private void RegisterEventBus(IServiceCollection services)
        {
            var topicName = Configuration.GetSection("serviceBus").GetSection("TopicName").Value;
            var subscriptionClientName = Configuration.GetSection("serviceBus").GetSection("SubscriptionClientName").Value;

            var iLifetimeScope = new ContainerBuilder().Build();

            services.AddSingleton<IEventBus, EventBusServiceBus>(sp =>
            {
                var serviceBusPersisterConnection = sp.GetRequiredService<IServiceBusPersisterConnection>();
                var logger = sp.GetRequiredService<ILogger<EventBusServiceBus>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                return new EventBusServiceBus(serviceBusPersisterConnection, topicName, logger,
                    eventBusSubcriptionsManager, subscriptionClientName, iLifetimeScope);
            });
        }
    }

    static class CustomExtensionsMethods
    {
        public static IServiceCollection AddCustomMvc(this IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            return services;
        }

        public static IServiceCollection AddCustomSettings(this IServiceCollection services, IConfiguration configuration)
        {
            //configure settings
            services.Configure<BackgroundTaskSettings>(configuration);

            return services;
        }

        public static IServiceCollection AddBackgroundTasks(this IServiceCollection services)
        {
            //configure background task
            services.AddSingleton<IHostedService, OrderPaymentService>();

            return services;
        }

        public static IServiceCollection AddSubscriptionsManager(this IServiceCollection services)
        {
            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            return services;
        }

        public static IServiceCollection AddCustomIntegrations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IServiceBusPersisterConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultServiceBusPersisterConnection>>();

                var topicName = configuration.GetSection("serviceBus").GetSection("TopicName").Value;
                var serviceBusConnectionString = configuration.GetSection("serviceBus").GetSection("EventBusConnection").Value;

                return new DefaultServiceBusPersisterConnection(serviceBusConnectionString, logger, topicName);
            });


            return services;
        }
    }
}
