using System.Linq.Expressions;
using InteractiveChat.Data.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace InteractiveChat.Data.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _dbContext;

    // DbSet represents a table of database
    internal DbSet<T> dbSet;

    public Repository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        dbSet = _dbContext
            .Set<
                T>(); // Takes the collection of type T and saves it in dbSet, then we can do crud operations on that collection
    }


    public void Add(T entity)
    {
        dbSet.Add(entity);
    }

    public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false)
    {
        IQueryable<T> query;
        if (tracked)
            query = dbSet;
        else
            query = dbSet.AsNoTracking();

        query = query.Where(filter);
        if (!string.IsNullOrEmpty(includeProperties))
            foreach (var includeProp in includeProperties
                         .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProp);
        return query.FirstOrDefault();
    }

    public void Delete(T entity)
    {
        var obj = dbSet.FirstOrDefault(o => o.Equals(entity));
        if (obj != null)
        {
            dbSet.Remove(obj); // Corrected removal
            _dbContext.SaveChanges(); // Persist the deletion
        }
    }

}