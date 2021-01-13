namespace CarRenting.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class license : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cars", "License", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cars", "License");
        }
    }
}
