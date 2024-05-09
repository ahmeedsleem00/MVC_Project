using Demo.DAL.Models;
using Demo.PL.Helper;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class AccountController : Controller
    {


		private readonly UserManager<ApplicationUser> _userManager;

		private readonly SignInManager<ApplicationUser> _signInManager;

		//public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
		//{
		//	_userManager = userManager;
		//	_signInManager = signInManager;
		//}


		#region    Sign Up

		[HttpGet]
		public IActionResult SignUp()
		{
			return View();
		}




		[HttpPost]
		public async Task<IActionResult> SignUp(SignUpViewModel model)
		{

			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByNameAsync(model.UserName);

				if (user is null)
				{

					user = new ApplicationUser()
					{
						UserName = model.UserName,
						Email = model.Email,
						FirstName = model.FirstName,
						LastName = model.LastName,
						IsAgree = model.IsAgree

					};

					var result = await _userManager.CreateAsync(user, model.Password);
				

					if (result.Succeeded)

						return RedirectToAction(nameof(SignIn));


					foreach (var error in result.Errors)
					{


						ModelState.AddModelError(string.Empty, error.Description);

					}

				}

				ModelState.AddModelError(string.Empty, " UserName Is Already Exits  ");

			}


			return View(model);


		}



		#endregion

		#region    Sign In


		[HttpGet]
		public IActionResult SignIn()
		{


			return View();
		}



		[HttpPost]
		public async Task<IActionResult> SignIn(SignInViewModel model)
		{

			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByNameAsync(model.Email);

				if (user is not null)
				{

					var flag = await _userManager.CheckPasswordAsync(user, model.Password);

				     if (flag) 
				     {

						var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe,lockoutOnFailure: false);

                        if (result.Succeeded)
                        {
							return RedirectToAction(nameof(HomeController.Index), controllerName: "Home");
                        }


                     }
  

				}









				ModelState.AddModelError(string.Empty, errorMessage: "Invalid Login !");

				
			}




			return View();


		}



















		#endregion

		#region   Sign Out



		public async Task<IActionResult> SignOut()
		{


			await _signInManager.SignOutAsync();
			return RedirectToAction(nameof(SignIn));


		}









		#endregion

		#region ForgetPassword
		public IActionResult ForgetPassword()
		{
			return View();
		}

		public async Task<IActionResult> SendResetPasswordUrl(ForgetPasswordViewModel model)
		{
			if (!ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user is not null)
				{
					//Generate Token
					var token = await _userManager.GeneratePasswordResetTokenAsync(user);

					//Create URL Which Send in Body of The Email
					var url = Url.Action(action: "ResetPassword", controller: "Action", new { email = model.Email, token = token }, Request.Scheme);

					//https://localhost:44393/Account/ResetPasswordUrl?email=ahmed@gmail.com?token=

					//Create Email
					var email = new Email()
					{
						Subject = "Reset Your Password",
						Recipients = model.Email,
						Body = url
					};

					//Send Email
					EmailSettings.SendEmail(email);

					//Redirect To Action
					return RedirectToAction(nameof(CheckYourInbox));
				}

				ModelState.AddModelError(string.Empty, errorMessage: "Invalid Email");

			}
			return View(nameof(ForgetPassword), model);
		}

		public IActionResult CheckYourInbox()
		{
			return View();
		}

		#endregion

		#region Reset Password

		[HttpGet]
		public IActionResult ResetPassword(string email, string token)
		{

			TempData[key:"email"] =email;
			TempData[key:"token"] =token;



			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var email = TempData[key:"email"] as string;
				var token = TempData[key:"token"] as string;

			var user = await _userManager.FindByEmailAsync(email);
				if(user is not null)
				{
					var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
					if (result.Succeeded)
					{
						return RedirectToAction(nameof(SignIn));
						foreach(var error in result.Errors)
						{
							ModelState.AddModelError(string.Empty, error.Description);
						} 
					};
				}
				ModelState.AddModelError(string.Empty, errorMessage: "Invalid Reset Password!!");
			}

			return View();
		}


		#endregion

		public IActionResult AccessDenied()
		{
			return View();
		}

    }


}
