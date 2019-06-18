using CustomerReviewsModule.Core.Models;

namespace CustomerReviewsModule.Core.Services
{
    public interface ICustomerReviewService
    {
        CustomerReview[] GetByIds(string[] ids);

        void SaveCustomerReviews(CustomerReview[] items);

        void DeleteCustomerReviews(string[] ids);
    }
}
