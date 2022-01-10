using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Configuration;

namespace Kartverket.ReportGenerator.Services
{
    public class RegisterService: IRegisterService
    {
        private Dictionary<string, string> _areas;


        public Dictionary<string, string> GetFylker()
        {
            return GetCodeList("d96bcbd6-d6ce-4fd4-a999-102051685aec");

        }

        public Dictionary<string, string> GetKommuner()
        {
            return GetCodeList("85d75e07-7caa-4876-b0b1-27513ce57670");
        }


        Dictionary<string, string> GetCodeList(string systemid)
        {
            Dictionary<string, string> CodeValues = new Dictionary<string, string>();
            string url = System.Web.Configuration.WebConfigurationManager.AppSettings["RegistryUrl"] + "api/kodelister/" + systemid;
            System.Net.WebClient c = new System.Net.WebClient();
            c.Encoding = System.Text.Encoding.UTF8;
            var data = c.DownloadString(url);
            var response = Newtonsoft.Json.Linq.JObject.Parse(data);

            var codeList = response["containeditems"];

            foreach (var code in codeList)
            {
                var codevalue = code["codevalue"].ToString();
                var label = code["description"].ToString();
                var status = code["status"].ToString();
                if (status == "Utgått")
                    label = label + " (utgått)";
                else if (status == "Sendt inn")
                    label = label + " (ny)";

                if (!CodeValues.ContainsKey(codevalue))
                {
                    CodeValues.Add(codevalue, label);
                }
            }

            CodeValues = CodeValues.OrderBy(o => o.Value).ToDictionary(o => o.Key, o => o.Value);

            return CodeValues;
        }

    }
}

