using Autofac;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Kartverket.ReportGenerator.Models.Translations;
using System.Collections.Specialized;

namespace Kartverket.ReportGenerator
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DependencyConfig.Configure(new ContainerBuilder());
            log4net.Config.XmlConfigurator.Configure();
        }

        protected void Application_BeginRequest()
        {
            ValidateReturnUrl(Context.Request.QueryString);

            var cookie = Context.Request.Cookies["_culture"];
            if (cookie == null)
            {
                cookie = new HttpCookie("_culture", Culture.NorwegianCode);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }

            if (!string.IsNullOrEmpty(cookie.Value))
            {
                var culture = new CultureInfo(cookie.Value);
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
            }
        }

        protected void Application_EndRequest()
        {
            try
            {
                var redirectUri = HttpContext.Current.Request.Url.AbsoluteUri;

                var loggedInCookie = Context.Request.Cookies["_loggedIn"];
                if (string.IsNullOrEmpty(Request.QueryString["autologin"]) && loggedInCookie != null && loggedInCookie.Value == "true" && !Request.IsAuthenticated)
                {
                    //if (Request.Path != "/Report/SignOut" && Request.Path != "/signout-callback-oidc" && Request.QueryString["logout"] != "true" && Request.Path != "/shared-partials-scripts" && Request.Path != "/shared-partials-styles" && !Request.Path.Contains("Content") && Request.Path != "/Content/local-styles" && Request.Path != "/Content/bower_components/kartverket-felleskomponenter/assets/js/scripts" && Request.Path != "/Scripts/local-scripts" && !Request.Path.Contains("node_modules") && !Request.Path.Contains("dist"))
                    if (Request.Path != "/Report/SignOut" && Request.Path != "/signout-callback-oidc" && Request.QueryString["logout"] != "true" && !Request.Path.Contains("MainNavigation.js")
                        && !Request.Path.Contains("GeoNorgeFooter.js") && !Request.Path.Contains("main.min.css")
                        && !Request.Path.Contains("vendorfonts.min.css") && !Request.Path.Contains("vendor.css") 
                        && !Request.Path.Contains("node-modules/scripts?v=")
                        && !Request.Path.Contains("Content/bower_components/kartverket-felleskomponenter/assets/css/styles?v=")
                        && !Request.Path.Contains("Content/site.css")
                         && !Request.Path.Contains("Content/shared-partials-styles?v=")
                        && Request.Path != "/node_modules/jquery/dist/jquery.js" && Request.Path != "/dist/main.css" && Request.Path != "/Content/site.css" && Request.Path != "/Content/bower_components/kartverket-felleskomponenter/assets/css/vendor.min.css")
                        Response.Redirect("/Report/SignIn?autologin=true&ReturnUrl=" + redirectUri);
                }
            }

            catch (Exception ex)
            {
            }
        }

        void ValidateReturnUrl(NameValueCollection queryString)
        {
            if (queryString != null)
            {
                var returnUrl = queryString.Get("returnUrl");
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    returnUrl = returnUrl.Replace("http://", "");
                    returnUrl = returnUrl.Replace("https://", "");

                    var host = Request.Url.Host;
                    if (returnUrl.StartsWith("localhost:44352"))
                        host = "localhost";

                    if (!returnUrl.StartsWith(host))
                        HttpContext.Current.Response.StatusCode = 400;
                }
            }
        }
    }
}

