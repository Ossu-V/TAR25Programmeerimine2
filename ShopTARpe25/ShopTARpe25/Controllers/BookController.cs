using Microsoft.AspNetCore.Mvc;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Models;
using System;
using System.Linq;

namespace ShopTARpe25.Controllers
{
    public class BookController : Controller
    {
        private readonly ILibaryServices _libaryServices;

        public BookController(ILibaryServices libaryServices)
        {
            _libaryServices = libaryServices;
        }

        [HttpGet]
        public IActionResult Index(string searchTitle, int? searchYear)
        {
            var domainBooks = _libaryServices.GetAllBooks();

            if (!string.IsNullOrEmpty(searchTitle))
            {
                domainBooks = _libaryServices.GetBooksByTitle(searchTitle);
            }
            else if (searchYear.HasValue)
            {
                domainBooks = _libaryServices.GetBooksByYear(searchYear.Value);
            }

            var viewModels = domainBooks.Select(b => new BookIndexViewModel
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                PublishedYear = b.PublishedYear,
                AvailabilityStatus = b.IsAvailable ? "Saadaval" : "Laenutuses"
            }).ToList();

            return View(viewModels);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(BookCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var dto = new BookDto
                {
                    Title = viewModel.Title,
                    Author = viewModel.Author,
                    PublishedYear = viewModel.PublishedYear
                };

                _libaryServices.AddBook(dto);
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult EditTitle(Guid id, string newTitle)
        {
            _libaryServices.UpdateBookTitle(id, newTitle);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            try
            {
                _libaryServices.DeleteBook(id);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
