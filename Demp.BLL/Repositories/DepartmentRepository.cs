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
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        // NOTE Const vs readonly
        // Const take initial value in declaration line (then cannot update it)
        // readonly take initial value in constructor (then cannot update it)
        // Creating object from DepartmentRepository depands on AppDbContext object injection
        //Creating Object from DbContext depend on creating obj from DbContextOptions
        //That Calling Last Override of Configure
        private readonly AppDbContext _dbContext;
        public DepartmentRepository(AppDbContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }

    }
}
