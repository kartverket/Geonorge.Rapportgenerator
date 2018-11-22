using Kartverket.ReportGenerator.Models;
using Kartverket.ReportGenerator.Models.Translations;
using Kartverket.ReportGenerator.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

                string role = GetSecurityClaim("role");
                if (!string.IsNullOrWhiteSpace(role) && role.Equals("nd.metadata_admin"))
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

                string role = GetSecurityClaim("role");
                if (string.IsNullOrWhiteSpace(role) && !role.Equals("nd.metadata_admin"))
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

        private string GetSecurityClaim(string type)
        {
            string result = null;
            foreach (var claim in System.Security.Claims.ClaimsPrincipal.Current.Claims)
            {
                if (claim.Type == type && !string.IsNullOrWhiteSpace(claim.Value))
                {
                    result = claim.Value;
                    break;
                }
            }

            // bad hack, must fix BAAT
            if (!string.IsNullOrWhiteSpace(result) && type.Equals("organization") && result.Equals("Statens kartverk"))
            {
                result = "Kartverket";
            }

            return result;
        }

    }
}