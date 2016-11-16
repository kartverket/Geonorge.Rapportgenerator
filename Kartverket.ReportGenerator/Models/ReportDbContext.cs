﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Kartverket.ReportGenerator.Models
{
    public class ReportDbContext : DbContext
    {
        public ReportDbContext():base("DefaultConnection"){
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ReportDbContext, Kartverket.ReportGenerator.Migrations.Configuration>("DefaultConnection"));
        }

        public virtual DbSet<MetadataEntry> MetadataEntries { get; set; }
    }
}