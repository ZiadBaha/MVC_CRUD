using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models
{
    public  class Employee :ModelBase
    {
        //[Required]
        //[MaxLength] 
        //Write Them By Fluent To Make it poco class only
        public string Name { get; set; }

        public int? age { get; set; }
        
        public string  Address { get; set; }
        public decimal Salary { get; set; }
        public bool IsActive { get; set; } //In Sql Will be bit
        public string Email { get; set; }

        public string PhoneNumber { get; set; }
        public DateTime HireDate { get; set; }
        public string ImageName { get; set; }
        public bool IsDeleted { get; set; } = false;// for back end only
        // we have to types OF Delete 
        //1. Hard Delete :Delete From DB at Once
        //2. Soft Delete: Declare Property Called IsDeleted ,when we delete emmloyee
        //from DB Make tis flag true and when we get data from this table make condition
        // to ensure that this prop is not true
        public DateTime CreationDate { get; set; } = DateTime.Now; //for back end only 
        //[ForeignKey("Department")]
        public int? DepartmentId { get; set; } //ForignKey column (Look At Department Config)
        //Navigational Property[One]
       //  [InverseProperty(nameof(Models.Department.Employees))] Using if we have more than one relation
       //[InverseProperty("Employees")]
        public Department Department { get; set; }
    }
}
