using System;
using System.Collections.Generic;

namespace ShopTARpe25.Models
{
    public class ReaderLoansViewModel
    {
        public string ReaderName { get; set; } = string.Empty;
        public List<LoanItemViewModel> Loans { get; set; } = new List<LoanItemViewModel>();
    }

    public class LoanItemViewModel
    {
        public Guid Id { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
