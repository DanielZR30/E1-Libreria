using Books.Domain.Entities.Categories;
using Microsoft.EntityFrameworkCore;

namespace Books.Persistence.Seeds.Categories
{
    public class CategorySeeder : IDataSeeder
    {
        private readonly DataContext _context;

        public CategorySeeder(DataContext context)
        {
            _context = context;
        }

        public int Order => 2;

        public async Task SeedAsync(CancellationToken cancellationToken = default)
        {
            if (await _context.Categories.AnyAsync(cancellationToken))
            {
                return;
            }

            List<Category> categories =
            [
                new Category("Novela", "Obras narrativas de ficción en prosa de considerable extensión."),
                new Category("Ciencia Ficción", "Literatura especulativa que explora futuros alternativos, ciencia y tecnología."),
                new Category("Clásicos", "Obras literarias que han trascendido su época y poseen valor universal."),
                new Category("Realismo Mágico", "Corriente narrativa donde los elementos irreales se presentan como parte de la realidad cotidiana."),
                new Category("Ensayo", "Escritos reflexivos y analíticos sobre diversos temas.")
            ];

            await _context.Categories.AddRangeAsync(categories, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
