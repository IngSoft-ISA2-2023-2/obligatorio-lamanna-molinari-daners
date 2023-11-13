using Microsoft.EntityFrameworkCore;
using PharmaGo.IDataAccess;
using System.Linq.Expressions;


namespace PharmaGo.DataAccess.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        private readonly PharmacyGoDbContext _context;

        public BaseRepository(PharmacyGoDbContext context)
        {
            _context = context;
        }

        public virtual IEnumerable<T> GetAllByExpression(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression);
        }

        public virtual T GetOneByExpression(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().FirstOrDefault(expression);
        }

        public virtual void InsertOne(T elem)
        {
            _context.Set<T>().Add(elem);
        }

        public virtual void DeleteOne(T elem)
        {
            _context.Set<T>().Remove(elem);
        }

        public void UpdateOne(T elem)
        {
            _context.Set<T>().Update(elem);
        }

        public virtual bool Exists(T elem)
        {
            T? _elem = _context.Set<T>().Find(elem);
            if (_elem is null) return false;
            return true;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public bool Exists(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Any(expression);
        }

        public virtual IEnumerable<T> GetAllBasicByExpression(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression);
        }


        public virtual T GetOneDetailByExpression(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().FirstOrDefault(expression);
        }
    }
}
