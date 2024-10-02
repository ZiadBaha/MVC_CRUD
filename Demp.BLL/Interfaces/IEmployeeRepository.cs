using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demp.BLL.Interfaces
{
    public interface IEmployeeRepository:IGenericRepository<Employee>
    {
        /*Here only signature for Methods belongs to Employee only*/
        IQueryable<Employee> GetEmployeesByAddress(string address);
        //I Need Filter To be Applied In DB not Application So I cannot use Task bec DB cannot work Async


        IQueryable<Employee> SearchByName(string name);
       void DetachEnitity(Employee employee);
    }
}
