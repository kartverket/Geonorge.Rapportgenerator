using Kartverket.ReportGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kartverket.ReportGenerator.Controllers
{
    public class WfsController : Controller
    {
        // GET: Wfs
        public ActionResult Index()
        {
            ViewBag.StoredQueries = new Wfs().GetStoredQueries();
            return View();
        }
    }
}