using Books.Application.Utilities.Mediator;

namespace Books.Application.UseCases.Books.Queries.GetBookById
{
    public class GetBookByIdQuery : IRequest<BookDetailsDTO?>
    {
        public Guid Id { get; set; }

        public GetBookByIdQuery(Guid id)
        {
            Id = id;
        }

        public GetBookByIdQuery() { }
    }
}
