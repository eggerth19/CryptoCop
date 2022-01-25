using System.ComponentModel.DataAnnotations;

namespace Cryptocop.Software.API.Models.InputModels
{
    public class OrderInputModel
    {
        [Required]
        public int AddressId { get; set; }
        [Required]
        public int PaymentCardId { get; set; }
    }
}


/*
• AddressId (int)
• PaymentCardId (int)
*/