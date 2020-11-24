using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBooksRepository booksRepository;
        public BooksController(IBooksRepository booksRepository)
        {
            this.booksRepository = booksRepository;
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
        public IActionResult Create(Book book)
        {
            if (ModelState.IsValid)
            {
                booksRepository.AddBook(book);
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int id)
        {
            booksRepository.DeleteBook(id);
            return RedirectToAction("Index");
        }
    }
}
