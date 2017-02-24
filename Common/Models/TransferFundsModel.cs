namespace Common.Models
{
    public sealed class TransferFundsModel
    {
        public long SaAccountNumber { get; set; }
        public long DaAccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string Username { get; set; }
    }
}