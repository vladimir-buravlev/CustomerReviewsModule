using System;
using CustomerReviewsModule.Core.Models;
using CustomerReviewsModule.Core.Services;
using CustomerReviewsModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace CustomerReviewsModule.Data.Services
{
    public class ProductRatingService : ServiceBase, IProductRatingService
    {
        private readonly Func<ICustomerReviewRepository> _repositoryFactory;

        public ProductRatingService(Func<ICustomerReviewRepository> repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public ProductRating GetByProductId(string productId)
        {
            if (String.IsNullOrEmpty(productId))
                throw new ArgumentNullException(nameof(productId));

            using (var repository = _repositoryFactory())
            {
                return repository.GetByProductId(productId).ToModel(AbstractTypeFactory<ProductRating>.TryCreateInstance());
            }
        }
    }
}
