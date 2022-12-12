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
    public class OrderDetailsRepository : Repository<OrderDetails>, IOrderDetailsRepository
    {
        private readonly ApplicationDBContext _db;

        public OrderDetailsRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }


        public void Update(OrderDetails obj)
        {
            _db.OrderDetails.Update(obj);


        }

    }
}
