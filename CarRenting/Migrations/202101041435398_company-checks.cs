namespace CarRenting.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class companychecks : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.CheckCarTypes", newName: "CarTypeChecks");
            DropPrimaryKey("dbo.CarTypeChecks");
            AddColumn("dbo.Checks", "CompanyId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.CarTypeChecks", new[] { "CarType_Id", "Check_Id" });
            CreateIndex("dbo.Checks", "CompanyId");
            AddForeignKey("dbo.Checks", "CompanyId", "dbo.Companies", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Checks", "CompanyId", "dbo.Companies");
            DropIndex("dbo.Checks", new[] { "CompanyId" });
            DropPrimaryKey("dbo.CarTypeChecks");
            DropColumn("dbo.Checks", "CompanyId");
            AddPrimaryKey("dbo.CarTypeChecks", new[] { "Check_Id", "CarType_Id" });
            RenameTable(name: "dbo.CarTypeChecks", newName: "CheckCarTypes");
        }
    }
}
