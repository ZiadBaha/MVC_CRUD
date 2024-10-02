using Demo.DAL.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace Demo.PL.Models
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Department Code Is Required!!")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Department Name Is Required!!")]
        public string Name { get; set; }
        //Dispaly Name in View
        [Display(Name = "Date Of Creation")]
        public DateTime DateOfCreation { get; set; }
        //Navigational property [Many]
        [InverseProperty(nameof(Employee.Department))]
        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
    }
}
