using System;

namespace ShopTARpe25.Models
{
    public class BookDeleteViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int PublishedYear { get; set; }
        public string AvailabilityStatus { get; set; }
    }
}