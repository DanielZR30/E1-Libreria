using Books.Application.Contracts.Repositories;
using Books.Application.Utilities.Mediator;
using Books.Domain.Entities.Books;

namespace Books.Application.UseCases.Books.Queries.GetBookById
{
    public class GetBookByIdUseCase : IRequestHandler<GetBookByIdQuery, BookDetailsDTO?>
    {
        private readonly IBooksRepository _repository;

        public GetBookByIdUseCase(IBooksRepository repository)
        {
            _repository = repository;
        }

        public async Task<BookDetailsDTO?> Handle(GetBookByIdQuery query)
        {
            Book? book = await _repository.GetByIdWithDetailsAsync(query.Id);

            if (book is null)
            {
                return null;
            }

            return new BookDetailsDTO
            {
                Id = book.Id,
                Title = book.Title,
                Isbn = book.Isbn.Value,
                PublicationYear = book.PublicationYear,
                Synopsis = book.Synopsis,
                AuthorId = book.AuthorId,
                AuthorName = book.Author?.Name ?? string.Empty,
                AuthorBiography = book.Author?.Biography,
                CategoryId = book.CategoryId,
                CategoryName = book.Category?.Name ?? string.Empty,
                CategoryDescription = book.Category?.Description,
                CreatedAt = book.CreatedAt
            };
        }
    }
}
