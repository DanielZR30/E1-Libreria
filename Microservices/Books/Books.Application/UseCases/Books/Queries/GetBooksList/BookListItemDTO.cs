namespace Books.Application.UseCases.Books.Queries.GetBooksList
{
    public class BookListItemDTO
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = null!;
        public string Isbn { get; init; } = null!;
        public int PublicationYear { get; init; }
        public Guid AuthorId { get; init; }
        public string Author { get; init; } = null!;
        public Guid CategoryId { get; init; }
        public string Category { get; init; } = null!;
    }
}
