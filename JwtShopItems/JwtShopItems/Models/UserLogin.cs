using System.ComponentModel.DataAnnotations;

namespace JwtShopItems.Models
{
    public class UserLogin
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
