namespace CarRenting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class carimages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CarImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CarId = c.Int(nullable: false),
                        ImagePath = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cars", t => t.CarId, cascadeDelete: true)
                .Index(t => t.CarId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CarImages", "CarId", "dbo.Cars");
            DropIndex("dbo.CarImages", new[] { "CarId" });
            DropTable("dbo.CarImages");
        }
    }
}
