using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyBookShop_DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T:class   //необзодио создать интерфейс с дженерки типом
    {
            T Find(int id);             //метод который будет искать данные по id

        //долее метод который будет возвращать перечисление наших параметров Т, у него несколько входных параметров
        IEnumerable<T> GetAll(
            Expression<Func<T, bool>> filter = null,                    //функция фильтра возращает бул
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBY = null,
            string includeProperty = null,
            bool isTracking = true
            );

        T FirstODefault(
            Expression<Func<T, bool>> filter = null,                    //функция фильтра возращает бул
            string includeProperty = null,
            bool isTracking = true
            );


        void Add(T entity);
        void Remove(T entity);
        void Save();
    }
}
