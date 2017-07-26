using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ICRC.Data.Infrastructure
{
    public class RepositoryBase<T>where T :class
    {
        private ICRCEntities dataContext;

        private readonly IDbSet<T> dbSet;


        protected IDbFactory DbFactory
        {
            get;
            private set;
        }

        protected ICRCEntities DbContext
        {
            get { return dataContext ?? (dataContext = DbFactory.Init()); }
        }

        protected RepositoryBase(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;
            dbSet = DbContext.Set<T>();
        }

        #region Methods
            
        public virtual void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Update(T entity)
        {                        
            dbSet.Attach(entity);
            dataContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public virtual void Delete(Expression<Func<T,bool>>where)
        {
            IEnumerable<T> objects = dbSet.Where(where);
            foreach(T obj in objects)
            {
                dbSet.Remove(obj);
            }
            
        }

        public virtual IEnumerable<T> GetAll()
        {
            return dbSet.AsQueryable<T>();
        }

        public virtual IEnumerable<T> GetMany(Expression<Func<T,bool>>where)
        {
            return dbSet.Where(where).AsQueryable();
        }

        public virtual T GetByID(int ID)
        {
            return dbSet.Find(ID);
        }

        public T Get(Expression<Func<T,bool>>where)
        {
            return dbSet.Where(where).FirstOrDefault<T>();
        }

        #endregion


    }
}









