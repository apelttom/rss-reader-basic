using Microsoft.EntityFrameworkCore;
using RSSreader.Models;

namespace RSSreader.Data
{
    public class RSSreaderContext : DbContext
    {
        public RSSreaderContext (DbContextOptions<RSSreaderContext> options)
            : base(options)
        {
        }

        public DbSet<Feed> Feed { get; set; } = default!;
        public DbSet<Article> Article { get; set; } = default!;
    }
}
