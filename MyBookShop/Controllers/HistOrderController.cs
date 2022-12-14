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

        private readonly IOrderHistoryRepository _OrderHistoryRepo;

        public HistOrderController(IOrderHistoryRepository orderHistoryRepo)
        {
            _OrderHistoryRepo = orderHistoryRepo;
        }

        public IActionResult Index(string searchName = null, string searchEmail = null, string searchPhone = null, string Status = null)
        {
            OrderHistoryVM orderHistory = new OrderHistoryVM()
            { OrderHistory = _OrderHistoryRepo.GetAll(),
                statusList = WC.listStatus.ToList().Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = i, Value = i })
            };
            if (!string.IsNullOrEmpty(searchName))
            {
                orderHistory.OrderHistory = orderHistory.OrderHistory.Where(u => u.FullName.ToLower().Contains(searchName.ToLower()));
            }
            if (!string.IsNullOrEmpty(searchEmail))
            {
                orderHistory.OrderHistory = orderHistory.OrderHistory.Where(u => u.Email.ToLower().Contains(searchEmail.ToLower()));
            }
            if (!string.IsNullOrEmpty(searchPhone))
            {
                orderHistory.OrderHistory = orderHistory.OrderHistory.Where(u => u.PhoneNumber.ToLower().Contains(searchPhone.ToLower()));
            }
            if (!string.IsNullOrEmpty(Status) && Status != "--Order Status--")
            {
                orderHistory.OrderHistory = orderHistory.OrderHistory.Where(u => u.OrderStatus.ToLower().Contains(Status.ToLower()));
            }

            return View(orderHistory);

        }
    }
}
