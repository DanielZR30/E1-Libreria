using Books.Domain.Entities.Books;
using Books.Domain.Exceptions;

namespace Books.Domain.Entities.Authors
{
    public sealed class Author
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public string? Biography { get; private set; }
        public ICollection<Book> Books { get; private set; } = new List<Book>();

        private Author() { }

        public Author(string name, string? biography = null)
        {
            ApplyNameRules(name);
            ApplyBiographyRules(biography);

            Id = Guid.CreateVersion7();
            Name = name.Trim();
            Biography = biography?.Trim();
        }

        public void Update(string name, string? biography = null)
        {
            ApplyNameRules(name);
            ApplyBiographyRules(biography);

            Name = name.Trim();
            Biography = biography?.Trim();
        }

        private static void ApplyNameRules(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new BussinesRuleException("El nombre del autor es requerido.");
            }

            if (name.Trim().Length < 2)
            {
                throw new BussinesRuleException("El nombre del autor debe tener al menos 2 caracteres.");
            }

            if (name.Trim().Length > 100)
            {
                throw new BussinesRuleException("El nombre del autor debe tener máximo 100 caracteres.");
            }
        }

        private static void ApplyBiographyRules(string? biography)
        {
            if (biography is not null && biography.Trim().Length > 2048)
            {
                throw new BussinesRuleException("La biografía del autor debe tener máximo 2048 caracteres.");
            }
        }
    }
}
