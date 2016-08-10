namespace Kartverket.ReportGenerator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MetadataEntries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UUID = c.String(),
                        Tittel = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ReportQueries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        Tittel = c.String(),
                        DataSource = c.String(),
                        MetadataID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MetadataEntries", t => t.MetadataID, cascadeDelete: true)
                .Index(t => t.MetadataID);
            
            CreateTable(
                "dbo.QueryParameters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tittel = c.String(),
                        Datatype = c.String(),
                        ReportQueryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ReportQueries", t => t.ReportQueryID, cascadeDelete: true)
                .Index(t => t.ReportQueryID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QueryParameters", "ReportQueryID", "dbo.ReportQueries");
            DropForeignKey("dbo.ReportQueries", "MetadataID", "dbo.MetadataEntries");
            DropIndex("dbo.QueryParameters", new[] { "ReportQueryID" });
            DropIndex("dbo.ReportQueries", new[] { "MetadataID" });
            DropTable("dbo.QueryParameters");
            DropTable("dbo.ReportQueries");
            DropTable("dbo.MetadataEntries");
        }
    }
}
