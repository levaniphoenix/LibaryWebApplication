using DAL.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBooksRepository booksRepository;
        private readonly IWebHostEnvironment hostingEnvironment;
        public BooksController(IBooksRepository booksRepository,
                                IWebHostEnvironment hostingEnvironment)
        {
            this.booksRepository = booksRepository;
            this.hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View(booksRepository.GetAllBooks());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(BookCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if(model.Photo!=null)
                {
                    string uploadFolder=Path.Combine(hostingEnvironment.WebRootPath, "images");
                    uniqueFileName=Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    string filePath=Path.Combine(uploadFolder, uniqueFileName);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    model.Photo.CopyTo(fs);
                    fs.Close();
                }
                Book book = new Book
                {
                    Name = model.Name,
                    Publisher = model.Publisher,
                    AuthorId = model.AuthorId,
                    Condition = model.Condition,
                    Quantity = model.Quantity,
                    PhotoPath = uniqueFileName,
                };
                booksRepository.AddBook(book);
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int id)
        {
            var book = booksRepository.GetBook(id);
            
            //delete the image
            string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
            if (book.PhotoPath != null)
                System.IO.File.Delete(Path.Combine(uploadFolder, book.PhotoPath));
            
            //delete the book from databse
            booksRepository.DeleteBook(id);
            
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var book = booksRepository.GetBook(id);
            BookEditViewModel model = new BookEditViewModel
            {
                Id = book.Id,
                Name = book.Name,
                Publisher = book.Publisher,
                AuthorId = book.AuthorId,
                Condition = book.Condition,
                Quantity = book.Quantity,
                oldPhotoPath = book.PhotoPath,
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(BookEditViewModel model)
        {
            if(ModelState.IsValid)
            {
                string uniqueFileName = model.oldPhotoPath;
                if (model.Photo != null)
                {
                    string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    if (model.oldPhotoPath != null)
                        System.IO.File.Delete(Path.Combine(uploadFolder, uniqueFileName));

                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    string filePath = Path.Combine(uploadFolder, uniqueFileName);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    model.Photo.CopyTo(fs);
                    fs.Close();
                }
                    Book book = new Book
                    {
                        Id = model.Id,
                        Name = model.Name,
                        Publisher = model.Publisher,
                        AuthorId = model.AuthorId,
                        Condition = model.Condition,
                        Quantity = model.Quantity,
                        PhotoPath = uniqueFileName,
                    };
                booksRepository.EditBook(book);  
                return RedirectToAction("index");
            }
            return View(model);
        }
        public IActionResult Details(int id)
        {
            Book book = booksRepository.GetBook(id);
            return View(book);
        }

    }
}
