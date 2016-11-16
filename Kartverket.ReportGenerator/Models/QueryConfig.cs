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

            Data DOK = new Data { Value = "e4eb3a1d-481e-45a2-8a58-ead15240a9b0", Name = "Det offentlige kartgrunnlaget" };

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


            //Data Tilgjengelighet_tettsted = new Data
            //{
            //    Value = "9c075b5d-1fb5-414e-aaf5-c6390db896d1",
            //    Name =  "Tilgjengelighet - tettsted",
            //    QueryUrl = "https://wfs.geonorge.no/skwms1/wfs.tilgjengelighettettsted?service=WFS&version=2.0.0&request=GetFeature&resultType=hits&STOREDQUERY_ID=urn:ogc:def:storedQuery:OGC-WFS::getAntallHCPlasserPrAdmEnhet&admEnhNr=16"
            //};

            //queries.Add(
            //    new Query
            //    {
            //        Data = Tilgjengelighet_tettsted,
            //        Value = "HCPlasserPrAdmEnhetElRullestolTilgjengelig",
            //        Name = "HC-parkeringsplasser tilgjengelighet el. rullestol",
            //        QueryUrl = "https://wfs.geonorge.no/skwms1/wfs.tilgjengelighettettsted?service=WFS&version=2.0.0&request=GetFeature&resultType=hits&STOREDQUERY_ID=urn:ogc:def:storedQuery:OGC-WFS::getHCPlasserPrAdmEnhetElRullestol&admEnhNr=03"
            //    }
            //);

        }

        public void AddQuery(Query query)
        {
            queries.Add(query);
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


    public class Data
    {
        public string Value { get; set; }
        public string Name { get; set; }
        public string QueryUrl { get; set; }
    }

    public class Query
    {
        public string Value { get; set; }
        public string Name { get; set; }
        public string QueryUrl { get; set; }
        public Data Data { get; set; }

    }
}