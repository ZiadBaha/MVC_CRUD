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
    public class EmployeeReository : GenericRepository<Employee>, IEmployeeRepository
    {

        public EmployeeReository(AppDbContext context) //Ask CLR To Create Obj From AppDbContext
            : base(context)
        {

        }

        public IQueryable<Employee> GetEmployeesByAddress(string address)
        {
            return context.Employees.Where(e => e.Address.Trim().ToLower().Contains(address.Trim().ToLower()));
        }

        public IQueryable<Employee> SearchByName(string name)
       => context.Employees.Where(E => E.Name.ToLower().Contains(name));

        public void DetachEnitity(Employee employee)
        {
            var entry = context.Entry(employee);
            entry.State = EntityState.Detached;
        }
    }
}
