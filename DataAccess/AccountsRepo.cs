using System;
using System.Data.Entity;
using System.Linq;
using Common;
using Common.Models;

namespace DataAccess
{
    public class AccountsRepo : ConnectionClass
    {
        public AccountsRepo() : base()
        { }

        public IQueryable<string> GetBankAccountTypes() => from a in Entity.AccountTypes select a.Name;

        public void AddBankAccount(BankAccount bankAccount)
        {
            Entity.BankAccounts.Add(bankAccount);
            Entity.SaveChanges();
        }

        public BankAccount GetAccountByAccountNo(long accountNo)
        {
            return Entity.BankAccounts.SingleOrDefault(a => a.AccountNumber == accountNo);
        }

        public BankAccount GetWhitdrawnAccount(string username, long accountNo)
        {

            return Entity.BankAccounts.SingleOrDefault(a => a.AccountNumber == accountNo && Equals(a.Username_Fk.ToLower(), username.ToLower()) && a.AccountType_Fk == "Savings");
        }

        public IQueryable<BalanceModel> GetBalancesForUser(string username)
        {
            return from a in Entity.BankAccounts
                   join aT in Entity.AccountTypes
                   on a.AccountType_Fk equals aT.Name
                   where a.Username_Fk == username
                   select new BalanceModel
                   {
                       Iban = a.Iban,
                       AccountNumber = a.AccountNumber,
                       AccountType = aT.Name,
                       CurrencyCode = a.Currency_Fk,
                       Balance = a.Balance,
                       FriendlyName = a.FriendlyName
                   };

        }

        public IQueryable<BankAccount> GetAccountsForUser(string username)
        {
            return Entity.BankAccounts.Where(u => u.Username_Fk == username);
        }

        public IQueryable<long> GetSavingsAccountsForUser(string username) 
            => from b in Entity.BankAccounts where b.Username_Fk == username && b.AccountType_Fk == "Savings" select b.AccountNumber;

        public void UpdateAccount(BankAccount account)
        {
            Entity.Entry(account).State = EntityState.Modified;
            Entity.SaveChanges();
        }

        public void Update()
        {
            Entity.SaveChanges();
        }

        public IQueryable<BankAccount> GetTodaysExpiredFixedAccounts()
        {
            var today = DateTime.Now;
            return
                Entity.BankAccounts.Where(
                    ba =>
                        ba.Expired == false && ba.AccountType_Fk == "Fixed"  && DbFunctions.TruncateTime(ba.DateExpired) <= DbFunctions.TruncateTime(today) ); 
        }

    }

}
