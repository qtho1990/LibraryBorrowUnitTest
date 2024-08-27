using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Request;
using Repository.Response;
using Service.Interface;
using System.Security.Claims;

namespace LibrarySystem.Controllers
{
    [Route("api/")]
    [ApiController]
    public class BookRentingController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRentingService _rentingService;

        public BookRentingController(IUserService userService, IRentingService rentingService) 
        {
            _userService = userService;
            _rentingService = rentingService;
        }

        [HttpPost("renting")]
        [Authorize(Roles ="Student")]
        public IActionResult RentingBooks(RentingRequest request)
        {
            var user = (UserResponse) GetCurrentUser();
            var response = _rentingService.RentingBooks(request, user!.Id);
            if (response.Result == null)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("returning")]
        [Authorize(Roles = "Student")]
        public IActionResult ReturningBooks(ReturningRequest request)
        {
            var user = (UserResponse) GetCurrentUser();
            var response = _rentingService.ReturningBooks(request, user!.Id);

            if (response.Result == null)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }


        private object? GetCurrentUser()
        {
            var userIdString = HttpContext.User.FindFirst(ClaimTypes.Sid)!.Value;
            var response = _userService.GetUserById(Int32.Parse(userIdString));
            return response.Result;
        }
    }
}
