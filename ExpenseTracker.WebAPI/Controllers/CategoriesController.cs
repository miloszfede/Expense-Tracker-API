using System.Collections.Generic;
using System.Threading.Tasks;
using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Interfaces;
using ExpenseTracker.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Get all categories
        /// </summary>
        /// <returns>List of all categories</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
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
            var category = await _categoryService.GetCategoryByIdAsync(id);
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
            var categories = await _categoryService.GetCategoriesByUserIdAsync(userId);
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
            var categories = await _categoryService.GetCategoriesByTypeAsync(type);
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
            var createdCategory = await _categoryService.CreateCategoryAsync(createCategoryDto);
            return CreatedAtAction(nameof(GetById), new { id = createdCategory.Id }, createdCategory);
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
                var updatedCategory = await _categoryService.UpdateCategoryAsync(id, updateCategoryDto);
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
            await _categoryService.DeleteCategoryAsync(id);
            return NoContent();
        }
    }
}