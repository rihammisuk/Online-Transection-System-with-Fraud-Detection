namespace Demo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RiskIP_ClassAdd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RiskIPs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        HighRiskIP = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RiskIPs");
        }
    }
}
