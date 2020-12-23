namespace CarRenting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class compprop : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.Companies", "CompanyName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Companies", "CompanyName", c => c.String());
            DropColumn("dbo.Companies", "Email");
        }
    }
}
