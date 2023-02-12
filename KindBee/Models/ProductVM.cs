using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace KindBee.Models
{
    public class ProductVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime DateOfManufacture { get; set; }
        [Precision(18, 2)]
        public decimal? Price { get; set; }
        public int Quantity { get; set; } = 0;
        public IFormFile Image { get; set; }
    }
}
