using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demp.BLL.Interfaces
{
    public interface IDepartmentRepository:IGenericRepository<Department>
    {
        // IEnumerable => Return records to enumerat it (Reading)
        // ICollecion => Return records and enable =>  Read - Update - add 
        // IQuarable => Get records with filteration 
        //I can filter after get receds while enumerating but IQuerable get records filtered from DB
        // IReadOnlyList => Read but cannot enumerate (only return records in response)
        // --------------- 
        // Signature of five methods CRUD operations
        

        /*Here only signature for Methods belongs to Department only*/
    }
}
