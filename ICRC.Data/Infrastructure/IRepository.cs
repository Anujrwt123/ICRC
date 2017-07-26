using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ICRC.Data.Infrastructure
{
    public interface IRepository<T> where T:class
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);        
        void Delete(Expression<Func<T, bool>> where);
        IEnumerable<T> GetAll();
        T GetByID(int ID);
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);
    }
}
