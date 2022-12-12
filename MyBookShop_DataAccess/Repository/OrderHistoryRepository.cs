using MyBookShop_DataAccess.Repository.IRepository;
using MyBookShop_Models;
using MyBookShop_Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookShop_DataAccess.Repository
{
    public class OrderHistoryRepository : Repository<OrderHistory>, IOrderHistoryRepository
    {
        private readonly ApplicationDBContext _db;

        public OrderHistoryRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }


        public void Update(OrderHistory obj)
        {
            _db.OrderHistory.Update(obj);


        }

    }
}
