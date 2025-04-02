using System.ComponentModel.DataAnnotations;

namespace MusicAPI.Models
{
    public class UserLogin
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
