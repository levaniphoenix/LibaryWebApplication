using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public interface IBooksRepository
    {
        Book GetBook(int Id);
        IEnumerable<Book> GetAllBooks();
        IEnumerable<Author> GetAllAuthors();
        void AddBook(Book book);
        void AddAuthor(Author author);

    }
}
