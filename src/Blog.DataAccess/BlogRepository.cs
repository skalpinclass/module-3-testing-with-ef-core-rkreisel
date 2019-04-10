using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Blog.DataAccess
{
    public class BlogRepository : IBlogRepository
    {
        private BlogContext blogContext;

        public BlogRepository(BlogContext blogContext)
        {
            this.blogContext = blogContext ?? throw new ArgumentNullException(nameof(blogContext));
        }

        public IEnumerable<Blog> GetAllBlogEntries() =>
            blogContext
                .Blogs
                .Include(b => b.Posts)
                .AsNoTracking();
    }
}
