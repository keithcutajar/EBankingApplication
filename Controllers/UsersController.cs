using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;

// TODO clean stuff redundant code everywhere
namespace BankSystemWebApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly HttpClient _client;
        public new const string Url = "http://localhost:7471/api/UsersApi/";

        public UsersController()
        {
            _client = new HttpClient {BaseAddress = new Uri(Url)};
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SignIn(string username, string code)
        {
            try
            {
                var loginModel = new
                {
                    Username = username, Password = code
                };

                var responseMessage = await _client.PostAsJsonAsync(Url + "LoginUser", loginModel);

                if (responseMessage.IsSuccessStatusCode)
                {
                    FormsAuthentication.SetAuthCookie(username, true);
                    return RedirectToAction("Index", "Home");
                }

                ViewBag.Error = "Incorrect Credentials";
                return View();
            }
            catch
            {
                Session["msg"] = "Something Went Wrong Try Again Later.";
                return View();
            }
        }

        [Authorize]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

    }
}