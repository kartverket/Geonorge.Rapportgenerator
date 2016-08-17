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
            return View();
        }

        public ActionResult Details()
        {
            ViewBag.fylker = _registerService.GetFylker();
            ViewBag.kommuner = _registerService.GetKommuner();
            return View();
        }

    }
}
