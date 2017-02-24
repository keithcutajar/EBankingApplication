using System.Linq;

namespace DataAccess
{
    public sealed class CurrenciesRepo : ConnectionClass
    {
        public IQueryable<string> Currencies => from c in Entity.Currencies select c.CurrencyCode;
    }
}
