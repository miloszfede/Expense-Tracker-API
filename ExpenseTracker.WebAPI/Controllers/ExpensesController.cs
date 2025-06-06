using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Features.Commands;
using ExpenseTracker.Application.Features.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ExpensesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExpensesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all expenses (Admin only)
        /// </summary>
        /// <returns>List of all expenses</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetAll()
        {
            var query = new GetAllExpensesQuery();
            var expenses = await _mediator.Send(query);
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
            var query = new GetExpenseByIdQuery { Id = id };
            var expense = await _mediator.Send(query);
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
            var query = new GetExpensesByUserIdQuery { UserId = userId };
            var expenses = await _mediator.Send(query);
            return Ok(expenses);
        }

        /// <summary>
        /// Get expenses by category
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="categoryId">Category ID</param>
        /// <returns>List of expenses for specified category</returns>
        [HttpGet("user/{userId}/category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetByCategory(int userId, int categoryId)
        {
            var query = new GetExpensesByCategoryQuery { UserId = userId, CategoryId = categoryId };
            var expenses = await _mediator.Send(query);
            return Ok(expenses);
        }

        /// <summary>
        /// Get expenses by date range
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <returns>List of expenses in specified date range</returns>
        [HttpGet("user/{userId}/daterange")]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetByDateRange(int userId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var query = new GetExpensesByDateRangeQuery { UserId = userId, StartDate = startDate, EndDate = endDate };
            var expenses = await _mediator.Send(query);
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
            var command = new CreateExpenseCommand 
            { 
                Amount = createExpenseDto.Amount,
                Date = createExpenseDto.Date,
                Note = createExpenseDto.Note,
                CategoryId = createExpenseDto.CategoryId,
                UserId = createExpenseDto.UserId
            };
            var createdExpense = await _mediator.Send(command);
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
                var command = new UpdateExpenseCommand 
                { 
                    Id = id,
                    Amount = updateExpenseDto.Amount,
                    Date = updateExpenseDto.Date,
                    Note = updateExpenseDto.Note,
                    CategoryId = updateExpenseDto.CategoryId
                };
                var updatedExpense = await _mediator.Send(command);
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
            var command = new DeleteExpenseCommand { Id = id };
            var result = await _mediator.Send(command);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}