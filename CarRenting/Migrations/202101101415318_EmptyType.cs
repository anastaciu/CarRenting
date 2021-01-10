namespace CarRenting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmptyType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CarTypes", "Type", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CarTypes", "Type", c => c.String());
        }
    }
}
