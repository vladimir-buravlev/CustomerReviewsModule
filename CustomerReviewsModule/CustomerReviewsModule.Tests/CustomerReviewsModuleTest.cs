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
        public void GetByIds_Null_ThrowsArgumentNullException()
        {
            Action actual = () => CustomerReviewService.GetByIds(null);

            Assert.Throws<ArgumentNullException>(actual);
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
        public void SaveCustomerReviews_NonExistingItem_ReturnsNewItem()
        {
            string nonExistingReviewId = Guid.NewGuid().ToString();
            string testProductId = "testProductId";
            var item = new CustomerReview
            {
                Id = nonExistingReviewId,
                ProductId = testProductId,
                CreatedDate = DateTime.Now,
                CreatedBy = "xUnit tests",
                AuthorNickname = "xUnit",
                Content = "xUnit likes this",
                Rating = 1
            };

            CustomerReviewService.SaveCustomerReviews(new[] { item });
            var getByIdsResult = CustomerReviewService.GetByIds(new[] { nonExistingReviewId });

            Assert.Single(getByIdsResult);
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
                Content = "xUnit likes this " + DateTime.Now.ToString(),
                Rating = 1
            };

            CustomerReviewService.SaveCustomerReviews(new[] { item });
            var getByIdsResult = CustomerReviewService.GetByIds(new[] { testReviewId });

            Assert.Single(getByIdsResult);
        }

        [Fact]
        public void DeleteCustomerReviews_Null_ThrowsArgumentNullException()
        {
            Action actual = () => CustomerReviewService.DeleteCustomerReviews(null);

            Assert.Throws<ArgumentNullException>(actual);
        }

        [Fact]
        public void DeleteCustomerReviews_NonExistingItem_ReturnsEmpty()
        {
            string nonExistingId = "nonExistingId";
            CustomerReviewService.DeleteCustomerReviews(new[] { nonExistingId });
            var getByIdsResult = CustomerReviewService.GetByIds(new[] { nonExistingId });

            Assert.Empty(getByIdsResult);
        }

        [Fact]
        public void DeleteCustomerReviews_ExistingItem_ItemDeletes()
        {
            string dropReviewId = "dropReviewId";
            CreateNewReview(dropReviewId);
            CustomerReviewService.DeleteCustomerReviews(new[] { dropReviewId });
            var getByIdsResult = CustomerReviewService.GetByIds(new[] { dropReviewId });

            Assert.NotNull(getByIdsResult);
            Assert.Empty(getByIdsResult);
        }

        [Fact]
        public void SearchCustomerReviews_Null_ThrowsArgumentNullException()
        {
            Action actual = () => CustomerReviewSearchService.SearchCustomerReviews(null);

            Assert.Throws<ArgumentNullException>(actual);
        }

        [Fact]
        public void SearchCustomerReviews_NonExistingItem_ReturnsEmpty()
        {
            string nonExistingId = "nonExistingId";
            var criteria = new CustomerReviewSearchCriteria { ProductIds = new[] { nonExistingId } };
            var searchResult = CustomerReviewSearchService.SearchCustomerReviews(criteria);

            Assert.NotNull(searchResult);
            Assert.Equal(0, searchResult.TotalCount);
        }

        [Fact]
        public void SearchCustomerReviews_ExistingItem_ReturnsItem()
        {
            string testProductId = "testProductId";
            var criteria = new CustomerReviewSearchCriteria { ProductIds = new[] { testProductId } };
            var searchResult = CustomerReviewSearchService.SearchCustomerReviews(criteria);

            Assert.NotNull(searchResult);
            Assert.NotEqual(0, searchResult.TotalCount);
        }

        private void CreateNewReview(string rewiewId)
        {
            var item = new CustomerReview
            {
                Id = rewiewId,
                ProductId = "newProductId",
                CreatedDate = DateTime.Now,
                CreatedBy = "xUnit tests",
                AuthorNickname = "xUnit",
                Content = "xUnit likes this",
                Rating = 1
            };

            CustomerReviewService.SaveCustomerReviews(new[] { item });
        }

        private ICustomerReviewSearchService CustomerReviewSearchService
        {
            get
            {
                return new CustomerReviewSearchService(GetRepository, CustomerReviewService);
            }
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
            string connectionString = "VirtoCommerce2"; // is it your base?
            var repository = new CustomerReviewRepository(connectionString, new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor(null));
            return repository;
        }
    }
}
