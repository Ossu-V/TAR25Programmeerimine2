using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShopTARpe25.ApplicationServices.Services
{
    public class LibaryServices : ILibaryServices
    {
        private readonly ShopTARpe25Context _context;

        public LibaryServices(ShopTARpe25Context context)
        {
            _context = context;
        }

        public void AddBook(BookDto dto)
        {
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Author = dto.Author,
                PublishedYear = dto.PublishedYear,
                IsAvailable = true
            };
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public IEnumerable<Book> GetBooksByYear(int year)
        {
            return _context.Books.Where(b => b.PublishedYear == year).ToList();
        }

        public IEnumerable<Book> GetBooksByTitle(string title)
        {
            return _context.Books.Where(b => b.Title.ToLower().Contains(title.ToLower())).ToList();
        }

        public IEnumerable<Book> GetBorrowedBooks()
        {
            return _context.Books.Where(b => b.IsAvailable == false).ToList();
        }

        public IEnumerable<Loan> GetLoansByReader(Guid readerId)
        {
            return _context.Loans.Include(l => l.Book).Where(l => l.ReaderId == readerId).ToList();
        }

        public void UpdateBookTitle(Guid bookId, string newTitle)
        {
            var book = _context.Books.Find(bookId);
            if (book != null)
            {
                book.Title = newTitle;
                _context.SaveChanges();
            }
        }

        public void ReturnBook(Guid loanId)
        {
            var loan = _context.Loans.Include(l => l.Book).FirstOrDefault(l => l.Id == loanId);
            if (loan != null && loan.ReturnDate == null)
            {
                loan.ReturnDate = DateTime.Now;
                loan.Book.IsAvailable = true;
                _context.SaveChanges();
            }
        }

        public void DeleteBook(Guid bookId)
        {
            var book = _context.Books.Include(b => b.Loans).FirstOrDefault(b => b.Id == bookId);
            if (book != null)
            {
                if (book.Loans.Any(l => l.ReturnDate == null))
                {
                    throw new InvalidOperationException("Raamat on välja laenutatud!");
                }
                _context.Books.Remove(book);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return _context.Books.ToList();
        }

        public void AddReader(string fullName, string email)
        {
            var reader = new Reader
            {
                Id = Guid.NewGuid(),
                FullName = fullName,
                Email = email
            };
            _context.Readers.Add(reader);
            _context.SaveChanges();
        }

        public IEnumerable<Reader> GetAllReaders()
        {
            return _context.Readers.ToList();
        }

        public void CreateLoan(Guid bookId, Guid readerId)
        {
            var book = _context.Books.Find(bookId);
            if (book != null && book.IsAvailable)
            {
                var loan = new Loan
                {
                    Id = Guid.NewGuid(),
                    BookId = bookId,
                    ReaderId = readerId,
                    LoanDate = DateTime.Now,
                    ReturnDate = null
                };
                book.IsAvailable = false;
                _context.Loans.Add(loan);
                _context.SaveChanges();
            }
        }
    }
}
