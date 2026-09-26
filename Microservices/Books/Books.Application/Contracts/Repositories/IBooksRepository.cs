using Books.Application.Utilities.Pagination;
using Books.Domain.Entities.Books;

namespace Books.Application.Contracts.Repositories
{
    public interface IBooksRepository : IRepository<Book>
    {
        Task<PaginationResponse<Book>> GetPagedListAsync(PaginationRequest request,
                                                         Guid? categoryId = null,
                                                         string? searchTerm = null,
                                                         CancellationToken cancellationToken = default);

        Task<Book?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);

        Task<PaginationResponse<Book>> GetByCategoryPagedListAsync(Guid categoryId,
                                                                   PaginationRequest request,
                                                                   CancellationToken cancellationToken = default);
    }
}
