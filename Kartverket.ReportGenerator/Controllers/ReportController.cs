using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kartverket.ReportGenerator.Models;

namespace Kartverket.ReportGenerator.Controllers
{
    public class ReportController : Controller
    {
        private ReportDbContext db = new ReportDbContext();

        // GET: Report
        public ActionResult Index()
        {
            var reportQueries = db.ReportQueries.Include(r => r.MetadataEntry);
            return View(reportQueries.ToList());
        }

        public ActionResult Details()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
