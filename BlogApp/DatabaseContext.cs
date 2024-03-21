using BlogApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogApp
{
    public class DatabaseContext: DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options):base(options)
        {

        }

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<UserModel> Users { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure entity relationships if necessary
        }
    }
}
