using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBookShop_DataAccess;
using MyBookShop_DataAccess.Repository.IRepository;
using MyBookShop_Models;
using System.Security.Cryptography;

namespace MyBookShop.Controllers
{
    public class GenreController : Controller
    {
        private readonly IGenreRepository _genreRep;
        public GenreController(IGenreRepository genreRep)
        {
            _genreRep = genreRep;
        }


        public IActionResult Index()
        {
            IEnumerable<Genre> objList = _genreRep.GetAll();

            return View(objList);
        }

        public IActionResult Upsert(int? id)
        {
            Genre genre = new Genre();
            if (id != null)                       
            {
                genre = _genreRep.Find(id.GetValueOrDefault());         
                if (genre == null)
                {
                    return NotFound();
                }
                return View(genre);              
            }
            return View(genre);


        }

        [HttpPost,ActionName("Upsert")]
        [ValidateAntiForgeryToken]
        public IActionResult UpsertPost(Genre obj)          
        {
            if (obj.GenreId == 0)
            {
                _genreRep.Add(obj);                
                _genreRep.Save();                
                return RedirectToAction("Index");

            }
            else
            {
                _genreRep.Update(obj);             
                _genreRep.Save();                
                return RedirectToAction("Index");

            }


            return View(); ;        
        }



        //GET - DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _genreRep.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }
        //POST - DELETE
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _genreRep.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            _genreRep.Remove(obj);
            _genreRep.Save();
            return RedirectToAction("Index");


        }




    }
}
