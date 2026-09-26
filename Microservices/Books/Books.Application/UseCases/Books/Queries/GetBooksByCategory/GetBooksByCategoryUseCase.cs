using Books.Application.Contracts.Repositories;
using Books.Application.UseCases.Books.Queries.GetBooksList;
using Books.Application.Utilities.Mediator;
using Books.Application.Utilities.Pagination;
using Books.Domain.Entities.Books;

namespace Books.Application.UseCases.Books.Queries.GetBooksByCategory
{
    public class GetBooksByCategoryUseCase : IRequestHandler<GetBooksByCategoryQuery, PaginationResponse<BookListItemDTO>>
    {
        private readonly IBooksRepository _repository;

        public GetBooksByCategoryUseCase(IBooksRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginationResponse<BookListItemDTO>> Handle(GetBooksByCategoryQuery query)
        {
            PaginationRequest pagination = query.Pagination;

            PaginationResponse<Book> response = await _repository.GetByCategoryPagedListAsync(
                query.CategoryId,
                pagination);

            List<BookListItemDTO> items = response.Items.Select(b => b.ToListItemDTO()).ToList();

            return PaginationResponse<BookListItemDTO>.Create(items, response.TotalCount, pagination);
        }
    }
}
