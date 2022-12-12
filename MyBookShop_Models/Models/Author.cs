using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyBookShop_Models
{
    public class Author
    {
        [Key]
        public int AuthorId { get; set; }

        [DisplayName("Автор")]
        public string NameAuthor { get; set; }
    }
}
