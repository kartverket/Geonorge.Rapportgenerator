using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Xml.Serialization;

namespace Kartverket.ReportGenerator.Models
{
    public class Wfs
    {
        string url;

        public List<QueryData> GetStoredQueries(string url)
        {
        this.url = url; 
        var client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var resultData = response.Content.ReadAsStringAsync().Result;

                var buffer = Encoding.UTF8.GetBytes(resultData);
                using (var stream = new MemoryStream(buffer))
                {
                    var serializer = new XmlSerializer(typeof(ListStoredQueriesResponse));
                    return ParseStoredQueries((ListStoredQueriesResponse)serializer.Deserialize(stream));
                }
            }

            return null;
        }

        List<QueryData> ParseStoredQueries(ListStoredQueriesResponse list)
        {
            List<QueryData> qList = new List<QueryData>();

            var totalTypes = list.StoredQuery.Where(x => x.id.Contains("_Total"));

            foreach(var type in totalTypes)
            {
                var storedQueryName = type.id;
                var objectType = GetSubstringByString("::", "_", storedQueryName);
                var queryUrlTotal = RemoveQueryString(url) + "?service=WFS&version=2.0.0&request=GetFeature&resultType=hits&STOREDQUERY_ID=" + storedQueryName + "&admEnhNr=03";

                var storedQueries = list.StoredQuery.Where(y => y.id.Contains("::" + objectType + "_") && !y.id.Contains("_Total"));

                foreach(var query in storedQueries)
                {
                    QueryData qData = new QueryData();
                    storedQueryName = query.id;
                    qData.Title = query.Title;
                    qData.QueryUrlTotal = queryUrlTotal;
                    qData.QueryUrl = RemoveQueryString(url) + "?service=WFS&version=2.0.0&request=GetFeature&resultType=hits&STOREDQUERY_ID=" + storedQueryName + "&admEnhNr=03";

                    qList.Add(qData);
                }
            }


            return qList;
        }

        public string GetSubstringByString(string a, string b, string c)
        {
            return c.Substring((c.IndexOf(a) + a.Length), (c.IndexOf(b) - c.IndexOf(a) - a.Length));
        }

        string RemoveQueryString(string URL)
        {
            int startQueryString = URL.IndexOf("?");

            if (startQueryString != -1)
                URL = URL.Substring(0, startQueryString);

            return URL;
        }
    }

    public class QueryData
    {
        public string Title { get; set; }
        public string QueryUrl { get; set; }
        public string QueryUrlTotal { get; set; }
    }


    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.opengis.net/wfs/2.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.opengis.net/wfs/2.0", IsNullable = false)]
    public partial class ListStoredQueriesResponse
    {

        private ListStoredQueriesResponseStoredQuery[] storedQueryField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("StoredQuery")]
        public ListStoredQueriesResponseStoredQuery[] StoredQuery
        {
            get
            {
                return this.storedQueryField;
            }
            set
            {
                this.storedQueryField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.opengis.net/wfs/2.0")]
    public partial class ListStoredQueriesResponseStoredQuery
    {

        private string titleField;

        private string[] returnFeatureTypeField;

        private string idField;

        /// <remarks/>
        public string Title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ReturnFeatureType")]
        public string[] ReturnFeatureType
        {
            get
            {
                return this.returnFeatureTypeField;
            }
            set
            {
                this.returnFeatureTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }



}