namespace CarRenting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class resetfks : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Cars", "Company_Id", "dbo.Companies");
            DropForeignKey("dbo.Employees", "Company_Id", "dbo.Companies");
            DropForeignKey("dbo.Rents", "Car_Id", "dbo.Cars");
            DropIndex("dbo.Cars", new[] { "Company_Id" });
            DropIndex("dbo.Rents", new[] { "Car_Id" });
            DropIndex("dbo.Employees", new[] { "Company_Id" });
            DropColumn("dbo.Cars", "Company_Id");
            DropColumn("dbo.Rents", "Car_Id");
            DropColumn("dbo.Employees", "Company_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employees", "Company_Id", c => c.Int(nullable: false));
            AddColumn("dbo.Rents", "Car_Id", c => c.Int());
            AddColumn("dbo.Cars", "Company_Id", c => c.Int());
            CreateIndex("dbo.Employees", "Company_Id");
            CreateIndex("dbo.Rents", "Car_Id");
            CreateIndex("dbo.Cars", "Company_Id");
            AddForeignKey("dbo.Rents", "Car_Id", "dbo.Cars", "Id");
            AddForeignKey("dbo.Employees", "Company_Id", "dbo.Companies", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Cars", "Company_Id", "dbo.Companies", "Id");
        }
    }
}
