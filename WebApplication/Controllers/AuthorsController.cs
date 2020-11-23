using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IBooksRepository booksRepository;
        public AuthorsController(IBooksRepository booksRepository)
        {
            this.booksRepository = booksRepository;
        }
        public IActionResult Index()
        {
            return View(booksRepository.GetAllAuthors());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Author author)
        {
            if(ModelState.IsValid)
            {
                booksRepository.AddAuthor(author);
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
