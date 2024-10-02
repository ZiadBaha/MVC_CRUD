using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.Helper;
using Demo.PL.Models;
using Demp.BLL;
using Demp.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
	[Authorize]
	public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;
       
        public EmployeeController(
           IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            this.mapper = mapper;
          
        }
        //BaseUrl/Employee/Index => Request
        public async Task<IActionResult> Index(string SearchInp)
        {
            var employees = Enumerable.Empty<Employee>();
            if (string.IsNullOrEmpty(SearchInp))
            {
                employees = await  _unitOfWork.EmployeeRepository.GetAllAsync();
            }
            else
            {
                 employees= _unitOfWork.EmployeeRepository.SearchByName(SearchInp.ToLower());
            }
            var mappedEmployees=mapper.Map<IEnumerable<Employee>,IEnumerable< EmployeeViewModel>>(employees);
            return View(mappedEmployees);
        }

		public async Task<IActionResult> Search(string SearchInp)
		{
			var employees = Enumerable.Empty<Employee>();


			if (string.IsNullOrEmpty(SearchInp))
			{
				employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
			}
			else
			{
				employees =  _unitOfWork.EmployeeRepository.SearchByName(SearchInp.ToLower());

			}
			var result = mapper.Map<IEnumerable<EmployeeViewModel>>(employees);

			return PartialView("EmployeeTablePartialView", result);
		}

		//BaseUrl/Employee/Create => Request
		public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {

            employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "Images");

            var mappedEmp = mapper.Map<EmployeeViewModel, Employee>(employeeVM);


            if (ModelState.IsValid)
            {
                await _unitOfWork.EmployeeRepository.AddAsync(mappedEmp);
                await _unitOfWork.Complete();

                return RedirectToAction(nameof(Index));
            }
            return View(employeeVM);
        }

        // /Employee/Details/10
        // [HttpGet]
        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (!id.HasValue)
                return BadRequest(); //400
            var employee = await  _unitOfWork.EmployeeRepository.GetAsync(id.Value);
            var mappedEmp = mapper.Map<Employee, EmployeeViewModel>(employee);
            if (employee == null)
                return NotFound(); //404
            return View(ViewName, mappedEmp);
        }
        public async Task<IActionResult> Edit(int? id)
        {

            return await Details(id, "Edit");
         
        }
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Edit([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            // check if id send by form is the same id send by route [Security]
            if (id != employeeVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var employee = await _unitOfWork.EmployeeRepository.GetAsync(employeeVM.Id);

                    if (employeeVM.Image != null)
                    {
                        // Delete the old photo only if a new one is provided
                        if (!string.IsNullOrEmpty(employee.ImageName))
                        {
                            DocumentSettings.DeleteFile(employee.ImageName, "Images");
                        }
                        // Upload the new photo
                        employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "Images");
                    }
                    else
                    {
                        // If no new image is provided, keep the existing one
                        employeeVM.ImageName = employee.ImageName;
                    }
                    _unitOfWork.EmployeeRepository.DetachEnitity(employee);
                    var mappedEmployee = mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    _unitOfWork.EmployeeRepository.Update(mappedEmployee);
                    await _unitOfWork.Complete();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    /// Log exception
                    /// friendly message
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(employeeVM);
        }

        public  async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Delete([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
                return BadRequest();
            if (ModelState.IsValid) //server side validation
            {
                try
                {
                    var mappedEmployee = mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                    if (mappedEmployee.ImageName != null)
                    {
                        DocumentSettings.DeleteFile(employeeVM.ImageName, "Images");
                    }

                    _unitOfWork.EmployeeRepository.Delete(mappedEmployee);
                    var count = await _unitOfWork.Complete();
                    //if (count > 0) 
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception ex)
                {
                    // log exception
                    // friendly message

                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(employeeVM);
        }
    }
}

