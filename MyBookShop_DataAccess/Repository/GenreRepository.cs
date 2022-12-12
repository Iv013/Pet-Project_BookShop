using MyBookShop_DataAccess.Repository.IRepository;
using MyBookShop_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookShop_DataAccess.Repository
{
    public class GenreRepository : Repository<Genre>, IGenreRepository
    {
        private readonly ApplicationDBContext _db;

        public GenreRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Genre Genre)
        {
            var objFromDB = _db.Genre.FirstOrDefault(x => x.GenreId == Genre.GenreId);

            if (objFromDB != null) ;
            objFromDB.NameGenre = Genre.NameGenre;

        }

    }
}
