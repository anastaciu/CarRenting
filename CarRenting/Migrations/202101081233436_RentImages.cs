namespace CarRenting.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class RentImages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DamageImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RentId = c.Int(nullable: false),
                        ImagePath = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Rents", t => t.RentId, cascadeDelete: true)
                .Index(t => t.RentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DamageImages", "RentId", "dbo.Rents");
            DropIndex("dbo.DamageImages", new[] { "RentId" });
            DropTable("dbo.DamageImages");
        }
    }
}
