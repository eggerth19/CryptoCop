using System.ComponentModel.DataAnnotations;

namespace Cryptocop.Software.API.Models.Entities
{
    public class JwtToken
    {
        [Key]
        public int Id { get; set; }
        public bool Blacklisted { get; set; }
    }
}