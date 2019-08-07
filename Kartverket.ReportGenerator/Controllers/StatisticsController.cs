using Geonorge.AuthLib.Common;
using Kartverket.ReportGenerator.Models;
using Kartverket.ReportGenerator.Models.Translations;
using Kartverket.ReportGenerator.Services;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Kartverket.ReportGenerator.Controllers
{

    public class StatisticsController : Controller
    {
        private readonly IStatisticsService _statisticsService;

        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        // GET: Statistics
        public ActionResult Index(string measurement, string organization, DateTime? FromDate, DateTime? ToDate)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (IsAdmin())
                {
                    ViewBag.Role = "Admin";
                }
            }

            StatisticsReport model = null;

            try
            {
            if (string.IsNullOrEmpty(measurement))
            measurement = Measurement.NumberOfMetadataTotal;

            if (!FromDate.HasValue)
                FromDate = DateTime.Now.AddYears(-1);

            if (!ToDate.HasValue)
                ToDate = DateTime.Now;

            model = _statisticsService.GetReport(measurement, organization, FromDate, ToDate);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return View("Error");
            }

            return View(model);
        }
        [Authorize]
        public ActionResult Create()
        {
            if (User.Identity.IsAuthenticated)
            {

                if (!IsAdmin())
                {
                    return new HttpUnauthorizedResult();
                }
            }
            else
                return new HttpUnauthorizedResult();

            try { 
            _statisticsService.CreateReport();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return View("Error");
            }

            return RedirectToAction("Index");
        }

        private bool IsAdmin()
        {
            return ClaimsPrincipal.Current.IsInRole(GeonorgeRoles.MetadataAdmin);
        }

        public void SignIn()
        {
            var redirectUrl = Url.Action(nameof(ReportController.Index), "Report");
            HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = redirectUrl },
                OpenIdConnectAuthenticationDefaults.AuthenticationType);
        }

        public void SignOut()
        {
            var redirectUri = WebConfigurationManager.AppSettings["GeoID:PostLogoutRedirectUri"];
            HttpContext.GetOwinContext().Authentication.SignOut(
                new AuthenticationProperties { RedirectUri = redirectUri },
                OpenIdConnectAuthenticationDefaults.AuthenticationType,
                CookieAuthenticationDefaults.AuthenticationType);
        }

        /// <summary>
        /// This is the action responding to /signout-callback-oidc route after logout at the identity provider
        /// </summary>
        /// <returns></returns>
        public ActionResult SignOutCallback()
        {
            return RedirectToAction(nameof(ReportController.Index), "Report");
        }

    }
}