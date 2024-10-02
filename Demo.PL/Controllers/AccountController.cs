using Demo.DAL.Models;
using Demo.PL.Helper;
using Demo.PL.Models;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

namespace Demo.PL.Controllers
{
	
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly ImailSettings _mailSettings;

		public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
			ImailSettings mailSettings
            )
        {
			_userManager = userManager;
			_signInManager = signInManager;
			_mailSettings = mailSettings;
		}
		#region Sign Up
		public IActionResult SignUp()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> SignUp(SignUpViewModel model)
		{
			if (ModelState.IsValid) //server side validation
			{
				var user = await _userManager.FindByNameAsync(model.UserName);
				if (user is null)
				{
					user = new ApplicationUser()
					{
						UserName = model.UserName,
						Email = model.Email,
						IsAgree = model.IsAgree,
						FName = model.FName,
						LName = model.LName,
						PhoneNumber=model.PhoneNumber
					};
					//Create User
					var result = await _userManager.CreateAsync(user, model.Password);
					if (result.Succeeded)
						return RedirectToAction(nameof(SignIn));
					foreach(var error in result.Errors)
						ModelState.AddModelError(string.Empty,error.Description);
				}
				ModelState.AddModelError(string.Empty, "UserName Is Already Exist");


			}
			return View(model);

		}
		#endregion

		#region Sign In
		public IActionResult SignIn()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> SignIn(SignInViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if(user is not null)
				{
					var flag=await _userManager.CheckPasswordAsync(user, model.Password);
					if (flag) //Correct Password 
					{
						var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
						if (result.Succeeded)
							return RedirectToAction(nameof(HomeController.Index), "Home");
						}
				}
				ModelState.AddModelError(string.Empty, "Invalid Login");
			}
			return View(model);
		}


        // GoogleLogin
        public IActionResult GoogleLogin()
        {
            var prop = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse")
            };

            return Challenge(prop, GoogleDefaults.AuthenticationScheme);
        }

        // GoogleResponse
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(

                claim => new
                {
                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value,
                }
                );

            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region SignOut
        public async  new Task<IActionResult> SignOut()
		{
			//New bec Homecontroller inherit SignOut from Controller
			await _signInManager.SignOutAsync(); //remove  token from Cookies
			return RedirectToAction(nameof(SignIn));
		}
		#endregion

		#region Forget Password
		public IActionResult ForgetPassword()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SendResetPasswordUrl(ForgetPasswordViewModel model)

		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if(user is not null)
				{
					//create token to make this url unique for one user one time
					var token =  await _userManager.GeneratePasswordResetTokenAsync(user);
					var resetPasswordUrl = Url.Action("ResetPassword", "Account", new { email = model.Email, token = token },Request.Scheme);
					//https://localhost:5001/Account/ResetPassword?email=sara@gmail.com&token=sdfgr3fghjkiuy
					var email = new Email
					{
						Subject = "Reset Your Password",
						Recipients = model.Email,
						Body = resetPasswordUrl
					};
					_mailSettings.SendMail(email);
					return RedirectToAction(nameof(CheckYourInbox));
				}
				ModelState.AddModelError(string.Empty, "Invalid Email");

			}
			return View(model);
		}
		
		public IActionResult CheckYourInbox()
		{
			return View();
		}

		#endregion

		#region Reset Password
		public IActionResult ResetPassword(string email ,string token)
		{
			TempData["email"] = email;
			TempData["token"] = token;
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetViewModel model)
		{
			if (ModelState.IsValid)
			{
				string email = TempData["email"] as string;
				string token = TempData["token"] as string;
				var user = await _userManager.FindByEmailAsync(email);
				var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
				if (result.Succeeded)
					return RedirectToAction(nameof(SignIn));
				foreach (var error in result.Errors)
					ModelState.AddModelError(string.Empty, error.Description);
			}
			return View(model);
		}
		#endregion

	}
}
