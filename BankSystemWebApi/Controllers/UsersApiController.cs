using System.Web.Http;
using BusinessLogic;
using Common.Models;
using SecureKeyApp;

namespace BankSystemWebApi.Controllers
{
    public class UsersApiController : ApiController
    {
        [HttpPost]
        public IHttpActionResult AuthenticateUser(LoginModel user )
        {
            if (new UsersBl().AuthenticateUser(user))
            {
               return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        public IHttpActionResult LoginUser(LoginModel userLogin)
        {
            var user = new UsersBl().GetUserByUsername(userLogin.Username); 
            var secureKey = new SecureKey().GetSecureKey(user.Username.ToLower(), user.Password).ToString();
            if (userLogin.Password.Equals(secureKey))
            {
                return Ok();
            }
            return BadRequest();
        }

    }
}
