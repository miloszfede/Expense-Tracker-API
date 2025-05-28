using System;
using System.Collections.Generic;
using ExpenseTracker.Domain.Common;
using ExpenseTracker.Domain.Enums;

namespace ExpenseTracker.Domain.Entities
{
    public class Category : BaseEntity
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public CategoryType Type { get; set; }
        public bool IsDefault { get; set; }

        public User User { get; set; }
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
        public ICollection<Income> Incomes { get; set; } = new List<Income>();
    }
}