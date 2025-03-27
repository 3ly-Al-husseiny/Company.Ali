using Company.Ali.DAL.Models;
using Company.Ali.PL.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using NuGet.DependencyResolver;
using System.Threading.Tasks;

namespace Company.Ali.PL.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        #region Index
        [HttpGet]
        public IActionResult Index(string? SearchInput)
        {
            IEnumerable<UserDto> Users;
            if (string.IsNullOrEmpty(SearchInput))
            {
                Users = _userManager.Users.Select(U => new UserDto()
                {
                    Id = U.Id,
                    UserName = U.UserName,
                    Email = U.Email,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    Roles = _userManager.GetRolesAsync(U).Result
                });
            }
            else
            {
                Users = _userManager.Users.Select(U => new UserDto()
                {
                    Id = U.Id,
                    UserName = U.UserName,
                    Email = U.Email,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    Roles = _userManager.GetRolesAsync(U).Result
                }).Where(U => U.FirstName.ToLower().Contains(SearchInput.ToLower()));
            }
            return View(Users);
        }

        #endregion

        #region Details

        public async Task<IActionResult> Details(string Id, string ViewName)
        {
            if (Id is null) return BadRequest("Invalid Id");
            var user = await _userManager.FindByIdAsync(Id);
            if (user is null) return NotFound(new { StatusCode = 404, message = $"User With Id :{Id} is not found :(" });

            var model = new UserDto()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = _userManager.GetRolesAsync(user).Result
            };
            
            return View(ViewName,model);
        }
        #endregion

        #region Edit
        
        [HttpGet]
        public async Task<IActionResult> Edit(string Id ) 
        {
            if (Id is null) return BadRequest("Invalid Id :(");
            var user = await _userManager.FindByIdAsync(Id);
            if (user is null) return NotFound(new { StatusCode = 404, message = "User Not Found :(" });
            var model = new UserDto()
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Roles = _userManager.GetRolesAsync(user).Result
            };
            return View("Edit", model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute]string? id , UserDto model) 
        {
            if (ModelState.IsValid)
            {
                if (id is null) return BadRequest("Invalid Id :(");
                var user = await _userManager.FindByIdAsync(id);
                if (user is null) return NotFound("User Is not Found :(");
                // Updating the User Values depending on the New Values with Model . 

                user.UserName = model.UserName;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;


                var result = await _userManager.UpdateAsync(user); // Update & Save Changes
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View("Edit",model);
        }

        #endregion

        #region Delete

        [HttpGet]
        public async Task<IActionResult> Delete(string? id) 
        {
            return await Details(id, "Delete"); // Re-Use the Details Action
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute]string id ,UserDto model) 
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest("Invalid Operation :(");

                var user = await _userManager.FindByIdAsync(id);
                if (user is null) return NotFound(new { StatusCode = 404, message = $"User with Id: {id} is not found" });

                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }
        #endregion


    }
}
