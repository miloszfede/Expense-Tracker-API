using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class AdminController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Get all users (Admin only)
        /// </summary>
        /// <returns>List of all users</returns>
        [HttpGet("users")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            try
            {
                var users = await _unitOfWork.Users.GetAllAsync();
                var userDtos = users.Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    RoleId = u.RoleId
                });

                return Ok(userDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching users", error = ex.Message });
            }
        }

        /// <summary>
        /// Get all roles (Admin only)
        /// </summary>
        /// <returns>List of all roles</returns>
        [HttpGet("roles")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllRoles()
        {
            try
            {
                var roles = await _unitOfWork.Roles.GetAllAsync();
                var roleDtos = roles.Select(r => new
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description
                });

                return Ok(roleDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching roles", error = ex.Message });
            }
        }

        /// <summary>
        /// Get system statistics (Admin only)
        /// </summary>
        /// <returns>System statistics</returns>
        [HttpGet("stats")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<object>> GetSystemStats()
        {
            try
            {
                var users = await _unitOfWork.Users.GetAllAsync();
                var categories = await _unitOfWork.Categories.GetAllAsync();
                var expenses = await _unitOfWork.Expenses.GetAllAsync();
                var incomes = await _unitOfWork.Incomes.GetAllAsync();

                var stats = new
                {
                    TotalUsers = users.Count(),
                    TotalCategories = categories.Count(),
                    TotalExpenses = expenses.Count(),
                    TotalIncomes = incomes.Count(),
                    TotalExpenseAmount = expenses.Sum(e => e.Amount),
                    TotalIncomeAmount = incomes.Sum(i => i.Amount)
                };

                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching statistics", error = ex.Message });
            }
        }

        /// <summary>
        /// Test endpoint accessible by any authenticated user
        /// </summary>
        /// <returns>Success message with user info</returns>
        [HttpGet("user-test")]
        [Authorize] 
        public ActionResult<object> UserTest()
        {
            var username = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            return Ok(new
            {
                Message = "This endpoint is accessible by any authenticated user",
                Username = username,
                Role = role,
                Timestamp = DateTime.UtcNow
            });
        }
    }
}
