using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Models;

public class LibraryContext : IdentityDbContext
{
    public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

    public DbSet<Book> Books { get; set; }
    public DbSet<Reader> Readers { get; set; }
    public DbSet<BookReader> BookReaders { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookReader>()
            .HasOne(br => br.Book)
            .WithMany(b => b.BookReaders)
            .HasForeignKey(br => br.BookId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<BookReader>()
            .HasOne(br => br.Reader)
            .WithMany(r => r.BookReaders)
            .HasForeignKey(br => br.ReaderId)
            .OnDelete(DeleteBehavior.Restrict);
        base.OnModelCreating(modelBuilder);
    }
}
