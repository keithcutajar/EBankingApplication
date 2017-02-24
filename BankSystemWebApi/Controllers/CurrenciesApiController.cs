using System.Web.Http;
using System.Web.Http.Cors;
using BusinessLogic;

namespace BankSystemWebApi.Controllers
{
    [EnableCors("*", "*", "*")]
    public class CurrenciesApiController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetCurrencies() => Ok(new CurrenciesBl().GetCurrencies());
    }
}
