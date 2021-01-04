namespace CarRenting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cartypechecks : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Checks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CheckCarTypes",
                c => new
                    {
                        Check_Id = c.Int(nullable: false),
                        CarType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Check_Id, t.CarType_Id })
                .ForeignKey("dbo.Checks", t => t.Check_Id, cascadeDelete: true)
                .ForeignKey("dbo.CarTypes", t => t.CarType_Id, cascadeDelete: true)
                .Index(t => t.Check_Id)
                .Index(t => t.CarType_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CheckCarTypes", "CarType_Id", "dbo.CarTypes");
            DropForeignKey("dbo.CheckCarTypes", "Check_Id", "dbo.Checks");
            DropIndex("dbo.CheckCarTypes", new[] { "CarType_Id" });
            DropIndex("dbo.CheckCarTypes", new[] { "Check_Id" });
            DropTable("dbo.CheckCarTypes");
            DropTable("dbo.Checks");
        }
    }
}
