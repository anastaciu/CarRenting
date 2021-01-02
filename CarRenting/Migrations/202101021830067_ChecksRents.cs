namespace CarRenting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChecksRents : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Checks", "Car_Id", "dbo.Cars");
            DropIndex("dbo.Checks", new[] { "Car_Id" });
            CreateTable(
                "dbo.ChecksRents",
                c => new
                    {
                        Checks_Id = c.Int(nullable: false),
                        Rent_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Checks_Id, t.Rent_Id })
                .ForeignKey("dbo.Checks", t => t.Checks_Id, cascadeDelete: true)
                .ForeignKey("dbo.Rents", t => t.Rent_Id, cascadeDelete: true)
                .Index(t => t.Checks_Id)
                .Index(t => t.Rent_Id);
            
            AddColumn("dbo.Rents", "IsConfirmed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Rents", "IsDelivered", c => c.Boolean(nullable: false));
            AddColumn("dbo.Rents", "IsReceived", c => c.Boolean(nullable: false));
            AddColumn("dbo.Rents", "IsCarDamaged", c => c.Boolean(nullable: false));
            DropColumn("dbo.Checks", "Car_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Checks", "Car_Id", c => c.Int());
            DropForeignKey("dbo.ChecksRents", "Rent_Id", "dbo.Rents");
            DropForeignKey("dbo.ChecksRents", "Checks_Id", "dbo.Checks");
            DropIndex("dbo.ChecksRents", new[] { "Rent_Id" });
            DropIndex("dbo.ChecksRents", new[] { "Checks_Id" });
            DropColumn("dbo.Rents", "IsCarDamaged");
            DropColumn("dbo.Rents", "IsReceived");
            DropColumn("dbo.Rents", "IsDelivered");
            DropColumn("dbo.Rents", "IsConfirmed");
            DropTable("dbo.ChecksRents");
            CreateIndex("dbo.Checks", "Car_Id");
            AddForeignKey("dbo.Checks", "Car_Id", "dbo.Cars", "Id");
        }
    }
}
