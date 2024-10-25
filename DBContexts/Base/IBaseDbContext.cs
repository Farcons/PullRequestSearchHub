using System.Linq.Expressions;

namespace PipelinesTeste2.DBContexts.Base
{
    public interface IBaseDbContext<TEntity> where TEntity : SystemBaseIdentifier
    {
        void Add(TEntity entity);
        void AddRange(List<TEntity> entity);
        TEntity Find(Guid id);
        List<TEntity> Find(IEnumerable<Guid> ids);
        TEntity First(Expression<Func<TEntity, bool>> exp);
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> exp);
        TEntity FirstOrDefault();
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> exp);
    }
}