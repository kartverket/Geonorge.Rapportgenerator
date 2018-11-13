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

    [Authorize]
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
            if (string.IsNullOrEmpty(measurement))
                measurement = Measurement.NumberOfMetadataTotal;

            if (!FromDate.HasValue)
                FromDate = DateTime.Now.AddYears(-1);

            if (!ToDate.HasValue)
                ToDate = DateTime.Now;


            var model = _statisticsService.GetReport(measurement, organization, FromDate, ToDate);

            return View(model);
        }
        //Todo set up async execution queue?
        public ActionResult Create()
        {
            _statisticsService.CreateReport();

            return RedirectToAction("Index");
        }

    }
}