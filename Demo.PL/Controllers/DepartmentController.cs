using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.Models;
using Demp.BLL.Interfaces;
using Demp.BLL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
   
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;

        public DepartmentController(
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var departments =  await _unitOfWork.DepartmentRepository.GetAllAsync();
            var mappedDept = mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(departments);
            return View(mappedDept);
        }
        public IActionResult Create() { 
            return View();
        }
        [HttpPost]
        public  async Task<IActionResult> Create(DepartmentViewModel departmentViewModel)
        {
            if(ModelState.IsValid) 
            {
                var mappedDept = mapper.Map<DepartmentViewModel, Department>(departmentViewModel);
              await  _unitOfWork.DepartmentRepository.AddAsync(mappedDept);
              await  _unitOfWork.Complete();
                 return RedirectToAction(nameof(Index));
            }
            return View(departmentViewModel);
        }
        // /Department/Details/10

        public async  Task<IActionResult> Details(int? id , string ViewName = "Details")
        {
            if (!id.HasValue)
                return BadRequest(); //400
            var department = await _unitOfWork.DepartmentRepository.GetAsync(id.Value);
            var mappedDept = mapper.Map<Department, DepartmentViewModel>(department);

            if (department == null)
                return NotFound(); //404
            return View(ViewName, mappedDept);
        }
        public  async Task<IActionResult> Edit(int? id)
        {
            return await  Details(id , "Edit");
          
        }
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Edit([FromRoute]int id ,DepartmentViewModel departmentViewModel)
        {
            if(id != departmentViewModel.Id)
                return BadRequest();
            if(ModelState.IsValid)
            {
                try
                {
                    var mappedDept = mapper.Map<DepartmentViewModel, Department>(departmentViewModel);
                    _unitOfWork.DepartmentRepository.Update(mappedDept);
                   await  _unitOfWork.Complete();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    
                    ModelState.AddModelError(string.Empty , ex.Message);
                }
            }
            return View(departmentViewModel);
        }

        public async Task<IActionResult> Delete(int? id) {
            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  async Task<IActionResult> Delete([FromRoute]int id ,DepartmentViewModel departmentViewModel)
        {
            if (id != departmentViewModel.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var mappedDept = mapper.Map<DepartmentViewModel, Department>(departmentViewModel);


                    _unitOfWork.DepartmentRepository.Delete(mappedDept);
                    await _unitOfWork.Complete();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
               
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(departmentViewModel);
        }
    }
}
