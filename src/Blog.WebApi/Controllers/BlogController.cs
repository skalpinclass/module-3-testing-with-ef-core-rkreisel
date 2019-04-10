using Blog.DataAccess;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Blog.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class BlogController : ControllerBase
    {
        private IBlogRepository blogRepository;

        public BlogController(IBlogRepository blogRepository)
        {
            this.blogRepository = blogRepository;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        public ActionResult<IEnumerable<DataAccess.Blog>> Get()
        {
            var entries = blogRepository.GetAllBlogEntries();
            return Ok(entries);
        }

        //[HttpPost]
        //[Produces("application/json")]
        //[ProducesResponseType(200)]
        //public void Post([FromBody] DataAccess.Blog value)
        //{
        //    db.Blogs.Add(value);
        //    db.SaveChanges();
        //}

        //[HttpPut("{id}")]
        //[Produces("application/json")]
        //[ProducesResponseType(200)]
        //public void Put(int id, [FromBody] DataAccess.Blog value)
        //{
        //    db.Blogs.Update(value);
        //    db.SaveChanges();
        //}

        //[HttpDelete("{id}")]
        //[ProducesResponseType(200)]
        //public void Delete(int id)
        //{
        //    var blog = db.Blogs.Find(id);
        //    db.Blogs.Remove(blog);
        //    db.SaveChanges();
        //}
    }
}