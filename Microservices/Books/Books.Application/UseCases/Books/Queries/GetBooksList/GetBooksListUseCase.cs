using Books.Application.Contracts.Repositories;
using Books.Application.Utilities.Mediator;
using Books.Application.Utilities.Pagination;
using Books.Domain.Entities.Books;

namespace Books.Application.UseCases.Books.Queries.GetBooksList
{
    public class GetBooksListUseCase : IRequestHandler<GetBooksListQuery, PaginationResponse<BookListItemDTO>>
    {
        private readonly IBooksRepository _repository;

        public GetBooksListUseCase(IBooksRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginationResponse<BookListItemDTO>> Handle(GetBooksListQuery query)
        {
            PaginationRequest pagination = query.Pagination;

            PaginationResponse<Book> response = await _repository.GetPagedListAsync(
                pagination,
                query.CategoryId,
                query.SearchTerm);

            List<BookListItemDTO> items = response.Items.Select(b => b.ToListItemDTO()).ToList();

            return PaginationResponse<BookListItemDTO>.Create(items, response.TotalCount, pagination);
        }
    }
}
