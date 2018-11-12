using Kartverket.ReportGenerator.Models;
using Kartverket.ReportGenerator.Models.Translations;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kartverket.ReportGenerator.Controllers
{

    //Todo refactor, use DI, set up async execution queue?

    [Authorize]
    public class StatisticsController : Controller
    {
        // GET: Statistics
        public ActionResult Index()
        {
            var model = GetResult();

            return View(model);
        }

        public ActionResult Create()
        {
            CreateResult();

            return RedirectToAction("Index");
        }

        private void CreateResult()
        {
            ReportDbContext _db = new ReportDbContext();

            DateTime date = DateTime.Now;

            List<string> measurements = new List<string>();
            const string NumberOfMetadataTotal = "Antall metadata totalt"; 
            measurements.Add(NumberOfMetadataTotal);
            const string NumberOfProductSpesifications = "Antall produktspesifikasjoner i produktspesifikasjonsregisteret";
            measurements.Add(NumberOfProductSpesifications);

            foreach (var measurement in measurements)
            {
                int count = 0;

                if (measurement == NumberOfMetadataTotal)
                {
                    List<string> organizations = GetTotalMetadataOrganizations();

                    foreach (var organization in organizations)
                    {
                        count = GetTotalMetadata(organization);
                        _db.StatisticalData.Add(new Models.Statistics { Date = date, Organization = organization, Measurement = measurement, Count = count });
                    }
                }
                else if (measurement == NumberOfProductSpesifications)
                {
                List<string> organizations = GetTotalProductspesificationsOrganizations();
                foreach (var organization in organizations)
                    {
                        count = GetTotalProductspesifications(organization);
                        _db.StatisticalData.Add(new Models.Statistics { Date = date, Organization = organization, Measurement = measurement, Count = count });
                    }
                }

                _db.SaveChanges();

            }
        }

        private List<string> GetTotalMetadataOrganizations()
        {
            //Todo implement dynamic
            List<string> organizations = new List<string>();
            organizations.Add("Kartverket");
            organizations.Add("Norges geologiske undersøkelse");

            return organizations;
        }

        private List<string> GetTotalProductspesificationsOrganizations()
        {
            //Todo implement dynamic
            List<string> organizations = new List<string>();
            organizations.Add("Kartverket");
            organizations.Add("Norges geologiske undersøkelse");

            return organizations;
        }

        private int GetTotalProductspesifications(string organization)
        {
            int counter = 0;
            var url = "http://register.dev.geonorge.no/api/produktspesifikasjoner.json";
            System.Net.WebClient c = new System.Net.WebClient();
            c.Encoding = System.Text.Encoding.UTF8;
            c.Headers.Remove("Accept-Language");
            c.Headers.Add("Accept-Language", Culture.NorwegianCode);
            var data = c.DownloadString(url);
            var response = Newtonsoft.Json.Linq.JObject.Parse(data);

            var result = response["containeditems"];
            foreach (var item in result)
            {
                JToken ownerToken = item["owner"];
                string ownerValue = ownerToken?.ToString();

                if (!string.IsNullOrWhiteSpace(ownerValue) && ownerValue == organization)
                    counter++;
            }

            return counter;
        }

        private int GetTotalMetadata(string organization)
        {
            var url = "http://kartkatalog.dev.geonorge.no/api/search?facets%5b0%5dname=organization&facets%5b0%5dvalue=" + Server.UrlEncode(organization);
            System.Net.WebClient c = new System.Net.WebClient();
            c.Encoding = System.Text.Encoding.UTF8;
            c.Headers.Remove("Accept-Language");
            c.Headers.Add("Accept-Language", Culture.NorwegianCode);
            var data = c.DownloadString(url);
            var response = Newtonsoft.Json.Linq.JObject.Parse(data);

            return (int) response["NumFound"];
        }

        private Kartverket.ReportApi.ReportResult GetResult(string measurement = "Antall metadata totalt", string organization = "Kartverket")
        {
            ReportDbContext _db = new ReportDbContext();
            var list = _db.StatisticalData
                .Where(c => c.Measurement == measurement && c.Organization == organization)
                .GroupBy(x => x.Date)
                .Select(g => new {
                    Date = g.Key,
                    Count = g.Sum(x => x.Count)
                }).OrderBy(o => o.Date).ToList();

            Kartverket.ReportApi.ReportResult reportResult = new Kartverket.ReportApi.ReportResult();
            var data = new List<ReportApi.ReportResultData>();

            ReportApi.ReportResultData resultData = new ReportApi.ReportResultData();

            if (list.Count > 0) {

                resultData = new ReportApi.ReportResultData
                {
                    Label = measurement,
                    TotalDataCount = 0,
                    Values = new List<ReportApi.ReportResultDataValue>()
                };

            }

            foreach (var item in list)
            {
                resultData.Values.Add(new ReportApi.ReportResultDataValue { Key = item.Date.ToString(), Value = item.Count.ToString() });

            }

            data.Add(resultData);
            reportResult.Data = data;
            reportResult.TotalDataCount = list.Max(m => m.Count);

            ViewBag.MinCount = list.Min(m => m.Count);

            return reportResult;
        }
    }
}