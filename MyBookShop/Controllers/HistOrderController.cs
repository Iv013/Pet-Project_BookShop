using Microsoft.AspNetCore.Mvc;
using MyBookShop_DataAccess.Repository;
using MyBookShop_DataAccess.Repository.IRepository;
using MyBookShop_Models.Models;
using MyBookShop_Models.Models.VIewModel;
using MyBookShop_Utility;

namespace MyBookShop.Controllers
{
    public class HistOrderController : Controller
    {
         private  DateTime dd = new DateTime();
        private readonly IOrderHistoryRepository _OrderHistoryRepo;
        private readonly IOrderDetailsRepository _OrderDetailsRepo;
        private readonly IAuthorRepository _AuthorRepo;
        [BindProperty]
        public OrderHistoryVM orderHistory { get; set; }
        public HistOrderController(IOrderHistoryRepository orderHistoryRepo, IOrderDetailsRepository OrderDetailsRepo, IAuthorRepository AuthorRepo)
        {
            _OrderHistoryRepo = orderHistoryRepo;
            _OrderDetailsRepo = OrderDetailsRepo;
            _AuthorRepo = AuthorRepo;
        }

        public IActionResult Index(string searchName = null, DateTime? SearchDate = null, string searchPhone = null, string Status = null)
        {
             orderHistory = new OrderHistoryVM()
            { OrderHistory = _OrderHistoryRepo.GetAll().OrderByDescending(x=>x.HistoryId),
                statusList = WC.listStatus.ToList().Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = i, Value = i })
            };
            if (!string.IsNullOrEmpty(searchName))
                orderHistory.OrderHistory = orderHistory.OrderHistory.Where(u => u.FullName.ToLower().Contains(searchName.ToLower()));
       

            if (SearchDate != null)
                orderHistory.OrderHistory = orderHistory.OrderHistory.Where(u => u.DateEndState.Month == SearchDate.Value.Month);
            

            if (!string.IsNullOrEmpty(searchPhone))
                orderHistory.OrderHistory = orderHistory.OrderHistory.Where(u => u.PhoneNumber.ToLower().Contains(searchPhone.ToLower()));

            if (!string.IsNullOrEmpty(Status) && Status != "--Order Status--")
            {
                orderHistory.OrderHistory = orderHistory.OrderHistory.Where(u => u.OrderStatus.ToLower().Contains(Status.ToLower()));
            }

            return View(orderHistory);

       

        }
        public IActionResult Details(int id)
        {
            OrderDetailsHistoryVM orderHistory = new OrderDetailsHistoryVM()
            {
                SelectedOrderHistory = _OrderHistoryRepo.FirstOfDefault(x => x.HistoryId == id),
                detailList = _OrderDetailsRepo.GetAll(x => x.OrderHeaderId == id, includeProperty: "Book").ToList(),
              //  Author = new List<Author>()
            };
            orderHistory.detailList.ForEach(x => x.Book.Author = _AuthorRepo.FirstOfDefault(a => a.AuthorId == x.Book.AuthorId));
     
            //{

            //    OrderHeader = _OrderHeadRepo.FirstOfDefault(x => (x.Id == id)),
            //    OrderDetails = _OrderDetailsRepo.GetAll(x => x.OrderHeaderId == id, includeProperty: "Book").ToList(),
            //    Author = new List<Author>()
            //};
            //foreach (var obj in orderVM.OrderDetails)
            //{
            //    var tempAutor = _AuthorRepo.FirstOfDefault(x => x.AuthorId == obj.Book.AuthorId);
            //    orderVM.Author.Add(tempAutor);
            //}

            return View(orderHistory);
        }
    }
}
