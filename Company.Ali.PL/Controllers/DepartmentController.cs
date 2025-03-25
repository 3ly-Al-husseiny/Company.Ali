using Company.Ali.BLL.Interfaces;
using Company.Ali.BLL.Repositories;
using Company.Ali.DAL.Models;
using Company.Ali.PL.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.IdentityModel.Abstractions;
using System.Threading.Tasks;

namespace Company.Ali.PL.Controllers
{
    [Authorize]
    // MVC Controller
    public class DepartmentController : Controller
    {
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        // Ask CLR Create Object From DepartmentRepository
        public DepartmentController(/*IDepartmentRepository departmentRepository*/ IUnitOfWork unitOfWork)
        {
            //_departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;

        }

        [HttpGet] // GET: /Department/Index
        public async Task<IActionResult> Index()
        {
            var department = await _unitOfWork.DepartmentRepository.GetAllAsync();
            return View(department);
        }
        //public IActionResult Create()
        //{
        //    return View();
        //}

        public async Task<IActionResult> Create(CreateDepartmentDto model)
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
                _unitOfWork.DepartmentRepository.AddAsync(department);
                var count = await _unitOfWork.Complete();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> Details(int? id , string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");
            else
            {
                var department = await _unitOfWork.DepartmentRepository.GetAsync(id.Value);
                if (department is null) return NotFound(new { statusCode = 404, message = $"Department With Id: {id} is not Found" });


                //// Manual Mapping from Department to CreateDepartmentDto
                //var model = new CreateDepartmentDto()
                //{
                //    Code = department.Code,
                //    Name = department.Name,
                //    CreateAt = department.CreateAt
                //};

                return View(viewName,department);
            }
        }



        [HttpGet]

        public async Task<IActionResult> Edit(int? id)
        {

            //if (id is null) return BadRequest("Invalid ID");

            //var department = _departmentRepository.Get(id.Value);
            //if (department is null) return NotFound(new { statusCode = 404, message = $"Department With Id: {id} is not Found" });

            //return Details(id , "Edit");

            if (id is null) return BadRequest("Invalid Id");
            var department = await _unitOfWork.DepartmentRepository.GetAsync(id.Value);
            if (department is null) return NotFound(new { statusCode = 404, message = $"Department With Id: {id} is not found" });
            var model = new CreateDepartmentDto()
            {
                Name = department.Name,
                Code = department.Code,
                CreateAt = department.CreateAt,
            };
            return View(model);
        }
        

        



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateDepartmentDto model)
        {
            if (ModelState.IsValid)
            {
                //if (id != model.Id) return BadRequest(); //

                var department = new Department()
                {
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt,
                    Id = id
                };
                _unitOfWork.DepartmentRepository.Update(department);
                var count = await _unitOfWork.Complete();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }




        [HttpGet]
        public Task<IActionResult> Delete(int? id)
        {

            //if (id is null) return BadRequest("Invalid ID");

            //var department = _departmentRepository.Get(id.Value);
            //if (department is null) return NotFound(new { statusCode = 404, message = $"Department With Id: {id} is not Found" });
           
            return Details(id , "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, Department department)
        {
            
                if (id != department.Id) return BadRequest("Invalid Id"); // 404
                _unitOfWork.DepartmentRepository.Delete(department);
                var count = await _unitOfWork.Complete();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                return View(department);
        }

    }
}
