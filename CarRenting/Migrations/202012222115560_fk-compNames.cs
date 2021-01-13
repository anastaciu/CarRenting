namespace CarRenting.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class fkcompNames : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "CompanyName", c => c.String());
            AddColumn("dbo.Companies", "CompanyPhoneNumber", c => c.String());
            AddColumn("dbo.Companies", "CompanyCellNumber", c => c.String());
            DropColumn("dbo.Companies", "Name");
            DropColumn("dbo.Companies", "PhoneNumber");
            DropColumn("dbo.Companies", "CellNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Companies", "CellNumber", c => c.String());
            AddColumn("dbo.Companies", "PhoneNumber", c => c.String());
            AddColumn("dbo.Companies", "Name", c => c.String());
            DropColumn("dbo.Companies", "CompanyCellNumber");
            DropColumn("dbo.Companies", "CompanyPhoneNumber");
            DropColumn("dbo.Companies", "CompanyName");
        }
    }
}
