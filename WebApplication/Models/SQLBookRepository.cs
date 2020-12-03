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
            Author author = context.Authors.Single(c=> c.Id==book.AuthorId);
            book.AuthorFullName = author.Name + " " + author.Surname;

            context.Books.Add(book);
            context.SaveChanges();
        }
        public IEnumerable<Author> GetAllAuthors()
        {
            return context.Authors;
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return context.Books;
        }

        public Book GetBook(int Id)
        {
            Book book = context.Books.Find(Id);
            if(book!=null)
            {
                return book;
            }
            return null;
        }
        public Author GetAuthor(int Id)
        {
            Author author = context.Authors.Find(Id);
            if (author != null) return author;
            return null;
        }

        public void DeleteBook(int Id)
        {
            Book book = context.Books.Find(Id);
            if(book!=null)
            {
                context.Books.Remove(book);
                context.SaveChanges();
            }
        }
        public void DeleteAuthor(int Id)
        {
            Author author = context.Authors.Find(Id);
            if(author!=null)
            {
                context.Authors.Remove(author);
                context.SaveChanges();
            }
        }
        public void EditBook(Book book)
        {
            Author author = context.Authors.Single(c => c.Id == book.AuthorId);
            book.AuthorFullName = author.Name + " " + author.Surname;

            var bookChanges = context.Books.Attach(book);
            bookChanges.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
        }
    }
}
