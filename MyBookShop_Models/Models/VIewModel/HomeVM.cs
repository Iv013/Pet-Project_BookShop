using MyBookShop_Models.Models;

namespace MyBookShop_Models.ViewModel
{
    public class HomeVM
    {
        public IEnumerable<Book> Books { get; set; }
        public IEnumerable<Genre> Genres { get; set; }

    }
}
