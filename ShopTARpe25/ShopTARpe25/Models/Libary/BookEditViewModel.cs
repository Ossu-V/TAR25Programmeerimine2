using System;
using System.ComponentModel.DataAnnotations;

namespace ShopTARpe25.Models
{
    public class BookEditViewModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Author { get; set; }
        public int PublishedYear { get; set; }
    }
}