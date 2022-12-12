using MyBookShop_Models.Models;

namespace MyBookShop_DataAccess.Repository.IRepository
{
    public interface IOrderHistoryRepository : IRepository<OrderHistory>
    {
        void Update(OrderHistory obj);
    }
}
