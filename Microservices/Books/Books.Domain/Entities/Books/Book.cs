using Books.Domain.Common.ValueObjects;
using Books.Domain.Entities.Authors;
using Books.Domain.Entities.Categories;
using Books.Domain.Exceptions;

namespace Books.Domain.Entities.Books
{
    public sealed class Book
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; } = null!;
        public Isbn Isbn { get; private set; } = null!;
        public int PublicationYear { get; private set; }
        public string? Synopsis { get; private set; }
        public Guid AuthorId { get; private set; }
        public Author Author { get; private set; } = null!;
        public Guid CategoryId { get; private set; }
        public Category Category { get; private set; } = null!;
        public DateTime CreatedAt { get; private set; }

        private Book() { }

        public Book(string title,
                    Isbn isbn,
                    int publicationYear,
                    Guid authorId,
                    Guid categoryId,
                    string? synopsis = null)
        {
            ApplyTitleRules(title);
            ApplyIsbnRules(isbn);
            ApplyPublicationYearRules(publicationYear);
            ApplyAuthorIdRules(authorId);
            ApplyCategoryIdRules(categoryId);
            ApplySynopsisRules(synopsis);

            Id = Guid.CreateVersion7();
            Title = title.Trim();
            Isbn = isbn;
            PublicationYear = publicationYear;
            AuthorId = authorId;
            CategoryId = categoryId;
            Synopsis = synopsis?.Trim();
            CreatedAt = DateTime.UtcNow;
        }

        public void Update(string title,
                           Isbn isbn,
                           int publicationYear,
                           Guid authorId,
                           Guid categoryId,
                           string? synopsis = null)
        {
            ApplyTitleRules(title);
            ApplyIsbnRules(isbn);
            ApplyPublicationYearRules(publicationYear);
            ApplyAuthorIdRules(authorId);
            ApplyCategoryIdRules(categoryId);
            ApplySynopsisRules(synopsis);

            Title = title.Trim();
            Isbn = isbn;
            PublicationYear = publicationYear;
            AuthorId = authorId;
            CategoryId = categoryId;
            Synopsis = synopsis?.Trim();
        }

        private static void ApplyTitleRules(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new BussinesRuleException("El título del libro es requerido.");
            }

            if (title.Trim().Length > 256)
            {
                throw new BussinesRuleException("El título del libro debe tener máximo 256 caracteres.");
            }
        }

        private static void ApplyIsbnRules(Isbn isbn)
        {
            if (isbn is null)
            {
                throw new BussinesRuleException("El ISBN del libro es requerido.");
            }
        }

        private static void ApplyPublicationYearRules(int publicationYear)
        {
            int currentYear = DateTime.UtcNow.Year;
            if (publicationYear < 1000 || publicationYear > currentYear + 1)
            {
                throw new BussinesRuleException($"El año de publicación debe estar entre 1000 y {currentYear + 1}.");
            }
        }

        private static void ApplyAuthorIdRules(Guid authorId)
        {
            if (authorId == Guid.Empty)
            {
                throw new BussinesRuleException("El autor del libro es requerido.");
            }
        }

        private static void ApplyCategoryIdRules(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
            {
                throw new BussinesRuleException("La categoría del libro es requerida.");
            }
        }

        private static void ApplySynopsisRules(string? synopsis)
        {
            if (synopsis is not null && synopsis.Trim().Length > 4000)
            {
                throw new BussinesRuleException("La sinopsis del libro debe tener máximo 4000 caracteres.");
            }
        }
    }
}
