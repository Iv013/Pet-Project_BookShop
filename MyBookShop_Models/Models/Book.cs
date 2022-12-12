
using MyBookShop_Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KeyAttribute = System.ComponentModel.DataAnnotations.KeyAttribute;

namespace MyBookShop_Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }
        [Required]

        [DisplayName("Название")]
        public string Title { get; set; }
        
        [DisplayName("Короткое описание")]
        public string  ShortDesc{ get; set; }

        [DisplayName("Описание")]
        public string Description { get; set; }

        [DisplayName("Обложка")]
        public string  Image { get; set; }

        public int AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public virtual Author Author { get; set; }


        public int GenreId { get; set; }
        [ForeignKey("GenreId")]
        public virtual Genre Genre { get; set; }


        [Required]
        [DisplayName("Цена")]
        [Range(0, int.MaxValue, ErrorMessage = "Значение не должно быть меньше нуля")]    // добавить диапазон проверки на валидность
        public double Price { get; set; }

        [Required]
        [DisplayName("Количество на складе")]
        [Range(0, int.MaxValue, ErrorMessage = "Значение не должно быть меньше нуля")]    // добавить диапазон проверки на валидность
        public double Amount { get; set; }


    }
}
