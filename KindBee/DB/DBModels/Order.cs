namespace KindBee.DB.DBModels
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime DateOfRegistration { get; set; } = DateTime.Now;
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<Position> Positions { get; set; }
        public Order()
        {
            Positions = new List<Position>();
        }
    }
}
