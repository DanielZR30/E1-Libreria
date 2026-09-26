using Books.Domain.Common.ValueObjects;
using Books.Domain.Entities.Authors;
using Books.Domain.Entities.Books;
using Books.Domain.Entities.Categories;
using Microsoft.EntityFrameworkCore;

namespace Books.Persistence.Seeds.Books
{
    public class BookSeeder : IDataSeeder
    {
        private readonly DataContext _context;

        public BookSeeder(DataContext context)
        {
            _context = context;
        }

        public int Order => 3;

        public async Task SeedAsync(CancellationToken cancellationToken = default)
        {
            if (await _context.Books.AnyAsync(cancellationToken))
            {
                return;
            }

            List<Author> authors = await _context.Authors.ToListAsync(cancellationToken);
            List<Category> categories = await _context.Categories.ToListAsync(cancellationToken);

            Author gabo = authors.First(a => a.Name.Contains("García Márquez"));
            Author orwell = authors.First(a => a.Name.Contains("Orwell"));
            Author austen = authors.First(a => a.Name.Contains("Austen"));
            Author asimov = authors.First(a => a.Name.Contains("Asimov"));
            Author cervantes = authors.First(a => a.Name.Contains("Cervantes"));

            Category novela = categories.First(c => c.Name == "Novela");
            Category sciFi = categories.First(c => c.Name == "Ciencia Ficción");
            Category clasicos = categories.First(c => c.Name == "Clásicos");
            Category realismo = categories.First(c => c.Name == "Realismo Mágico");

            List<Book> books =
            [
                new Book("Cien años de soledad", Isbn.Create("978-0307474728"), 1967, gabo.Id, realismo.Id, "Historia de las siete generaciones de la familia Buendía en el pueblo mítico de Macondo."),
                new Book("El amor en los tiempos del cólera", Isbn.Create("978-0307389732"), 1985, gabo.Id, novela.Id, "Inolvidable historia de amor entre Florentino Ariza y Fermina Daza que perdura a lo largo de medio siglo."),
                new Book("Crónica de una muerte anunciada", Isbn.Create("978-1400034956"), 1981, gabo.Id, novela.Id, "Reconstrucción periodística y literaria del asesinato anunciado de Santiago Nasar."),
                new Book("1984", Isbn.Create("978-0451524935"), 1949, orwell.Id, sciFi.Id, "Inquietante visión de una sociedad totalitaria vigilada permanentemente por el Gran Hermano."),
                new Book("Rebelión en la granja", Isbn.Create("978-0451526342"), 1945, orwell.Id, clasicos.Id, "Sátira alegórica sobre la corrupción del poder y la tiranía en un régimen revolucionario."),
                new Book("Orgullo y prejuicio", Isbn.Create("978-0141439518"), 1813, austen.Id, clasicos.Id, "Brillante comedia romántica sobre las relaciones, clases sociales y malentendidos entre Elizabeth Bennet y el señor Darcy."),
                new Book("Sensatez y sentimientos", Isbn.Create("978-0141439662"), 1811, austen.Id, novela.Id, "Contraste de personalidades y vivencias sentimentales entre las hermanas Elinor y Marianne Dashwood."),
                new Book("Fundación", Isbn.Create("978-0553293357"), 1951, asimov.Id, sciFi.Id, "El psicohistoriador Hari Seldon prevé la caída del Imperio Galáctico e inicia un plan para reducir la era de oscuridad."),
                new Book("Yo, Robot", Isbn.Create("978-0553382563"), 1950, asimov.Id, sciFi.Id, "Colección de relatos conectados que establecen y exploran las Tres Leyes de la Robótica."),
                new Book("Don Quijote de la Mancha", Isbn.Create("978-8424116286"), 1605, cervantes.Id, clasicos.Id, "Las cómicas y reflexivas aventuras del hidalgo Alonso Quijano y su fiel escudero Sancho Panza.")
            ];

            await _context.Books.AddRangeAsync(books, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
