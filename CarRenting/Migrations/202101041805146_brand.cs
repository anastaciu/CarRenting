namespace CarRenting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class brand : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cars", "Brand", c => c.String());
            DropColumn("dbo.Cars", "Marca");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cars", "Marca", c => c.String());
            DropColumn("dbo.Cars", "Brand");
        }
    }
}
