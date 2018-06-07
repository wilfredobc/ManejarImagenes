namespace ManageImage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgregandoModeloImages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImageTitle = c.String(nullable: false, maxLength: 250),
                        ImagePath = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Images");
        }
    }
}
