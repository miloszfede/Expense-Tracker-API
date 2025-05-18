using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExpanseTracker.Controllers;

[ApiController]
[Route("Expense")]
public class ExpenseController : ControllerBase
{
    public static readonly List<Expense> Expenses = new List<Expense>()
    {
        new Expense(2300, CategoryController.Categories[0], "car"),
        new Expense(100, CategoryController.Categories[1], "Cinema"),
        new Expense(1500, CategoryController.Categories[2], "Car"),
        new Expense(1000, CategoryController.Categories[3], "Food"),
        new Expense(112)
    };

    /// <summary>
    /// Get all expenses
    /// </summary>
    /// <returns>all expenses</returns>
    [HttpGet]
    public ActionResult<IEnumerable<Expense>> GetExpenses()
    {
        return Ok(Expenses.Select(i => new { i.Id, i.Amount, i.Category, i.Date, i.Note }));
    }

    /// <summary>
    /// Get expense by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Expense by Id</returns>
    [HttpGet("{id}")]
    public ActionResult GetExpenseById(Guid id)
    {
        var Expense = FindExpenseById(id);
        if (Expense == null)
        {
            return NotFound();
        }
        return Ok(new { Expense.Id, Expense.Amount, Expense.Category, Expense.Date, Expense.Note });
    }

    /// <summary>
    /// Delete expense by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Removes expense</returns>
    [HttpDelete("{id}")]
    public ActionResult DeleteExpenseById(Guid id)
    {
        var Expense = FindExpenseById(id);
        if (Expense == null)
        {
            return NotFound();
        }
        Expenses.Remove(Expense);
        return NoContent();
    }

    /// <summary>
    /// Update expense by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Updated expense</returns>
    [HttpPut("{id}")]
    public ActionResult UpdateExpenseById(Guid id, [FromBody] Expense updateExpense)
    {
        var Expense = FindExpenseById(id);
        if (Expense == null)
        {
            return NotFound();
        }
        Expense.Amount = updateExpense.Amount;
        Expense.Category = updateExpense.Category;
        Expense.Note = updateExpense.Note ?? Expense.Note;
        Expense.Date = updateExpense.Date == default ? DateTime.Now : updateExpense.Date;

        return Ok(new { Expense.Id, Expense.Amount, Expense.Category, Expense.Date, Expense.Note });
    }

    /// <summary>
    /// Add new expense
    /// </summary>
    /// <returns>New expense by Id</returns>
    [HttpPost]
    public ActionResult AddExpense([FromBody] Expense addExpense)
    {
        if (addExpense.Amount <= 0)
        {
            return BadRequest("Amount must be greater than zero.");
        }
        addExpense.Date = addExpense.Date == default ? DateTime.Now : addExpense.Date;
        Expenses.Add(addExpense);
        return CreatedAtAction(nameof(GetExpenseById), new { id = addExpense.Id }, new { addExpense.Id, addExpense.Amount, addExpense.Date, addExpense.Note });
    }

        /// <summary>
    /// Get whole amount of all expenses
    /// </summary>
    /// <returns>Whole amount of all expenses</returns>
    [HttpGet("total")]
    public ActionResult<decimal> GetTotalExpenses()
    {
        return Ok(Expense.CountAllExpenses());
    }
    private Expense? FindExpenseById(Guid id)
    {
        return Expenses.FirstOrDefault(e => e.Id == id);
    }

}