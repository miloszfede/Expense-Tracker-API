using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Features.Commands;
using ExpenseTracker.Application.Features.Queries;
using ExpenseTracker.Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all categories
        /// </summary>
        /// <returns>List of all categories</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
        {
            var query = new GetAllCategoriesQuery();
            var categories = await _mediator.Send(query);
            return Ok(categories);
        }

        /// <summary>
        /// Get category by ID
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <returns>Category with specified ID</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetById(int id)
        {
            var query = new GetCategoryByIdQuery { Id = id };
            var category = await _mediator.Send(query);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        /// <summary>
        /// Get categories by user ID
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>List of categories for specified user</returns>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetByUserId(int userId)
        {
            var query = new GetCategoriesByUserIdQuery { UserId = userId };
            var categories = await _mediator.Send(query);
            return Ok(categories);
        }

        /// <summary>
        /// Get categories by type
        /// </summary>
        /// <param name="type">Category type (Income or Expense)</param>
        /// <returns>List of categories of specified type</returns>
        [HttpGet("type/{type}")]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetByType(CategoryType type)
        {
            var query = new GetCategoriesByTypeQuery { Type = type.ToString() };
            var categories = await _mediator.Send(query);
            return Ok(categories);
        }

        /// <summary>
        /// Create a new category
        /// </summary>
        /// <param name="createCategoryDto">Category data to create</param>
        /// <returns>Created category</returns>
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> Create([FromBody] CreateCategoryDto createCategoryDto)
        {
            try
            {
                var command = new CreateCategoryCommand 
                { 
                    Name = createCategoryDto.Name,
                    Type = createCategoryDto.Type,
                    UserId = createCategoryDto.UserId,
                    IsDefault = createCategoryDto.IsDefault
                };
                var createdCategory = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetById), new { id = createdCategory.Id }, createdCategory);
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("already exists"))
            {
                return Conflict(new { message = ex.Message });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { 
                    message = "Validation failed", 
                    errors = ex.Errors.Select(e => new { field = e.PropertyName, error = e.ErrorMessage }) 
                });
            }
        }

        /// <summary>
        /// Update an existing category
        /// </summary>
        /// <param name="id">Category ID to update</param>
        /// <param name="updateCategoryDto">Updated category data</param>
        /// <returns>Updated category</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDto>> Update(int id, [FromBody] UpdateCategoryDto updateCategoryDto)
        {
            try
            {
                var command = new UpdateCategoryCommand 
                { 
                    Id = id,
                    Name = updateCategoryDto.Name,
                    IsDefault = updateCategoryDto.IsDefault
                };
                var updatedCategory = await _mediator.Send(command);
                return Ok(updatedCategory);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Delete a category
        /// </summary>
        /// <param name="id">Category ID to delete</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var command = new DeleteCategoryCommand { Id = id };
            var result = await _mediator.Send(command);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}