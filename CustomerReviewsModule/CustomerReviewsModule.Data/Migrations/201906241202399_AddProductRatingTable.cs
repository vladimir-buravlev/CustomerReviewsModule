namespace CustomerReviewsModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProductRatingTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductRating",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ProductId = c.String(nullable: false, maxLength: 128),
                        Rating = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ProductRating");
        }
    }
}
