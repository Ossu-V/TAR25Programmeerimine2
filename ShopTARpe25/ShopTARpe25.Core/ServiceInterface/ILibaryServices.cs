using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using System;
using System.Collections.Generic;

namespace ShopTARpe25.Core.ServiceInterface
{
    public interface ILibaryServices
    {
        void AddBook(BookDto dto);
        IEnumerable<Book> GetBooksByYear(int year);
        IEnumerable<Book> GetBooksByTitle(string title);
        IEnumerable<Book> GetBorrowedBooks();
        IEnumerable<Loan> GetLoansByReader(Guid readerId);
        void UpdateBookTitle(Guid bookId, string newTitle);
        void ReturnBook(Guid loanId);
        void DeleteBook(Guid bookId);

        IEnumerable<Book> GetAllBooks();

    }
}
