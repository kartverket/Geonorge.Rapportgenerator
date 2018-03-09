using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kartverket.ReportGenerator.Models;
using Kartverket.ReportGenerator.Services;
using Kartverket.ReportApi;
using Kartverket.ReportGenerator.Helpers;

namespace Kartverket.ReportGenerator.Controllers
{
    public class ReportController : Controller
    {
        
        private readonly IReportService _reportService;
        private readonly IRegisterService _registerService;

        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ReportController(IReportService reportService, IRegisterService registerService)
        {
            _reportService = reportService;
            _registerService = registerService;
        }

        
        public ActionResult Index()
        {
            try
            { 
                QueryConfig queries = _reportService.GetQueries();
                ViewBag.Queries = queries.GetQueries();
                ViewBag.fylker = _registerService.GetFylker();
                ViewBag.kommuner = _registerService.GetKommuner();
                ViewBag.selectedAreas = new string[]{};
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return View("Error");
            }

            return View();
        }

        [Route("setculture/{culture}")]
        public ActionResult SetCulture(string culture, string returnUrl)
        {
            // Validate input
            culture = CultureHelper.GetImplementedCulture(culture);
            // Save culture in a cookie
            HttpCookie cookie = Request.Cookies["_culture"];
            if (cookie != null)
                cookie.Value = culture;   // update cookie value
            else
            {
                cookie = new HttpCookie("_culture");
                cookie.Value = culture;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);

            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Index");
        }

        public ActionResult Details(string[] areas, string data, string query, string action)
        {
            try
            {
                QueryConfig queries = _reportService.GetQueries();
                ViewBag.Queries = queries.GetQueries();
                var fylker = _registerService.GetFylker();
                var kommuner = _registerService.GetKommuner();
                ViewBag.fylker = fylker;
                ViewBag.kommuner = kommuner;
                Dictionary<string, string> codeList = new Dictionary<string, string>();
                codeList = fylker.Union(kommuner).ToDictionary(k => k.Key, v => v.Value);
                ViewBag.selectedAreas = areas;
                ViewBag.data = data;
                var queryConfig = queries.GetQuery(query, data);
                ViewBag.query = queryConfig;
                ReportQuery reportQuery = new ReportQuery();
                reportQuery.Parameters = new List<ReportQueryParameter>();
                reportQuery.QueryName = queryConfig.Value;
                foreach (string area in areas)
                {
                    reportQuery.Parameters.Add(new ReportQueryParameter { Name = "area", Value = area });
                }
                reportQuery.Parameters.Add(new ReportQueryParameter { Name = "data", Value = data });
                ReportResult result = _reportService.GetQueryResult(reportQuery);

                if (action == "Excel")
                {
                    var fileStream = new ExcelReportGenerator().CreateExcelSheet(reportQuery, result, codeList);

                    var fileStreamResult = new FileStreamResult(fileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                    fileStreamResult.FileDownloadName = "Rapport-" + query + ".xlsx";
                    return fileStreamResult;
                }
                else
                    return View(result);
            }

            catch (Exception ex)
            {
                Log.Error(ex);
                return View("Error");
            }
        }

    }
}
