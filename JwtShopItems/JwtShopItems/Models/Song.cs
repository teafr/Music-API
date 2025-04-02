using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MusicAPI.Models
{
    [Table("songs")]
    public class Song
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        [Required]
        public string Name { get; set; }

        [Column("author_id")]
        [ForeignKey("Author")]
        public int AuthorId { get; set; }

        [Column("genre_id")]
        [ForeignKey("Genre")]
        public int GenreId { get; set; }

        [JsonIgnore]
        public Genre? Genre { get; set; }
        
        [JsonIgnore]
        public Author? Author { get; set; }
    }
}
