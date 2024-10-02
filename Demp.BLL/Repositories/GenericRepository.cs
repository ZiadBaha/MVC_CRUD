using Demo.DAL.Data;
using Demo.DAL.Models;
using Demp.BLL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demp.BLL.Repositories
{
    public class GenericRepository<T>:IGenericRepository<T> where T : ModelBase
    {
        private protected readonly AppDbContext context;

        public GenericRepository(AppDbContext context)
        {
            this.context = context;
        }
        public async Task AddAsync(T entity)
        {
           // context.Set<T>().Add(entity);
            await context.AddAsync(entity);
        }
        public void Update(T entity)
        {
            context.Update(entity);
        }

        public void Delete(T entity)
        {
            context.Remove(entity);
        }

        public  async Task<T> GetAsync(int id)
        {
            return  await context.FindAsync<T>(id);
        }

        public  async Task< IEnumerable<T>> GetAllAsync()
        {
            if(typeof(T)==typeof(Employee))
              return (IEnumerable<T>) await context.Set<Employee>().Include(E=>E.Department).ToListAsync();
            else
                return await context.Set<T>().AsNoTracking().ToListAsync();
        }
        


    }
}

