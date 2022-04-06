using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MPara.Repositories.Abstract;

namespace MPara.Repositories.Concrete.EntityFramework
{
    public class EfRepository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext _context;

        public EfRepository(DbContext context)
        {
            _context = context;
        }

        public void Add(T obj)
        {
            _context.Set<T>().Add(obj);
        }

        public void Delete(T obj)
        {
            _context.Set<T>().Remove(obj);
        }

        public void Update(T obj)
        {
            _context.Entry(obj).State = EntityState.Modified;
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

    }
}
