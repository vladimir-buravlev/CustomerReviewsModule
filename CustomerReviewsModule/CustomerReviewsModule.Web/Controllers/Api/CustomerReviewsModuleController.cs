using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using CustomerReviewsModule.Core.Models;
using CustomerReviewsModule.Core.Services;
using CustomerReviewsModule.Web.Security;
using VirtoCommerce.Domain.Commerce.Model.Search;
using VirtoCommerce.Platform.Core.Web.Security;

namespace CustomerReviewsModule.Web.Controllers.Api
{
    [RoutePrefix("api/CustomerReviewsModule")]
    public class CustomerReviewsModuleController : ApiController
    {
        private readonly ICustomerReviewSearchService _customerReviewSearchService;
        private readonly ICustomerReviewService _customerReviewService;

        public CustomerReviewsModuleController()
        {
        }

        public CustomerReviewsModuleController(ICustomerReviewSearchService customerReviewSearchService, ICustomerReviewService customerReviewService)
        {
            _customerReviewSearchService = customerReviewSearchService;
            _customerReviewService = customerReviewService;
        }

        /// <summary>
        /// Return product Customer review search results
        /// </summary>
        [HttpPost]
        [Route("search")]
        [ResponseType(typeof(GenericSearchResult<CustomerReview>))]
        [CheckPermission(Permission = CustomerReviewsPredefinedPermissions.Read)]
        public IHttpActionResult SearchCustomerReviews(CustomerReviewSearchCriteria criteria)
        {
            var result = _customerReviewSearchService.SearchCustomerReviews(criteria);
            return Ok(result);
        }


        /// <summary>
        ///  Create new or update existing customer review
        /// </summary>
        /// <param name="customerReviews">Customer reviews</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(void))]
        [CheckPermission(Permission = CustomerReviewsPredefinedPermissions.Update)]
        public IHttpActionResult Update(CustomerReview[] customerReviews)
        {
            _customerReviewService.SaveCustomerReviews(customerReviews);
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Delete Customer Reviews by IDs
        /// </summary>
        /// <param name="ids">IDs</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("")]
        [ResponseType(typeof(void))]
        [CheckPermission(Permission = CustomerReviewsPredefinedPermissions.Delete)]
        public IHttpActionResult Delete([FromUri] string[] ids)
        {
            _customerReviewService.DeleteCustomerReviews(ids);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
