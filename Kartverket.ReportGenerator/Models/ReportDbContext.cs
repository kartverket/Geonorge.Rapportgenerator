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

        public virtual DbSet<MetadataEntry> MetadataEntries { get; set; }
        public virtual DbSet<ReportQuery> ReportQueries { get; set; }
        public virtual DbSet<QueryParameter> QueryParameters { get; set; }
    }
}