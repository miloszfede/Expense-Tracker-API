using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncomesController : ControllerBase
    {
        private readonly IIncomeService _incomeService;

        public IncomesController(IIncomeService incomeService)
        {
            _incomeService = incomeService;
        }

        /// <summary>
        /// Get all incomes
        /// </summary>
        /// <returns>List of all incomes</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IncomeDto>>> GetAll()
        {
            var incomes = await _incomeService.GetAllIncomesAsync();
            return Ok(incomes);
        }

        /// <summary>
        /// Get income by ID
        /// </summary>
        /// <param name="id">Income ID</param>
        /// <returns>Income with specified ID</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<IncomeDto>> GetById(int id)
        {
            var income = await _incomeService.GetIncomeByIdAsync(id);
            if (income == null)
            {
                return NotFound();
            }
            return Ok(income);
        }

        /// <summary>
        /// Get incomes by user ID
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>List of incomes for specified user</returns>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<IncomeDto>>> GetByUserId(int userId)
        {
            var incomes = await _incomeService.GetIncomesByUserIdAsync(userId);
            return Ok(incomes);
        }

        /// <summary>
        /// Get incomes by category ID
        /// </summary>
        /// <param name="categoryId">Category ID</param>
        /// <returns>List of incomes for specified category</returns>
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<IncomeDto>>> GetByCategoryId(int categoryId)
        {
            var incomes = await _incomeService.GetIncomesByCategoryIdAsync(categoryId);
            return Ok(incomes);
        }

        /// <summary>
        /// Get incomes by date range for a specific user
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="startDate">Start date of the range</param>
        /// <param name="endDate">End date of the range</param>
        /// <returns>List of incomes within the specified date range</returns>
        [HttpGet("user/{userId}/daterange")]
        public async Task<ActionResult<IEnumerable<IncomeDto>>> GetByDateRange(
            int userId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var incomes = await _incomeService.GetIncomesByDateRangeAsync(userId, startDate, endDate);
            return Ok(incomes);
        }

        /// <summary>
        /// Create a new income
        /// </summary>
        /// <param name="createIncomeDto">Income data to create</param>
        /// <returns>Created income</returns>
        [HttpPost]
        public async Task<ActionResult<IncomeDto>> Create([FromBody] CreateIncomeDto createIncomeDto)
        {
            var createdIncome = await _incomeService.CreateIncomeAsync(createIncomeDto);
            return CreatedAtAction(nameof(GetById), new { id = createdIncome.Id }, createdIncome);
        }

        /// <summary>
        /// Update an existing income
        /// </summary>
        /// <param name="id">Income ID to update</param>
        /// <param name="updateIncomeDto">Updated income data</param>
        /// <returns>Updated income</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<IncomeDto>> Update(int id, [FromBody] UpdateIncomeDto updateIncomeDto)
        {
            try
            {
                var updatedIncome = await _incomeService.UpdateIncomeAsync(id, updateIncomeDto);
                return Ok(updatedIncome);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Delete an income
        /// </summary>
        /// <param name="id">Income ID to delete</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _incomeService.DeleteIncomeAsync(id);
            return NoContent();
        }
    }
}