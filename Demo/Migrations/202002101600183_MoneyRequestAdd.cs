namespace Demo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoneyRequestAdd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MoneyRequests",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.String(),
                        UserName = c.String(),
                        RequestID = c.Int(nullable: false),
                        RequestDate = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MoneyRequests");
        }
    }
}
