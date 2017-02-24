using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using BankSystemWebApi.Filters;
using BusinessLogic;
using Common.Models;

namespace BankSystemWebApi.Controllers
{
    [EnableCors("*", "*", "*")]
    public class TransactionsApiController : ApiController
    {
        [HmacAuthentication]
        public IHttpActionResult GetTransactionsAccount(long accountNo)
        {
            var transactionLst = new TransactionsBl().GeTransactionsForAccount(accountNo).ToList();
            var transactions = from t in transactionLst
                               select new
                               {
                                   t.AccountNo_Fk,
                                   t.Currency_Fk,
                                   t.Amount,
                                   t.Details,
                                   TransactionTimestamp = t.Transaction_Timestamp
                               };

            return Ok(transactions);
        }

        [HmacAuthentication]
        public IHttpActionResult GetTransactionsAccount(long accountNo, DateTime startDate, DateTime endDate)
        {
            var transactionLst = new TransactionsBl().GeTransactionsForAccountDates(accountNo, startDate, endDate).ToList();
            var transactions = from t in transactionLst
                               select new
                               {
                                   t.AccountNo_Fk,
                                   t.Currency_Fk,
                                   t.Amount,
                                   t.Details,
                                   TransactionTimestamp = t.Transaction_Timestamp
                               };

            return Ok(transactions);
        }

        [HmacAuthentication]
        [HttpPost]
        public IHttpActionResult TransferFunds(TransferFundsModel transferFundsModel)
        {
            var message = new AccountsBl().TransferFunds(transferFundsModel);
            if (message == "Success")
            {
                return Ok(transferFundsModel);
            }
            return BadRequest(message);
        }

    }
}
