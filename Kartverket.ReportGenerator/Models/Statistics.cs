using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kartverket.ReportGenerator.Models
{
    public class Statistics
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        [MaxLength(255)]
        public string Organization { get; set; }
        [MaxLength(255)]
        public string Measurement { get; set; }
        public int Count { get; set; }

    }
}