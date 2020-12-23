namespace CarRenting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cmpprop : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "CellNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Companies", "CellNumber");
        }
    }
}
