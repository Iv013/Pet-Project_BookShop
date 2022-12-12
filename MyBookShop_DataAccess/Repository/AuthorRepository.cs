using MyBookShop_DataAccess.Repository.IRepository;
using MyBookShop_Models.Models;

namespace MyBookShop_DataAccess.Repository
{
    public class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        private readonly ApplicationDBContext _db;

        public AuthorRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Author author)
        {
            var objFromDB = _db.Author.FirstOrDefault(x => x.AuthorId == author.AuthorId);

            if (objFromDB != null) ;
            objFromDB.NameAuthor = author.NameAuthor;
           
        }
    }
}
