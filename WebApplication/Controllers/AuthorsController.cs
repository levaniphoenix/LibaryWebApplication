using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IBooksRepository booksRepository;
        private readonly IWebHostEnvironment hostingEnvironment;

        public AuthorsController(IBooksRepository booksRepository, IWebHostEnvironment hostingEnvironment)
        {
            this.booksRepository = booksRepository;
            this.hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View(booksRepository.GetAllAuthors());
        }
        [Authorize(Roles = "admin")]
        public IActionResult Edit(int id)
        {
            var author = booksRepository.GetAuthor(id);
            var model = new AuthorEditViewModel
            { 
                Name=author.Name,
                Surname=author.Surname,
                Birthplace=author.Birthplace,
                oldPhotoPath=author.PhotoPath,
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(AuthorEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = model.oldPhotoPath;
                if (model.Photo != null)
                {
                    string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    if(model.oldPhotoPath!=null)
                        System.IO.File.Delete(Path.Combine(uploadFolder, uniqueFileName));

                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    string filePath = Path.Combine(uploadFolder, uniqueFileName);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    model.Photo.CopyTo(fs);
                    fs.Close();
                }
                var author = new Author
                { 
                    Id=model.Id,
                    Name=model.Name,
                    Surname=model.Surname,
                    Birthplace=model.Birthplace,
                    PhotoPath=uniqueFileName
                };
                booksRepository.EditAuthor(author);
                return RedirectToAction("index");
            }
            return View(model);
        }
        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(AuthorCreateViewModel model)
        {
            if(ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (model.Photo != null)
                {
                    string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    string filePath = Path.Combine(uploadFolder, uniqueFileName);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    model.Photo.CopyTo(fs);
                    fs.Close();
                }
                var author = new Author
                {
                    Name=model.Name,
                    Surname=model.Surname,
                    Birthplace=model.Birthplace,
                    PhotoPath=uniqueFileName
                };
                booksRepository.AddAuthor(author);
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Details(int id)
        {
            var author = booksRepository.GetAuthor(id);
            return View(author);
        }
        [Authorize(Roles = "admin")]
        public IActionResult Delete(int id)
        {
            var author = booksRepository.GetAuthor(id);

            //delete the image
            string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
            if(author.PhotoPath!=null)
                System.IO.File.Delete(Path.Combine(uploadFolder, author.PhotoPath));

            //delete the book from databse
            booksRepository.DeleteAuthor(id);

            return RedirectToAction("Index");
        }
    }
}
