namespace Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EarsContext : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Callout",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Crew = c.Guid(nullable: false),
                        Route = c.String(nullable: false),
                        Location = c.Geography(),
                        LastSignal = c.DateTime(nullable: false),
                        IsFinished = c.Boolean(nullable: false),
                        LastBroadcast = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Crew", t => t.Crew, cascadeDelete: true)
                .Index(t => t.Crew);
            
            CreateTable(
                "dbo.Crew",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ApplicationId = c.String(nullable: false, maxLength: 256),
                        CreatedOn = c.DateTime(nullable: false),
                        Location = c.Geography(),
                        LastSeenOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GcmUserId = c.String(nullable: false, maxLength: 256),
                        CreatedOn = c.DateTime(nullable: false),
                        Location = c.Geography(),
                        LastSeenOn = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Callout", "Crew", "dbo.Crew");
            DropIndex("dbo.Callout", new[] { "Crew" });
            DropTable("dbo.Users");
            DropTable("dbo.Crew");
            DropTable("dbo.Callout");
        }
    }
}
