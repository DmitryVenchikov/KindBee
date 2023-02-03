namespace KindBee.DB.DBModels
{
    public class Basket
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<Position>? Positions { get; set; }
        public decimal? TotalSum
        {
            get
            {
                decimal? total = 0;
                foreach (var t in Positions)
                {
                    total += t.Quantity * t.Product.Price;
                }
                return total;
            }
        }
    }
}
