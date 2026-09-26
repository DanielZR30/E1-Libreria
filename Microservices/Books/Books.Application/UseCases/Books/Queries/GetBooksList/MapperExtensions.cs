using Books.Domain.Entities.Books;

namespace Books.Application.UseCases.Books.Queries.GetBooksList
{
    public static class MapperExtensions
    {
        public static BookListItemDTO ToListItemDTO(this Book book)
        {
            return new BookListItemDTO
            {
                Id = book.Id,
                Title = book.Title,
                Isbn = book.Isbn.Value,
                PublicationYear = book.PublicationYear,
                AuthorId = book.AuthorId,
                Author = book.Author?.Name ?? string.Empty,
                CategoryId = book.CategoryId,
                Category = book.Category?.Name ?? string.Empty
            };
        }
    }
}
