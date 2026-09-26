using Books.Application.UseCases.Books.Queries.GetBookById;
using Books.Application.UseCases.Books.Queries.GetBooksByCategory;
using Books.Application.UseCases.Books.Queries.GetBooksList;
using Books.Application.Utilities.Mediator;
using Books.Application.Utilities.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Books.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BooksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Query 1: Consultar todos los libros con paginación y filtros opcionales.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] int pageNumber = 1,
                                                 [FromQuery] int pageSize = PaginationRequest.DEFAULT_PAGE_SIZE,
                                                 [FromQuery] Guid? categoryId = null,
                                                 [FromQuery] string? searchTerm = null)
        {
            GetBooksListQuery query = new()
            {
                Pagination = new PaginationRequest(pageNumber, pageSize),
                CategoryId = categoryId,
                SearchTerm = searchTerm
            };

            PaginationResponse<BookListItemDTO> result = await _mediator.Send(query);

            return StatusCode(StatusCodes.Status200OK, result);
        }

        /// <summary>
        /// Query 2: Consultar la información detallada de un libro por su ID.
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            GetBookByIdQuery query = new(id);

            BookDetailsDTO? result = await _mediator.Send(query);

            if (result is null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { message = $"No se encontró un libro con el identificador '{id}'." });
            }

            return StatusCode(StatusCodes.Status200OK, result);
        }

        /// <summary>
        /// Query 3: Consultar libros pertenecientes a una categoría determinada.
        /// </summary>
        [HttpGet("category/{categoryId:guid}")]
        public async Task<IActionResult> GetByCategory([FromRoute] Guid categoryId,
                                                       [FromQuery] int pageNumber = 1,
                                                       [FromQuery] int pageSize = PaginationRequest.DEFAULT_PAGE_SIZE)
        {
            GetBooksByCategoryQuery query = new(categoryId, new PaginationRequest(pageNumber, pageSize));

            PaginationResponse<BookListItemDTO> result = await _mediator.Send(query);

            return StatusCode(StatusCodes.Status200OK, result);
        }
    }
}
