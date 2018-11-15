using Kartverket.ReportGenerator.Models;
using Kartverket.ReportGenerator.Models.Translations;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kartverket.ReportApi;
using System.Web.Configuration;

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

            List<string> measurements = GetMeasurements(); 

            foreach (var measurement in measurements)
            {
                if (measurement == Measurement.NumberOfMetadataTotal)
                {
                    var result = GetMetadataResult("");

                    foreach (var data in result)
                    {
                        _dbContext.StatisticalData.Add(new Models.Statistics { Date = date, Organization = data.Key, Measurement = measurement, Count = data.Value });
                    }
                }
                else if (measurement == Measurement.NumberOfMetadataForDatasetTotal)
                {
                    var result = GetMetadataResult("dataset");

                    foreach (var data in result)
                    {
                        _dbContext.StatisticalData.Add(new Models.Statistics { Date = date, Organization = data.Key, Measurement = measurement, Count = data.Value });
                    }
                }
                else if (measurement == Measurement.NumberOfMetadataForServicesTotal)
                {
                    var result = GetMetadataResult("service");

                    foreach (var data in result)
                    {
                        _dbContext.StatisticalData.Add(new Models.Statistics { Date = date, Organization = data.Key, Measurement = measurement, Count = data.Value });
                    }
                }
                else if (measurement == Measurement.NumberOfMetadataForApplicationsTotal)
                {
                    var result = GetMetadataResult("software");

                    foreach (var data in result)
                    {
                        _dbContext.StatisticalData.Add(new Models.Statistics { Date = date, Organization = data.Key, Measurement = measurement, Count = data.Value });
                    }
                }
                else if (measurement == Measurement.NumberOfMetadataForServiceLayerTotal)
                {
                    var result = GetMetadataResult("servicelayer");

                    foreach (var data in result)
                    {
                        _dbContext.StatisticalData.Add(new Models.Statistics { Date = date, Organization = data.Key, Measurement = measurement, Count = data.Value });
                    }
                }
                else if (measurement == Measurement.NumberOfProductSpesifications)
                {

                    var result = GetRegisterResult("8e726684-f216-4497-91be-6ab2496a84d3");

                    foreach (var data in result)
                    {
                        _dbContext.StatisticalData.Add(new Models.Statistics { Date = date, Organization = data.Key, Measurement = measurement, Count = data.Value });
                    }
                }
                else if (measurement == Measurement.NumberOfCartographyDocuments)
                {

                    var result = GetRegisterResult("5eacb130-d61f-469d-8454-e96943491ba0");

                    foreach (var data in result)
                    {
                        _dbContext.StatisticalData.Add(new Models.Statistics { Date = date, Organization = data.Key, Measurement = measurement, Count = data.Value });
                    }
                }
                else if (measurement == Measurement.NumberOfProductsheets)
                {

                    var result = GetRegisterResult("a42bc2b3-2314-4b7e-8007-71d9b10f2c04");

                    foreach (var data in result)
                    {
                        _dbContext.StatisticalData.Add(new Models.Statistics { Date = date, Organization = data.Key, Measurement = measurement, Count = data.Value });
                    }
                }
                else if (measurement == Measurement.NumberOfCartographyFiles)
                {

                    var result = GetRegisterCartographyResult();

                    foreach (var data in result)
                    {
                        _dbContext.StatisticalData.Add(new Models.Statistics { Date = date, Organization = data.Key, Measurement = measurement, Count = data.Value });
                    }
                }

                else if (measurement == Measurement.NumberOfSymbols)
                {

                    var result = GetRegisterSymbolResult();

                    foreach (var data in result)
                    {
                        _dbContext.StatisticalData.Add(new Models.Statistics { Date = date, Organization = data.Key, Measurement = measurement, Count = data.Value });
                    }
                }
                else if (measurement == Measurement.NumberOfSymbolPackages)
                {

                    var result = GetRegisterSymbolPackagesResult();

                    foreach (var data in result)
                    {
                        _dbContext.StatisticalData.Add(new Models.Statistics { Date = date, Organization = data.Key, Measurement = measurement, Count = data.Value });
                    }
                }

                else if (measurement == Measurement.NumberOfCodelists)
                {
                    var result = GetCodelists();

                    foreach (var data in result)
                    {
                        _dbContext.StatisticalData.Add(new Models.Statistics { Date = date, Organization = data.Organization, Measurement = data.Measurement, Count = data.Count });
                    }
                }

                _dbContext.SaveChanges();

            }
        }

        private List<Statistics> GetCodelists()
        {
            List<Statistics> codelists = new List<Statistics>();

            var organizations = GetRegisterResult("fcb0685d-24eb-4156-9ac8-25fa30759094");

            foreach (var data in organizations)
            {
                codelists.Add(new Statistics { Organization = data.Key, Measurement = Measurement.NumberOfOrganizations, Count = data.Value });
            }

            var epsgs = GetRegisterResult("37b9dc41-d868-4cbc-84f9-39557041fb2c");

            foreach (var data in epsgs)
            {
                codelists.Add(new Statistics { Organization = data.Key, Measurement = Measurement.NumberOfEpsgCodes, Count = data.Value });
            }

            organizationCodeList = new OrganizationCodeList();
            SetCodeListRegisterWithSubregister("28f22b09-098f-48e2-bc37-27fc63674318");

            foreach (var data in organizationCodeList.Organizations)
            {
                codelists.Add(new Statistics { Organization = data.Key, Measurement = Measurement.NumberOfCodelists, Count = data.Value.CountCodeList });
            }

            foreach (var data in organizationCodeList.Organizations)
            {
                codelists.Add(new Statistics { Organization = data.Key, Measurement = Measurement.NumberOfCodelistValues, Count = data.Value.CountCodeListValues });
            }

            return codelists;
        }


        OrganizationCodeList organizationCodeList;

        private void SetCodeListRegisterWithSubregister(string systemId)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            var url = WebConfigurationManager.AppSettings["RegistryUrl"] + "api/ApiRoot?systemid=" + systemId;

            System.Net.WebClient c = new System.Net.WebClient();
            c.Encoding = System.Text.Encoding.UTF8;
            c.Headers.Remove("Accept-Language");
            c.Headers.Add("Accept-Language", Culture.NorwegianCode);
            var data = c.DownloadString(url);
            var response = Newtonsoft.Json.Linq.JObject.Parse(data);

            JToken ownerToken = response["owner"];
            string owner = ownerToken?.ToString();

            if (!organizationCodeList.Organizations.ContainsKey(owner))
                organizationCodeList.Organizations.Add(owner, new CodeList { CountCodeList = 0, CountCodeListValues = 0 });

            var containedItemClassToken = response["containedItemClass"];
            string containedItemClass = containedItemClassToken?.ToString();

            var items = response["containeditems"];

            if(containedItemClass == "CodelistValue")
            {
                organizationCodeList.Organizations[owner].CountCodeList = organizationCodeList.Organizations[owner].CountCodeList + 1;

                foreach (var item in items)
                {
                    organizationCodeList.Organizations[owner].CountCodeListValues = organizationCodeList.Organizations[owner].CountCodeListValues + 1;
                }
            }


            var containedSubRegisters = response["containedSubRegisters"];
            foreach(var item in containedSubRegisters)
            {
                JToken uuidToken = item["uuid"];
                string uuid = uuidToken?.ToString();

                SetCodeListRegisterWithSubregister(uuid);
            }
        }

        private Dictionary<string, int> GetRegisterSymbolPackagesResult()
        {
            Dictionary<string, Dictionary<string, int>> organizationPackages = new Dictionary<string, Dictionary<string, int>>();

            var url = WebConfigurationManager.AppSettings["RegistryUrl"] + "symbol/api/symbols";

            System.Net.WebClient c = new System.Net.WebClient();
            c.Encoding = System.Text.Encoding.UTF8;
            c.Headers.Remove("Accept-Language");
            c.Headers.Add("Accept-Language", Culture.NorwegianCode);
            var data = c.DownloadString(url);
            var response = Newtonsoft.Json.Linq.JArray.Parse(data);

            foreach (var item in response)
            {
                JToken ownerToken = item["Owner"];
                string owner = ownerToken?.ToString();

                JToken packageToken = item["PackageUuid"];
                string package = packageToken?.ToString();

                if (!organizationPackages.ContainsKey(owner))
                {
                    Dictionary<string, int> keyValuePairs = new Dictionary<string, int>();
                    keyValuePairs.Add(package, 1);

                    organizationPackages.Add(owner, keyValuePairs);
                }
                else {

                    if (!organizationPackages[owner].ContainsKey(package))
                    {
                        organizationPackages[owner].Add(package, 1);
                    }
                }
            }

            Dictionary<string, int> result = new Dictionary<string, int>();

            foreach (var organization in organizationPackages)
            {
                result.Add(organization.Key, organization.Value.Count);
            }

            return result;
        }

        private Dictionary<string, int> GetRegisterSymbolResult()
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            var url = WebConfigurationManager.AppSettings["RegistryUrl"] + "symbol/api/symbols";

            System.Net.WebClient c = new System.Net.WebClient();
            c.Encoding = System.Text.Encoding.UTF8;
            c.Headers.Remove("Accept-Language");
            c.Headers.Add("Accept-Language", Culture.NorwegianCode);
            var data = c.DownloadString(url);
            var response = Newtonsoft.Json.Linq.JArray.Parse(data);

            foreach (var item in response)
            {
                JToken ownerToken = item["Owner"];
                string owner = ownerToken?.ToString();

                if (!result.ContainsKey(owner))
                    result.Add(owner, 1);
                else
                    result[owner] = result[owner] + 1;

            }

            return result;
        }

        private Dictionary<string, int> GetRegisterCartographyResult()
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            var url = WebConfigurationManager.AppSettings["RegistryUrl"] + "kartografi/api/kartografi";

            System.Net.WebClient c = new System.Net.WebClient();
            c.Encoding = System.Text.Encoding.UTF8;
            c.Headers.Remove("Accept-Language");
            c.Headers.Add("Accept-Language", Culture.NorwegianCode);
            var data = c.DownloadString(url);
            var response = Newtonsoft.Json.Linq.JArray.Parse(data);

            foreach (var item in response)
            {
                JToken ownerToken = item["Owner"];
                string owner = ownerToken?.ToString();

                if (!result.ContainsKey(owner))
                    result.Add(owner, 1);
                else
                    result[owner] = result[owner] + 1;

            }

            return result;
        }

        private Dictionary<string, int> GetRegisterResult(string systemId)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            var url = WebConfigurationManager.AppSettings["RegistryUrl"] + "api/ApiRoot?systemid=" + systemId;

            System.Net.WebClient c = new System.Net.WebClient();
            c.Encoding = System.Text.Encoding.UTF8;
            c.Headers.Remove("Accept-Language");
            c.Headers.Add("Accept-Language", Culture.NorwegianCode);
            var data = c.DownloadString(url);
            var response = Newtonsoft.Json.Linq.JObject.Parse(data);

            var items = response["containeditems"];
            foreach (var item in items)
            {
                JToken ownerToken = item["owner"];
                string owner = ownerToken?.ToString();

                if (!result.ContainsKey(owner))
                    result.Add(owner, 1);
                else
                    result[owner] = result[owner] + 1;

            }

            return result;
        }

        private Dictionary<string, int> GetMetadataResult(string type)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();

            var url = WebConfigurationManager.AppSettings["KartkatalogenUrl"] + "api/search";

            if (!string.IsNullOrEmpty(type))
                url = url + "?facets%5b0%5dname=type&facets%5b0%5dvalue=" + type;

            System.Net.WebClient c = new System.Net.WebClient();
            c.Encoding = System.Text.Encoding.UTF8;
            c.Headers.Remove("Accept-Language");
            c.Headers.Add("Accept-Language", Culture.NorwegianCode);
            var data = c.DownloadString(url);
            var response = Newtonsoft.Json.Linq.JObject.Parse(data);
            var organizations = response.SelectToken("Facets").Where(s => (string)s["FacetField"] == "organization").Select(o => o.SelectToken("FacetResults")).Values();

            foreach (var item in organizations)
            {
                var organization = item.SelectToken("Name").ToString();
                var count = (int) item.SelectToken("Count");

                result.Add(organization, count);
            } 

            return result;
        }

        public StatisticsReport GetReport(string measurement, string organization, DateTime? fromDate, DateTime? toDate)
        {
            TimeSpan ts = new TimeSpan(23,59,59);
            toDate = toDate.Value.Date + ts;

            StatisticsReport statisticsReport = new StatisticsReport();

            bool organizationSelected = !string.IsNullOrEmpty(organization);

            var list = _dbContext.StatisticalData
                .Where(c => c.Measurement == measurement && (!organizationSelected || (organizationSelected && c.Organization == organization)) && (c.Date >= fromDate && c.Date <= toDate))
                .GroupBy(x => x.Date)
                .Select(g => new {
                    Date = g.Key,
                    Count = g.Sum(x => x.Count)
                }).OrderBy(o => o.Date).ToList();

            ReportResult reportResult = new ReportResult();
            var data = new List<ReportResultData>();

            ReportResultData resultData = new ReportResultData();

            resultData = new ReportResultData
            {
                Label = measurement,
                TotalDataCount = 0,
                Values = new List<ReportResultDataValue>()
            };

            foreach (var item in list)
            {
                resultData.Values.Add(new ReportResultDataValue { Key = item.Date.ToString(), Value = item.Count.ToString() });
            }

            data.Add(resultData);
            reportResult.Data = data;

            if(list.Count > 0)
                reportResult.TotalDataCount = list.Max(m => m.Count);

            statisticsReport.ReportResult = reportResult;

            if (list.Count > 0)
                statisticsReport.MinimumCount = list.Min(m => m.Count);

            statisticsReport.MeasurementsAvailable = GetMeasurements();
            statisticsReport.OrganizationsAvailable = GetOrganizations();

            statisticsReport.MeasurementSelected = measurement;
            statisticsReport.OrganizationSelected = organization;

            statisticsReport.FromDate = fromDate;
            statisticsReport.ToDate = toDate;

            return statisticsReport;
        }

        private List<string> GetOrganizations()
        {
            return _dbContext.StatisticalData.Select(o => o.Organization).Distinct().ToList();
        }

        private List<string> GetMeasurements()
        {
            List<string> measurements = new List<string>();

            measurements.Add(Measurement.NumberOfMetadataTotal);
            measurements.Add(Measurement.NumberOfMetadataForDatasetTotal);
            measurements.Add(Measurement.NumberOfMetadataForServicesTotal);
            measurements.Add(Measurement.NumberOfMetadataForApplicationsTotal);
            measurements.Add(Measurement.NumberOfMetadataForServiceLayerTotal);
            measurements.Add(Measurement.NumberOfProductSpesifications);
            measurements.Add(Measurement.NumberOfCartographyDocuments);
            measurements.Add(Measurement.NumberOfProductsheets);
            measurements.Add(Measurement.NumberOfCartographyFiles);
            measurements.Add(Measurement.NumberOfSymbols);
            measurements.Add(Measurement.NumberOfSymbolPackages);
            measurements.Add(Measurement.NumberOfCodelists);
            measurements.Add(Measurement.NumberOfCodelistValues);
            measurements.Add(Measurement.NumberOfOrganizations);
            measurements.Add(Measurement.NumberOfEpsgCodes);

            return measurements;
        }
    }

    internal class OrganizationCodeList
    {
        public OrganizationCodeList()
        {
            Organizations = new Dictionary<string, CodeList>();
        }
        public Dictionary<string, CodeList> Organizations;
    }

    internal class CodeList
    {
        public int CountCodeList { get; set; }
        public int CountCodeListValues { get; set; }
    }

    public static class Measurement
    {
        public const string NumberOfMetadataTotal = "Antall metadata totalt";

        public const string NumberOfMetadataForDatasetTotal = "Antall metadata for datasett";
        public const string NumberOfMetadataForServicesTotal = "Antall metadata for tjenester";
        public const string NumberOfMetadataForApplicationsTotal = "Antall metadata for applikasjoner";
        public const string NumberOfMetadataForServiceLayerTotal = "Antall metadata for tjenestelag";

        public const string NumberOfProductSpesifications = "Antall produktspesifikasjoner i produktspesifikasjonsregisteret";
        public const string NumberOfCartographyDocuments = "Antall tegneregler i tegneregelregisteret";
        public const string NumberOfProductsheets = "Antall produktark i produktarkregisteret";

        public const string NumberOfCartographyFiles = "Antall kartografi-filer i digital kartografi-registeret";
        public const string NumberOfSymbols = "Antall symboler i symbolregisteret";
        public const string NumberOfSymbolPackages = "Antall symbolpakker i symbolregisteret";

        public const string NumberOfCodelists = "Antall kodelister under kodeliste";
        public const string NumberOfCodelistValues = "Antall kodelisteverdier under kodeliste";

        //public const string NumberOfMetadataCodelists = "Antall metdatakodelister";
        //public const string NumberOfMetadataCodelistValues = "Antall metdatakodelisteverdier";

        public const string NumberOfOrganizations = "Antall organisasjoner i organisasjonsregister";
        public const string NumberOfEpsgCodes = "Antall EPSG-koder i EPSG-koderegister ";

        //public const string NumberOfProductSpesifications = "Antall SOSI-kodelister i SOSI kodelisteregisteret";
        //public const string NumberOfProductSpesifications = "Antall SOSI-kodelisteverdier i SOSI kodelisteregisteret";

        //public const string NumberOfProductSpesifications = "Antall kodelister totalt i hele registeret"; //Summere opp 1)kodelister under kodeliste, 2)metdatakodelister, 3)organisasjoner, 4)EPSG-koder, 5)SOSI-kodelister
        //public const string NumberOfProductSpesifications = "Antall kodelisteverdier totalt i hele registeret"; //Summere opp 1)kodelisteverdier under kodeliste, 2)metdatakodelister, 3)organisasjoner, 4)EPSG-koder, 5)SOSI-kodelister
    }

    public interface IStatisticsService
    {
        void CreateReport();
        StatisticsReport GetReport(string measurement, string organization, DateTime? fromDate, DateTime? toDate);
    }
}