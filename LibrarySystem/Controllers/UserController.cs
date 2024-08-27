using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Request;
using Service.Interface;

namespace LibrarySystem.Controllers
{
    [Route("api/")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _service;
        public UserController(IUserService service) 
        {
            _service = service;
        }

        [HttpGet("users")]
        [Authorize]
        public IActionResult GetAllUsers()
        {
            var response = _service.GetAllUser();
            return Ok(response);
        }

        [HttpGet("users/{id}")]
        [Authorize]
        public IActionResult GetUserById([FromRoute] int id)
        {
            var response = _service.GetUserById(id);
            if (response.Result == null)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpDelete("users/{id}")]
        [Authorize]
        public IActionResult DeleteUserById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var response = _service.DeleteUserById(id);

            if (response == 0)
            {
                return BadRequest($"Can not delete user No.{id}");
            }
            return Ok($"Delete book No.{id} successfully");
        }

        [HttpPost("users")]
        public IActionResult CreateUser([FromBody] CreateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = _service.CreateUser(request);

            if (response.Result == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("users/{id}")]
        [Authorize]
        public IActionResult Updatebook([FromBody] UpdateUserRequest request, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var response = _service.UpdateUserById(id, request);

            if (response.Result == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
