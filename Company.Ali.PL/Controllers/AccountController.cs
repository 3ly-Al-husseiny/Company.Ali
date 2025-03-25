using Company.Ali.DAL.Models;
using Company.Ali.PL.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Company.Ali.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public AccountController(UserManager<ApplicationUser> userManager , SignInManager<ApplicationUser> signInManager)
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
            return View();
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
    }
}
