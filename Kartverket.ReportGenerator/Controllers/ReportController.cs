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

namespace Kartverket.ReportGenerator.Controllers
{
    public class ReportController : Controller
    {
        
        private readonly IReportService _reportService;
        private readonly IRegisterService _registerService;


        public ReportController(IReportService reportService, IRegisterService registerService)
        {
            _reportService = reportService;
            _registerService = registerService;
        }

        
        public ActionResult Index()
        {
            ViewBag.Queries = new QueryConfig().GetQueries();
            ViewBag.fylker = _registerService.GetFylker();
            ViewBag.kommuner = _registerService.GetKommuner();
            ViewBag.selectedAreas = new string[]{};
            ViewBag.data = "";
            ViewBag.query = "";
            return View();
        }

        public ActionResult Details(string[] areas, string data, string query, string action)
        {
            ViewBag.Queries = new QueryConfig().GetQueries();
            ViewBag.fylker = _registerService.GetFylker();
            ViewBag.kommuner = _registerService.GetKommuner();
            ViewBag.selectedAreas = areas;
            ViewBag.data = data;
            ViewBag.query = query;
            ReportQuery reportQuery = new ReportQuery();
            reportQuery.Parameters = new List<ReportQueryParameter>();
            reportQuery.QueryName = query;
            foreach (string area in areas)
            {
                reportQuery.Parameters.Add(new ReportQueryParameter { Name = "area", Value = area });
            }
            reportQuery.Parameters.Add(new ReportQueryParameter { Name = "data", Value = data });
            ReportResult result = _reportService.GetQueryResult(reportQuery);

            if (action == "Excel")
            {
                var fileStream = new ExcelReportGenerator().CreateExcelSheet(reportQuery, result);

                var fileStreamResult = new FileStreamResult(fileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                fileStreamResult.FileDownloadName = "Rapport-" + query + ".xlsx";
                return fileStreamResult;
            }
            else
                return View(result);
        }

    }
}
