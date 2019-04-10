using Microsoft.EntityFrameworkCore;

namespace Blog.DataAccess
{
    public class BlogContext : DbContext
    {
        public virtual DbSet<Blog> Blogs { get; set; }

        public BlogContext(DbContextOptions options) : base(options)
        { }
    }
}
