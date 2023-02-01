namespace KindBee.DB.DBModels
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime DateOfRegistration { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }
        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }
    }
}
