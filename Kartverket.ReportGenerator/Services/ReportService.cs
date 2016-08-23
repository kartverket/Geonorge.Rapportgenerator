using Kartverket.ReportApi;
using Kartverket.ReportGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace Kartverket.ReportGenerator.Services
{
    public class ReportService: IReportService
    {
        private readonly ReportDbContext _dbContext;

        public ReportService(ReportDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ReportResult GetQueryResult(ReportApi.ReportQuery query = null)
        {
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
}