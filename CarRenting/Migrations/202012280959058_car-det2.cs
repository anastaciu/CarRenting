namespace CarRenting.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class cardet2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cars", "Price", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cars", "Price");
        }
    }
}
