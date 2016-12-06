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

            //Fixed queries for DOK data from registry

            ObjectType datapackage = new ObjectType { Name = "Datapakker", Value = "datapackage" };

            Data DOK = new Data { ObjectType = datapackage, Value = "e4eb3a1d-481e-45a2-8a58-ead15240a9b0", Name = "Det offentlige kartgrunnlaget" };

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

        }

        public void AddQuery(Query query)
        {
            queries.Add(query);
        }

        public List<Query> GetQueries()
        {
            return queries.ToList();
        }

        public Query GetQuery(string value, string data)
        {
            return queries.Where(q => q.Value == value && (q.Data.Name == data || q.Data.Value == data) ).FirstOrDefault();
        }
    }


    public class Data
    {
        public string Value { get; set; }
        public string Name { get; set; }
        public string QueryUrl { get; set; }
        public ObjectType ObjectType { get; set; }
    }

    public class Query
    {
        public string Value { get; set; }
        public string Name { get; set; }
        public string QueryUrl { get; set; }
        public Data Data { get; set; }

    }

    public class ObjectType
    {
        public string Value { get; set; }
        public string Name { get; set; }
    }
}