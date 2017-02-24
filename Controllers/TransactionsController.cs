using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using BankSystemWebApp.Filters;
using BankSystemWebApp.Handlers;
using BankSystemWebApp.Utilities;
using Newtonsoft.Json;

namespace BankSystemWebApp.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly HttpClient _client;
        public new const string Url = "http://localhost:7471/api/TransactionsApi/";

        public TransactionsController()
        {
            var requestDelagatingHandler = new RequestDelagatingHandler();
            _client = HttpClientFactory.Create(requestDelagatingHandler);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [Authorize] 
        [HttpGet]
        [EncryptedActionParameter]
        public async Task<ActionResult> List(long accountNumber = 0)
        {
            var responseMessage = await _client.GetAsync(Url + "GetTransactionsAccount/" + "?accountNo=" + accountNumber);
            if (!responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }
            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
            var transactions = JsonConvert.DeserializeObject(responseData);

            ViewBag.Acc = SecurityEncDec.Enc(accountNumber.ToString());

            return View(transactions);
        }

        [Authorize]
        [HttpGet]
        public async Task<PartialViewResult> PartialList(string dateFrom, string endDate, string accountNo)
        {
            var df = Convert.ToDateTime(dateFrom);
            var dt = Convert.ToDateTime(endDate);

            var accNo = Convert.ToInt64(SecurityEncDec.Dec(accountNo));

            var responseMessage = await _client.GetAsync(Url + "GetTransactionsAccount/" + "?accountNo=" + accNo + "&startDate=" + dt + "&endDate=" + df);
    
            var responseData = responseMessage.Content.ReadAsStringAsync().Result;

            var transactions = JsonConvert.DeserializeObject(responseData);

            return PartialView("ListPartial", transactions);
        }
        
    }
}