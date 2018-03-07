using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace Kartverket.ReportGenerator.Services
{
    public class MetadataService
    {
        public Metadata GetMetadata(string uuid)
        {
            var client = new System.Net.WebClient { UseDefaultCredentials = true };
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json; charset=utf-8");
            var result = JsonConvert.DeserializeObject<Metadata>(Encoding.UTF8.GetString(client.DownloadData(ConfigurationManager.AppSettings["KartkatalogenUrl"] + "api/getdata/" + uuid)));

            return result;
        }

        public string GetWfsDistributionUrl(Related[] Related, string uuid = "")
        {
            if(Related != null)
            { 
                for (int r = 0; r < Related.Length; r++)
                {
                    var distributionDetails= Related[r].DistributionDetails;
                    if(distributionDetails != null && distributionDetails.Protocol == "OGC:WFS")
                    {
                        return distributionDetails.URL;
                    }
                }
            }

            if (uuid == "638b5ee7-6ab0-4a27-a71a-716bb3a4541d")
                return "https://wfs.geonorge.no/skwms1/wfs.tilgjengelighetfriluft?service=WFS&request=GetCapabilities";
            else if(uuid == "9c075b5d-1fb5-414e-aaf5-c6390db896d1")
                return "https://wfs.geonorge.no/skwms1/wfs.tilgjengelighettettsted?service=WFS&request=GetCapabilities";


            return null;
        }

        public string GetListStoredQueriesUrl(string url)
        {
            url = RemoveQueryString(url);
            url = url + "?service=WFS&version=2.0.0&&request=ListStoredQueries";
            return url;
        }

        string RemoveQueryString(string URL)
        {
            int startQueryString = URL.IndexOf("?");

            if (startQueryString != -1)
                URL = URL.Substring(0, startQueryString);

            return URL;
        }
    }

    public class Metadata
    {
        public string Abstract { get; set; }
        public Boundingbox BoundingBox { get; set; }
        public Constraints Constraints { get; set; }
        public Contactmetadata ContactMetadata { get; set; }
        public Contactowner ContactOwner { get; set; }
        public Contactpublisher ContactPublisher { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateMetadataUpdated { get; set; }
        public DateTime DateUpdated { get; set; }
        public Distributiondetails DistributionDetails { get; set; }
        public Distributionformat DistributionFormat { get; set; }
        public Distributionformat1[] DistributionFormats { get; set; }
        public string UnitsOfDistribution { get; set; }
        public string EnglishTitle { get; set; }
        public string HierarchyLevel { get; set; }
        public Keywordsplace[] KeywordsPlace { get; set; }
        public Keywordstheme[] KeywordsTheme { get; set; }
        public object[] KeywordsInspire { get; set; }
        public Keywordsnationalinitiative[] KeywordsNationalInitiative { get; set; }
        public Keywordsnationaltheme[] KeywordsNationalTheme { get; set; }
        public object[] KeywordsOther { get; set; }
        public object[] KeywordsConcept { get; set; }
        public string MaintenanceFrequency { get; set; }
        public string MetadataLanguage { get; set; }
        public string MetadataStandard { get; set; }
        public string MetadataStandardVersion { get; set; }
        public object[] OperatesOn { get; set; }
        public Related[] Related { get; set; }
        public string ProcessHistory { get; set; }
        public string ProductPageUrl { get; set; }
        public string CoverageUrl { get; set; }
        public string Purpose { get; set; }
        public Qualityspecification[] QualitySpecifications { get; set; }
        public string ResolutionScale { get; set; }
        public string SpatialRepresentation { get; set; }
        public string SpecificUsage { get; set; }
        public string Status { get; set; }
        public string SupplementalDescription { get; set; }
        public Thumbnail1[] Thumbnails { get; set; }
        public string Title { get; set; }
        public string TopicCategory { get; set; }
        public string Uuid { get; set; }
        public string ResourceReferenceCode { get; set; }
        public string ResourceReferenceCodespace { get; set; }
        public string MetadataXmlUrl { get; set; }
        public string MetadataEditUrl { get; set; }
        public string OrganizationLogoUrl { get; set; }
        public string ServiceDistributionUrlForDataset { get; set; }
        public string ServiceDistributionProtocolForDataset { get; set; }
        public string ServiceUuid { get; set; }
    }

    public class Boundingbox
    {
        public string EastBoundLongitude { get; set; }
        public string NorthBoundLatitude { get; set; }
        public string SouthBoundLatitude { get; set; }
        public string WestBoundLongitude { get; set; }
    }

    public class Constraints
    {
        public string AccessConstraints { get; set; }
        public string OtherConstraints { get; set; }
        public string OtherConstraintsLink { get; set; }
        public string OtherConstraintsLinkText { get; set; }
        public string SecurityConstraints { get; set; }
        public string SecurityConstraintsNote { get; set; }
        public string UseConstraints { get; set; }
        public string UseLimitations { get; set; }
        public string OtherConstraintsAccess { get; set; }
    }

    public class Contactmetadata
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Organization { get; set; }
        public string OrganizationEnglish { get; set; }
        public string Role { get; set; }
    }

    public class Contactowner
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Organization { get; set; }
        public string OrganizationEnglish { get; set; }
        public string Role { get; set; }
    }

    public class Contactpublisher
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Organization { get; set; }
        public string OrganizationEnglish { get; set; }
        public string Role { get; set; }
    }

    public class Distributiondetails
    {
        public string Protocol { get; set; }
        public string ProtocolName { get; set; }
        public string URL { get; set; }
    }

    public class Distributionformat
    {
        public string Name { get; set; }
        public string Version { get; set; }
    }

    public class Distributionformat1
    {
        public string Name { get; set; }
        public string Version { get; set; }
    }

    public class Keywordsplace
    {
        public string KeywordValue { get; set; }
        public string Type { get; set; }
    }

    public class Keywordstheme
    {
        public string EnglishKeyword { get; set; }
        public string KeywordValue { get; set; }
        public string Type { get; set; }
    }

    public class Keywordsnationalinitiative
    {
        public string EnglishKeyword { get; set; }
        public string KeywordValue { get; set; }
        public string Thesaurus { get; set; }
        public string Type { get; set; }
    }

    public class Keywordsnationaltheme
    {
        public string KeywordValue { get; set; }
        public string Thesaurus { get; set; }
        public string Type { get; set; }
    }

    public class Related
    {
        public Constraints1 Constraints { get; set; }
        public Contactowner1 ContactOwner { get; set; }
        public Distributiondetails1 DistributionDetails { get; set; }
        public string HierarchyLevel { get; set; }
        public Keywordsnationaltheme1[] KeywordsNationalTheme { get; set; }
        public Thumbnail[] Thumbnails { get; set; }
        public string Title { get; set; }
        public string Uuid { get; set; }
        public string OrganizationLogoUrl { get; set; }
        public string ServiceUuid { get; set; }
        public string ParentIdentifier { get; set; }
    }

    public class Constraints1
    {
        public string AccessConstraints { get; set; }
        public string OtherConstraintsAccess { get; set; }
    }

    public class Contactowner1
    {
        public string Organization { get; set; }
        public string Role { get; set; }
    }

    public class Distributiondetails1
    {
        public string Name { get; set; }
        public string Protocol { get; set; }
        public string URL { get; set; }
    }

    public class Keywordsnationaltheme1
    {
        public string KeywordValue { get; set; }
        public string Thesaurus { get; set; }
    }

    public class Thumbnail
    {
        public string Type { get; set; }
        public string URL { get; set; }
    }

    public class Qualityspecification
    {
        public DateTime Date { get; set; }
        public string DateType { get; set; }
        public string Explanation { get; set; }
        public bool Result { get; set; }
        public string Title { get; set; }
    }

    public class Thumbnail1
    {
        public string Type { get; set; }
        public string URL { get; set; }
    }

}