using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq;
using System.Web.Configuration;

namespace Kartverket.ReportGenerator.Models
{

    public static class QueryConfig
    {
        static List<Query> queries;

        static QueryConfig()
        {
            queries = new List<Query>();

            Data DOK = new Data { Value = "register-DOK", Name = "Det offentlige kartgrunnlaget" };

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


            Data Tilgjengelighet_tettsted = new Data
            {
                Value = "9c075b5d-1fb5-414e-aaf5-c6390db896d1",
                Name =  "Tilgjengelighet - tettsted",
                QueryUrl = "https://wfs.geonorge.no/skwms1/wfs.tilgjengelighettettsted?service=WFS&version=2.0.0&request=GetFeature&resultType=hits&STOREDQUERY_ID=urn:ogc:def:storedQuery:OGC-WFS::getHCPlasserPrAdmEnhet&admEnhNr=16"
            };

            queries.Add(
                new Query
                {
                    Data = Tilgjengelighet_tettsted,
                    Value = "HCPlasserPrAdmEnhetForMaksBredde",
                    Name = "HC-parkeringsplasser bredde < 200 cm",
                    QueryUrl = "https://wfs.geonorge.no/skwms1/wfs.tilgjengelighettettsted?service=WFS&version=2.0.0&request=GetFeature&resultType=hits&STOREDQUERY_ID=urn:ogc:def:storedQuery:OGC-WFS::getHCPlasserPrAdmEnhetForMaksBredde&admEnhNr=16&bredde=200"
                }
            );

            queries.Add(
                new Query
                {
                    Data = Tilgjengelighet_tettsted,
                    Value = "HCPlasserPrAdmEnhetForMaksLengde",
                    Name = "HC-parkeringsplasser lengde < 600 cm",
                    QueryUrl = "https://wfs.geonorge.no/skwms1/wfs.tilgjengelighettettsted?service=WFS&version=2.0.0&request=GetFeature&resultType=hits&STOREDQUERY_ID=urn:ogc:def:storedQuery:OGC-WFS::getHCPlasserPrAdmEnhetForMaksLengde&admEnhNr=16&lengde=600"
                }
            );

            queries.Add(
                new Query
                {
                    Data = Tilgjengelighet_tettsted,
                    Value = "HCPlasserPrAdmEnhetRullestolTilgjengelig",
                    Name = "HC-parkeringsplasser tilgjgengelighet man. rullestol = Tilgjengelig",
                    QueryUrl = "https://wfs.geonorge.no/skwms1/wfs.tilgjengelighettettsted?service=WFS&version=2.0.0&request=GetFeature&resultType=hits&STOREDQUERY_ID=urn:ogc:def:storedQuery:OGC-WFS::getHCPlasserPrAdmEnhetRullestol&admEnhNr=03&tilgjengvurderingRullestol=tilgjengelig"
                }
            );

            queries.Add(
               new Query
               {
                   Data = Tilgjengelighet_tettsted,
                   Value = "HCPlasserPrAdmEnhetRullestolIkkeTilgjengelig",
                   Name = "HC-parkeringsplasser tilgjgengelighet man. rullestol = Ikke tilgjengelig",
                   QueryUrl = "https://wfs.geonorge.no/skwms1/wfs.tilgjengelighettettsted?service=WFS&version=2.0.0&request=GetFeature&resultType=hits&STOREDQUERY_ID=urn:ogc:def:storedQuery:OGC-WFS::getHCPlasserPrAdmEnhetRullestol&admEnhNr=03&tilgjengvurderingRullestol=ikkeTilgjengelig"
               }
           );

            queries.Add(
               new Query
               {
                   Data = Tilgjengelighet_tettsted,
                   Value = "HCPlasserPrAdmEnhetRullestolVanskeligTilgjengelig",
                   Name = "HC-parkeringsplasser tilgjgengelighet man. rullestol = vanskelig tilgjengelig",
                   QueryUrl = "https://wfs.geonorge.no/skwms1/wfs.tilgjengelighettettsted?service=WFS&version=2.0.0&request=GetFeature&resultType=hits&STOREDQUERY_ID=urn:ogc:def:storedQuery:OGC-WFS::getHCPlasserPrAdmEnhetRullestol&admEnhNr=03&tilgjengvurderingRullestol=vanskeligTilgjengelig"
               }
           );

            queries.Add(
               new Query
               {
                   Data = Tilgjengelighet_tettsted,
                   Value = "HCPlasserPrAdmEnhetRullestolIkkeVurdert",
                   Name = "HC-parkeringsplasser tilgjgengelighet man. rullestol = ikke vurdert",
                   QueryUrl = "https://wfs.geonorge.no/skwms1/wfs.tilgjengelighettettsted?service=WFS&version=2.0.0&request=GetFeature&resultType=hits&STOREDQUERY_ID=urn:ogc:def:storedQuery:OGC-WFS::getHCPlasserPrAdmEnhetRullestol&admEnhNr=03&tilgjengvurderingRullestol=ikkeVurdert"
               }
           );

            queries.Add(
                new Query
                {
                    Data = Tilgjengelighet_tettsted,
                    Value = "HCPlasserPrAdmEnhetElRullestolTilgjengelig",
                    Name = "HC-parkeringsplasser tilgjgengelighet el. rullestol = Tilgjengelig",
                    QueryUrl = "https://wfs.geonorge.no/skwms1/wfs.tilgjengelighettettsted?service=WFS&version=2.0.0&request=GetFeature&resultType=hits&STOREDQUERY_ID=urn:ogc:def:storedQuery:OGC-WFS::getHCPlasserPrAdmEnhetElRullestol&admEnhNr=03&tilgjengvurderingElRull=tilgjengelig"
                }
            );

            queries.Add(
               new Query
               {
                   Data = Tilgjengelighet_tettsted,
                   Value = "HCPlasserPrAdmEnhetElRullestolIkkeTilgjengelig",
                   Name = "HC-parkeringsplasser tilgjgengelighet el. rullestol = Ikke tilgjengelig",
                   QueryUrl = "https://wfs.geonorge.no/skwms1/wfs.tilgjengelighettettsted?service=WFS&version=2.0.0&request=GetFeature&resultType=hits&STOREDQUERY_ID=urn:ogc:def:storedQuery:OGC-WFS::getHCPlasserPrAdmEnhetElRullestol&admEnhNr=03&tilgjengvurderingElRull=ikkeTilgjengelig"
               }
           );

            queries.Add(
               new Query
               {
                   Data = Tilgjengelighet_tettsted,
                   Value = "HCPlasserPrAdmEnhetElRullestolVanskeligTilgjengelig",
                   Name = "HC-parkeringsplasser tilgjgengelighet el. rullestol = vanskelig tilgjengelig",
                   QueryUrl = "https://wfs.geonorge.no/skwms1/wfs.tilgjengelighettettsted?service=WFS&version=2.0.0&request=GetFeature&resultType=hits&STOREDQUERY_ID=urn:ogc:def:storedQuery:OGC-WFS::getHCPlasserPrAdmEnhetElRullestol&admEnhNr=03&tilgjengvurderingElRull=vanskeligTilgjengelig"
               }
           );

            queries.Add(
               new Query
               {
                   Data = Tilgjengelighet_tettsted,
                   Value = "HCPlasserPrAdmEnhetElRullestolIkkeVurdert",
                   Name = "HC-parkeringsplasser tilgjgengelighet el. rullestol = ikke vurdert",
                   QueryUrl = "https://wfs.geonorge.no/skwms1/wfs.tilgjengelighettettsted?service=WFS&version=2.0.0&request=GetFeature&resultType=hits&STOREDQUERY_ID=urn:ogc:def:storedQuery:OGC-WFS::getHCPlasserPrAdmEnhetElRullestol&admEnhNr=03&tilgjengvurderingElRull=ikkeVurdert"
               }
           );

        }

        public static List<Query> GetQueries()
        {
            return queries.ToList();
        }

        public static Query GetQuery(string value)
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