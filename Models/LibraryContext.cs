using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Models;

public class LibraryContext : DbContext
{
    public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }
    public DbSet<Book> Books { get; set; }
    public DbSet<Reader> Readers { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure many-to-many between Book and Reader using join table BookReader
        modelBuilder.Entity<Book>()
            .HasMany(b => b.Readers)
            .WithMany(r => r.Books)
            .UsingEntity<Dictionary<string, object>>(
                "BookReader",
                br => br.HasOne<Reader>().WithMany().HasForeignKey("ReaderId").OnDelete(DeleteBehavior.Cascade),
                br => br.HasOne<Book>().WithMany().HasForeignKey("BookId").OnDelete(DeleteBehavior.Cascade),
                br =>
                {
                    br.HasKey("BookId", "ReaderId");
                    br.ToTable("BookReaders");
                }
            );

        base.OnModelCreating(modelBuilder);
    }
}