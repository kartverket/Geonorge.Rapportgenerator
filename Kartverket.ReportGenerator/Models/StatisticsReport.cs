using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Kartverket.ReportApi;

namespace Kartverket.ReportGenerator.Models
{
    public class StatisticsReport
    {
        public ReportResult ReportResult { get; set; }
        public int MinimumCount { get; set; }
        public string MeasurementSelected { get; set; }
        public List<string> MeasurementsAvailable { get; set; }
        public string OrganizationSelected { get; set; }
        public List<string> OrganizationsAvailable { get; set; }
        [Display(Name = "Fra dato")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime? FromDate { get; set; } = DateTime.Now.AddYears(-1);
        [Display(Name = "Til dato")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime? ToDate { get; set; } = DateTime.Now;

    }
}