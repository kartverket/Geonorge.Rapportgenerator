using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Kartverket.ReportGenerator.Models
{
    public class ReportQuery
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Tittel { get; set; }
        public string DataSource { get; set; }

        [ForeignKey("MetadataEntry")]
        [Required]
        public int MetadataID { get; set; }
        public virtual MetadataEntry MetadataEntry { get; set; }

        public virtual List<QueryParameter> QueryParameters { get; set; }
    }
}