using System.Collections.Generic;

namespace Blog.DataAccess
{
    public interface IBlogRepository
    {
        IEnumerable<Blog> GetAllBlogEntries();
    }
}