using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DbQueryMock<TEntity> : Mock<DbSet<TEntity>> where TEntity : class
{
    private readonly IEnumerable<TEntity> _entities;
    public DbQueryMock(IEnumerable<TEntity> entities)
    {
        _entities = (entities ?? Enumerable.Empty<TEntity>()).ToList();
        var data = _entities.AsQueryable();
        As<IQueryable<TEntity>>().Setup(x => x.Provider).Returns(data.Provider);
        As<IQueryable<TEntity>>().Setup(x => x.Expression).Returns(data.Expression);
        As<IQueryable<TEntity>>().Setup(x => x.ElementType).Returns(data.ElementType);
        As<IQueryable<TEntity>>().Setup(x => x.GetEnumerator()).Returns(() => data.GetEnumerator());
        As<IEnumerable>().Setup(x => x.GetEnumerator()).Returns(() => data.GetEnumerator());
    }
}