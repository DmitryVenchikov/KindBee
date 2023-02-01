using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace KindBee.DB.DBModels
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime DateOfManufacture { get; set; }
        [Precision(18, 2)]
        public decimal Price { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
        public virtual ICollection<Basket>? Baskets { get; set; }
    }
}
