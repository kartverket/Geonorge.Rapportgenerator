using Kartverket.ReportGenerator.Models;
using Kartverket.ReportGenerator.Models.Translations;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kartverket.ReportApi;

namespace Kartverket.ReportGenerator.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly ReportDbContext _dbContext;

        public StatisticsService(ReportDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void CreateReport()
        {
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
                        _dbContext.StatisticalData.Add(new Models.Statistics { Date = date, Organization = organization, Measurement = measurement, Count = count });
                    }
                }
                else if (measurement == NumberOfProductSpesifications)
                {
                    List<string> organizations = GetTotalProductspesificationsOrganizations();
                    foreach (var organization in organizations)
                    {
                        count = GetTotalProductspesifications(organization);
                        _dbContext.StatisticalData.Add(new Models.Statistics { Date = date, Organization = organization, Measurement = measurement, Count = count });
                    }
                }

                _dbContext.SaveChanges();

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
            var url = "http://kartkatalog.dev.geonorge.no/api/search?facets%5b0%5dname=organization&facets%5b0%5dvalue=" + HttpContext.Current.Server.UrlEncode(organization);
            System.Net.WebClient c = new System.Net.WebClient();
            c.Encoding = System.Text.Encoding.UTF8;
            c.Headers.Remove("Accept-Language");
            c.Headers.Add("Accept-Language", Culture.NorwegianCode);
            var data = c.DownloadString(url);
            var response = Newtonsoft.Json.Linq.JObject.Parse(data);

            return (int)response["NumFound"];
        }

        public StatisticsReport GetReport(string measurement = "Antall metadata totalt", string organization = "Kartverket")
        {
            StatisticsReport statisticsReport = new StatisticsReport();

            var list = _dbContext.StatisticalData
                .Where(c => c.Measurement == measurement && c.Organization == organization)
                .GroupBy(x => x.Date)
                .Select(g => new {
                    Date = g.Key,
                    Count = g.Sum(x => x.Count)
                }).OrderBy(o => o.Date).ToList();

            ReportResult reportResult = new ReportResult();
            var data = new List<ReportApi.ReportResultData>();

            ReportApi.ReportResultData resultData = new ReportApi.ReportResultData();

            if (list.Count > 0)
            {

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

            statisticsReport.ReportResult = reportResult;

            statisticsReport.MinimumCount = list.Min(m => m.Count);

            return statisticsReport;
        }
    }

    public interface IStatisticsService
    {
        void CreateReport();
        StatisticsReport GetReport(string measurement = "Antall metadata totalt", string organization = "Kartverket");
    }
}