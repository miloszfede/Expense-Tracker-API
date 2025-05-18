using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExpanseTracker.Controllers;

[ApiController]
[Route("Category")]
public class CategoryController : ControllerBase
{
    public static readonly List<Category> Categories = new List<Category>()
    {
        new Category("Car"),
        new Category("Rent"),
        new Category("Groceries"),
        new Category("Food delivery")
    };

    /// <summary>
    /// Get all categories
    /// </summary>
    /// <returns>All categories</returns>
    [HttpGet]
    public ActionResult<IEnumerable<Category>> GetCategorys()
    {
        return Ok(Categories.Select(i => new { i.Id, i.Name }));
    }

    /// <summary>
    /// Get category by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Category by Id</returns>
    [HttpGet("{id}")]
    public ActionResult GetCategoryById(Guid id)
    {
        var Category = FindCategoryById(id);
        if (Category == null)
        {
            return NotFound();
        }
        return Ok(new { Category.Id, Category.Name });
    }

    /// <summary>
    /// Delete category by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Delete category by Id</returns>
    [HttpDelete("{id}")]
    public ActionResult DeleteCategoryById(Guid id)
    {
        var Category = FindCategoryById(id);
        if (Category == null)
        {
            return NotFound();
        }
        Categories.Remove(Category);
        return NoContent();
    }

    /// <summary>
    /// Edit category by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Edit category by Id</returns>
    [HttpPut("{id}")]
    public ActionResult UpdateCategoryById(Guid id, [FromBody] Category updateCategory)
    {
        var Category = FindCategoryById(id);
        if (Category == null)
        {
            return NotFound();
        }
        Category.Name = updateCategory.Name;
        
        return Ok(new { Category.Id, Category.Name });
    }

    /// <summary>
    /// Add new category by Id
    /// </summary>
    /// <returns>Add new category</returns>
    [HttpPost]
    public ActionResult AddCategory([FromBody] Category addCategory)
    {
        if (string.IsNullOrWhiteSpace(addCategory.Name))
        {
            return BadRequest("Name must be implemented");
        }
        Categories.Add(addCategory);
        return CreatedAtAction(nameof(GetCategoryById), new { id = addCategory.Id }, new { addCategory.Id, addCategory.Name });
    }

    private Category? FindCategoryById(Guid id)
    {
        return Categories.FirstOrDefault(e => e.Id == id);
    }
}