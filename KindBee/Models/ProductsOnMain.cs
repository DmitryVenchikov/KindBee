using KindBee.DB.DBModels;

namespace KindBee.Models
{
    public class ProductOnMain
    {
        public Product Product { get; set; }
        public int QuantityInBasket { get; set; } = 0;
    }
}
