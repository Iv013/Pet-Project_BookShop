using Microsoft.EntityFrameworkCore;
using MyBookShop_DataAccess.Repository.IRepository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookShop_DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDBContext _db;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDBContext db)
        {
            _db = db;
            this.dbSet = db.Set<T>();
        }

        public void Add(T entity)
        {
           dbSet.Add(entity);
        }

        public T Find(int id)
        {
            return dbSet.Find(id);
        }

        public T FirstOfDefault(System.Linq.Expressions.Expression<Func<T, bool>> filter = null, string includeProperty = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperty != null)
            {
                foreach (var includeProp in includeProperty.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }    
            if (!isTracking)
            {
                query = query.AsNoTracking();
            }
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(System.Linq.Expressions.Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBY = null, string includeProperty = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;
            if (filter!= null){
                query= query.Where(filter);
            }
            if(includeProperty!= null)
            { 
                foreach(var includeProp in includeProperty.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)){
                    query = query.Include(includeProp);
                }           
            }
            if (orderBY != null)
            {
                query = orderBY(query);
            }
            if (!isTracking)
            {
                query = query.AsNoTracking();
            }
            return query.ToList();

        }

        public void Remove(T entity)
        {
           dbSet.Remove(entity);
        }
        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }

        public void Save()
        {
           _db.SaveChanges();
        }
    }
}
