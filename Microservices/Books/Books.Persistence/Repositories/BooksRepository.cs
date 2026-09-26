using Books.Application.Contracts.Repositories;
using Books.Application.Utilities.Pagination;
using Books.Domain.Entities.Books;
using Books.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Books.Persistence.Repositories
{
    public class BooksRepository : Repository<Book>, IBooksRepository
    {
        private readonly DataContext _context;

        public BooksRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PaginationResponse<Book>> GetPagedListAsync(PaginationRequest request,
                                                                      Guid? categoryId = null,
                                                                      string? searchTerm = null,
                                                                      CancellationToken cancellationToken = default)
        {
            IQueryable<Book> query = _context.Books
                                             .Include(b => b.Author)
                                             .Include(b => b.Category)
                                             .AsNoTracking();

            if (categoryId.HasValue)
            {
                query = query.Where(b => b.CategoryId == categoryId.Value);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                string term = searchTerm.Trim().ToLower();
                query = query.Where(b => b.Title.ToLower().Contains(term) ||
                                         b.Author.Name.ToLower().Contains(term));
            }

            query = query.OrderBy(b => b.Title);

            return await query.ToPagedListAsync(request, cancellationToken);
        }

        public async Task<Book?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Books
                                 .Include(b => b.Author)
                                 .Include(b => b.Category)
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }

        public async Task<PaginationResponse<Book>> GetByCategoryPagedListAsync(Guid categoryId,
                                                                               PaginationRequest request,
                                                                               CancellationToken cancellationToken = default)
        {
            IQueryable<Book> query = _context.Books
                                             .Include(b => b.Author)
                                             .Include(b => b.Category)
                                             .AsNoTracking()
                                             .Where(b => b.CategoryId == categoryId)
                                             .OrderBy(b => b.Title);

            return await query.ToPagedListAsync(request, cancellationToken);
        }
    }
}
