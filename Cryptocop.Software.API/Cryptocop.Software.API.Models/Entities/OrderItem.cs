using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cryptocop.Software.API.Models.Entities
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public string ProductIdentifier { get; set; }
        public float Quantity { get; set; }
        public float UnitPrice { get; set; }
        public float TotalPrice { get; set; }
    }
}