using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
	public class UserController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper mapper;

        public UserController(
            UserManager<ApplicationUser> userManager
            ,SignInManager<ApplicationUser> signInManager,
			IMapper mapper

            )
        {
			_userManager = userManager;
			_signInManager = signInManager;
            this.mapper = mapper;
        }
		public async Task< IActionResult> Index(string SearchValue)
		{
			if (string.IsNullOrEmpty(SearchValue))
			{
				var users = await _userManager.Users.Select(U => new UserViewModel()
				{
					Id = U.Id,
					Fname = U.FName,
					Lname = U.LName,
					Email = U.Email,
					PhoneNumber=U.PhoneNumber,
					Roles = _userManager.GetRolesAsync(U).Result
				}).ToListAsync();
                return View(users);
            }
            else
			{
				var users = await _userManager.Users.Where(U => U.Email
								  .ToLower()
								  .Contains(SearchValue.ToLower()))
								  .Select(U => new UserViewModel()
								  {
									  Id = U.Id,
									  Fname = U.FName,
									  Lname = U.LName,
									  Email = U.Email,
									  PhoneNumber=U.PhoneNumber,
									  Roles = _userManager.GetRolesAsync(U).Result
								  }).ToListAsync();

			return View(users);
			}

		}
		public async Task<IActionResult> Search(string SearchValue)
		{
			var users = Enumerable.Empty<UserViewModel>();


			if (string.IsNullOrEmpty(SearchValue))
			{
				users = await _userManager.Users.Select(U => new UserViewModel()
				{
					Id = U.Id,
					Fname = U.FName,
					Lname = U.LName,
					Email = U.Email,
					PhoneNumber = U.PhoneNumber,
					Roles = _userManager.GetRolesAsync(U).Result
				}).ToListAsync();
			}
			else
			{

				users = await _userManager.Users.Where(U => U.Email
								  .ToLower()
								  .Contains(SearchValue.ToLower()))
								  .Select(U => new UserViewModel()
								  {
									  Id = U.Id,
									  Fname = U.FName,
									  Lname = U.LName,
									  Email = U.Email,
									  PhoneNumber = U.PhoneNumber,
									  Roles = _userManager.GetRolesAsync(U).Result
								  }).ToListAsync();

			}
			return PartialView("UserTableView",users);

		}

		public async Task<IActionResult> Details(string Id ,string viewName="Details")
		{
			if (Id is null)
				return BadRequest();
			var user =  await _userManager.FindByIdAsync(Id);
			if (user is null)
				return NotFound();
			var mappedUser = mapper.Map<ApplicationUser, UserViewModel>(user);
			return View(viewName,mappedUser);

		}
		public async Task< IActionResult> Edit(string Id)
		{
			return await  Details(Id, "Edit");
		}
		[HttpPost]
		public  async Task<IActionResult> Edit(UserViewModel model, [FromRoute] string id)
		{
			if (id != model.Id)
				return BadRequest();
			if (ModelState.IsValid)
			{
				try
				{
					var user =await  _userManager.FindByIdAsync(id);
					user.PhoneNumber = model.PhoneNumber;
					user.FName = model.Fname;
					user.LName = model.Lname;
                    await _userManager.UpdateAsync(user);
                    return RedirectToAction(nameof(Index));
                }
				catch(Exception ex)
				{
                       ModelState.AddModelError(string.Empty, ex.Message);
				}
				
			}
			return View(model);
		}

		public async Task<IActionResult> Delete(string Id)
		{
			return  await Details(Id, "Delete");
		}
		[HttpPost]
		public async Task<IActionResult> ConfirmDelete(string Id)
		{
			try
			{
				var user =  await _userManager.FindByIdAsync(Id);
				await _userManager.DeleteAsync(user);
				return RedirectToAction(nameof(Index));
			}
			catch(Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
				return RedirectToAction("Error", "Home");
			}
		}



	}
}
