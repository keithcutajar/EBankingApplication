using System.Collections.Generic;
using System.Linq;

namespace DataAccess
{
    public class InterestRatesRepo : ConnectionClass
    {
        public Dictionary<decimal, int> InterestRates
            => Entity.InterestRates.OrderBy(i => i.Rate).Select(i => new {i.Rate, i.Month}).ToDictionary(i => i.Rate, i => i.Month);

        public decimal GetInterestRate(int month)
        {
            return Entity.InterestRates.Where(x => x.Month == month).Select(x => x.Rate).SingleOrDefault(); 
        }
    }
}
