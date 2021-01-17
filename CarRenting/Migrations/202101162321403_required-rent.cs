namespace CarRenting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requiredrent : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Rents", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Rents", new[] { "ApplicationUserId" });
            AlterColumn("dbo.Rents", "ApplicationUserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Rents", "ApplicationUserId");
            AddForeignKey("dbo.Rents", "ApplicationUserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rents", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Rents", new[] { "ApplicationUserId" });
            AlterColumn("dbo.Rents", "ApplicationUserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Rents", "ApplicationUserId");
            AddForeignKey("dbo.Rents", "ApplicationUserId", "dbo.AspNetUsers", "Id");
        }
    }
}
