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
            ViewBag.fylker = _registerService.GetFylker();
            ViewBag.kommuner = _registerService.GetKommuner();
            ViewBag.selectedAreas = new string[]{};
            ViewBag.data = "";
            ViewBag.query = "";
            return View();
        }

        public ActionResult Details(string[] area, string data, string query)
        {
            ViewBag.fylker = _registerService.GetFylker();
            ViewBag.kommuner = _registerService.GetKommuner();
            ViewBag.selectedAreas = area;
            ViewBag.data = data;
            ViewBag.query = query;
            ReportResult result = _reportService.GetQueryResult(new ReportApi.ReportQuery());
            return View(result);
        }

    }
}
