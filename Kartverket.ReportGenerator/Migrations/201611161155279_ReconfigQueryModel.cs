namespace Kartverket.ReportGenerator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReconfigQueryModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ConfigReportQueries", "MetadataID", "dbo.ConfigMetadataEntries");
            DropForeignKey("dbo.ConfigQueryParameters", "ReportQueryID", "dbo.ConfigReportQueries");
            DropIndex("dbo.ConfigReportQueries", new[] { "MetadataID" });
            DropIndex("dbo.ConfigQueryParameters", new[] { "ReportQueryID" });
            CreateTable(
                "dbo.MetadataEntries",
                c => new
                    {
                        Uuid = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Uuid);

            DropTable("dbo.ConfigQueryParameters");
            DropTable("dbo.ConfigReportQueries");
            DropTable("dbo.ConfigMetadataEntries");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ConfigQueryParameters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tittel = c.String(),
                        Datatype = c.String(),
                        ReportQueryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConfigReportQueries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        Tittel = c.String(),
                        DataSource = c.String(),
                        MetadataID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConfigMetadataEntries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UUID = c.String(),
                        Tittel = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.MetadataEntries");
            CreateIndex("dbo.ConfigQueryParameters", "ReportQueryID");
            CreateIndex("dbo.ConfigReportQueries", "MetadataID");
            AddForeignKey("dbo.ConfigQueryParameters", "ReportQueryID", "dbo.ConfigReportQueries", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ConfigReportQueries", "MetadataID", "dbo.ConfigMetadataEntries", "Id", cascadeDelete: true);
        }
    }
}
