using System;
using System.Collections.Generic;
using ExpenseTracker.Domain.Common;

namespace ExpenseTracker.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
        public ICollection<Income> Incomes { get; set; } = new List<Income>();
        public ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}