using Books.Application.Contracts.Repositories;
using Books.Application.UseCases.Books.Queries.GetBookById;
using Books.Application.UseCases.Books.Queries.GetBooksByCategory;
using Books.Application.UseCases.Books.Queries.GetBooksList;
using Books.Application.Utilities.Mediator;
using Books.Application.Utilities.Pagination;
using Books.Domain.Common.ValueObjects;
using Books.Domain.Entities.Authors;
using Books.Domain.Entities.Books;
using Books.Domain.Entities.Categories;
using Microsoft.Extensions.DependencyInjection;

namespace Books.Tests;

[TestClass]
public sealed class ApplicationTests
{
    private class FakeBooksRepository : IBooksRepository
    {
        private readonly List<Book> _books;

        public FakeBooksRepository(List<Book> books)
        {
            _books = books;
        }

        public Task<Book> CreateAsync(Book entity, CancellationToken cancellationToken = default) => Task.FromResult(entity);
        public Task<Book> UpdateAsync(Book entity, CancellationToken cancellationToken = default) => Task.FromResult(entity);
        public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task<IEnumerable<Book>> GetListAsync(CancellationToken cancellationToken = default) => Task.FromResult<IEnumerable<Book>>(_books);
        public Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => Task.FromResult(_books.FirstOrDefault(b => b.Id == id));

        public Task<Book?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_books.FirstOrDefault(b => b.Id == id));
        }

        public Task<PaginationResponse<Book>> GetPagedListAsync(PaginationRequest request, Guid? categoryId = null, string? searchTerm = null, CancellationToken cancellationToken = default)
        {
            var query = _books.AsQueryable();
            if (categoryId.HasValue) query = query.Where(b => b.CategoryId == categoryId.Value);
            if (!string.IsNullOrWhiteSpace(searchTerm)) query = query.Where(b => b.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));

            var items = query.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();
            return Task.FromResult(PaginationResponse<Book>.Create(items, query.Count(), request));
        }

        public Task<PaginationResponse<Book>> GetByCategoryPagedListAsync(Guid categoryId, PaginationRequest request, CancellationToken cancellationToken = default)
        {
            var query = _books.Where(b => b.CategoryId == categoryId);
            var items = query.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();
            return Task.FromResult(PaginationResponse<Book>.Create(items, query.Count(), request));
        }
    }

    private (List<Book> books, FakeBooksRepository repo, Author author, Category category) CreateTestData()
    {
        var author = new Author("Gabriel García Márquez");
        var category = new Category("Novela");
        var isbn1 = Isbn.Create("978-0307474728");
        var isbn2 = Isbn.Create("978-0307389732");

        var book1 = new Book("Cien años de soledad", isbn1, 1967, author.Id, category.Id, "Obra cumbre");
        var book2 = new Book("El amor en los tiempos del cólera", isbn2, 1985, author.Id, category.Id, "Historia de amor");

        // Set navigation properties using reflection for testing
        typeof(Book).GetProperty("Author")?.SetValue(book1, author);
        typeof(Book).GetProperty("Category")?.SetValue(book1, category);
        typeof(Book).GetProperty("Author")?.SetValue(book2, author);
        typeof(Book).GetProperty("Category")?.SetValue(book2, category);

        var list = new List<Book> { book1, book2 };
        return (list, new FakeBooksRepository(list), author, category);
    }

    [TestMethod]
    public async Task GetBooksListUseCase_ShouldReturnAllBooks()
    {
        var (books, repo, author, category) = CreateTestData();
        var useCase = new GetBooksListUseCase(repo);

        var result = await useCase.Handle(new GetBooksListQuery());

        Assert.AreEqual(2, result.TotalCount);
        Assert.AreEqual(2, result.Items.Count);
        Assert.AreEqual("Cien años de soledad", result.Items[0].Title);
        Assert.AreEqual(author.Name, result.Items[0].Author);
        Assert.AreEqual(category.Name, result.Items[0].Category);
    }

    [TestMethod]
    public async Task GetBookByIdUseCase_ExistingId_ShouldReturnDetails()
    {
        var (books, repo, author, category) = CreateTestData();
        var useCase = new GetBookByIdUseCase(repo);

        var result = await useCase.Handle(new GetBookByIdQuery(books[0].Id));

        Assert.IsNotNull(result);
        Assert.AreEqual(books[0].Id, result.Id);
        Assert.AreEqual("Cien años de soledad", result.Title);
        Assert.AreEqual(author.Name, result.AuthorName);
        Assert.AreEqual(category.Name, result.CategoryName);
    }

    [TestMethod]
    public async Task GetBookByIdUseCase_NonExistingId_ShouldReturnNull()
    {
        var (_, repo, _, _) = CreateTestData();
        var useCase = new GetBookByIdUseCase(repo);

        var result = await useCase.Handle(new GetBookByIdQuery(Guid.NewGuid()));

        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task GetBooksByCategoryUseCase_ShouldFilterCorrectly()
    {
        var (books, repo, _, category) = CreateTestData();
        var useCase = new GetBooksByCategoryUseCase(repo);

        var result = await useCase.Handle(new GetBooksByCategoryQuery(category.Id));

        Assert.AreEqual(2, result.TotalCount);
    }

    [TestMethod]
    public async Task SimpleMediator_ShouldResolveAndExecuteUseCase()
    {
        var (books, repo, _, _) = CreateTestData();

        var services = new ServiceCollection();
        services.AddScoped<IBooksRepository>(_ => repo);
        services.AddScoped<IMediator, SimpleMediator>();
        services.AddScoped<IRequestHandler<GetBooksListQuery, PaginationResponse<BookListItemDTO>>, GetBooksListUseCase>();

        var provider = services.BuildServiceProvider();
        var mediator = provider.GetRequiredService<IMediator>();

        var result = await mediator.Send(new GetBooksListQuery());

        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.TotalCount);
    }
}
