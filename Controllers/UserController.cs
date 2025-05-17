using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExpanseTracker.Controllers;

[ApiController]
[Route("User")]

public class UserController : ControllerBase
{
    private static readonly List<User> Users = new List<User>
    {
        new User("wojtek", "wojtek@gmail.com", "passw0rd"),
        new User("maciek", "maciek@gmail.com", "passw0rddd"),
        new User("krzysiek", "krzysiek@gmail.com", "pppassw0rd"),
        new User("krystian", "krystian@gmail.com", "pAAassw0rd"),
        new User("michal", "michal@gmail.com", "HASLO")
    };

    [HttpGet]
    public ActionResult<IEnumerable<User>> GetUsers()
    {
        return Ok(Users.Select(u => new { u.Id, u.Username, u.Email }));
    }

    [HttpGet("{id}")]
    public ActionResult GetUserById(Guid id)
    {
        var user = FindUserById(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(new { user.Id, user.Username, user.Email });
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteUserById(Guid id)
    {
        var user = FindUserById(id);
        if (user == null)
        {
            return NotFound();
        }
        Users.Remove(user);
        return NoContent();
    }

    [HttpPut("{id}")]
    public ActionResult UpdateUserById(Guid id, [FromBody] User updateUser)
    {
        var user = FindUserById(id);
        if (user == null)
        {
            return NotFound();
        }

        user.Username = updateUser.Username;
        user.Email = updateUser.Email;

        if (!string.IsNullOrWhiteSpace(updateUser.PasswordHash))
        {
            user.SetPassword(updateUser.PasswordHash);
        }

        return Ok(new { user.Id, user.Username, user.Email });
    }

    [HttpPost]
    public ActionResult AddUser([FromBody] User addUser)
    {
        if (string.IsNullOrWhiteSpace(addUser.Username) || string.IsNullOrWhiteSpace(addUser.Email))
        {
            return BadRequest("Username and Email are required.");
        }

        if (string.IsNullOrWhiteSpace(addUser.PasswordHash))
        {
            return BadRequest("Password is required.");
        }

        var newUser = new User(addUser.Username, addUser.Email, addUser.PasswordHash);
        Users.Add(newUser);

        return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, new { newUser.Id, newUser.Username, newUser.Email });
    }
    private User? FindUserById(Guid id)
    {
        return Users.FirstOrDefault(u => u.Id == id);
    }
}