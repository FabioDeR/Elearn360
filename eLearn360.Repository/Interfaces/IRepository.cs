using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Repository.Interfaces
{
    public interface  IRepository<TKey, T> where T : class
    {
        Task<IEnumerable<T>> Get();
        Task<T> GetById(TKey id);
        Task<T> Post(T entity);
        Task Put(TKey id, T entity);
        Task Delete(TKey id, T entity);
      
    }
}
