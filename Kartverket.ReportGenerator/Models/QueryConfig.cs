using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq;
using System.Web.Configuration;

namespace Kartverket.ReportGenerator.Models
{

    public class QueryConfig
    {
        List<Query> queries;

        public QueryConfig()
        {
            queries = new List<Query>();

            Group datapackage = new Group { Name = "datapackage", Value = "Datapakker" };
            Group theme = new Group { Name = "theme", Value = "Tema" };

            Data DOK = new Data { Group = datapackage, Value = "register-DOK", Name = "Det offentlige kartgrunnlaget" };

            queries.Add(
                new Query
                {
                    Data = DOK,
                    Value = "register-DOK-selectedAndAdditional",
                    Name = "Antall DOK-datasett valgt og tillegg pr kommune",
                    QueryUrl = WebConfigurationManager.AppSettings["RegistryUrl"] + "api/report"
                }
            );
            queries.Add(
                new Query
                {
                    Data = DOK,
                    Value = "register-DOK-selectedTheme",
                    Name = "Valgte DOK-datasett pr tema",
                    QueryUrl = WebConfigurationManager.AppSettings["RegistryUrl"] + "api/report"
                }
            );
            queries.Add(
                new Query
                {
                    Data = DOK,
                    Value = "register-DOK-coverage",
                    Name = "DOK-datasett dekning og valgt pr kommune",
                    QueryUrl = WebConfigurationManager.AppSettings["RegistryUrl"] + "api/report"
                }
            );


            Data HC_Parking = new Data
            {
                Group = theme,
                Value = "hc-parking",
                Name = "HC-parkeringsplasser",
                QueryUrl = "https://wfs.geonorge.no/skwms1/wfs.tilgjengelighettettsted?service=WFS&version=2.0.0&request=GetFeature&resultType=hits&STOREDQUERY_ID=urn:ogc:def:storedQuery:OGC-WFS::getHCPlasserPrAdmEnhet&admEnhNr=16"
            };

            queries.Add(
                new Query
                {
                    Data = HC_Parking,
                    Value = "HCPlasserPrAdmEnhetForMaksBredde",
                    Name = "Bredde < 200 cm",
                    QueryUrl = "https://wfs.geonorge.no/skwms1/wfs.tilgjengelighettettsted?service=WFS&version=2.0.0&request=GetFeature&resultType=hits&STOREDQUERY_ID=urn:ogc:def:storedQuery:OGC-WFS::getHCPlasserPrAdmEnhetForMaksBredde&admEnhNr=16&bredde=200"
                }
            );
        }

        public List<Query> GetQueries()
        {
            return queries.ToList();
        }

        public Query GetQuery(string value)
        {
            return queries.Where(q => q.Value == value).FirstOrDefault();
        }
    }

    public class Group
    {
        public string Value { get; set; }
        public string Name { get; set; }
    }

    public class Data
    {
        public string Value { get; set; }
        public string Name { get; set; }
        public string QueryUrl { get; set; }
        public Group Group { get; set; }
    }

    public class Query
    {
        public string Value { get; set; }
        public string Name { get; set; }
        public string QueryUrl { get; set; }
        public Data Data { get; set; }

    }
}