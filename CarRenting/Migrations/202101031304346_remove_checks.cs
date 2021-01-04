namespace CarRenting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class remove_checks : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Checks", "Car_Id", "dbo.Cars");
            DropIndex("dbo.Checks", new[] { "Car_Id" });
            DropTable("dbo.Checks");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Checks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Car_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Checks", "Car_Id");
            AddForeignKey("dbo.Checks", "Car_Id", "dbo.Cars", "Id");
        }
    }
}
