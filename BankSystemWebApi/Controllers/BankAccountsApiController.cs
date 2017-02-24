using System.Web.Http;
using System.Web.Http.Cors;
using BankSystemWebApi.Filters;
using BusinessLogic;
using Common.Models;

namespace BankSystemWebApi.Controllers
{
    [EnableCors("*", "*", "*")] 
    public class BankAccountsApiController : ApiController
    {
        public IHttpActionResult GetBankAccountTypes()
        {
            return Ok(new AccountsBl().GetBankAccountTypes());
        }
        
        [HmacAuthentication]
        public IHttpActionResult GetSavingsBankAccount(string username)
        {
            return Ok(new AccountsBl().GetSavingsAccountsForUser(username));
        }

        [HmacAuthentication]
        public IHttpActionResult GetBalancesUser(string username)
        {
            return Ok(new AccountsBl().GetBalancesForUser(username));
        }
        
        [HmacAuthentication] 
        [HttpPost]
        public IHttpActionResult OpenAccount(NewBankAccountModel newBankAccount)
        {
            var message =  new AccountsBl().AddBankAccount(newBankAccount);
            if (message == "Success")
            {
                return Ok(newBankAccount);
            }
            return BadRequest(message);
        }
    }
}