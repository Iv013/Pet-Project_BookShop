using Microsoft.AspNetCore.Mvc;
using MyBookShop_DataAccess.Repository.IRepository;
using MyBookShop_Models.Models;

namespace MyBookShop.Controllers
{
    public class AuthorController : Controller
    {
       
        private readonly IAuthorRepository _AuthRepo;
        public AuthorController(IAuthorRepository AuthRepo) => _AuthRepo = AuthRepo;

        public IActionResult Index()
        {
            IEnumerable<Author> objList = _AuthRepo.GetAll();
            return View(objList);
        }

        public IActionResult Create()
        {    
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Author obj)              
        {
            if (ModelState.IsValid)
            {     
                _AuthRepo.Add(obj);                  
                _AuthRepo.Save();                    
                return RedirectToAction("Index");       
            }

            return View(obj); ;       
        }

        //GET - EDIT
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();
            var obj = _AuthRepo.Find(id.GetValueOrDefault());
            if (obj == null) return NotFound();
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Author obj)
        {
            if (ModelState.IsValid)
            {
                _AuthRepo.Update(obj);
                _AuthRepo.Save();
                return RedirectToAction("Index");
            }

            return View(obj);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();
            var obj = _AuthRepo.Find(id.GetValueOrDefault());
            if (obj == null) return NotFound();
            return View(obj);
        }

        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _AuthRepo.Find(id.GetValueOrDefault());
            if (obj == null) return NotFound();
            _AuthRepo.Remove(obj);
            _AuthRepo.Save();
            return RedirectToAction("Index");
        }
    }
}
