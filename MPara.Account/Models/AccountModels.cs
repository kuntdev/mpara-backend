using System;
namespace MPara.Account.Models
{
    public class AccountCreateRequest
    {
        public AccountType Type { get; set; }
        public AccountCurrency Currency { get; set; }
    }

    public class AccountCreateResponse
    {
        public int AccountId { get; set; }
        public AccountType Type { get; set; }
        public AccountCurrency Currency { get; set; }
        public double Amount { get; set; }
        public int Number { get; set; }
        public int BrannchCode { get; set; }
    }

    public enum AccountType
    {
        Vadesiz,
        Vadeli
    }

    public enum AccountCurrency
    {
        TRY,
        USD,
        EUR
    }


}
