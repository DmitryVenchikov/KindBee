namespace KindBee.DB.DBModels
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime DateOfRegistration { get; set; }
        public int BasketId { get; set; }
        public virtual Basket? Basket { get; set; }
    }
}
