using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demp.BLL.Interfaces
{
    public interface IGenericRepository<T> where T:ModelBase
    {
       Task<IEnumerable<T>> GetAllAsync();
       Task<T> GetAsync(int id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        //Update ,Delete doesnot contain RemoveAsync ,UpdateAsync
        //bec they only change object state
    }
}
