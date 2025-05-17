using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExpanseTracker.Controllers;

[ApiController]
[Route("Income")]
public class IncomeController : ControllerBase
{
    public static readonly List<Income> Incomes = new List<Income>()
    {
        new Income(500, "Praca"),
        new Income(100),
        new Income(1500),
        new Income(5600),
        new Income(7120)
    };

    [HttpGet]
    public ActionResult<IEnumerable<Income>> GetIncomes()
    {
        return Ok(Incomes.Select(i => new { i.Id, i.Amount, i.Date, i.Note }));
    }

    [HttpGet("{id}")]
    public ActionResult GetIncomeById(Guid id)
    {
        var income = FindIncomeById(id);
        if (income == null)
        {
            return NotFound();
        }
        return Ok(new { income.Id, income.Amount, income.Date, income.Note });
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteIncomeById(Guid id)
    {
        var income = FindIncomeById(id);
        if (income == null)
        {
            return NotFound();
        }
        Incomes.Remove(income);
        return NoContent();
    }

    [HttpPut("{id}")]
    public ActionResult UpdateIncomeById(Guid id, [FromBody] Income updateIncome)
    {
        var income = FindIncomeById(id);
        if (income == null)
        {
            return NotFound();
        }
        income.Amount = updateIncome.Amount;
        income.Note = updateIncome.Note ?? income.Note;
        income.Date = updateIncome.Date == default ? DateTime.Now : updateIncome.Date;

        return Ok(new { income.Id, income.Amount, income.Date, income.Note });
    }

    [HttpPost]
    public ActionResult AddIncome([FromBody] Income addIncome)
    {
        if (addIncome.Amount <= 0)
        {
            return BadRequest("Amount must be greater than zero.");
        }
        addIncome.Date = addIncome.Date == default ? DateTime.Now : addIncome.Date;
        Incomes.Add(addIncome);
        return CreatedAtAction(nameof(GetIncomeById), new { id = addIncome.Id }, new { addIncome.Id, addIncome.Amount, addIncome.Date, addIncome.Note });
    }

    [HttpGet("total")]
    public ActionResult<decimal> GetTotalExpenses()
    {
        return Ok(Income.CountAllIncomes());
    }

    private Income? FindIncomeById(Guid id)
    {
        return Incomes.FirstOrDefault(i => i.Id == id);
    }
    

}