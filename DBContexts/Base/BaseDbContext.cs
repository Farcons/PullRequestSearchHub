using Microsoft.EntityFrameworkCore;
using PipelineSearchHub.DBContexts.SystemCollections;
using PipelineSearchHub.DBContexts.SystemUsers;
using System.Linq.Expressions;

namespace PipelineSearchHub.DBContexts.Base
{
    public class BaseDbContext<TEntity> : IBaseDbContext<TEntity> where TEntity : SystemBaseIdentifier
    {
        protected readonly AppDbContext _dbContext;

        public BaseDbContext(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddRange(List<TEntity> entity)
        {
            _dbContext.Set<TEntity>().AddRange(entity);
        }

        public void Add(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
        }

        public TEntity FirstOrDefault()
        {
            return _dbContext.Set<TEntity>().FirstOrDefault();
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> exp)
        {
            return _dbContext.Set<TEntity>().FirstOrDefault(exp);
        }

        public TEntity First(Expression<Func<TEntity, bool>> exp)
        {
            return _dbContext.Set<TEntity>().First(exp);
        }

        public TEntity Find(Guid id)
        {
            return _dbContext.Set<TEntity>().Find(id);
        }

        public List<TEntity> Find(IEnumerable<Guid> ids)
        {
            return _dbContext.Set<TEntity>().Where((TEntity e) => ids.Contains(e.Id)).ToList();
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> exp)
        {
            return _dbContext.Set<TEntity>().Where(exp);
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<SystemUserCollection> SystemUserCollections { get; set; }
        public DbSet<SystemUser> SystemUsers { get; set; }
    }
}
