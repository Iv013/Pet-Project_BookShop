using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyBookShop_Models.Models
{
    public class Genre
    {
        [Key]
        public int GenreId { get; set; }

        [DisplayName("Жанр")]
        public string NameGenre { get; set; }
    }
}
