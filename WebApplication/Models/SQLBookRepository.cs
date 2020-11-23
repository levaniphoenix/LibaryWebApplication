using DAL;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class SQLBookRepository : IBooksRepository
    {
        private readonly AppDbContext context;

        public SQLBookRepository(AppDbContext context)
        {
            this.context = context;
        }

        public void AddAuthor(Author author)
        {
            context.Authors.Add(author);
            context.SaveChanges();
        }

        public void AddBook(Book book)
        {
            
        }
        public IEnumerable<Author> GetAllAuthors()
        {
            return context.Authors;
        }

        public IEnumerable<Book> GetAllBooks()
        {
            throw new NotImplementedException();
        }

        public Book GetBook(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
