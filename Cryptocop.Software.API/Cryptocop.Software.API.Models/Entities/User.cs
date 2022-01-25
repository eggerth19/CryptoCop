using System.ComponentModel.DataAnnotations;

namespace Cryptocop.Software.API.Models.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
    }
}