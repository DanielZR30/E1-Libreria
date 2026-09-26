using Books.Application.Utilities.Mediator;
using Books.Application.Utilities.Pagination;

namespace Books.Application.UseCases.Books.Queries.GetBooksList
{
    public class GetBooksListQuery : IRequest<PaginationResponse<BookListItemDTO>>
    {
        public PaginationRequest Pagination { get; set; } = PaginationRequest.Standart();
        public Guid? CategoryId { get; set; }
        public string? SearchTerm { get; set; }
    }
}
