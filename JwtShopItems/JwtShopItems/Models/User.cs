using System.ComponentModel.DataAnnotations;

namespace JwtShopItems.Models
{
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
