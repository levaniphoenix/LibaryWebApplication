using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public interface IBooksRepository
    {
        Book GetBook(int id);
        Author GetAuthor(int id);
        IEnumerable<Book> GetAllBooks();
        IEnumerable<Author> GetAllAuthors();
        void AddBook(Book book);
        void AddAuthor(Author author);
        void DeleteBook(int id);
        void DeleteAuthor(int id);
        void EditBook(Book book);

    }
}
