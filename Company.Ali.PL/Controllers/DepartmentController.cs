using Company.Ali.BLL.Interfaces;
using Company.Ali.BLL.Repositories;
using Company.Ali.DAL.Models;
using Company.Ali.PL.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Abstractions;

namespace Company.Ali.PL.Controllers
{
    // MVC Controller
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;

        // Ask CLR Create Object From DepartmentRepository
        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        [HttpGet] // GET: /Department/Index
        public IActionResult Index()
        {
            var department = _departmentRepository.GetAll();
            return View(department);
        }
        //public IActionResult Create()
        //{
        //    return View();
        //}

        public IActionResult Create(CreateDepartmentDto model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                // mapping
                var department = new Department()
                {
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt
                };
                var count = _departmentRepository.Add(department);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }



        [HttpGet]
        public IActionResult Details(int? id , string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");
            else
            {
                var department = _departmentRepository.Get(id.Value);
                if (department is null) return NotFound(new { statusCode = 404, message = $"Department With Id: {id} is not Found" });


                // Manual Mapping from Department to CreateDepartmentDto
                var model = new CreateDepartmentDto()
                {
                    Code = department.Code,
                    Name = department.Name,
                    CreateAt = department.CreateAt
                };
                return View(viewName,model);
            }
        }



        [HttpGet]

        public IActionResult Edit(int? id)
        {

            //if (id is null) return BadRequest("Invalid ID");

            //var department = _departmentRepository.Get(id.Value);
            //if (department is null) return NotFound(new { statusCode = 404, message = $"Department With Id: {id} is not Found" });

            return Details(id , "Edit");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int? id, Department department)
        {
            if (ModelState.IsValid)
            {
                if (id is null) return BadRequest("Invalid ID");
                if (department is null) return NotFound(new { statusCode = 404, message = $"Department With Id: {id} is not Found" });
                var count = _departmentRepository.Update(department);
                if (count > 0)
                { 
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(department);
        }




        [HttpGet]
        public IActionResult Delete(int? id)
        {

            //if (id is null) return BadRequest("Invalid ID");

            //var department = _departmentRepository.Get(id.Value);
            //if (department is null) return NotFound(new { statusCode = 404, message = $"Department With Id: {id} is not Found" });
           
            return Details(id , "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, Department department)
        {
            if (ModelState.IsValid)
            {
                if (id != department.Id) return BadRequest("Invalid Id"); // 404
                var count = _departmentRepository.Delete(department);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(department);
        }

    }
}
