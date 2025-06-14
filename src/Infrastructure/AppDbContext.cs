using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class AppDbContext : DbContext
{
    public DbSet<Note> Notes => Set<Note>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}
