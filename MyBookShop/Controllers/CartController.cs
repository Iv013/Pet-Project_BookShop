using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBookShop_DataAccess;
using MyBookShop_DataAccess.Repository.IRepository;
using MyBookShop_Models;
using MyBookShop_Models.Models;
using MyBookShop_Models.VIewModel;
using MyBookShop_Utility;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace MyBookShop.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IBookRepository _bookRepo;
        private readonly IApplicationUserRepository _appUserRepo;
        private readonly IOrderDetailsRepository _orderDetailsRepo;
        private readonly IOrderHeaderRepository _orderHeaderRepo;


        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IEmailSender _emailSender;
        public CartController(IBookRepository bookRepo, IWebHostEnvironment webHostEnvironment, IEmailSender emailSender
            , IApplicationUserRepository appUserRepo
            , IOrderDetailsRepository orderDetailsRepo
            , IOrderHeaderRepository orderHeaderRepo)
        {
            _bookRepo=bookRepo ;
            _appUserRepo=appUserRepo ;
            _orderDetailsRepo=orderDetailsRepo ;
            _orderHeaderRepo=orderHeaderRepo ;
            this.webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
              
        } 

        public IActionResult Index()
        {
            List<ShopingCart> shopingCartList = FindProductIncart();
            List<int> prodInCart = shopingCartList.Select(x => x.BookId).ToList();

            var Book = _bookRepo.GetAll(includeProperty:$"{WC.GenreName},{WC.AuthorName}");

            IEnumerable<Book> productList = Book.Where(u => prodInCart.Contains(u.BookId));
            return View(productList);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            return RedirectToAction(nameof(Summary));
        }


        public IActionResult Summary()
        {

            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<ShopingCart> shopingCartList = FindProductIncart();
            List<int> prodInCart = shopingCartList.Select(x => x.BookId).ToList();
            IEnumerable<Book> bookList = _bookRepo.GetAll(u => prodInCart.Contains(u.BookId), includeProperty:WC.AuthorName);
            BookUserVM BookUserVM = new BookUserVM()
            {
                ApplicationUser = _appUserRepo.FirstODefault(x => x.Id == userID),
                BookList = bookList.ToList(),
            };
            return View(BookUserVM); ;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")] 
        public async Task<IActionResult> SummaryPost(BookUserVM BookUserVM)
        {
            var pathtoTemplate = webHostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                + "templates" + Path.DirectorySeparatorChar.ToString() + "Inquiry.html";

            var subject = "new Inquiry";
            string HtmlBody = "";
            using (StreamReader sr = System.IO.File.OpenText(pathtoTemplate))
            {
                HtmlBody = sr.ReadToEnd();  
            }
            StringBuilder productListSB = new StringBuilder(); 
            foreach (var book in BookUserVM.BookList)
            {
                productListSB.Append($"-  Название:{book.Title} <span style='font-size:14px;'> Автор: {book.Author.NameAuthor}</span><br/>");
            }

            string messageBody = string.Format(HtmlBody,  
                BookUserVM.ApplicationUser.FullName,
                BookUserVM.ApplicationUser.Email,
                BookUserVM.ApplicationUser.PhoneNumber,
                BookUserVM.Adress,
                productListSB.ToString()
                );
            await _emailSender.SendEmailAsync(WC.AdminEmail, subject, messageBody);

            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            OrderHeader orderHeader = new OrderHeader()
            { ApplicationUserId = userID
           , FullName = BookUserVM.ApplicationUser.FullName
           , Email = BookUserVM.ApplicationUser.Email
           , PhoneNumber = BookUserVM.ApplicationUser.PhoneNumber
           , City = BookUserVM.ApplicationUser.City
           , Region = BookUserVM.ApplicationUser.Region
           , StreetAddress = BookUserVM.ApplicationUser.StreetAddress
           , active_state = true
           , DateStartState = DateTime.Now
           , OrderStatus = WC.StatusPending
            };


            _orderHeaderRepo.Add(orderHeader);
            _orderHeaderRepo.Save();
            foreach (var book in BookUserVM.BookList)
            {
                OrderDetails OrderDeatails = new OrderDetails()
                {
                    OrderHeaderId = orderHeader.Id,
                    BookId = book.BookId

                };
                _orderDetailsRepo.Add(OrderDeatails);


            }
            _orderDetailsRepo.Save();


            return RedirectToAction(nameof(InquaryConfirmation)); ;
        }

        public IActionResult InquaryConfirmation()
        {


            HttpContext.Session.Clear();
            return View(); ;
        }



        public IActionResult Remove(int id)
        {
            List<ShopingCart> shopingCartList = FindProductIncart();
            shopingCartList.Remove(shopingCartList.FirstOrDefault(x => x.BookId == id));
            TempData[WC.Success] = "Книга удалена из корзины";
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
