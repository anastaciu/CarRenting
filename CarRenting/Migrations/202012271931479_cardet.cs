namespace CarRenting.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class cardet : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cars", "Marca", c => c.String());
            AddColumn("dbo.Cars", "Type", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cars", "Type");
            DropColumn("dbo.Cars", "Marca");
        }
    }
}
