namespace CarRenting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class checksrentsupdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CheckLists", "Tires", c => c.Boolean(nullable: false));
            AddColumn("dbo.CheckLists", "Body", c => c.Boolean(nullable: false));
            AddColumn("dbo.CheckLists", "Mechanics", c => c.Boolean(nullable: false));
            AddColumn("dbo.CheckLists", "Interior", c => c.Boolean(nullable: false));
            AddColumn("dbo.CheckLists", "UnderBody", c => c.Boolean(nullable: false));
            AddColumn("dbo.CheckLists", "Windows", c => c.Boolean(nullable: false));
            AddColumn("dbo.CheckLists", "ImagePath", c => c.String());
            DropColumn("dbo.CheckLists", "Description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CheckLists", "Description", c => c.String());
            DropColumn("dbo.CheckLists", "ImagePath");
            DropColumn("dbo.CheckLists", "Windows");
            DropColumn("dbo.CheckLists", "UnderBody");
            DropColumn("dbo.CheckLists", "Interior");
            DropColumn("dbo.CheckLists", "Mechanics");
            DropColumn("dbo.CheckLists", "Body");
            DropColumn("dbo.CheckLists", "Tires");
        }
    }
}
