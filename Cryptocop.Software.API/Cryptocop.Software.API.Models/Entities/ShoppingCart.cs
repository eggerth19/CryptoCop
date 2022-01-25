using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cryptocop.Software.API.Models.Entities
{
    public class ShoppingCart
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int userId { get; set; }
    }
}