using Microsoft.AspNetCore.Mvc.Rendering;
using MyBookShop_Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookShop_DataAccess.Repository.IRepository
{
    public interface IBookRepository : IRepository<Book>
    {
        void Update(Book obj);

        IEnumerable<SelectListItem> GetDropDownList(string obj);
    }
}
