using System;
using ExpenseTracker.Domain.Common;

namespace ExpenseTracker.Domain.Entities
{
    public class Income : BaseEntity
    {
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public Guid CategoryId { get; set; }
        public User User { get; set; }
        public Category Category { get; set; }
    }
}