using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyBookShop_DataAccess;
using MyBookShop_DataAccess.Repository.IRepository;
using MyBookShop_Models;
using MyBookShop_Models.VIewModel;
using MyBookShop_Utility;
using NuGet.ContentModel;
using System.Security.Cryptography;

namespace MyBookShop.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRep;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BookController(IBookRepository bookRep, IWebHostEnvironment webHostEnvironment)
        {
            _bookRep = bookRep;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            IEnumerable<Book> objList = _bookRep.GetAll(includeProperty:$"{WC.AuthorName},{WC.GenreName}");

            return View(objList);
        }

        public IActionResult Upsert(int? id)
        {
            VMBook VMBook = new VMBook()
            {
                Book = new Book(),
                AuthorSelectList = _bookRep.GetDropDownList(WC.AuthorName),
                GenreSelectList = _bookRep.GetDropDownList(WC.GenreName)
            };

            if (id != null)                       
            {
                VMBook.Book= _bookRep.Find(id.GetValueOrDefault());
                if (VMBook.Book==null) return NotFound();
            } 
            return View(VMBook);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(VMBook BookVM)              
        {
            var files = HttpContext.Request.Form.Files;             //возвращает список файлов которые мы выбрали
            string upload = _webHostEnvironment.WebRootPath + WC.ImagePath; //собирается полный путь до папки гду будут хранитья картинки
            string fileName = Guid.NewGuid().ToString(); //делаем уникальное имя
            if (BookVM.Book.BookId == 0)
            {

                string extension = Path.GetExtension(files[0].FileName);  
                using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);  
                }

                BookVM.Book.Image = fileName + extension; //добавляем ссылку на файл

                _bookRep.Add(BookVM.Book);             //добавляем данный в дбконтекст
                TempData[WC.Success] = "Данные по книге успешно добавлены";
            }
            else //нужно обновить объект
            {
                var objfromDB = _bookRep.FirstODefault(u => u.BookId == BookVM.Book.BookId , isTracking: false);
                if (files.Count > 0)
                {
                  //  string upload = webRootPath + WC.ImagePath;
                //    string fileName = Guid.NewGuid().ToString(); //делаем уникальное имя
                    string extension = Path.GetExtension(files[0].FileName);   //получаем тип файла
                    var oldFile = Path.Combine(upload, objfromDB.Image);
                    if (System.IO.File.Exists(oldFile))
                    {
                        System.IO.File.Delete(oldFile);
                    }


                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);  //копируем файл
                    }
                    BookVM.Book.Image = fileName + extension;

                }
                else
                {
                    BookVM.Book.Image = objfromDB.Image;
                }
                TempData[WC.Success] = "Данные по книге успешно обновлены";
                _bookRep.Update(BookVM.Book);

            }
            _bookRep.Save();

            return RedirectToAction("Index");          // переходим на метод индекс чтобы вернуться на экран с категориями

            //}
            BookVM.AuthorSelectList = _bookRep.GetDropDownList(WC.AuthorName);
          BookVM.GenreSelectList = _bookRep.GetDropDownList(WC.GenreName);
            return View(BookVM); ;          // переходим на метод индекс чтобы вернуться на экран с категориями
        }



        //GET - DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
       //     Product product = _ProdRep.FirstODefault(u => u.Id == id, includeProperty: "Category,ApplicationType");
            var obj = _bookRep.FirstODefault(u => u.BookId == id, includeProperty: $"{WC.AuthorName},{WC.GenreName}");
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
            var obj = _bookRep.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            _bookRep.Remove(obj);
            TempData[WC.Success] = "Данные по книге удалены";
            _bookRep.Save();
            return RedirectToAction("Index");


        }




    }
}
