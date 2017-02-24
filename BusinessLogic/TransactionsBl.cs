using System;
using System.Linq;
using Common;
using DataAccess;

namespace BusinessLogic
{
    public class TransactionsBl
    {
        public IQueryable<Transaction> GeTransactionsForAccount(long accountNo)
        {
            return new TransactionsRepo().GeTransactionsForAccount(accountNo);
        }
        public IQueryable<Transaction> GeTransactionsForAccountDates(long accountNo, DateTime startDate , DateTime endDate)
        {
            return new TransactionsRepo().GeTransactionsForAccountDates(accountNo,startDate,endDate);
        }

    }
}
