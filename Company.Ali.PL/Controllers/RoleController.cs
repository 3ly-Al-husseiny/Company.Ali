using AspNetCoreGeneratedDocument;
using Company.Ali.DAL.Models;
using Company.Ali.PL.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company.Ali.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager , UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        #region Index
        [HttpGet]
        public IActionResult Index(string? SearchInput)
        {
            IEnumerable<RoleDto> roles;
            if (string.IsNullOrEmpty(SearchInput))
            {
                roles = _roleManager.Roles.Select(r => new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name,
                });
            }
            else
            {
                roles = _roleManager.Roles
                    .Select(r => new RoleDto
                    {
                        Id = r.Id,
                        Name = r.Name,
                    })
                    .Where(r => r.Name.ToLower().Contains(SearchInput.ToLower()));
            }
            return View(roles);
        }
        #endregion

        #region Create

        [HttpGet]
        public IActionResult Create() 
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(RoleDto model) 
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                // Check if the role is Unique
                var IsUnique = await _roleManager.FindByNameAsync(model.Name);
                if (IsUnique is null) 
                {
                    var role = new IdentityRole()
                    {
                        Name = model.Name
                    };

                    // Check if the Operation is Succeeded
                    var result = await _roleManager.CreateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }
                ModelState.AddModelError("","This Name has been already taken , Your role name should be unique :)");
            }
            return View(model);
        }


        #endregion

        #region Details
        public async Task<IActionResult> Details(string id, string viewName)
        {
            if (id is null) return BadRequest("Invalid Id");
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null) return NotFound(new { StatusCode = 404, message = $"Role with Id: {id} is not found :(" });

            var model = new RoleDto
            {
                Id = role.Id,
                Name = role.Name,
            };

            return View(viewName, model);
        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id is null) return BadRequest("Invalid Id :(");
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null) return NotFound(new { StatusCode = 404, message = "Role Not Found :(" });

            var model = new RoleDto
            {
                Id = role.Id,
                Name = role.Name,
            };
            return View("Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleDto model)
        {
            if (ModelState.IsValid)
            {
                if (id is null) return BadRequest("Invalid Id :(");
                var role = await _roleManager.FindByIdAsync(id);
                if (role is null) return NotFound("Role is not found :(");
                // Check if the name is unique
                var IsUnique = await _roleManager.FindByNameAsync(model.Name);
                if (IsUnique is null)
                {
                    // Upadate only the Name 
                    role.Name = model.Name;
                    var result = await _roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                ModelState.AddModelError("", "This Name Has Been Taken by another User , Your name should be Unique :)");
            }
            return View("Edit", model);
        }
        #endregion

        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id, RoleDto model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest("Invalid Operation :(");

                var role = await _roleManager.FindByIdAsync(id);
                if (role is null) return NotFound(new { StatusCode = 404, message = $"Role with Id: {id} is not found" });

                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }
        #endregion


        #region Add Role

        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId) 
        {
            if (roleId is null) return BadRequest("Invalid Id");
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null) return NotFound( new { StatusCode = 404, message = $"Role With Id: {roleId} is not found" });
            var UsersInRole = new List<UsersInRoleViewModel>();
            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var UserInRole = new UsersInRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    UserInRole.IsSelected = true;
                }
                else 
                {
                    UserInRole.IsSelected = false;
                }
                UsersInRole.Add(UserInRole);
            }

            ViewData["roleId"] = roleId;
            return View(UsersInRole);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId, List<UsersInRoleViewModel> users)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null)
                return NotFound();
            if (ModelState.IsValid)
            {
                foreach (var user in users)
                {
                    var appUser = await _userManager.FindByIdAsync(user.UserId);
                    if (appUser is not null)
                    {
                        if (user.IsSelected && !await _userManager.IsInRoleAsync(appUser, role.Name))
                        {
                            await _userManager.AddToRoleAsync(appUser, role.Name);
                        }
                        else if (!user.IsSelected && !await _userManager.IsInRoleAsync(appUser, role.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(appUser, role.Name);
                        }
                    }
                }
                return RedirectToAction(nameof(Index), new { id = roleId });
            }
            return View(users);
        }

        #endregion

    }
}
