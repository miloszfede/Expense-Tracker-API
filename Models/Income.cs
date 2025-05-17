using ExpanseTracker.Controllers;

namespace ExpenseTracker.Models;

public class Income
{
    public Guid Id { get; private set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public string? Note { get; set; }

    public Income()
    {
        Id = Guid.NewGuid();
    }

    public Income(decimal amount, string? note = null, DateTime? date = null)
    {
        Id = Guid.NewGuid();
        Amount = amount;
        Note = note;
        Date = date ?? DateTime.Now;
    }
    
    public static decimal CountAllIncomes()
    {
        return IncomeController.Incomes.Sum(e => e.Amount);
    }
}