namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adddivisionmodel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Division",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DivisionName = c.String(),
                        IsDelete = c.Boolean(nullable: false),
                        CreateDate = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdateDate = c.DateTimeOffset(precision: 7),
                        DeleteDate = c.DateTimeOffset(precision: 7),
                        DepartmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Department", t => t.DepartmentId, cascadeDelete: true)
                .Index(t => t.DepartmentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Division", "DepartmentId", "dbo.Department");
            DropIndex("dbo.Division", new[] { "DepartmentId" });
            DropTable("dbo.Division");
        }
    }
}
