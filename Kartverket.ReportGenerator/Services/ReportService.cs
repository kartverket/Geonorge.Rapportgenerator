using Kartverket.ReportApi;
using Kartverket.ReportGenerator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Xml.Serialization;

namespace Kartverket.ReportGenerator.Services
{
    public class ReportService: IReportService
    {
        private readonly ReportDbContext _dbContext;
        private readonly IRegisterService _registerService;

        public ReportService(ReportDbContext dbContext, IRegisterService registerService)
        {
            _dbContext = dbContext;
            _registerService = registerService;
        }

        public QueryConfig GetQueries()
        {
            QueryConfig config = new QueryConfig();
            var metadataService = new MetadataService();
            var metadataList = _dbContext.MetadataEntries.ToList();
            foreach(var entry in metadataList)
            { 
                var metadata = metadataService.GetMetadata(entry.Uuid);
                var wfsUrl = metadataService.GetWfsDistributionUrl(metadata.Related);
                var SqUrl = metadataService.GetListStoredQueriesUrl(wfsUrl);
                var queries = new Wfs().GetStoredQueries(SqUrl);
                foreach (var query in queries)
                {
                    config.AddQuery(new Query
                    {
                        Data = new Data { ObjectType = new ObjectType
                        { Value = metadata.Uuid, Name = metadata.Title },
                            Name = query.ObjectType,
                            Value = query.ObjectType
                          , QueryUrl = query.QueryUrlTotal }
                        ,
                        Name = query.Title,
                        Value = query.Title.Replace(' ', '_'),
                        QueryUrl = query.QueryUrl
                    }
                    );
                }

            }

            return config;
        }

        public ReportResult GetQueryResult(ReportApi.ReportQuery query = null)
        {
            if (query.QueryName.StartsWith("register-"))
            {
                return GetRegistryQueryResult(query);
            } else
            {
                return GetWfsUURegistryQueryResult(query);
            }

        }

        private ReportResult GetWfsUURegistryQueryResult(ReportQuery query)
        {
            var data = query.Parameters.Where(p => p.Name == "data").Select(a => a.Value).First();
            var queryList = GetQueries();
            var queryInfo = queryList.GetQuery(query.QueryName, data);
            string reportStoredQuery = queryInfo.QueryUrl;
            string reportStoredQueryTotal = queryInfo.Data.QueryUrl;

            var fylker = _registerService.GetFylker();
            var kommuner = _registerService.GetKommuner();
            var admUnits = fylker.Union(kommuner).ToDictionary(k => k.Key, v => v.Value);
            admUnits.Add("*", "Hele landet");

            ReportResult reportResult = new ReportResult();
            reportResult.Data = new List<ReportResultData>();

            var areas = query.Parameters.Where(p => p.Name == "area").Select(a => a.Value).ToList();

            foreach(var area in areas)
            {
                string areaUnit = (area == "Hele landet" ? "*" : area);

                reportStoredQuery = SetAdmEnhNr(reportStoredQuery, areaUnit);
                reportStoredQueryTotal = SetAdmEnhNr(reportStoredQueryTotal, areaUnit);

                FeatureCollection result = GetFeature(reportStoredQuery);
                FeatureCollection resultTotal = GetFeature(reportStoredQueryTotal);

                reportResult.TotalDataCount = 0;

                ReportResultData reportResultData = new ReportResultData();
                List<ReportResultDataValue> reportResultDataValues = new List<ReportResultDataValue>();
                reportResultData.Label = admUnits.ContainsKey(area) ? admUnits[area] : area;
                reportResultData.TotalDataCount = resultTotal.numberMatched;
                ReportResultDataValue reportResultDataValue = new ReportResultDataValue();
                reportResultDataValue.Key = queryInfo.Data.Name;
                reportResultDataValue.Value = result.numberMatched.ToString();
                reportResultDataValues.Add(reportResultDataValue);

                //additional start
                reportResultDataValue = new ReportResultDataValue();
                reportResultDataValue.Key = "";
                reportResultDataValue.Value = "";
                reportResultDataValues.Add(reportResultDataValue);
                //additional end

                reportResultData.Values = reportResultDataValues;
                reportResult.Data.Add(reportResultData);
            }

            return reportResult;
        }

        private string SetAdmEnhNr(string reportStoredQuery, string areaUnit)
        {
            Uri uri = new Uri(reportStoredQuery);
            System.Collections.Specialized.NameValueCollection nvc = HttpUtility.ParseQueryString(uri.Query);
            nvc["admEnhNr"] = (areaUnit ?? "").ToString();
            Uri newUri = new UriBuilder(uri) { Query = nvc.ToString() }.Uri;

            return newUri.ToString();

        }

        private FeatureCollection GetFeature(string reportUrl)
        {
            //Disable SSL sertificate errors
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
            delegate (object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                                    System.Security.Cryptography.X509Certificates.X509Chain chain,
                                    System.Net.Security.SslPolicyErrors sslPolicyErrors)
            {
                return true; // **** Always accept
            };

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

            HttpResponseMessage response = client.GetAsync(reportUrl).Result;

            if (response.IsSuccessStatusCode)
            {
                var resultData = response.Content.ReadAsStringAsync().Result;

                var buffer = Encoding.UTF8.GetBytes(resultData);
                using (var stream = new MemoryStream(buffer))
                {
                    var serializer = new XmlSerializer(typeof(FeatureCollection));
                    return (FeatureCollection)serializer.Deserialize(stream);
                }
            }

            return null;
        }

        public ReportResult GetRegistryQueryResult(ReportApi.ReportQuery query = null) { 

            string reportUrl = WebConfigurationManager.AppSettings["RegistryUrl"] + "api/report";

            //Disable SSL sertificate errors
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
            delegate (object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                                    System.Security.Cryptography.X509Certificates.X509Chain chain,
                                    System.Net.Security.SslPolicyErrors sslPolicyErrors)
            {
                return true; // **** Always accept
            };

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(query,
                new Newtonsoft.Json.JsonSerializerSettings
                { ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver() }
                );
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(reportUrl, content).Result;

            if (response.IsSuccessStatusCode)
            {

                var result = response.Content.ReadAsAsync<object>().Result;
                var reportResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ReportResult>(result.ToString());

                return reportResult;
            }

            return null;
        }

    }


    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.opengis.net/wfs/2.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.opengis.net/wfs/2.0", IsNullable = false)]
    public partial class FeatureCollection
    {

        private System.DateTime timeStampField;

        private ushort numberMatchedField;

        private byte numberReturnedField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime timeStamp
        {
            get
            {
                return this.timeStampField;
            }
            set
            {
                this.timeStampField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort numberMatched
        {
            get
            {
                return this.numberMatchedField;
            }
            set
            {
                this.numberMatchedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte numberReturned
        {
            get
            {
                return this.numberReturnedField;
            }
            set
            {
                this.numberReturnedField = value;
            }
        }
    }


}