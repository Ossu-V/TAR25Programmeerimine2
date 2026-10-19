namespace ShopTARpe25.Core.Domain
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int PublishedYear { get; set; }
        public string IsAvailable { get; set; }

        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    }
}