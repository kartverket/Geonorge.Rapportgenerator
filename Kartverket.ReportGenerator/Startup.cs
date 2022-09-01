using System;
using System.Threading.Tasks;
using Autofac;
using Geonorge.AuthLib.NetFull;
using Microsoft.Owin;
using Microsoft.Owin.Host.SystemWeb;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartup(typeof(Kartverket.ReportGenerator.Startup))]

namespace Kartverket.ReportGenerator
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Use Autofac as an Owin middleware
            var container = DependencyConfig.Configure(new ContainerBuilder());
            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();  // requires Autofac.Mvc5.Owin nuget package installed

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                CookieManager = new SystemWebCookieManager()
            });

            app.UseGeonorgeAuthentication();
        }
    }
}
