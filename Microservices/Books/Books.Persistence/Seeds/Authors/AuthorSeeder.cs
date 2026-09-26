using Books.Domain.Entities.Authors;
using Microsoft.EntityFrameworkCore;

namespace Books.Persistence.Seeds.Authors
{
    public class AuthorSeeder : IDataSeeder
    {
        private readonly DataContext _context;

        public AuthorSeeder(DataContext context)
        {
            _context = context;
        }

        public int Order => 1;

        public async Task SeedAsync(CancellationToken cancellationToken = default)
        {
            if (await _context.Authors.AnyAsync(cancellationToken))
            {
                return;
            }

            List<Author> authors =
            [
                new Author("Gabriel García Márquez", "Escritor y periodista colombiano, Premio Nobel de Literatura en 1982."),
                new Author("George Orwell", "Escritor y periodista británico, autor de célebres distopías políticas."),
                new Author("Jane Austen", "Novelista británica cuyas obras retratan la sociedad rural de principios del siglo XIX."),
                new Author("Isaac Asimov", "Escritor y profesor de bioquímica estadounidense de origen ruso, prolífico autor de ciencia ficción."),
                new Author("Miguel de Cervantes", "Novelista, poeta y dramaturgo español, máxima figura de la literatura en español.")
            ];

            await _context.Authors.AddRangeAsync(authors, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
