using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web2_API.Data;
using Web2_API.Filters;
using Web2_API.Models.Domain;
using Web2_API.Models.DTO;
using Web2_API.Repository;

namespace Web2_API.Controllers
{

    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IBookRepository _bookRepository;

        public BooksController(AppDbContext dbContext, IBookRepository bookRepository)
        {
            _dbContext = dbContext;
            _bookRepository = bookRepository;
        }
        [AllowAnonymous]
        [HttpGet("get-all-books")]
        public IActionResult GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
[FromQuery] string? sortBy, [FromQuery] bool isAscending,
[FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 100)
        {
            // su dung reposity pattern
            var allBooks = _bookRepository.GetAllBooks(filterOn, filterQuery, sortBy,
            isAscending, pageNumber, pageSize);
            return Ok(allBooks);
        }

        [HttpGet]
        [Route("get-book-by-id/{id}")]
        public IActionResult GetBookById([FromRoute] int id)
        {
            var bookWithIdDTO = _bookRepository.GetBookById(id);
            return Ok(bookWithIdDTO);
        }
        [HttpPost("add-book")]
        [ValidateModel]

        public IActionResult AddBook([FromBody] AddBookRequestDTO addBookRequestDTO)
        {
            if (ValidateAddBook(addBookRequestDTO))
            {
                var bookAdd = _bookRepository.AddBook(addBookRequestDTO);
                return Ok(bookAdd);
            }
            return BadRequest(ModelState);
        }

        [HttpPut("update-book-by-id/{id}")]
        public IActionResult UpdateBookById(int id, [FromBody] AddBookRequestDTO bookDTO)
        {
            var updateBook = _bookRepository.UpdateBookById(id, bookDTO);
            return Ok(updateBook);
        }

        [HttpDelete("delete-book-by-id/{id}")]
        public IActionResult DeleteBookById(int id)
        {
            var deleteBook = _bookRepository.DeleteBookById(id);
            return Ok(deleteBook);
        }
        private bool ValidateAddBook(AddBookRequestDTO addBookRequestDTO)
        {
            if (addBookRequestDTO == null)
            {
                ModelState.AddModelError(nameof(addBookRequestDTO), "Please add book data");
                return false;
            }
            var publisherExists = _dbContext.Publishers.Any(p => p.Id == addBookRequestDTO.PublisherID);
            if (!publisherExists)
            {
                ModelState.AddModelError(nameof(addBookRequestDTO.PublisherID),
                    $"Publisher với ID {addBookRequestDTO.PublisherID} không tồn tại trong hệ thống.");
            }
            if (addBookRequestDTO.AuthorIds != null && addBookRequestDTO.AuthorIds.Any())
            {
                if (addBookRequestDTO.AuthorIds.Count != addBookRequestDTO.AuthorIds.Distinct().Count())
                {
                    ModelState.AddModelError(nameof(addBookRequestDTO.AuthorIds), "Không được phép gán trùng lặp cùng một Tác giả nhiều lần.");
                }
                foreach (var authorId in addBookRequestDTO.AuthorIds)
                {
                    var authorExists = _dbContext.Authors.Any(a => a.Id == authorId);
                    if (!authorExists)
                    {
                        ModelState.AddModelError(nameof(addBookRequestDTO.AuthorIds), $"Tác giả với ID {authorId} không tồn tại trong hệ thống.");
                    }
                }
            }
            if (string.IsNullOrEmpty(addBookRequestDTO.Description))
            {
                ModelState.AddModelError(nameof(addBookRequestDTO.Description), $"{nameof(addBookRequestDTO.Description)} cannot be null");
            }
            if (addBookRequestDTO.Rate < 0 || addBookRequestDTO.Rate > 5)
            {
                ModelState.AddModelError(nameof(addBookRequestDTO.Rate), $"{nameof(addBookRequestDTO.Rate)} cannot be less than 0 and more than 5");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        
        
        }
    }
}
