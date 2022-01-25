using System;
using System.Collections.Generic;

namespace Cryptocop.Software.API.Models.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string CardholderName { get; set; }
        public string CreditCard { get; set; }
        public string OrderDate { get; set; }
        public float TotalPrice { get; set; }
        public IEnumerable<OrderItemDto> OrderItems { get; set; }
    }
}

/*
• Id (int)
• Email (string)
• FullName (string)
• StreetName (string)
• HouseNumber (string)
• ZipCode (string)
• Country (string)
• City (string)
• CardholderName (string)
• CreditCard (string)
• OrderDate (string)
• Represented as 01.01.2020
• TotalPrice (float)
• OrderItems (list of OrderItemDto)
*/
