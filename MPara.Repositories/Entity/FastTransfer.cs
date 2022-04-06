using System;
namespace MPara.Repositories.Entity
{
    public class FastTransfer : BaseEntity
    {
        public int FastTransferId { get; set; }
        public string NickName { get; set; }
        public double Amount { get; set; }
        public int Receiver { get; set; }

        // relations
        public int AppUserId{ get; set; }
    }
}
