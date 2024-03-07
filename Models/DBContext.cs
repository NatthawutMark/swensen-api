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

    public DbSet<ProductModel> PRODUCT { get; set; }
    public DbSet<ProductImageModel> PRODUCT_IMAGE { get; set; }
    public DbSet<CategoryModel> CATEGORY { get; set; }
    public DbSet<AdminModel> ADMIN { get; set; }
    public DbSet<Status> STATUS { get; set; }
    public DbSet<Users> USERS { get; set; }
}