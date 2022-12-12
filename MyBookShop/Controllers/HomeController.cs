using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBookShop_DataAccess;
using MyBookShop_DataAccess.Repository.IRepository;
using MyBookShop_Models;
using MyBookShop_Models.ViewModel;

using MyBookShop_Utility;
using System.Diagnostics;

namespace MyBookShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;


        private readonly IGenreRepository _genreRepo; 
        private readonly IBookRepository _bookRepo
            ;

        public HomeController(ILogger<HomeController> logger,  IGenreRepository genreRepo, IBookRepository bookRepo  )
        {
            _logger = logger;
            _bookRepo = bookRepo;
            _genreRepo=genreRepo;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Books = _bookRepo.GetAll(includeProperty: $"{WC.AuthorName},{WC.GenreName}"),
                Genres = _genreRepo.GetAll()
            };
            return View(homeVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Details(int id)
        {
            List<ShopingCart> shopingCartList = FindProductIncart();  

            DetailsVM detailsVM = new DetailsVM()                
            {
                Book = _bookRepo.GetAll(includeProperty: $"{WC.AuthorName},{WC.GenreName}")  
            .Where(x => x.BookId == id).FirstOrDefault(),
                ExistInCart = shopingCartList.Select(x => x).Where(x => x.BookId == id).Count() > 0 ? true : false
            };
            return View(detailsVM);
        }



        [HttpPost, ActionName("Details")]
        public IActionResult DetailsPost(int id)
        {
            List<ShopingCart> shopingCartList = FindProductIncart();
            shopingCartList.Add(new ShopingCart { BookId = id });
            TempData[WC.Success] = "Книга добавлена в корзину корзины";
            HttpContext.Session.Set(WC.SessionCart, shopingCartList);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveFromCart(int id)
        {
            List<ShopingCart> shopingCartList = FindProductIncart();
            var itemToRemove = shopingCartList.SingleOrDefault(r => r.BookId == id); 
            if (itemToRemove != null) shopingCartList.Remove(itemToRemove);  
            HttpContext.Session.Set(WC.SessionCart, shopingCartList);
            return RedirectToAction(nameof(Index));
        }

        public List<ShopingCart> FindProductIncart()
        {
            if (HttpContext.Session.Get<IEnumerable<ShopingCart>>(WC.SessionCart) != null &&
                            HttpContext.Session.Get<IEnumerable<ShopingCart>>(WC.SessionCart).Count() > 0)
            {
                return HttpContext.Session.Get<List<ShopingCart>>(WC.SessionCart);
            }
            return new List<ShopingCart>();
        }
    }
}