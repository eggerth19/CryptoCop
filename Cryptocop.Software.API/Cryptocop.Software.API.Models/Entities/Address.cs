using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cryptocop.Software.API.Models.Entities
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
    }
}


/*
TODO: check foreign key UserId
*/