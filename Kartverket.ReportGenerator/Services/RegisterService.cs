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
            return GetCodeList("11EC4661-ACF4-4636-A960-68A8160642A0");

        }

        public Dictionary<string, string> GetKommuner()
        {
            return GetCodeList("54DDDFA8-A9D3-4115-8541-4B0905779054");
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
                var codevalue = code["label"].ToString();
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

