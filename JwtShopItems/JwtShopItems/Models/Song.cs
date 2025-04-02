using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JwtShopItems.Models
{
    public class Song
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [ForeignKey("Author")]
        public int AuthorId { get; set; }

        public Genre Genre { get; set; }

        public Author Author { get; set; }
    }
}
