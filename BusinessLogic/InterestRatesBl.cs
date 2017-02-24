using System;
using System.Collections.Generic;
using DataAccess;

namespace BusinessLogic
{
    public sealed class InterestRatesBl
    {
        public Dictionary<decimal, int> InterestRates => new InterestRatesRepo().InterestRates;

        public decimal GetInterestRate(int month)
        {
            return new InterestRatesRepo().GetInterestRate(month);
        }

        public void InterestsRoutine()
        {
            var accountRepo = new AccountsRepo();
            var expiredAccounts = accountRepo.GetTodaysExpiredFixedAccounts();
            
            foreach (var acc in expiredAccounts)
            {
                acc.Expired = true;
                if (acc.DateExpired == null)
                {
                    continue;
                }
                var monthsBetween = (acc.DateExpired.Value.Year - acc.DateOpened.Year)*12 + acc.DateExpired.Value.Month -
                                    acc.DateOpened.Month;  
                acc.Balance = CalculateInterest(monthsBetween, acc.Balance) + acc.Balance;

                accountRepo.Update();
            }
        }

        public decimal CalculateInterest(int months, decimal balance)
        {
            var monthD = Convert.ToDecimal(months);
            var rate = new InterestRatesRepo().GetInterestRate(months);
            var monthRate = 12/monthD;
            const decimal deductTax = 0.85m; 
            var interest = ((rate / 100 * balance) / monthRate) * deductTax;
            return interest;
        }
    }
}
