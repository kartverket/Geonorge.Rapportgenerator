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
            return GetCodeList("152ee358-ce3d-40f7-b7ee-85195b40a35a");

        }

        public Dictionary<string, string> GetKommuner()
        {
            return GetCodeList("f99d6d80-32e1-4935-8464-601d90741443");
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
                if (status == "Tilbaketrukket")
                    label = label + " (utgått)";
                else if (status == "Sendt inn")
                    label = label + " (ny)";

                if (status != "Tilbaketrukket" && !CodeValues.ContainsKey(codevalue))
                {
                    CodeValues.Add(codevalue, label);
                }
            }

            CodeValues = CodeValues.OrderBy(o => o.Value).ToDictionary(o => o.Key, o => o.Value);

            return CodeValues;
        }

    }
}

