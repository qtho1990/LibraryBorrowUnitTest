using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Request;
using Service.Interface;

namespace LibrarySystem.Controllers
{
    [Route("api/")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService) 
        {
            _bookService = bookService;   
        }

        [HttpGet("books")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllBooks()
        {
            var response = _bookService.GetAllBook();
            return Ok(response);
        }

        [HttpGet("books/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetBookById(int id)
        {
            var response = _bookService.GetBookById(id);
            if (response.Result == null)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("books")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateBook([FromBody] CreateBookRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = _bookService.CreateBook(request);

            if (response.Result == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("books/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Updatebook([FromBody] UpdateBookRequest request,[FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var response = _bookService.UpdateBookById(id, request);

            if (response.Result == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("books/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteBook([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = _bookService.DeleteBookById(id);

            if (response == 0)
            {
                return BadRequest($"Can not delete book No.{id}");
            }
            return Ok($"Delete book No.{id} successfully");
        }
    }
}
