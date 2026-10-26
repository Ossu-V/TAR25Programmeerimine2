using System.ComponentModel.DataAnnotations;

namespace ShopTARpe25.Models
{
    public class BookCreateViewModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public int PublishedYear { get; set; }
    }
}