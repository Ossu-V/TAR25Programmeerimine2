using System;
using System.Collections.Generic;

namespace ShopTARpe25.Models
{
    public class ReaderLoansViewModel
    {
        public string ReaderName { get; set; }
        public List<LoanItemViewModel> Loans { get; set; } = new();
    }

    public class LoanItemViewModel
    {
        public Guid Id { get; set; }
        public string BookTitle { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Status { get; set; }
    }
}