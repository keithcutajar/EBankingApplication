namespace Common.Models
{
    public sealed class BalanceModel
    {
        public string Iban { get; set; }
        public long AccountNumber { get; set; }
        public string AccountType { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Balance { get; set; }
        public string FriendlyName { get; set; }
    }
}
