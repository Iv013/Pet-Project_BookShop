using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using MyBookShop_DataAccess.Repository.IRepository;
using MyBookShop_Models;
using MyBookShop_Models.Models;
using MyBookShop_Models.Models.VIewModel;
using MyBookShop_Models.VIewModel;
using MyBookShop_Utility;
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

            List<Book> productList = Book.Where(u => prodInCart.Contains(u.BookId)).ToList();
            return View(productList);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost(IEnumerable<Book> books)
        {
            List<ShopingCart> shopingCartslist = new List<ShopingCart>();
            foreach (Book book in books)
            {
                shopingCartslist.Add(new ShopingCart() { BookId = book.BookId });
            }
            HttpContext.Session.Set(WC.SessionCart, shopingCartslist);
            return RedirectToAction(nameof(Summary));
        }


        public IActionResult Summary()
        {
            ApplicationUser applcationUser;
            int tempId= 0;
            if (User.IsInRole(WC.AdminRole))
            {
                if (HttpContext.Session.Get<int>(WC.SessionOrderId) != 0)
                {
                    OrderHeader orderHeader1 = _orderHeaderRepo.FirstOfDefault(u => u.Id == HttpContext.Session.Get<int>(WC.SessionOrderId));
                    applcationUser = new ApplicationUser()
                    {
                        Email = orderHeader1.Email,
                        PhoneNumber = orderHeader1.PhoneNumber,
                        FullName = orderHeader1.FullName,
                        City = orderHeader1.City,
                        StreetAddress= orderHeader1.StreetAddress,
                        Region = orderHeader1.Region,
                       
                    };
                    tempId= orderHeader1.Id;
                }
                else
                {
                    applcationUser = new ApplicationUser();
                }
            }
            else
            {
                //Первый способ
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                applcationUser = _appUserRepo.FirstOfDefault(x => x.Id == claim.Value);
            }

            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<ShopingCart> shopingCartList = FindProductIncart();
            List<int> prodInCart = shopingCartList.Select(x => x.BookId).ToList();
            IEnumerable<Book> bookList = _bookRepo.GetAll(u => prodInCart.Contains(u.BookId), includeProperty:WC.AuthorName);
            BookUserVM BookUserVM = new BookUserVM()
            {
                ApplicationUser = applcationUser,//_appUserRepo.FirstOfDefault (x => x.Id == userID),
                BookList = bookList.ToList(),
                tempid=tempId
            };
            return View(BookUserVM); ;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")] 
        public async Task<IActionResult> SummaryPost(BookUserVM BookUserVM)
        {
            OrderHeader orderHeader;
          if (BookUserVM.tempid==0)
            await SendEmailOrder(BookUserVM);

            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (BookUserVM.tempid != 0)
            {
                orderHeader = _orderHeaderRepo.FirstOfDefault(x => x.Id == BookUserVM.tempid);
                orderHeader.FinalOrderTotal = BookUserVM.BookList.Sum(x => x.Price);
                orderHeader.OrderStatus = WC.StatusInProcess;
                _orderHeaderRepo.Update(orderHeader);
            } else
            {
                orderHeader = new OrderHeader()
                {
                    ApplicationUserId = userID
                     ,FullName = BookUserVM.ApplicationUser.FullName
                     , Email = BookUserVM.ApplicationUser.Email
                     , PhoneNumber = BookUserVM.ApplicationUser.PhoneNumber
                     , City = BookUserVM.ApplicationUser.City
                     ,Region = BookUserVM.ApplicationUser.Region
                     , StreetAddress = BookUserVM.ApplicationUser.StreetAddress
                     , DateStartState = DateTime.Now
                     , OrderStatus = WC.StatusPending
                     ,FinalOrderTotal = BookUserVM.BookList.Sum(x => x.Price)
                };

                _orderHeaderRepo.Add(orderHeader);

            }

            _orderHeaderRepo.Save();


            //  var Book = _bookRepo.GetAll(includeProperty: $"{WC.GenreName},{WC.AuthorName}");
            if (BookUserVM.tempid != 0)
            {
                var OrderDeatails2 = _orderDetailsRepo.GetAll(x => x.OrderHeaderId == BookUserVM.tempid);
                foreach (var order in OrderDeatails2)
                {
                    var bookedit = _bookRepo.FirstOfDefault(x => x.BookId == order.BookId, includeProperty: $"{WC.GenreName},{WC.AuthorName}");
                    bookedit.Amount++;
                    _bookRepo.Update(bookedit);
                    
                }
                _orderDetailsRepo.RemoveRange(OrderDeatails2);

            }
            
                foreach (var book in BookUserVM.BookList)
                {
                    OrderDetails OrderDeatails = new OrderDetails()
                    {
                        OrderHeaderId = orderHeader.Id,
                        BookId = book.BookId
                    };
                    var bookedit = _bookRepo.FirstOfDefault(x => x.BookId == book.BookId, includeProperty: $"{WC.GenreName},{WC.AuthorName}");
                    bookedit.Amount--;
                    _bookRepo.Update(bookedit);
                    _orderDetailsRepo.Add(OrderDeatails);
                }
                _orderDetailsRepo.Save();
                _bookRepo.Save();
            

             

            return RedirectToAction(nameof(InquaryConfirmation)); ;
        }









        private async Task SendEmailOrder(BookUserVM BookUserVM)
        {
            var pathtoTemplate = webHostEnvironment.WebRootPath
                            + Path.DirectorySeparatorChar.ToString()
                            + "templates" 
                            + Path.DirectorySeparatorChar.ToString() 
                            + "Inquiry.html";
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
                BookUserVM.ApplicationUser.City,
                productListSB.ToString()
                );
            await _emailSender.SendEmailAsync(WC.AdminEmail, subject, messageBody);
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
