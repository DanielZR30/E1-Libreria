using Books.Domain.Common.ValueObjects;
using Books.Domain.Entities.Authors;
using Books.Domain.Entities.Books;
using Books.Domain.Entities.Categories;
using Books.Domain.Exceptions;

namespace Books.Tests;

[TestClass]
public sealed class DomainTests
{
    [TestMethod]
    public void CreateAuthor_WithValidData_ShouldSucceed()
    {
        var author = new Author("Gabriel García Márquez", "Premio Nobel de Literatura 1982");

        Assert.AreNotEqual(Guid.Empty, author.Id);
        Assert.AreEqual("Gabriel García Márquez", author.Name);
        Assert.AreEqual("Premio Nobel de Literatura 1982", author.Biography);
    }

    [TestMethod]
    public void CreateAuthor_WithEmptyName_ShouldThrowException()
    {
        Assert.ThrowsExactly<BussinesRuleException>(() => new Author(""));
    }

    [TestMethod]
    public void CreateCategory_WithValidData_ShouldSucceed()
    {
        var category = new Category("Novela", "Obras narrativas de ficción");

        Assert.AreNotEqual(Guid.Empty, category.Id);
        Assert.AreEqual("Novela", category.Name);
    }

    [TestMethod]
    public void CreateCategory_WithShortName_ShouldThrowException()
    {
        Assert.ThrowsExactly<BussinesRuleException>(() => new Category("A"));
    }

    [TestMethod]
    public void CreateIsbn_WithValid13Digits_ShouldSucceed()
    {
        var isbn = Isbn.Create("978-0-307-47472-8");

        Assert.AreEqual("978-0-307-47472-8", isbn.Value);
    }

    [TestMethod]
    public void CreateIsbn_WithInvalidLength_ShouldThrowException()
    {
        Assert.ThrowsExactly<BussinesRuleException>(() => Isbn.Create("12345"));
    }

    [TestMethod]
    public void CreateBook_WithValidData_ShouldSucceed()
    {
        var authorId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        var isbn = Isbn.Create("978-0307474728");

        var book = new Book("Cien años de soledad", isbn, 1967, authorId, categoryId, "Obra cumbre del realismo mágico.");

        Assert.AreNotEqual(Guid.Empty, book.Id);
        Assert.AreEqual("Cien años de soledad", book.Title);
        Assert.AreEqual(isbn, book.Isbn);
        Assert.AreEqual(1967, book.PublicationYear);
        Assert.AreEqual(authorId, book.AuthorId);
        Assert.AreEqual(categoryId, book.CategoryId);
    }

    [TestMethod]
    public void CreateBook_WithInvalidYear_ShouldThrowException()
    {
        var isbn = Isbn.Create("978-0307474728");
        Assert.ThrowsExactly<BussinesRuleException>(() => new Book("Libro del futuro", isbn, 3000, Guid.NewGuid(), Guid.NewGuid()));
    }
}
