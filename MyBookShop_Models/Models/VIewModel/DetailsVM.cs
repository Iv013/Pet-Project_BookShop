namespace MyBookShop_Models.ViewModel
{
    public class DetailsVM
    {
        public Book Book { get; set; }
        public bool ExistInCart { get; set; }

        public DetailsVM()
        {
            Book = new Book();
        }
    }
}
