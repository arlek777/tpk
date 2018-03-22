using System.Data.Entity;
using TPK.Web.Models;

namespace TPK.Web.Data
{
    public class TPKDbContext : DbContext
    {
        public TPKDbContext (string connString)
            : base(connString)
        {
        }

        public DbSet<Content> Content { get; set; }
        public DbSet<Site> Site { get; set; }
    }
}
