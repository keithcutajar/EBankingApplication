using System.Web.Http;
using System.Web.Http.Cors;
using BusinessLogic;

namespace BankSystemWebApi.Controllers
{
    [EnableCors("*", "*", "*")]
    public class InterestRatesApiController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetInterestRates() => Ok(new InterestRatesBl().InterestRates);
    }
}
