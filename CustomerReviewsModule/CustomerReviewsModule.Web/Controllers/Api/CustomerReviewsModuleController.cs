using System.Web.Http;

namespace CustomerReviewsModule.Web.Controllers.Api
{
    [RoutePrefix("api/CustomerReviewsModule")]
    public class CustomerReviewsModuleController : ApiController
    {
        // GET: api/CustomerReviewsModule
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(new { result = "Hello world!" });
        }
    }
}
