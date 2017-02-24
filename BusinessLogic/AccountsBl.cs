using System;
using System.Linq;
using BusinessLogic.Utilities;
using Common;
using Common.Models;
using DataAccess;
using WebServices.Services.Yahoo;

namespace BusinessLogic
{
    public class AccountsBl
    {
        public IQueryable<string> GetBankAccountTypes()
        {
            return new AccountsRepo().GetBankAccountTypes();
        }

        public IQueryable<BalanceModel> GetBalancesForUser(string username)
        {
            return new AccountsRepo().GetBalancesForUser(username);
        }

        public IQueryable<BankAccount> GetAccountsForUser(string username)
        {
            return new AccountsRepo().GetAccountsForUser(username);
        }

        public IQueryable<long> GetSavingsAccountsForUser(string username)
        {
            return new AccountsRepo().GetSavingsAccountsForUser(username);
        }

        public string AddBankAccount(NewBankAccountModel newBankAccountModel)
        {
            var accountRepo = new AccountsRepo();
            var accountNumber = BankAccountNumberGenerator.GetUniqueAccountNumber();

            newBankAccountModel.AccountNumber = accountNumber;
            newBankAccountModel.Iban = IbanGenerator.GenerateIban(accountNumber);
            newBankAccountModel.Expired = false;

            var withdrawnAccount = accountRepo.GetWhitdrawnAccount(newBankAccountModel.Username_Fk,newBankAccountModel.WithdrawnAccountNo);

            if (withdrawnAccount == null)
            {
               return "Invalid Withdrawn Account";
            }
            if (newBankAccountModel.FriendlyName.Length <= 0)
            {
                return "A New Bank Account Must Be Given A Friendly Name";
            }
            if (newBankAccountModel.AmountWithdrawn <= 10)
            {
                return "A New Bank Account Must Have Some Funds Withdrawn (10 Minimum)";
            }
            if (!IsSufficientFunds(withdrawnAccount, newBankAccountModel.AmountWithdrawn))
            {
                return "Not Enough Funds (Withdrawn Account)";
            }
            if (newBankAccountModel.Currency_Fk == withdrawnAccount.Currency_Fk)
            {
                withdrawnAccount.Balance -= newBankAccountModel.AmountWithdrawn;
                newBankAccountModel.Balance += newBankAccountModel.AmountWithdrawn;

                accountRepo.UpdateAccount(withdrawnAccount);
            }
            if (newBankAccountModel.Currency_Fk != withdrawnAccount.Currency_Fk)
            {
                withdrawnAccount.Balance -= newBankAccountModel.AmountWithdrawn;
                newBankAccountModel.Balance += ConvertCurrency(withdrawnAccount.Currency_Fk, newBankAccountModel.Currency_Fk,
                    newBankAccountModel.AmountWithdrawn);

                accountRepo.UpdateAccount(withdrawnAccount);
            }

            newBankAccountModel.DateOpened = DateTime.Now;
            newBankAccountModel.DateExpired = newBankAccountModel.DateOpened.AddMonths(newBankAccountModel.DurationMonth);

            var newBankAccount = BankAccountModelToEntity(newBankAccountModel); 
            accountRepo.AddBankAccount(newBankAccount);
            return "Success";
        }

        public BankAccount BankAccountModelToEntity(NewBankAccountModel newBankAccountModel)
        {
            var bankAccount = new BankAccount
            {
                DateExpired = newBankAccountModel.DateExpired,
                DateOpened = newBankAccountModel.DateOpened,
                Currency_Fk = newBankAccountModel.Currency_Fk,
                AccountNumber = newBankAccountModel.AccountNumber,
                AccountType_Fk = newBankAccountModel.AccountType_Fk,
                Iban = newBankAccountModel.Iban,
                Balance = newBankAccountModel.Balance,
                FriendlyName = newBankAccountModel.FriendlyName,
                Expired = newBankAccountModel.Expired,
                Username_Fk = newBankAccountModel.Username_Fk
            };

            return bankAccount;
        }

        public string TransferFunds(TransferFundsModel transferFundsModel)
        {
            var saAccountNumber = transferFundsModel.SaAccountNumber;
            var daAccountNumber = transferFundsModel.DaAccountNumber;
            var amount = transferFundsModel.Amount;

            var accountRepo = new AccountsRepo();
            var transactionRepo = new TransactionsRepo
            {
                Entity = accountRepo.Entity
            };

            var saBankAccount = accountRepo.GetWhitdrawnAccount(transferFundsModel.Username,saAccountNumber);
            var daBankAccount = accountRepo.GetAccountByAccountNo(daAccountNumber);

            if (saBankAccount == null)
            {
                return "Invalid Sender Account";
            }
            if (daBankAccount == null || saBankAccount == daBankAccount)
            {
                return "Make Sure That Accounts Are Valid"; 
            }
            if (!IsSufficientFunds(saBankAccount, amount))
            {
                return "Insufficient Funds";
            }
            if (saBankAccount.AccountType_Fk.Equals("Fixed") || daBankAccount.AccountType_Fk.Equals("Fixed"))
            {
                return "Cannot Transfer To or From a Fixed Account";
            }

            var saTransaction = new Transaction();
            var daTransaction = new Transaction();

            //TODO make Transactions cleaner
            saTransaction.Currency_Fk = saBankAccount.Currency_Fk;
            saTransaction.AccountNo_Fk = saBankAccount.AccountNumber;
            saTransaction.Amount = -amount;
            saTransaction.Details = string.Concat("Transfer To Account ", daBankAccount.AccountNumber);
            saTransaction.Transaction_Timestamp = DateTime.Now;

            saBankAccount.Balance -= amount;

            daTransaction.Currency_Fk = daBankAccount.Currency_Fk;
            daTransaction.AccountNo_Fk = daBankAccount.AccountNumber;
            daTransaction.Details = string.Concat("Transfer From Account ", saBankAccount.AccountNumber);
            daTransaction.Transaction_Timestamp = DateTime.Now;

            if (IsAccountsSameCurrency(saBankAccount, daBankAccount))
            {
                daTransaction.Amount = amount;
                daBankAccount.Balance += amount;
            }
            if (!IsAccountsSameCurrency(saBankAccount, daBankAccount))
            {
                var convertedAmount = ConvertCurrency(saBankAccount.Currency_Fk, daBankAccount.Currency_Fk, amount);

                daTransaction.Amount = convertedAmount;
                daBankAccount.Balance += convertedAmount;
            }

            transactionRepo.AddTransaction(saTransaction);
            transactionRepo.AddTransaction(daTransaction);

            accountRepo.UpdateAccount(saBankAccount);
            accountRepo.UpdateAccount(daBankAccount);

            return "Success";
        }

        public bool IsSufficientFunds(BankAccount bankAccount, decimal amount)
        {
            return bankAccount.Balance >= amount;
        }

        public bool IsAccountsSameCurrency(BankAccount saBankAccount, BankAccount daBankAccount)
        {
            return saBankAccount.Currency_Fk == daBankAccount.Currency_Fk;
        }

        public decimal ConvertCurrency(string curFrom, string curTo, decimal amount)
        {
            var rate = new YahooCurrency().GetRate(curFrom, curTo);
            return amount * rate;
        }

    }
}
