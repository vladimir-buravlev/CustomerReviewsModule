using System;
using CustomerReviewsModule.Core.Models;
using CustomerReviewsModule.Core.Services;
using CustomerReviewsModule.Data.Repositories;
using CustomerReviewsModule.Data.Services;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using Xunit;

namespace CustomerReviewsModule.Tests
{
    public class CustomerReviewsModuleTest
    {
        public CustomerReviewsModuleTest()
        {
        }

        [Fact]
        public void GetByIds_NonExistingId_ReturnsEmpty()
        {
            string nonExistingId = "nonExistingId";
            var getByIdsResult = CustomerReviewService.GetByIds(new[] { nonExistingId });

            Assert.NotNull(getByIdsResult);
            Assert.Empty(getByIdsResult);
        }

        [Fact]
        public void GetByIds_ExistingId_ReturnsItemWithSameId()
        {
            string testReviewId = "testReviewId";
            var getByIdsResult = CustomerReviewService.GetByIds(new[] { testReviewId });

            Assert.Single(getByIdsResult);
            Assert.Equal(testReviewId, getByIdsResult[0].Id);
        }

        [Fact]
        public void SaveCustomerReviews_Null_ThrowsArgumentNullException()
        {
            Action actual = () => CustomerReviewService.SaveCustomerReviews(null);
            Assert.Throws<ArgumentNullException>(actual);
        }

        [Fact]
        public void SaveCustomerReviews_ExistingItem_ReturnsSameItem()
        {
            string testReviewId = "testReviewId";
            string testProductId = "testProductId";
            var item = new CustomerReview
            {
                Id = testReviewId,
                ProductId = testProductId,
                CreatedDate = DateTime.Now,
                CreatedBy = "xUnit tests",
                AuthorNickname = "xUnit",
                Content = "xUnit likes this"
            };

            CustomerReviewService.SaveCustomerReviews(new[] { item });
            var getByIdsResult = CustomerReviewService.GetByIds(new[] { testReviewId });

            Assert.Single(getByIdsResult);
        }

        private ICustomerReviewService CustomerReviewService
        {
            get
            {
                return new CustomerReviewService(GetRepository);
            }
        }

        private ICustomerReviewRepository GetRepository()
        {
            string connectionString = "VirtoCommerce2";
            var repository = new CustomerReviewRepository(connectionString, new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor(null));
            return repository;
        }
    }
}
