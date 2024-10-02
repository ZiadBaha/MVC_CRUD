using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models
{
    // ViewModel => List Columns that will be rendered to view (Id => in Model Not ViewModel )
    // Model     => List all columns that will be mapped to Database (drevied attribute => will be in ViewModel Not Model)
    public class Department:ModelBase
    {
        public string Code { get; set; }

        public string Name { get; set; }
        public DateTime DateOfCreation { get; set; }
        //Navigational property [Many]
        [InverseProperty(nameof(Employee.Department))]
        public ICollection<Employee> Employees { get; set; } =new HashSet<Employee>();
    }
}
