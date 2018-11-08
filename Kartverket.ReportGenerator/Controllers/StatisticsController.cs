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
    [Authorize]
    public class StatisticsController : Controller
    {
        // GET: Statistics
        public ActionResult Index()
        {
            var model = GetResult();

            Query query = new Query();
            query.Value = "Antall metadata totalt";
            ViewBag.query = query;

            return View(model);
        }

        public ActionResult Create()
        {
            CreateResult();

            return RedirectToAction("Index");
        }

        private void CreateResult()
        {
            DateTime date = DateTime.Now;

            ReportDbContext _db = new ReportDbContext();
            List<string> contents = new List<string>();
            contents.Add("antall metadata totalt");
            contents.Add("antall produktspesifikasjoner i produktspesifikasjonsregisteret");

            List<string> organizations = new List<string>();
            organizations.Add("Kartverket");
            organizations.Add("Norges geologiske undersøkelse");

            foreach (var content in contents)
            {
                foreach(var organization in organizations)
                {
                  int count = 0;
                    if (content == "antall metadata totalt")
                    {
                        count = GetTotalMetadata(organization);
                    }
                    else if (content == "antall produktspesifikasjoner i produktspesifikasjonsregisteret")
                    {
                        count = GetTotalProductspesifications(organization);
                    }
                    _db.StatisticalData.Add(new Models.statistics.Data { Date = date ,  Content = content, Organization = organization, Count = count });
                    _db.SaveChanges();
                }
            }
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

        private Kartverket.ReportApi.ReportResult GetResult(string content = "antall metadata totalt", string organization = "")
        {
            ReportDbContext _db = new ReportDbContext();
            var list = _db.StatisticalData.Where(c => c.Content == content).ToList();
            //Todo group by date, sum (Count)

            Kartverket.ReportApi.ReportResult reportResult = new Kartverket.ReportApi.ReportResult();
            var data = new List<ReportApi.ReportResultData>();
            int totalCount = 0;

            ReportApi.ReportResultData resultData = new ReportApi.ReportResultData();

            if (list.Count > 0) {

                resultData = new ReportApi.ReportResultData
                {
                    Label = list.First().Content,
                    TotalDataCount = list.First().Count,
                    Values = new List<ReportApi.ReportResultDataValue>()
                };

            }

            foreach (var item in list)
            {
               totalCount = totalCount + item.Count;
                resultData.Values.Add(new ReportApi.ReportResultDataValue { Key = item.Date.ToString(), Value = item.Count.ToString() });


            }

            resultData.TotalDataCount = totalCount;

            data.Add(resultData);
            reportResult.Data = data;
            reportResult.TotalDataCount = totalCount;

            return reportResult;
        }
    }
}