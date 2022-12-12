using Microsoft.AspNetCore.Mvc.Rendering;
using MyBookShop_Models;

namespace MyBookShop_Models.VIewModel
{
    public class VMBook
    {
        public Book Book { get; set; }
        public IEnumerable<SelectListItem> AuthorSelectList { get; set; }
        public IEnumerable<SelectListItem> GenreSelectList { get; set; }


    }
}
