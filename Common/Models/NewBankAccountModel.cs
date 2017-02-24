namespace Common.Models
{
    public sealed class NewBankAccountModel : BankAccount
    {
        public long WithdrawnAccountNo { get; set; }
        public decimal AmountWithdrawn { get; set;}
        public int DurationMonth { get; set; }

    }
}
