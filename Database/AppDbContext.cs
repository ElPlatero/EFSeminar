using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCoreSeminar.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}