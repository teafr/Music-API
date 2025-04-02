using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace JwtShopItems.Models
{
    public class Author : IDatabaseModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [JsonIgnore]
        public List<Song> Songs { get; set; }
    }
}
