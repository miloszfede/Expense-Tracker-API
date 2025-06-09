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
    public class IncomesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ExpenseTracker.Infrastructure.Services.IAuthorizationService _authorizationService;

        public IncomesController(IMediator mediator, ExpenseTracker.Infrastructure.Services.IAuthorizationService authorizationService)
        {
            _mediator = mediator;
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// Get all incomes (Admin only)
        /// </summary>
        /// <returns>List of all incomes</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<IncomeDto>>> GetAll()
        {
            var query = new GetAllIncomesQuery();
            var incomes = await _mediator.Send(query);
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
            var query = new GetIncomeByIdQuery { Id = id };
            var income = await _mediator.Send(query);
            if (income == null)
            {
                return NotFound();
            }

            if (!_authorizationService.CanUserAccessResource(User, income.UserId))
            {
                return Forbid();
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
            if (!_authorizationService.CanUserAccessResource(User, userId))
            {
                return Forbid();
            }

            var query = new GetIncomesByUserIdQuery { UserId = userId };
            var incomes = await _mediator.Send(query);
            return Ok(incomes);
        }

        /// <summary>
        /// Get incomes by category
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="categoryId">Category ID</param>
        /// <returns>List of incomes for specified category</returns>
        [HttpGet("user/{userId}/category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<IncomeDto>>> GetByCategory(int userId, int categoryId)
        {
            if (!_authorizationService.CanUserAccessResource(User, userId))
            {
                return Forbid();
            }

            var query = new GetIncomesByCategoryQuery { UserId = userId, CategoryId = categoryId };
            var incomes = await _mediator.Send(query);
            return Ok(incomes);
        }

        /// <summary>
        /// Get incomes by date range
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <returns>List of incomes in specified date range</returns>
        [HttpGet("user/{userId}/daterange")]
        public async Task<ActionResult<IEnumerable<IncomeDto>>> GetByDateRange(int userId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (!_authorizationService.CanUserAccessResource(User, userId))
            {
                return Forbid();
            }

            var query = new GetIncomesByDateRangeQuery { UserId = userId, StartDate = startDate, EndDate = endDate };
            var incomes = await _mediator.Send(query);
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
            var currentUserId = _authorizationService.GetCurrentUserId(User);
            if (!currentUserId.HasValue)
            {
                return Unauthorized();
            }

            var command = new CreateIncomeCommand 
            { 
                Amount = createIncomeDto.Amount,
                Date = createIncomeDto.Date,
                Note = createIncomeDto.Note,
                CategoryId = createIncomeDto.CategoryId,
                UserId = currentUserId.Value 
            };
            var createdIncome = await _mediator.Send(command);
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
                var existingIncomeQuery = new GetIncomeByIdQuery { Id = id };
                var existingIncome = await _mediator.Send(existingIncomeQuery);
                if (existingIncome == null)
                {
                    return NotFound();
                }

                if (!_authorizationService.CanUserAccessResource(User, existingIncome.UserId))
                {
                    return Forbid();
                }

                var command = new UpdateIncomeCommand 
                { 
                    Id = id,
                    Amount = updateIncomeDto.Amount,
                    Date = updateIncomeDto.Date,
                    Note = updateIncomeDto.Note,
                    CategoryId = updateIncomeDto.CategoryId
                };
                var updatedIncome = await _mediator.Send(command);
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
            var existingIncomeQuery = new GetIncomeByIdQuery { Id = id };
            var existingIncome = await _mediator.Send(existingIncomeQuery);
            if (existingIncome == null)
            {
                return NotFound();
            }

            if (!_authorizationService.CanUserAccessResource(User, existingIncome.UserId))
            {
                return Forbid();
            }

            var command = new DeleteIncomeCommand { Id = id };
            var result = await _mediator.Send(command);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}