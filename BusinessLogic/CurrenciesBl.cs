using System.Linq;
using Common;
using DataAccess;

namespace BusinessLogic
{
    public sealed class CurrenciesBl
    {
        public IQueryable<string> GetCurrencies() => new CurrenciesRepo().Currencies;
    }
}
