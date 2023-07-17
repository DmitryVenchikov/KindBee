namespace KindBee.DB.DBModels
{

    public class Position
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int Quantity { get; set; }
        public int? BasketId { get; set; }
        public virtual Basket? Basket { get; set; }
        public int? OrderId { get; set; }
        public virtual Order? Order { get; set; }
       
    }
}
