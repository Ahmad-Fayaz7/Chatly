using System.Linq.Expressions;

namespace InteractiveChat.Data.Repository.IRepository;

public interface IRepository<T> where T : class
{
    void Add(T entity);
    T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false);
    void Delete(T entity);
    
}