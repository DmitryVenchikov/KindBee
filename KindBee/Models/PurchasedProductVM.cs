using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace KindBee.Models
{
    public class PurchasedProductVM
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int Quantity { get; set; } = 0;
    }
}
