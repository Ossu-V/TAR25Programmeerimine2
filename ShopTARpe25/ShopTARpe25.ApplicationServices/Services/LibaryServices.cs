using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopTARpe25.ApplicationServices.Services
{
    public class LibaryServices
    {
        private readonly ShopTARpe25Context _context;

        public LibaryServices
            (
                ShopTARpe25Context context
            )
        {
            _context = context;
        }

        public async Task AddBook(BookDto dto)
        {
            var newBook = new Book
            {
                Title = dto.Title,
                Author = dto.Author,
                PublishedYear = dto.PublishedYear,
                IsAvailable = true
            };

            await _context.Books.AddAsync(newBook);
            await _context.SaveChangesAsync();
        }

    }
}
