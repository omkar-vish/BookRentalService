using BookRentalService.Model;
using Microsoft.EntityFrameworkCore;

public class BookRentalDbContext : DbContext
{
    public BookRentalDbContext(DbContextOptions<BookRentalDbContext> options)
       : base(options) { }

    public DbSet<Book> Books { get; set; }
    public DbSet<Rental> Rentals { get; set; }
    public DbSet<User> Users { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    if (!optionsBuilder.IsConfigured)
    //    {
    //        optionsBuilder.UseSqlite("Data Source=bookrental.db");
    //    }
    //}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}