namespace KindBee.DB.DBModels
{
    public class Basket
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<Product>? Products { get; set; }
    }
}
