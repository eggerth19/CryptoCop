using System.ComponentModel.DataAnnotations;

namespace Cryptocop.Software.API.Models.InputModels
{
    public class PaymentCardInputModel
    {
        [Required]
        [MinLength(3)]
        public string CardholderName { get; set; }
        [Required]
        [CreditCard]
        public string CardNumber { get; set; }
        [Required]
        [Range(1,12)]
        public int Month { get; set; }
        [Required]
        [Range(0,99)]
        public int Year { get; set; }
    }
}


/*
• CardholderName* (string)
• A minimum length of 3 characters
• CardNumber* (string)
• Must be a valid credit card number
• Month (int)
• The range for this number is an inclusive 1 to 12
• Year (int)
• The range for this number is an inclusive 0 to 99
*/