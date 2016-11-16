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
            string type, typePrev = "", queryUrlTotal = "";
            //First 2 stored queries are default: GetFeatureById and GetFeatureByType
            for (int q = 2; q < list.StoredQuery.Length; q++)
            {
                QueryData qData = new QueryData();
                qData.Title = list.StoredQuery[q].Title;
                qData.QueryUrl = RemoveQueryString(url) + "?service=WFS&version=2.0.0&request=GetFeature&resultType=hits&STOREDQUERY_ID=" + list.StoredQuery[q].id + "&admEnhNr=03";
                type = list.StoredQuery[q].ReturnFeatureType[0].ToString();
                if(type != typePrev && list.StoredQuery[q].id.Contains("getAntall"))
                    queryUrlTotal = RemoveQueryString(url) + "?service=WFS&version=2.0.0&request=GetFeature&resultType=hits&STOREDQUERY_ID=" + list.StoredQuery[q].id + "&admEnhNr=03";

                qData.QueryUrlTotal = queryUrlTotal;
                if(!list.StoredQuery[q].id.Contains("getAntall"))
                    qList.Add(qData);

                typePrev = type;
            }
            return qList;
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