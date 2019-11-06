using Autofac;
using EventBus;
using EventBus.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using ServiceBus;
using System;
using TaskTest.Task;

namespace TaskTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Add any Autofac modules or registrations.
            // This is called AFTER ConfigureServices so things you
            // register here OVERRIDE things registered in ConfigureServices.
            //
            // You must have the call to AddAutofac in the Program.Main
            // method or this won't be called.
            builder.RegisterInstance(Log.Logger).As<Serilog.ILogger>();

            builder.RegisterType<DefaultServiceBusPersisterConnection>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<EventBusServiceBus>().AsImplementedInterfaces().SingleInstance().WithParameter("topicName", "topicName");

            builder.RegisterType<TimedHostedService>().AsImplementedInterfaces().SingleInstance();


            //builder.RegisterType<EventBusServiceBus>().SingleInstance();

            //builder.RegisterType<InMemoryEventBusSubscriptionsManager>().As<IEventBusSubscriptionsManager>().AsSelf();
            //builder.RegisterType<DefaultServiceBusPersisterConnection>().As<IServiceBusPersisterConnection>().AsSelf();

            ////builder.Register((Func<IComponentContext, IServiceBusPersisterConnection>)(cc => cc.Resolve<DefaultServiceBusPersisterConnection>())).As<IServiceBusPersisterConnection>().InstancePerLifetimeScope();
            ////builder.Register((Func<IComponentContext, IEventBus>)(cc => cc.Resolve<EventBusServiceBus>())).As<IEventBus>().InstancePerLifetimeScope();

            //var container = builder.Build();

            //var topicName = Configuration.GetSection("serviceBus").GetSection("TopicName").Value;
            //var subscriptionClientName = Configuration.GetSection("serviceBus").GetSection("SubscriptionClientName").Value;
            //var serviceBusConnectionString = Configuration.GetSection("serviceBus").GetSection("EventBusConnection").Value;

            //var eventBusSubcriptionsManager = container.Resolve<InMemoryEventBusSubscriptionsManager>();

            //var serviceBusPersisterConnection = container.Resolve<DefaultServiceBusPersisterConnection>(
            //    new NamedParameter("serviceBusConnectionString", serviceBusConnectionString),
            //    new NamedParameter("topicName", topicName));

            //container.Resolve<EventBusServiceBus>(
            //    new NamedParameter("serviceBusPersisterConnection", serviceBusPersisterConnection),
            //    new NamedParameter("topicName", topicName),
            //    new NamedParameter("subsManager", eventBusSubcriptionsManager),
            //    new NamedParameter("subscriptionClientName", subscriptionClientName));


            //var topicName = Configuration.GetSection("serviceBus").GetSection("TopicName").Value;
            //var serviceBusConnectionString = Configuration.GetSection("serviceBus").GetSection("EventBusConnection").Value;

            //builder.Resolve<DefaultServiceBusPersisterConnection>(
            //    new NamedParameter("serviceBusConnectionString", serviceBusConnectionString),
            //    new NamedParameter("topicName", topicName));

            //var instance = new ConnectionFactory()
            //{
            //    AutomaticRecoveryEnabled = true,
            //    EndpointResolverFactory = (Func<IEnumerable<AmqpTcpEndpoint>, IEndpointResolver>)(endpoints => (IEndpointResolver)new DefaultEndpointResolver(new[] { new AmqpTcpEndpoint("localhost", -1) })),
            //    VirtualHost = "/",
            //    UserName = "guest",
            //    Password = "guest",
            //    RequestedHeartbeat = 60
            //};
            //builder.RegisterInstance<ConnectionFactory>(instance).As<IConnectionFactory>().SingleInstance();
            //builder.Register<IConnection>((Func<IComponentContext, IConnection>)(cc => cc.Resolve<IConnectionFactory>().CreateConnection())).As<IConnection>().SingleInstance();
            //builder.Register<IModel>((Func<IComponentContext, IModel>)(cc => cc.Resolve<IConnection>().CreateModel())).As<IModel>().InstancePerLifetimeScope();
        }
    }
}
