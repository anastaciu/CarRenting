namespace CarRenting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class contribuinte : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "NiF", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Companies", "NiF");
        }
    }
}
