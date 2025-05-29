using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpenseService _expenseService;

        public ExpensesController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        /// <summary>
        /// Get all expenses
        /// </summary>
        /// <returns>List of all expenses</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetAll()
        {
            var expenses = await _expenseService.GetAllExpensesAsync();
            return Ok(expenses);
        }

        /// <summary>
        /// Get expense by ID
        /// </summary>
        /// <param name="id">Expense ID</param>
        /// <returns>Expense with specified ID</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseDto>> GetById(int id)
        {
            var expense = await _expenseService.GetExpenseByIdAsync(id);
            if (expense == null)
            {
                return NotFound();
            }
            return Ok(expense);
        }

        /// <summary>
        /// Get expenses by user ID
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>List of expenses for specified user</returns>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetByUserId(int userId)
        {
            var expenses = await _expenseService.GetExpensesByUserIdAsync(userId);
            return Ok(expenses);
        }

        /// <summary>
        /// Get expenses by category ID
        /// </summary>
        /// <param name="categoryId">Category ID</param>
        /// <returns>List of expenses for specified category</returns>
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetByCategoryId(int categoryId)
        {
            var expenses = await _expenseService.GetExpensesByCategoryIdAsync(categoryId);
            return Ok(expenses);
        }

        /// <summary>
        /// Get expenses by date range for a specific user
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="startDate">Start date of the range</param>
        /// <param name="endDate">End date of the range</param>
        /// <returns>List of expenses within the specified date range</returns>
        [HttpGet("user/{userId}/daterange")]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetByDateRange(
            int userId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var expenses = await _expenseService.GetExpensesByDateRangeAsync(userId, startDate, endDate);
            return Ok(expenses);
        }

        /// <summary>
        /// Create a new expense
        /// </summary>
        /// <param name="createExpenseDto">Expense data to create</param>
        /// <returns>Created expense</returns>
        [HttpPost]
        public async Task<ActionResult<ExpenseDto>> Create([FromBody] CreateExpenseDto createExpenseDto)
        {
            var createdExpense = await _expenseService.CreateExpenseAsync(createExpenseDto);
            return CreatedAtAction(nameof(GetById), new { id = createdExpense.Id }, createdExpense);
        }

        /// <summary>
        /// Update an existing expense
        /// </summary>
        /// <param name="id">Expense ID to update</param>
        /// <param name="updateExpenseDto">Updated expense data</param>
        /// <returns>Updated expense</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ExpenseDto>> Update(int id, [FromBody] UpdateExpenseDto updateExpenseDto)
        {
            try
            {
                var updatedExpense = await _expenseService.UpdateExpenseAsync(id, updateExpenseDto);
                return Ok(updatedExpense);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Delete an expense
        /// </summary>
        /// <param name="id">Expense ID to delete</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _expenseService.DeleteExpenseAsync(id);
            return NoContent();
        }
    }
}