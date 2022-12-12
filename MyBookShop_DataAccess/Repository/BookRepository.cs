using Microsoft.AspNetCore.Mvc.Rendering;
using MyBookShop_DataAccess.Repository.IRepository;
using MyBookShop_Models;
using MyBookShop_Models.Models;
using MyBookShop_Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookShop_DataAccess.Repository
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        private readonly ApplicationDBContext _db;

        public BookRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetDropDownList(string obj)
        {
            if (obj == WC.AuthorName)
            {
                return _db.Author.Select(i => new SelectListItem
                {
                    Text = i.NameAuthor,
                    Value = i.AuthorId.ToString()
                });
            }
                 if (obj == WC.GenreName)
                {
                    return _db.Genre.Select(i => new SelectListItem { Text = i.NameGenre, Value = i.GenreId.ToString() });
                }

                return null;
        }
    


        public void Update(Book obj)
        {
           _db.Book.Update(obj);


        }
    }
}
