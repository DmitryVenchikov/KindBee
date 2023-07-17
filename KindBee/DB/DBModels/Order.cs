namespace KindBee.DB.DBModels
{
    public enum Status
    {
        NEW, //новый
        PAID,  //оплачен
        REJECTED,  //отклонен сайтом
        CANCELED,  //отменен клиентом
        DELIVERED //доставлен
    }

    public class Order
    {
        public int Id { get; set; }
        public DateTime DateOfRegistration { get; set; } = DateTime.Now;
        public int CustomerId { get; set; }
        public Status Status { get; set; } = Status.NEW;
        public virtual Customer Customer { get; set; }
        public virtual ICollection<Position> Positions { get; set; }
        public Order()
        {
            Positions = new List<Position>();
        }
    }
}
