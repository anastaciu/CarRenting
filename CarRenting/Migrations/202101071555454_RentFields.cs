namespace CarRenting.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class RentFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rents", "IsConfirmed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Rents", "IsDelivered", c => c.Boolean(nullable: false));
            AddColumn("dbo.Rents", "IsReceived", c => c.Boolean(nullable: false));
            AddColumn("dbo.Rents", "IsChecked", c => c.Boolean(nullable: false));
            AddColumn("dbo.Rents", "DeliveryFaults", c => c.String());
            AddColumn("dbo.Rents", "KmsIn", c => c.Int(nullable: false));
            AddColumn("dbo.Rents", "KmsOut", c => c.Int(nullable: false));
            AddColumn("dbo.Rents", "IsDamaged", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rents", "IsDamaged");
            DropColumn("dbo.Rents", "KmsOut");
            DropColumn("dbo.Rents", "KmsIn");
            DropColumn("dbo.Rents", "DeliveryFaults");
            DropColumn("dbo.Rents", "IsChecked");
            DropColumn("dbo.Rents", "IsReceived");
            DropColumn("dbo.Rents", "IsDelivered");
            DropColumn("dbo.Rents", "IsConfirmed");
        }
    }
}
