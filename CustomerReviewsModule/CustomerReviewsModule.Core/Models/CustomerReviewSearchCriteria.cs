using VirtoCommerce.Domain.Commerce.Model.Search;

namespace CustomerReviewsModule.Core.Models
{
    public class CustomerReviewSearchCriteria : SearchCriteriaBase
    {
        public string[] ProductIds { get; set; }
        public bool? IsActive { get; set; }
    }
}
