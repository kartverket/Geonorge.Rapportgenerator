using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Kartverket.ReportGenerator.Models
{
    public class ReportDbContext : DbContext
    {
        public ReportDbContext():base("DefaultConnection"){
        }

        public virtual DbSet<ConfigMetadataEntry> MetadataEntries { get; set; }
        public virtual DbSet<ConfigReportQuery> ReportQueries { get; set; }
        public virtual DbSet<ConfigQueryParameter> QueryParameters { get; set; }
    }
}