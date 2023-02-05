using KindBee.DB.DBModels;

namespace KindBee.Models
{
    public class ProductsOnMain
    {
        public List<Product> Products { get; set; }
        public Basket Basket { get; set; }
    }
}
