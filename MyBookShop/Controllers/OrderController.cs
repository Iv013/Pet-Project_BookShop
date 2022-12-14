using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MyBookShop_DataAccess.Repository.IRepository;
using MyBookShop_Models;
using MyBookShop_Models.Models;
using MyBookShop_Models.Models.VIewModel;
using MyBookShop_Utility;

namespace MyBookShop.Controllers
{
    public class OrderController : Controller
    {   private readonly IBookRepository _bookRepo;
        private readonly IOrderHeaderRepository _OrderHeadRepo;
        private readonly IOrderDetailsRepository _OrderDetailsRepo;
        private readonly IAuthorRepository _AuthorRepo;
        private readonly IOrderHistoryRepository _OrderHistoryRepo;

        public OrderController(IOrderHeaderRepository OrderHeadRepo, 
            IOrderDetailsRepository OrderDetailsRepo, 
            IAuthorRepository AuthorRepo,
            IOrderHistoryRepository OrderHistoryRepo,
            IBookRepository bookRepo)
        {
            _OrderDetailsRepo = OrderDetailsRepo;
            _OrderHeadRepo = OrderHeadRepo;
            _AuthorRepo = AuthorRepo;
            _OrderHistoryRepo = OrderHistoryRepo;
            _bookRepo = bookRepo;
        }
        [BindProperty] 
        public OrderVM orderVM { get; set; }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetInquiryList()
        {
            return Json(new { data = _OrderHeadRepo.GetAll() });
        }


        public IActionResult Details(int id)
        {
            orderVM = new OrderVM()
            {
              
                OrderHeader = _OrderHeadRepo.FirstOfDefault(x => (x.Id == id)),
                OrderDetails = _OrderDetailsRepo.GetAll(x => x.OrderHeaderId == id, includeProperty: "Book").ToList(),
                Author = new List<Author>()  
            };
            foreach (var obj in orderVM.OrderDetails)
            {
                var tempAutor = _AuthorRepo.FirstOfDefault(x => x.AuthorId == obj.Book.AuthorId);
                orderVM.Author.Add(tempAutor);
            }

            return View(orderVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details()
        {
            List<ShopingCart> shopingCartsList = new List<ShopingCart>();


            orderVM.OrderDetails = _OrderDetailsRepo.GetAll(u => u.OrderHeaderId == orderVM.OrderHeader.Id).ToList(); ;
            foreach (var detail in orderVM.OrderDetails)
            {
                ShopingCart shopingCart = new ShopingCart() { BookId = detail.BookId };
                shopingCartsList.Add(shopingCart);
            }
            HttpContext.Session.Clear();
            HttpContext.Session.Set(WC.SessionCart, shopingCartsList);
            HttpContext.Session.Set(WC.SessionOrderId, orderVM.OrderHeader.Id);
            return RedirectToAction("Index", "Cart");
        }
        [HttpPost]
        public IActionResult InProcess()
        {
            ChangeOrderStatus( WC.StatusInProcess);
            TempData[WC.Success] = "Заказ отправлена на упаковку";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult InDelivery()
        {
            ChangeOrderStatus(WC.StatusDelivery);
            TempData[WC.Success] = "Заказ отправлен в доставку";
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult SetOrderComplite()
        {
            ChangeOrderStatus(WC.StatusComplited);
            OrderHeader orderHeader = _OrderHeadRepo.FirstOfDefault(u => u.Id == orderVM.OrderHeader.Id);
            AddOrderHistory(orderHeader, WC.StatusComplited);
            DeleteOrder(orderHeader);
            TempData[WC.Success] = "Заказ закрыт";
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cancel()
        {
            OrderHeader orderHeader = _OrderHeadRepo.FirstOfDefault(u => u.Id == orderVM.OrderHeader.Id);
            IEnumerable<OrderDetails> orderDetails = _OrderDetailsRepo.GetAll(u => u.OrderHeaderId == orderHeader.Id);
            foreach (var bookoreder in orderDetails)
            {
                var bookedit = _bookRepo.FirstOfDefault(x => x.BookId == bookoreder.BookId, includeProperty: $"{WC.GenreName},{WC.AuthorName}");
                bookedit.Amount++;
                _bookRepo.Update(bookedit);
            }
            _bookRepo.Save();

            AddOrderHistory(orderHeader, WC.StatusCancellind);
            _OrderDetailsRepo.RemoveRange(orderDetails);
            DeleteOrder(orderHeader);


            return RedirectToAction("Index");

        }


        private void DeleteOrder(OrderHeader orderHeader)
        {
            _OrderHeadRepo.Remove(orderHeader);
            _OrderHeadRepo.Save();
        }


        private void AddOrderHistory(OrderHeader orderHeader, string status )
        {
            OrderHistory orderHistory = new OrderHistory()
            {
                Id = orderHeader.Id,
                FinalOrderTotal = orderHeader.FinalOrderTotal,
                Region = orderHeader.Region,
                FullName = orderHeader.FullName,
                Email = orderHeader.Email,
                OrderStatus =  status,
                PhoneNumber = orderHeader.PhoneNumber,
                City = orderHeader.City,
                DateEndState = DateTime.Now,
                DateStartState = orderHeader.DateStartState
            };

            _OrderHistoryRepo.Add(orderHistory);
            _OrderHistoryRepo.Save();
        }

        private void ChangeOrderStatus(string newStatus)
        {
            OrderHeader orderHeader = _OrderHeadRepo.FirstOfDefault(u => u.Id == orderVM.OrderHeader.Id);
            AddOrderHistory(orderHeader, orderHeader.OrderStatus);
            orderHeader.OrderStatus = newStatus;
            orderHeader.DateStartState = DateTime.Now;
            _OrderHeadRepo.Update(orderHeader);
            _OrderHeadRepo.Save();
        }
    }
}
