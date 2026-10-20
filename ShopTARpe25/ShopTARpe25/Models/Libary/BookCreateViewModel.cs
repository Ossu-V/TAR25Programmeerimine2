using System.ComponentModel.DataAnnotations;

namespace ShopTARpe25.Models
{
    public class BookCreateViewModel
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Author { get; set; } = string.Empty;

        [Required]
        [Range(1, 2026)]
        public int PublishedYear { get; set; }
    }
}
