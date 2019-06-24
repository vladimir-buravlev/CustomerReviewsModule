using VirtoCommerce.Platform.Core.Common;

namespace CustomerReviewsModule.Core.Models
{
    public class ProductRating : AuditableEntity
    {
        public string ProductId { get; set; }
        public decimal Rating { get; set; }
    }
}
