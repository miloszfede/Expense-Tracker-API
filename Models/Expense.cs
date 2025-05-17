using ExpanseTracker.Controllers;

namespace ExpenseTracker.Models;

public class Expense
{
    public Guid Id { get; private set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public string? Note { get; set; }

    public Category? Category { get; set; }

    public Expense()
    {
        Id = Guid.NewGuid();
    }

    public Expense(decimal amount, Category? category = null, string? note = null, DateTime? date = null)
    {
        Id = Guid.NewGuid();
        Amount = amount;
        Note = note;
        Date = date ?? DateTime.Now;
        Category = category;
    }

    public static decimal CountAllExpenses()
    {
        return ExpenseController.Expenses.Sum(e => e.Amount);
    }
}