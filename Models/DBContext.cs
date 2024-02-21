using Microsoft.EntityFrameworkCore;
// using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace swensen_api.Models;

public class DBContext : DbContext
{
    public DBContext(DbContextOptions<DBContext> options) : base(options)
    {
    }

    public DbSet<Users> Users { get; set; }
    public DbSet<ProductModel> Product { get; set; }
    public DbSet<CategoryModel> Category { get; set; }
    public DbSet<AdminModel> Admin { get; set; }
}