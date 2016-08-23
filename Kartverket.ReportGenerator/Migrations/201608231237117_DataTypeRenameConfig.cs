namespace Kartverket.ReportGenerator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataTypeRenameConfig : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.MetadataEntries", newName: "ConfigMetadataEntries");
            RenameTable(name: "dbo.ReportQueries", newName: "ConfigReportQueries");
            RenameTable(name: "dbo.QueryParameters", newName: "ConfigQueryParameters");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.ConfigQueryParameters", newName: "QueryParameters");
            RenameTable(name: "dbo.ConfigReportQueries", newName: "ReportQueries");
            RenameTable(name: "dbo.ConfigMetadataEntries", newName: "MetadataEntries");
        }
    }
}
