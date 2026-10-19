namespace ShopTARpe25.Core.Domain
{
    public class Reader
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    }
}