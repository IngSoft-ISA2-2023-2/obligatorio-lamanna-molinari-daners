using System.Linq.Expressions;

namespace PharmaGo.IDataAccess
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAllByExpression(Expression<Func<T, bool>> expression);
        T GetOneByExpression(Expression<Func<T, bool>> expression);
        void InsertOne(T elem);
        void DeleteOne(T elem);
        void UpdateOne(T elem);
        void Save();
        bool Exists(Expression<Func<T, bool>> expression);
        bool Exists(T elem);
        IEnumerable<T> GetAllBasicByExpression(Expression<Func<T, bool>> expression);
        T GetOneDetailByExpression(Expression<Func<T, bool>> expression);
    }
}