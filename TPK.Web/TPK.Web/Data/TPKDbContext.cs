using Microsoft.EntityFrameworkCore;
using TPK.Web.Models;

namespace TPK.Web.Data
{
    public class TPKDbContext : DbContext
    {
        public TPKDbContext (DbContextOptions<TPKDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Category { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<Site> Site { get; set; }
    }
}
