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

    [HttpGet]
    public ActionResult<IEnumerable<Category>> GetCategorys()
    {
        return Ok(Categories.Select(i => new { i.Id, i.Name }));
    }

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