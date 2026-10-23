using Microsoft.AspNetCore.Mvc;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Models;
using System;
using System.Linq;

namespace ShopTARpe25.Controllers
{
    public class LibaryController : Controller
    {
        private readonly ILibaryServices _libaryServices;

        public LibaryController(ILibaryServices libaryServices)
        {
            _libaryServices = libaryServices;
        }

        [HttpGet]
        public IActionResult Index(string? searchTitle = null, int? searchYear = null)
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
            return View("CreateBook");
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

        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            var book = _libaryServices.GetAllBooks().FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            var viewModel = new BookDeleteViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                PublishedYear = book.PublishedYear,
                AvailabilityStatus = book.IsAvailable ? "Saadaval" : "Laenutuses"
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                _libaryServices.DeleteBook(id);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                var book = _libaryServices.GetAllBooks().FirstOrDefault(b => b.Id == id);
                var viewModel = new BookDeleteViewModel
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    PublishedYear = book.PublishedYear,
                    AvailabilityStatus = book.IsAvailable ? "Saadaval" : "Laenutuses"
                };
                return View("Delete", viewModel);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult BorrowedBooks()
        {
            var domainBooks = _libaryServices.GetBorrowedBooks();
            var viewModels = domainBooks.Select(b => new BookIndexViewModel
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                PublishedYear = b.PublishedYear,
                AvailabilityStatus = "Laenutuses"
            }).ToList();
            return View(viewModels);
        }

        [HttpPost]
        public IActionResult ReturnBook(Guid id)
        {
            _libaryServices.ReturnBook(id);
            return RedirectToAction(nameof(BorrowedBooks));
        }

        [HttpGet]
        public IActionResult CreateReader()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateReader(string fullName, string email)
        {
            if (!string.IsNullOrEmpty(fullName) && !string.IsNullOrEmpty(email))
            {
                _libaryServices.AddReader(fullName, email);
                return RedirectToAction(nameof(Readers));
            }
            return View();
        }

        [HttpGet]
        public IActionResult Readers()
        {
            var domainReaders = _libaryServices.GetAllReaders();
            var viewModels = domainReaders.Select(r => new ReaderIndexViewModel
            {
                Id = r.Id,
                FullName = r.FullName,
                Email = r.Email
            }).ToList();
            return View(viewModels);
        }

        [HttpGet]
        public IActionResult ReaderLoans(Guid id)
        {
            var reader = _libaryServices.GetAllReaders().FirstOrDefault(r => r.Id == id);
            if (reader == null)
            {
                return NotFound();
            }

            var loans = _libaryServices.GetLoansByReader(id);
            var viewModel = new ReaderLoansViewModel
            {
                ReaderName = reader.FullName,
                Loans = loans.Select(l => new LoanItemViewModel
                {
                    Id = l.Id,
                    BookTitle = l.Book != null ? l.Book.Title : "Tundmatu raamat",
                    LoanDate = l.LoanDate,
                    ReturnDate = l.ReturnDate,
                    Status = l.ReturnDate.HasValue ? "Tagastatud" : "Käes"
                }).ToList()
            };
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult CreateLoan(Guid id)
        {
            var book = _libaryServices.GetAllBooks().FirstOrDefault(b => b.Id == id);
            if (book == null || !book.IsAvailable)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.BookTitle = book.Title;
            ViewBag.BookId = book.Id;
            var readers = _libaryServices.GetAllReaders();
            return View(readers);
        }

        [HttpPost]
        public IActionResult CreateLoanConfirmed(Guid bookId, Guid readerId)
        {
            _libaryServices.CreateLoan(bookId, readerId);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var book = _libaryServices.GetAllBooks().FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            var viewModel = new BookEditViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                PublishedYear = book.PublishedYear
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(BookEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _libaryServices.UpdateBookTitle(viewModel.Id, viewModel.Title);
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

    }
}
