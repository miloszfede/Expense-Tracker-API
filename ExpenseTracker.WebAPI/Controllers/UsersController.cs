using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Features.Commands;
using ExpenseTracker.Application.Features.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>List of all users</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            var query = new GetAllUsersQuery();
            var users = await _mediator.Send(query);
            return Ok(users);
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>User with specified ID</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            var query = new GetUserByIdQuery { Id = id };
            var user = await _mediator.Send(query);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="createUserDto">User data to create</param>
        /// <returns>Created user</returns>
        [HttpPost]
        public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserDto createUserDto)
        {
            var command = new CreateUserCommand 
            { 
                Username = createUserDto.Username,
                Email = createUserDto.Email,
                Password = createUserDto.Password
            };
            var createdUser = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
        }

        /// <summary>
        /// Update an existing user
        /// </summary>
        /// <param name="id">User ID to update</param>
        /// <param name="updateUserDto">Updated user data</param>
        /// <returns>Updated user</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> Update(int id, [FromBody] UpdateUserDto updateUserDto)
        {
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
        /// Get user balance (total incomes - total expenses)
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>User balance information</returns>
        [HttpGet("{id}/balance")]
        public async Task<ActionResult<BalanceDto>> GetBalance(int id)
        {
            var query = new GetUserBalanceQuery { UserId = id };
            var balance = await _mediator.Send(query);
            if (balance == null)
            {
                return NotFound();
            }
            return Ok(balance);
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="id">User ID to delete</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
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