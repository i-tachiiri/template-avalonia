using Microsoft.EntityFrameworkCore;
using App.Data.Models;

namespace App.Data;

public class AppDbContext : DbContext
{
    public DbSet<Item> Items => Set<Item>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
