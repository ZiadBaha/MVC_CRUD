using Demo.DAL.Data;
using Demp.BLL.Interfaces;
using Demp.BLL.Repositories;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demp.BLL
{
    public class UnitOfWork : IUnitOfWork 
    {
        private readonly AppDbContext _dbContext;
        //automatic properties
        public IDepartmentRepository DepartmentRepository { get ; set ; }
        public IEmployeeRepository EmployeeRepository { get ; set; }

        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            DepartmentRepository = new DepartmentRepository(_dbContext);
            EmployeeRepository = new EmployeeReository(_dbContext);
        }

        public Task<int> Complete() 
        {
            return _dbContext.SaveChangesAsync();
        }
     

        public async ValueTask DisposeAsync()
        {
           await  _dbContext.DisposeAsync();

        }
        //Once reqest finished Object from unit of work wil be unreachable in heap 
        //but still exist and openning connection with Dbcontext (bec it ask CLR to create obkect from DbContext)
        // it is resposibipity for it to close(dispode this connection)
    }
}
