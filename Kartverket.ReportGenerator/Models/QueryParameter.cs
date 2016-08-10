using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Kartverket.ReportGenerator.Models
{
    public class QueryParameter
    {
        public int Id { get; set; }
        public string Tittel { get; set; }
        public string Datatype { get; set; }

        [ForeignKey("ReportQuery")]
        [Required]
        public int ReportQueryID { get; set; }
        public virtual ReportQuery ReportQuery { get; set; }
    }
}