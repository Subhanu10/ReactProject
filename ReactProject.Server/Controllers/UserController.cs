using DataAccessLayer;
using DataAccessLayer.Migrations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ReactProject.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userRepository.GetById(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Create(User user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await _userRepository.Add(user);
                return Ok("User created successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, User user)
        {
            if (id != user.Id) return BadRequest("Id mismatch");

            try
            {
                await _userRepository.Update(user);
                return Ok("User updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _userRepository.Delete(id);
                return Ok("User deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(User model)
        {
            var user = await _userRepository.Login(model.UserName, model.Password);
            if (user == null) return Unauthorized("Invalid username or password");

            return Ok(new
            {
                Message = "Login successfully",
                user.Id,
                user.UserName,
                user.Email,
                user.RoleId
            });
        }

        [HttpPost("AddRole")]
        public async Task<IActionResult> Add(Role role)
        {
            try
            {
                await _userRepository.Add(role);
                return Ok("Role created successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}


