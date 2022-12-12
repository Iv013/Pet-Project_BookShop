namespace MyBookShop_Models.VIewModel
{
    public class BookUserVM
    {
       public ApplicationUser ApplicationUser { get; set; }
       public List<Book> BookList { get; set; }
        public string Adress { get; set; }
    }
}
