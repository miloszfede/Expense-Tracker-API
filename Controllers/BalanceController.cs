using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExpanseTracker.Controllers;

[ApiController]
[Route("Balance")]
public class BalanceController : ControllerBase
{

    /// <summary>
    /// Get total balance
    /// </summary>
    /// <returns>Total balance</returns>
    [HttpGet("total")]
    public ActionResult<decimal> GetBalance()
    {
        var totalBalance = Income.CountAllIncomes() - Expense.CountAllExpenses();
        return Ok(totalBalance);
    }
}