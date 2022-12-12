using MyBookShop_Models.Models;

namespace MyBookShop_DataAccess.Repository.IRepository
{
    public interface IAuthorRepository:IRepository<Author>
    {
        void Update(Author category);
    }
}
