using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CustomerReviewsModule.Data.Models;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;

namespace CustomerReviewsModule.Data.Repositories
{
    public class CustomerReviewRepository : EFRepositoryBase, ICustomerReviewRepository
    {
        private readonly ISettingsManager _settingsManager;
        public CustomerReviewRepository()
        {
        }

        public CustomerReviewRepository(string nameOrConnectionString, ISettingsManager settingsManager, params IInterceptor[] interceptors)
            : base(nameOrConnectionString, null, interceptors)
        {
            Configuration.LazyLoadingEnabled = false;
            _settingsManager = settingsManager;
        }

        public IQueryable<CustomerReviewEntity> CustomerReviews => GetAsQueryable<CustomerReviewEntity>();
        public IQueryable<ProductRatingEntity> ProductRatings => GetAsQueryable<ProductRatingEntity>();

        public CustomerReviewEntity[] GetByIds(string[] ids)
        {
            return CustomerReviews.Where(x => ids.Contains(x.Id)).ToArray();
        }

        public void DeleteCustomerReviews(string[] ids)
        {
            List<string> productIds = new List<string>();
            var items = GetByIds(ids);
            foreach (var item in items)
            {
                productIds.Add(item.ProductId);
                Remove(item);
            }
            SaveChanges();
            IEnumerable<string> distinctProductIds = productIds.Distinct();
            foreach (string productId in distinctProductIds)
                RecalcRatingForProduct(productId);
        }

        public ProductRatingEntity GetByProductId(string productId)
        {
            return ProductRatings.Where(x => x.ProductId.Equals(productId)).FirstOrDefault() ?? new ProductRatingEntity();
        }

        public void RecalcRatingForProduct(string productId)
        {

            bool useWeightedMechanism = _settingsManager?.GetValue("CustomerReviewsModule.General.UseWeightedRatingMechanism", false) ?? false;
            decimal newRating;
            var productReviewRatings = CustomerReviews.Where(x => x.ProductId.Equals(productId)).Select(x => x.Rating).ToArray();
            if (useWeightedMechanism)
            {
                int summaryRating = 0;
                foreach (var productReviewRating in productReviewRatings)
                    summaryRating += productReviewRating;
                newRating = productReviewRatings.Length > 0 ? (decimal)summaryRating / (decimal)productReviewRatings.Length : 0;
            }
            else
            {
                int uniqueRatingValuesSum = productReviewRatings.Distinct().Sum();
                int uniqueRatingValuesCount = productReviewRatings.Distinct().Count();
                newRating = uniqueRatingValuesCount > 0 ? (decimal)uniqueRatingValuesSum / (decimal)uniqueRatingValuesCount : 0;
            }

            var productRating = ProductRatings.Where(x => x.ProductId.Equals(productId)).FirstOrDefault() ?? new ProductRatingEntity() { ProductId = productId };
            productRating.Rating = newRating;

            AddOrUpdate(productRating);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerReviewEntity>().ToTable("CustomerReview").HasKey(x => x.Id).Property(x => x.Id);
            modelBuilder.Entity<ProductRatingEntity>().ToTable("ProductRating").HasKey(x => x.Id).Property(x => x.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
