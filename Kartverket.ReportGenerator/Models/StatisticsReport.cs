using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kartverket.ReportApi;

namespace Kartverket.ReportGenerator.Models
{
    public class StatisticsReport
    {
        public ReportResult ReportResult { get; set; }
        public int MinimumCount { get; set; }
    }
}