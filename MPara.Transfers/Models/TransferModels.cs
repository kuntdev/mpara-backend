using System;
namespace MPara.Transfers.Models
{
    public class TransferCreateRequest
    {
        public int AccountId { get; set; }
        public int Receiver { get; set; }
        public double Amount { get; set; }


    }

    public class TransferListRequest
    {
        public int AccountId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }

    public class FastTransferCreateRequest
    {
        public int Receiver { get; set; }
        public string NickName { get; set; }
        public double Amount { get; set; }
    }

}
