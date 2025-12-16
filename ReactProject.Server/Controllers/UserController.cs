using DataAccessLayer;
using DataAccessLayer.Migrations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ReactProject.Server.Controllers
{
    public class UserController : Controller
    {

        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userRepository.GetAll();
            return Ok(users);
        }

        // GET: api/user/5
        [HttpGet("GetById{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userRepository.GetById(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // POST: api/user
        [HttpPost("Create")]
        public async Task<IActionResult> Create(User user)
        {
            await _userRepository.Add(user);
            return Ok("User created successfully");
        }

        // PUT: api/user/5
        [HttpPut("Update{id}")]
        public async Task<IActionResult> Update(int id, User user)
        {
            if (id != user.Id)
                return BadRequest("Id mismatch");

            await _userRepository.Update(user);
            return Ok("User updated successfully");
        }

        // DELETE: api/user/5
        [HttpDelete("Delete{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _userRepository.Delete(id);
            return Ok("User deleted successfully");
        }

        // POST: api/user/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(User model)
        {
            var user = await _userRepository.Login(model.UserName, model.Password);

            if (user == null)
                return Unauthorized("Invalid username or password");

            return Ok(new
            {
                Message = "Login successfully",
                user.Id,
                user.UserName,
                user.Email
            });
        }
    }
}
