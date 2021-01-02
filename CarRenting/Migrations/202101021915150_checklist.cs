namespace CarRenting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class checklist : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ChecksRents", "Checks_Id", "dbo.Checks");
            DropForeignKey("dbo.ChecksRents", "Rent_Id", "dbo.Rents");
            DropIndex("dbo.ChecksRents", new[] { "Checks_Id" });
            DropIndex("dbo.ChecksRents", new[] { "Rent_Id" });
            CreateTable(
                "dbo.CheckLists",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Rents", t => t.Id)
                .Index(t => t.Id);
            
            DropTable("dbo.Checks");
            DropTable("dbo.ChecksRents");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ChecksRents",
                c => new
                    {
                        Checks_Id = c.Int(nullable: false),
                        Rent_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Checks_Id, t.Rent_Id });
            
            CreateTable(
                "dbo.Checks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.CheckLists", "Id", "dbo.Rents");
            DropIndex("dbo.CheckLists", new[] { "Id" });
            DropTable("dbo.CheckLists");
            CreateIndex("dbo.ChecksRents", "Rent_Id");
            CreateIndex("dbo.ChecksRents", "Checks_Id");
            AddForeignKey("dbo.ChecksRents", "Rent_Id", "dbo.Rents", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ChecksRents", "Checks_Id", "dbo.Checks", "Id", cascadeDelete: true);
        }
    }
}
