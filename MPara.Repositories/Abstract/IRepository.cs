using System;
using System.Linq;
using System.Linq.Expressions;

namespace MPara.Repositories.Abstract
{
    public interface IRepository<T> where T : class
    {
        T GetById(int id);
        IQueryable<T> GetAll();
        IQueryable<T> Find(Expression<Func<T, bool>> predicate);
        void Add(T obj);
        void Delete(T obj);
        void Update(T obj);
        void Save();
    }
}
