using CustomerReviewsModule.Core.Models;

namespace CustomerReviewsModule.Core.Services
{
    public interface IProductRatingService
    {
        ProductRating GetByProductId(string productId);
    }
}
