namespace CarRenting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cartypeschecks : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Checks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Car_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cars", t => t.Car_Id)
                .Index(t => t.Car_Id);
            
            CreateTable(
                "dbo.CarTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Cars", "FuelLevel", c => c.String(nullable: false));
            AddColumn("dbo.Cars", "TypeId", c => c.Int(nullable: false));
            AddColumn("dbo.Cars", "Kms", c => c.Int(nullable: false));
            CreateIndex("dbo.Cars", "TypeId");
            AddForeignKey("dbo.Cars", "TypeId", "dbo.CarTypes", "Id", cascadeDelete: true);
            DropColumn("dbo.Cars", "Type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cars", "Type", c => c.String());
            DropForeignKey("dbo.Cars", "TypeId", "dbo.CarTypes");
            DropForeignKey("dbo.Checks", "Car_Id", "dbo.Cars");
            DropIndex("dbo.Checks", new[] { "Car_Id" });
            DropIndex("dbo.Cars", new[] { "TypeId" });
            DropColumn("dbo.Cars", "Kms");
            DropColumn("dbo.Cars", "TypeId");
            DropColumn("dbo.Cars", "FuelLevel");
            DropTable("dbo.CarTypes");
            DropTable("dbo.Checks");
        }
    }
}
