using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kartverket.ReportGenerator.Models.statistics
{
    public class Data
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Content { get; set; }
        public string Organization { get; set; }
        public int Count { get; set; }

    }
}