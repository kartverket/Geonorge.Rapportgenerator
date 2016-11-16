using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kartverket.ReportGenerator.Models
{
    public class MetadataEntry
    {
        [Key]
        public string Uuid { get; set; }
    }
}