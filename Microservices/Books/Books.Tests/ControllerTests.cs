using Books.Api.Controllers;
using Books.Application.UseCases.Books.Queries.GetBookById;
using Books.Application.UseCases.Books.Queries.GetBooksByCategory;
using Books.Application.UseCases.Books.Queries.GetBooksList;
using Books.Application.Utilities.Mediator;
using Books.Application.Utilities.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Books.Tests;

[TestClass]
public sealed class ControllerTests
{
    private class FakeMediator : IMediator
    {
        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
        {
            if (request is GetBooksListQuery listQuery)
            {
                var items = new List<BookListItemDTO>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Title = "Cien años de soledad",
                        Isbn = "978-0307474728",
                        PublicationYear = 1967,
                        AuthorId = Guid.NewGuid(),
                        Author = "Gabriel García Márquez",
                        CategoryId = Guid.NewGuid(),
                        Category = "Realismo Mágico"
                    }
                };
                var response = PaginationResponse<BookListItemDTO>.Create(items, items.Count, listQuery.Pagination);
                return Task.FromResult((TResponse)(object)response);
            }

            if (request is GetBookByIdQuery byIdQuery)
            {
                if (byIdQuery.Id == Guid.Empty)
                {
                    return Task.FromResult((TResponse)(object)null!);
                }

                var details = new BookDetailsDTO
                {
                    Id = byIdQuery.Id,
                    Title = "1984",
                    Isbn = "978-0451524935",
                    PublicationYear = 1949,
                    AuthorId = Guid.NewGuid(),
                    AuthorName = "George Orwell",
                    CategoryId = Guid.NewGuid(),
                    CategoryName = "Ciencia Ficción",
                    CreatedAt = DateTime.UtcNow
                };
                return Task.FromResult((TResponse)(object)details);
            }

            if (request is GetBooksByCategoryQuery categoryQuery)
            {
                var items = new List<BookListItemDTO>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Title = "Fundación",
                        Isbn = "978-0553293357",
                        PublicationYear = 1951,
                        AuthorId = Guid.NewGuid(),
                        Author = "Isaac Asimov",
                        CategoryId = categoryQuery.CategoryId,
                        Category = "Ciencia Ficción"
                    }
                };
                var response = PaginationResponse<BookListItemDTO>.Create(items, items.Count, categoryQuery.Pagination);
                return Task.FromResult((TResponse)(object)response);
            }

            throw new NotImplementedException();
        }

        public Task Send(IRequest request) => Task.CompletedTask;
    }

    [TestMethod]
    public async Task GetList_ShouldReturn200WithPaginationResponse()
    {
        var mediator = new FakeMediator();
        var controller = new BooksController(mediator);

        var actionResult = await controller.GetList(1, 10);
        var statusResult = actionResult as ObjectResult;

        Assert.IsNotNull(statusResult);
        Assert.AreEqual(StatusCodes.Status200OK, statusResult.StatusCode);

        var payload = statusResult.Value as PaginationResponse<BookListItemDTO>;
        Assert.IsNotNull(payload);
        Assert.AreEqual(1, payload.TotalCount);
        Assert.AreEqual("Cien años de soledad", payload.Items[0].Title);
    }

    [TestMethod]
    public async Task GetById_WhenFound_ShouldReturn200WithBookDetails()
    {
        var mediator = new FakeMediator();
        var controller = new BooksController(mediator);
        var id = Guid.NewGuid();

        var actionResult = await controller.GetById(id);
        var statusResult = actionResult as ObjectResult;

        Assert.IsNotNull(statusResult);
        Assert.AreEqual(StatusCodes.Status200OK, statusResult.StatusCode);

        var payload = statusResult.Value as BookDetailsDTO;
        Assert.IsNotNull(payload);
        Assert.AreEqual(id, payload.Id);
        Assert.AreEqual("1984", payload.Title);
    }

    [TestMethod]
    public async Task GetById_WhenNotFound_ShouldReturn404()
    {
        var mediator = new FakeMediator();
        var controller = new BooksController(mediator);

        var actionResult = await controller.GetById(Guid.Empty);
        var statusResult = actionResult as ObjectResult;

        Assert.IsNotNull(statusResult);
        Assert.AreEqual(StatusCodes.Status404NotFound, statusResult.StatusCode);
    }

    [TestMethod]
    public async Task GetByCategory_ShouldReturn200WithFilteredBooks()
    {
        var mediator = new FakeMediator();
        var controller = new BooksController(mediator);
        var categoryId = Guid.NewGuid();

        var actionResult = await controller.GetByCategory(categoryId, 1, 10);
        var statusResult = actionResult as ObjectResult;

        Assert.IsNotNull(statusResult);
        Assert.AreEqual(StatusCodes.Status200OK, statusResult.StatusCode);

        var payload = statusResult.Value as PaginationResponse<BookListItemDTO>;
        Assert.IsNotNull(payload);
        Assert.AreEqual(categoryId, payload.Items[0].CategoryId);
    }
}
