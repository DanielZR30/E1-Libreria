using Books.Domain.Entities.Books;
using Books.Domain.Exceptions;

namespace Books.Domain.Entities.Categories
{
    public sealed class Category
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public string? Description { get; private set; }
        public ICollection<Book> Books { get; private set; } = new List<Book>();

        private Category() { }

        public Category(string name, string? description = null)
        {
            ApplyNameRules(name);
            ApplyDescriptionRules(description);

            Id = Guid.CreateVersion7();
            Name = name.Trim();
            Description = description?.Trim();
        }

        public void Update(string name, string? description = null)
        {
            ApplyNameRules(name);
            ApplyDescriptionRules(description);

            Name = name.Trim();
            Description = description?.Trim();
        }

        private static void ApplyNameRules(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new BussinesRuleException("El nombre de la categoría es requerido.");
            }

            if (name.Trim().Length < 2)
            {
                throw new BussinesRuleException("El nombre de la categoría debe tener al menos 2 caracteres.");
            }

            if (name.Trim().Length > 64)
            {
                throw new BussinesRuleException("El nombre de la categoría debe tener máximo 64 caracteres.");
            }
        }

        private static void ApplyDescriptionRules(string? description)
        {
            if (description is not null && description.Trim().Length > 1024)
            {
                throw new BussinesRuleException("La descripción de la categoría debe tener máximo 1024 caracteres.");
            }
        }
    }
}
