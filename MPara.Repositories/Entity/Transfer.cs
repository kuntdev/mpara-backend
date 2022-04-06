using System;
using System.Collections.Generic;

namespace MPara.Repositories.Entity
{
    public class Transfer: BaseEntity
    {
        public int TransferId { get; set; }
        public int Sender { get; set; }
        public int Receiver { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverBank { get; set; }

        public double Amount { get; set; }

        //relation
        public int AccountId { get; set; }
    }
}
