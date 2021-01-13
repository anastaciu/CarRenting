namespace CarRenting.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class reqs : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Employees", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Employees", "Company_Id", "dbo.Companies");
            DropIndex("dbo.Employees", new[] { "ApplicationUserId" });
            DropIndex("dbo.Employees", new[] { "Company_Id" });
            RenameColumn(table: "dbo.Rents", name: "User_Id", newName: "ApplicationUserId");
            RenameIndex(table: "dbo.Rents", name: "IX_User_Id", newName: "IX_ApplicationUserId");
            AlterColumn("dbo.Employees", "ApplicationUserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Employees", "Company_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Employees", "ApplicationUserId");
            CreateIndex("dbo.Employees", "Company_Id");
            AddForeignKey("dbo.Employees", "ApplicationUserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Employees", "Company_Id", "dbo.Companies", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employees", "Company_Id", "dbo.Companies");
            DropForeignKey("dbo.Employees", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Employees", new[] { "Company_Id" });
            DropIndex("dbo.Employees", new[] { "ApplicationUserId" });
            AlterColumn("dbo.Employees", "Company_Id", c => c.Int());
            AlterColumn("dbo.Employees", "ApplicationUserId", c => c.String(maxLength: 128));
            RenameIndex(table: "dbo.Rents", name: "IX_ApplicationUserId", newName: "IX_User_Id");
            RenameColumn(table: "dbo.Rents", name: "ApplicationUserId", newName: "User_Id");
            CreateIndex("dbo.Employees", "Company_Id");
            CreateIndex("dbo.Employees", "ApplicationUserId");
            AddForeignKey("dbo.Employees", "Company_Id", "dbo.Companies", "Id");
            AddForeignKey("dbo.Employees", "ApplicationUserId", "dbo.AspNetUsers", "Id");
        }
    }
}
