using Microsoft.AspNetCore.Mvc;
using MyBookShop_DataAccess.Repository;
using MyBookShop_DataAccess.Repository.IRepository;
using MyBookShop_Models.Models;
using MyBookShop_Models.Models.VIewModel;

namespace MyBookShop.Controllers
{
    public class HistOrderController : Controller
    {

        private readonly IOrderHistoryRepository _OrderHistoryRepo;

        public HistOrderController(IOrderHistoryRepository orderHistoryRepo)
        {
            _OrderHistoryRepo = orderHistoryRepo;
        }

        public IActionResult Index()
        {
            OrderHistoryVM orderHistory = new OrderHistoryVM()
            { OrderHistory = _OrderHistoryRepo.GetAll()
            };
            


            return View(orderHistory);




        }
    }
}
