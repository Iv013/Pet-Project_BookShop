using Microsoft.AspNetCore.Mvc;
using MyBookShop_DataAccess.Repository.IRepository;
using MyBookShop_Models;
using MyBookShop_Models.Models;
using MyBookShop_Models.Models.VIewModel;
using MyBookShop_Utility;

namespace MyBookShop.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderHeaderRepository _OrderHeadRepo;
        private readonly IOrderDetailsRepository _OrderDetailsRepo;
        private readonly IAuthorRepository _AuthorRepo;

        public OrderController(IOrderHeaderRepository OrderHeadRepo, IOrderDetailsRepository OrderDetailsRepo, IAuthorRepository AuthorRepo)
        {
            _OrderDetailsRepo = OrderDetailsRepo;
            _OrderHeadRepo = OrderHeadRepo;
            _AuthorRepo = AuthorRepo;
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
                // = _.GetAll(x => x.InquiryHeaderId == id, includeProperty: "Book"),
                OrderHeader = _OrderHeadRepo.FirstODefault(x => x.Id == id),
                OrderDetails = _OrderDetailsRepo.GetAll(x => x.OrderHeaderId == id, includeProperty: "Book").ToList(),
                Author = new List<Author>()
                
            };
            foreach (var obj in orderVM.OrderDetails)
            {
                var tempAutor = _AuthorRepo.FirstODefault(x => x.AuthorId == obj.Book.AuthorId);
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
        [ValidateAntiForgeryToken]
        public IActionResult Delete()
        { 
            OrderHeader orderHeader= _OrderHeadRepo.FirstODefault(u => u.Id == orderVM.OrderHeader.Id);
            IEnumerable<OrderDetails> orderDetails = _OrderDetailsRepo.GetAll(u => u.OrderHeaderId == orderHeader.Id);
            _OrderHeadRepo.Remove(orderHeader);
            // _inquiryDeatailRepo.RemoveRange(inquiryDeatails);
            _OrderHeadRepo.Save();
            //_inquiryDeatailRepo.Save();   
            return RedirectToAction("Index");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cancel()
        {
            OrderHeader orderHeader = _OrderHeadRepo.FirstODefault(u => u.Id == orderVM.OrderHeader.Id);
            IEnumerable<OrderDetails> orderDetails = _OrderDetailsRepo.GetAll(u => u.OrderHeaderId == orderHeader.Id);
            _OrderHeadRepo.Remove(orderHeader);
            // _inquiryDeatailRepo.RemoveRange(inquiryDeatails);
            _OrderHeadRepo.Save();
            //_inquiryDeatailRepo.Save();   
            return RedirectToAction("Index");

        }



    }
}
