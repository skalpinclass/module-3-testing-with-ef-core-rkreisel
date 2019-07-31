using Blog.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.DataAccess.Tests
{
    [TestClass]
    public class BlogRepositoryTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullContextThrowsException()
        {
            var blogRepository = new BlogRepository(null);
            Assert.Fail();
        }

        [TestMethod]
        public void NullContextThrowsExceptionSingleLine() => Assert.ThrowsException<ArgumentNullException>(() => new BlogContext(null));

        [TestMethod]
        public void ReturnAllBlogObjects_Mock_DbContext()
        {
            var builder = new DbContextOptionsBuilder();
            var options = builder.Options;
            var blogContextMock = new Mock<BlogContext>(options);
            var blogs = new List<DataAccess.Blog>
            {
                new Blog(),
                new Blog()
            };
            var dbSetMock = new DbQueryMock<DataAccess.Blog>(blogs);
            blogContextMock
                .Setup(b => b.Blogs)
                .Returns(dbSetMock.Object);
            var blogRepository = new BlogRepository(blogContextMock.Object);

            var blogEntries = blogRepository.GetAllBlogEntries();

            Assert.AreEqual(2, blogEntries.Count());
        }

        [TestMethod]
        public void InMemory_Repository_Integration()
        {
            var options = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(databaseName: "repositoryDb")
                .Options;
            using (var context = new BlogContext(options))
            {
                var blogRepository = new BlogRepository(context);
                var blogEntries = blogRepository.GetAllBlogEntries();
                Assert.AreEqual(0, blogEntries.Count());
                context.Blogs.Add(new DataAccess.Blog());
                context.SaveChanges();
                blogEntries = blogRepository.GetAllBlogEntries();
                Assert.AreEqual(1, blogEntries.Count());
            }
        }
        [TestMethod]
        public void InMemory_Repository_Integration2()
        {
            var options = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(databaseName: "repositoryDb2")
                .Options;
            using (var context = new BlogContext(options))
            {
                var blogRepository = new BlogRepository(context);
                var blogEntries = blogRepository.GetAllBlogEntries();
                Assert.AreEqual(0, blogEntries.Count());
                context.Blogs.Add(new DataAccess.Blog());
                context.SaveChanges();
                blogEntries = blogRepository.GetAllBlogEntries();
                Assert.AreEqual(1, blogEntries.Count());
            }
        }

        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataTestMethod]
        public void RepoTestWithParametersAndIterationSpecificDB(int cnt)
        {
            var repo = BuildInMemRepo(cnt);
            var actual = repo.GetAllBlogEntries();
            Assert.AreEqual(actual.Count(), cnt);
        }

        [TestMethod]
        public void ReturnAllBlogObjects_Mock_Repository()
        {
            var blogRepository = new Mock<IBlogRepository>();
            blogRepository
                .Setup(b => b.GetAllBlogEntries())
                .Returns(new List<DataAccess.Blog> { new DataAccess.Blog() });
            var blogController = new BlogController(blogRepository.Object);
            var blogResult = blogController.Get().Result as OkObjectResult;
            Assert.IsNotNull(blogResult);
            var returnedBlogEntries = blogResult.Value as IEnumerable<DataAccess.Blog>;
            Assert.IsNotNull(returnedBlogEntries);
            Assert.AreEqual(1, returnedBlogEntries.Count());
        }

        [TestMethod]
        public void Inmemory_Controller_Integration()
        {
            var options = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(databaseName: "controllerDb")
                .Options;
            using (var context = new BlogContext(options))
            {
                var blogRepository = new BlogRepository(context);
                var controller = new BlogController(blogRepository);
                var objectResult = controller.Get().Result as OkObjectResult;
                var blogEntries = objectResult.Value as IEnumerable<DataAccess.Blog>;
                Assert.AreEqual(0, blogEntries.Count());
                context.Blogs.Add(new DataAccess.Blog());
                context.SaveChanges();
                objectResult = controller.Get().Result as OkObjectResult;
                blogEntries = objectResult.Value as IEnumerable<DataAccess.Blog>;
                Assert.AreEqual(1, blogEntries.Count());
            }
        }

        private BlogRepository BuildInMemRepo(int cnt)
        {
            var options = new DbContextOptionsBuilder()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
            BlogRepository result = null;
            var context = new BlogContext(options);
            result = new BlogRepository(context);
            for(var ndx = 1; ndx <= cnt; ndx++)
            {
                context.Blogs.Add(new Blog { Rating = 3 % ndx, Url = $"Url#{ndx}" });
            }
            context.SaveChanges();
            return result;
        }
    }
}
