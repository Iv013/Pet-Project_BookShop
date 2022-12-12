using Microsoft.AspNetCore.Mvc.Rendering;
using MyBookShop_Models.Models;

namespace MyBookShop_DataAccess.Repository.IRepository
{
    public interface IBookRepository : IRepository<Book>
    {
        void Update(Book obj);

        IEnumerable<SelectListItem> GetDropDownList(string obj);
    }
}
