using Books.Application.Utilities.Pagination;
using Books.Domain.Entities.Books;
using Books.Persistence;
using Books.Persistence.Repositories;
using Books.Persistence.Seeds.Authors;
using Books.Persistence.Seeds.Books;
using Books.Persistence.Seeds.Categories;
using Microsoft.EntityFrameworkCore;

namespace Books.Tests;

[TestClass]
public sealed class PersistenceTests
{
    private async Task<DataContext> CreateContextWithSeedAsync(string dbName)
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        var context = new DataContext(options);

        var authorSeeder = new AuthorSeeder(context);
        await authorSeeder.SeedAsync();

        var categorySeeder = new CategorySeeder(context);
        await categorySeeder.SeedAsync();

        var bookSeeder = new BookSeeder(context);
        await bookSeeder.SeedAsync();

        return context;
    }

    [TestMethod]
    public async Task Seeders_ShouldPopulateInitialCatalog()
    {
        using var context = await CreateContextWithSeedAsync(nameof(Seeders_ShouldPopulateInitialCatalog));

        var authorsCount = await context.Authors.CountAsync();
        var categoriesCount = await context.Categories.CountAsync();
        var booksCount = await context.Books.CountAsync();

        Assert.AreEqual(5, authorsCount);
        Assert.AreEqual(5, categoriesCount);
        Assert.AreEqual(10, booksCount);
    }

    [TestMethod]
    public async Task BooksRepository_GetPagedListAsync_ShouldReturnBooksWithDetails()
    {
        using var context = await CreateContextWithSeedAsync(nameof(BooksRepository_GetPagedListAsync_ShouldReturnBooksWithDetails));
        var repo = new BooksRepository(context);

        var result = await repo.GetPagedListAsync(new PaginationRequest(1, 10));

        Assert.AreEqual(10, result.TotalCount);
        Assert.AreEqual(10, result.Items.Count);
        Assert.IsNotNull(result.Items[0].Author);
        Assert.IsNotNull(result.Items[0].Category);
        Assert.IsNotNull(result.Items[0].Isbn);
    }

    [TestMethod]
    public async Task BooksRepository_GetByIdWithDetailsAsync_ShouldReturnBook()
    {
        using var context = await CreateContextWithSeedAsync(nameof(BooksRepository_GetByIdWithDetailsAsync_ShouldReturnBook));
        var existingBook = await context.Books.FirstAsync();
        var repo = new BooksRepository(context);

        var result = await repo.GetByIdWithDetailsAsync(existingBook.Id);

        Assert.IsNotNull(result);
        Assert.AreEqual(existingBook.Id, result.Id);
        Assert.AreEqual(existingBook.Title, result.Title);
        Assert.IsNotNull(result.Author);
        Assert.IsNotNull(result.Category);
    }

    [TestMethod]
    public async Task BooksRepository_GetByCategoryPagedListAsync_ShouldFilterByCategory()
    {
        using var context = await CreateContextWithSeedAsync(nameof(BooksRepository_GetByCategoryPagedListAsync_ShouldFilterByCategory));
        var sciFi = await context.Categories.FirstAsync(c => c.Name == "Ciencia Ficción");
        var repo = new BooksRepository(context);

        var result = await repo.GetByCategoryPagedListAsync(sciFi.Id, new PaginationRequest(1, 10));

        Assert.IsTrue(result.TotalCount > 0);
        foreach (var book in result.Items)
        {
            Assert.AreEqual(sciFi.Id, book.CategoryId);
        }
    }
}
