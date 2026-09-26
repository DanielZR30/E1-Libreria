using Books.Application.UseCases.Books.Queries.GetBooksList;
using Books.Application.Utilities.Mediator;
using Books.Application.Utilities.Pagination;

namespace Books.Application.UseCases.Books.Queries.GetBooksByCategory
{
    public class GetBooksByCategoryQuery : IRequest<PaginationResponse<BookListItemDTO>>
    {
        public Guid CategoryId { get; set; }
        public PaginationRequest Pagination { get; set; } = PaginationRequest.Standart();

        public GetBooksByCategoryQuery(Guid categoryId, PaginationRequest? pagination = null)
        {
            CategoryId = categoryId;
            if (pagination is not null)
            {
                Pagination = pagination;
            }
        }

        public GetBooksByCategoryQuery() { }
    }
}
