using Company.Ali.DAL.Models;
using Company.Ali.PL.DTOs;
using Company.Ali.PL.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Company.Ali.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        #region SignUp

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpDto model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                var _user = await _userManager.FindByNameAsync(model.UserName);
                if (_user is null)
                {
                    _user = await _userManager.FindByEmailAsync(model.Email);
                    if (_user is null)
                    {
                        var user = new ApplicationUser()
                        {
                            UserName = model.UserName,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            IsAgree = model.IsAgree,

                        };
                        var result = await _userManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("SignIn");
                        }
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                ModelState.AddModelError("", "Invalid SignUp !!");
            }
            return View();
        }








        #endregion

        #region SignIn

        [HttpGet]
        public IActionResult SignIn()
        {
            SignInDto tmpDto = new SignInDto();
            return View(tmpDto);
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var flag = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (flag)
                    {
                        // Sign in 
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                        if (result.Succeeded)
                        {
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                        }
                    }
                }
            }
            return View();
        }

        #endregion

        #region SignOut

        [HttpGet]
        public new async Task<IActionResult> SignOUt()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }

        #endregion

        #region Forget Password

        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }


        public async Task<IActionResult> SendResetPasswordURL(ResetPassword model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    // Reset The Password 

                    // Create Token
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    // Create URL
                    var url = Url.Action("ChangePassword","Account", new { email = model.Email, token }, Request.Scheme);

                    var email = new Email()
                    {
                        To = model.Email,
                        Subject = "Reset Vodafone Password",
                        Body = url

                    };




                    // Send Email 
                    var flag = EmailSettings.SendEmail(email);
                    if (flag)
                    {
                        // Check Your Inbox . 
                        return RedirectToAction("CheckYourInbox");
                    }
                }
            }
            ModelState.AddModelError("", "Invalid Reset Password Operation :(");
            return View(nameof(ResetPassword), model);
        }


        // Check Your Inbox
        [HttpGet]
        public IActionResult CheckYourInbox()
        {
            return View();
        }

        #endregion

        #region Change Password

        [HttpGet]
        public IActionResult ChangePassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePassword model)
        {
            if (ModelState.IsValid)
            {
                var email = TempData["email"] as string;
                var token = TempData["token"] as string;

                if (email is null && token is null) return BadRequest("Invalid Operations");
                var user = await _userManager.FindByEmailAsync(email);
                if (user is not null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(SignIn));
                    }
                }
                ModelState.AddModelError("", "Invalid Reset Password Operation :)");
            }
            return View();
        }
        #endregion
    }
}
