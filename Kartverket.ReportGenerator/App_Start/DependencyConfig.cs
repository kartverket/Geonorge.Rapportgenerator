using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Kartverket.ReportGenerator.Models;
using Kartverket.ReportGenerator.Services;

namespace Kartverket.ReportGenerator
{
    public static class DependencyConfig
    {
        public static void Configure(ContainerBuilder builder)
        {
            // in app
            builder.RegisterType<RegisterService>().As<IRegisterService>();
            builder.RegisterType<ReportService>().As<IReportService>();
            builder.RegisterType<StatisticsService>().As<IStatisticsService>();

            builder.RegisterControllers(typeof(WebApiApplication).Assembly).PropertiesAutowired();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired();
            builder.RegisterModule(new AutofacWebTypesModule());

            builder.RegisterType<ReportDbContext>().InstancePerRequest().AsSelf();

            var container = builder.Build();

            // dependency resolver for MVC
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // dependency resolver for Web API
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

        }
    }
}