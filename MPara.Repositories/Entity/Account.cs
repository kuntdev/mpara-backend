using System;
using System.Collections.Generic;

namespace MPara.Repositories.Entity
{
    public class Account : BaseEntity
    {
        public int AccountId { get; set; }
        public string Type { get; set; }
        public string Currency { get; set; }
        public double Amount { get; set; }
        public int Number { get; set; }
        public int BranchCode { get; set; }

        // relation
        public int AppUserId { get; set; }
        public List<Transfer> Transfers { get; set; }
    }
}
