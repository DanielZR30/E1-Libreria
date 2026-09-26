using Books.Domain.Entities.Authors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Books.Persistence.Configurations
{
    internal class AuthorConfig : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Name)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(a => a.Biography)
                   .HasMaxLength(2048);

            builder.HasMany(a => a.Books)
                   .WithOne(b => b.Author)
                   .HasForeignKey(b => b.AuthorId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
