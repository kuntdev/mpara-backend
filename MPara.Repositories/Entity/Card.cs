using System;
namespace MPara.Repositories.Entity
{
    public class Card : BaseEntity
    {
        public int CardId { get; set; }
        public CardType Type { get; set; }
        public string Number { get; set; }
        public string ExpireDate { get; set; }
        public double Limit { get; set; }
        public double AvaliableLimit { get; set; }
        public int Cvv { get; set; }

        //relation
        public int AppUserId { get; set; }
    }

    public enum CardType
    {
        CreditCard,
        DebitCard,
        VirtualCard
    }
}
