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
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all users (Admin only)
        /// </summary>
        /// <returns>List of all users</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            var query = new GetAllUsersQuery();
            var users = await _mediator.Send(query);
            return Ok(users);
        }

        /// <summary>
        /// Get user by ID (Users can only access their own data, Admins can access any)
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>User with specified ID</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var currentUserRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            // Users can only access their own data, admins can access any
            if (currentUserRole != "Admin" && currentUserId != id.ToString())
            {
                return Forbid();
            }

            var query = new GetUserByIdQuery { Id = id };
            var user = await _mediator.Send(query);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        /// <summary>
        /// Update an existing user (Users can only update their own data, Admins can update any)
        /// </summary>
        /// <param name="id">User ID to update</param>
        /// <param name="updateUserDto">Updated user data</param>
        /// <returns>Updated user</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> Update(int id, [FromBody] UpdateUserDto updateUserDto)
        {
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var currentUserRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            // Users can only update their own data, admins can update any
            if (currentUserRole != "Admin" && currentUserId != id.ToString())
            {
                return Forbid();
            }

            try
            {
                var command = new UpdateUserCommand 
                { 
                    Id = id,
                    Username = updateUserDto.Username,
                    Email = updateUserDto.Email
                };
                var updatedUser = await _mediator.Send(command);
                return Ok(updatedUser);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Get user balance (total incomes - total expenses) (Users can only access their own balance, Admins can access any)
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>User balance information</returns>
        [HttpGet("{id}/balance")]
        public async Task<ActionResult<BalanceDto>> GetBalance(int id)
        {
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var currentUserRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            // Users can only access their own balance, admins can access any
            if (currentUserRole != "Admin" && currentUserId != id.ToString())
            {
                return Forbid();
            }

            var query = new GetUserBalanceQuery { UserId = id };
            var balance = await _mediator.Send(query);
            if (balance == null)
            {
                return NotFound();
            }
            return Ok(balance);
        }

        /// <summary>
        /// Delete a user (Admin only)
        /// </summary>
        /// <param name="id">User ID to delete</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var command = new DeleteUserCommand { Id = id };
            var result = await _mediator.Send(command);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}