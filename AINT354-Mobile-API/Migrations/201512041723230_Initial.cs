namespace AINT354_Mobile_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Calendars",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        Description = c.String(nullable: false),
                        ColourId = c.Int(nullable: false),
                        OwnerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Colours", t => t.ColourId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.OwnerId, cascadeDelete: true)
                .Index(t => t.ColourId)
                .Index(t => t.OwnerId);
            
            CreateTable(
                "dbo.Colours",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ColourName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CalendarId = c.Guid(nullable: false),
                        CreatorId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        Title = c.String(nullable: false),
                        Body = c.String(),
                        Location = c.String(),
                        AllDay = c.Boolean(nullable: false),
                        StartDateTime = c.DateTime(nullable: false),
                        EndDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Calendars", t => t.CalendarId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.CreatorId)
                .Index(t => t.CalendarId)
                .Index(t => t.CreatorId);
            
            CreateTable(
                "dbo.EventComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EventId = c.Guid(nullable: false),
                        CreatorId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        OrderNo = c.Int(nullable: false),
                        Body = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorId, cascadeDelete: true)
                .ForeignKey("dbo.Events", t => t.EventId)
                .Index(t => t.EventId)
                .Index(t => t.CreatorId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Email = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventComments", "EventId", "dbo.Events");
            DropForeignKey("dbo.EventComments", "CreatorId", "dbo.Users");
            DropForeignKey("dbo.Events", "CreatorId", "dbo.Users");
            DropForeignKey("dbo.Calendars", "OwnerId", "dbo.Users");
            DropForeignKey("dbo.Events", "CalendarId", "dbo.Calendars");
            DropForeignKey("dbo.Calendars", "ColourId", "dbo.Colours");
            DropIndex("dbo.EventComments", new[] { "CreatorId" });
            DropIndex("dbo.EventComments", new[] { "EventId" });
            DropIndex("dbo.Events", new[] { "CreatorId" });
            DropIndex("dbo.Events", new[] { "CalendarId" });
            DropIndex("dbo.Calendars", new[] { "OwnerId" });
            DropIndex("dbo.Calendars", new[] { "ColourId" });
            DropTable("dbo.Users");
            DropTable("dbo.EventComments");
            DropTable("dbo.Events");
            DropTable("dbo.Colours");
            DropTable("dbo.Calendars");
        }
    }
}
