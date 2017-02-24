using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace DataAccess
{
    public class TransactionsRepo : ConnectionClass
    {
        public TransactionsRepo() : base()
        { }

        public IQueryable<Transaction> GeTransactionsForAccount(long accountNo)
        {
            return Entity.Transactions.Where(t => t.AccountNo_Fk == accountNo);
        }

        public IQueryable<Transaction> GeTransactionsForAccountDates(long accountNo, DateTime startDate, DateTime endDate)
        {
            return Entity.Transactions.Where(t => t.AccountNo_Fk == accountNo && DbFunctions.TruncateTime(t.Transaction_Timestamp) >= DbFunctions.TruncateTime(startDate) && DbFunctions.TruncateTime(t.Transaction_Timestamp) <= DbFunctions.TruncateTime(endDate));
        }

        public void AddTransaction(Transaction transaction)
        {
            Entity.Transactions.Add(transaction);
            Entity.SaveChanges();
        }
    }
}
