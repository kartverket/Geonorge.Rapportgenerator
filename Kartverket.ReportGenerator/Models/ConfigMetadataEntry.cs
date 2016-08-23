using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kartverket.ReportGenerator.Models
{
    public class ConfigMetadataEntry
    {
        public int Id { get; set; }
        public string UUID { get; set; }
        public string Tittel { get; set; }

        public virtual List<ConfigReportQuery> ReportQueries { get; set; }
    }
}