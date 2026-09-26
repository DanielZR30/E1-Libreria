using Books.Domain.Entities.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Books.Persistence.Configurations
{
    internal class BookConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Title)
                   .HasMaxLength(256)
                   .IsRequired();

            builder.OwnsOne(b => b.Isbn, isbn =>
            {
                isbn.Property(i => i.Value)
                    .HasColumnName("Isbn")
                    .HasMaxLength(20)
                    .IsRequired();
            });

            builder.Property(b => b.PublicationYear)
                   .IsRequired();

            builder.Property(b => b.Synopsis)
                   .HasMaxLength(4000);

            builder.Property(b => b.CreatedAt)
                   .IsRequired();

            builder.HasOne(b => b.Author)
                   .WithMany(a => a.Books)
                   .HasForeignKey(b => b.AuthorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Category)
                   .WithMany(c => c.Books)
                   .HasForeignKey(b => b.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
