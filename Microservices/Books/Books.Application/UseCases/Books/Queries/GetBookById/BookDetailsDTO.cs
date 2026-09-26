namespace Books.Application.UseCases.Books.Queries.GetBookById
{
    public class BookDetailsDTO
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = null!;
        public string Isbn { get; init; } = null!;
        public int PublicationYear { get; init; }
        public string? Synopsis { get; init; }
        public Guid AuthorId { get; init; }
        public string AuthorName { get; init; } = null!;
        public string? AuthorBiography { get; init; }
        public Guid CategoryId { get; init; }
        public string CategoryName { get; init; } = null!;
        public string? CategoryDescription { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
