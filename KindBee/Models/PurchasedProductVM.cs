using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace KindBee.Models
{
    public class PurchasedProductVM
    {
        [Required]
        public int Id { get; set; }
        public int Quantity { get; set; }
    }
}
