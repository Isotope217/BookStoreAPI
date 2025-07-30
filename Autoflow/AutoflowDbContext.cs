using Autoflow.Entities;
using Microsoft.EntityFrameworkCore;

namespace Autoflow;

public class AutoflowDbContext : DbContext
{
    public AutoflowDbContext(DbContextOptions<AutoflowDbContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
}