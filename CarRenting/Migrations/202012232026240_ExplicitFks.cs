namespace CarRenting.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class ExplicitFks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cars", "CompanyId", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "CompanyId", c => c.Int(nullable: false));
            AddColumn("dbo.Rents", "CarId", c => c.Int(nullable: false));
            CreateIndex("dbo.Cars", "CompanyId");
            CreateIndex("dbo.Employees", "CompanyId");
            CreateIndex("dbo.Rents", "CarId");
            AddForeignKey("dbo.Rents", "CarId", "dbo.Cars", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Employees", "CompanyId", "dbo.Companies", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Cars", "CompanyId", "dbo.Companies", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cars", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.Employees", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.Rents", "CarId", "dbo.Cars");
            DropIndex("dbo.Rents", new[] { "CarId" });
            DropIndex("dbo.Employees", new[] { "CompanyId" });
            DropIndex("dbo.Cars", new[] { "CompanyId" });
            DropColumn("dbo.Rents", "CarId");
            DropColumn("dbo.Employees", "CompanyId");
            DropColumn("dbo.Cars", "CompanyId");
        }
    }
}
