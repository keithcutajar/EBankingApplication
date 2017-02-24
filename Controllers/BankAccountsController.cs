using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using BankSystemWebApp.Handlers;
using Newtonsoft.Json;

namespace BankSystemWebApp.Controllers
{
    public sealed class BankAccountsController : Controller
    {
        private readonly HttpClient _client;
        public new const string Url = "http://localhost:7471/api/bankAccountsApi/";
        private static string _errorMsg;

        public BankAccountsController()
        {
            var requestDelagatingHandler = new RequestDelagatingHandler();
            _client = HttpClientFactory.Create(requestDelagatingHandler);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Accounts()
        {
            var responseMessage = await _client.GetAsync(Url + "GetBalancesUser/" + "?username=" + User.Identity.Name);
            if (!responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }
            var responseData = responseMessage.Content.ReadAsStringAsync().Result;

            var balances = JsonConvert.DeserializeObject(responseData);
            return View(balances);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> TransferFunds()
        {
            var responseMessage = await _client.GetAsync(Url + "GetSavingsBankAccount/" + "?username=" + User.Identity.Name);
             
            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
            var savingAccounts = JsonConvert.DeserializeObject(responseData);

            ViewBag.Error = _errorMsg;
            _errorMsg = null;

            return View(savingAccounts);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> TransferFunds(long saAccountNumber, long daAccountNumber = 0, decimal amount = 0)
        {
            var transferModel = new
            {
                SaAccountNumber = saAccountNumber,
                DaAccountNumber = daAccountNumber, 
                Amount = amount,
                Username = User.Identity.Name
            };

            var responseMessage = await _client.PostAsJsonAsync("http://localhost:7471/api/TransactionsApi/" + "TransferFunds", transferModel);

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }

            _errorMsg  = responseMessage.Content.ReadAsStringAsync().Result;
            return RedirectToAction("TransferFunds", "BankAccounts");
        }

        [Authorize]
        [HttpGet]
        public ActionResult CreateAccount()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateAccount(string currencyFk,string accountTypeFk, int durationMonth, string friendlyName, long withdrawnAccountNumber, decimal amountWhitdrawn  )
        {
            var username = HttpContext.User.Identity.Name;
            var createAccountModel = new
            {
                Currency_Fk = currencyFk,
                AccountType_Fk = accountTypeFk,
                DurationMonth = durationMonth,
                FriendlyName = friendlyName,
                WithdrawnAccountNo = withdrawnAccountNumber,
                AmountWithdrawn = amountWhitdrawn,
                Username_Fk = username
            };

            var responseMessage = await _client.PostAsJsonAsync(Url + "OpenAccount", createAccountModel);

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = responseMessage.Content.ReadAsStringAsync().Result;
            return View();
        }
       
    }
}
