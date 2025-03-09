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

        public IActionResult Details(int Id)
        {
            var department = _departmentRepository.Get(Id);
            var model = new CreateDepartmentDto()
            {
                Code = department.Code,
                Name = department.Name,
                CreateAt = department.CreateAt
            };
            return View(model);
        }

        public IActionResult Delete(int Id)
        {
            _departmentRepository.Delete(_departmentRepository.Get(Id));
            return RedirectToAction(nameof(Index));
        }

        //public IActionResult Update(int Id)
        //{
        //    var department = _departmentRepository.Get(Id);
        //    var model = new CreateDepartmentDto()
        //    {
        //        Code = department.Code,
        //        Name = department.Name,
        //        CreateAt = department.CreateAt
        //    };
        //    return View(model);
        //}

        //public IActionResult Update2()
        //{
        //    _departmentRepository.Update()
        //}

        //[HttpPost]
        //public IActionResult Edit(int Id)
        //{
        //    if (ModelState.IsValid) // Server Side Validation
        //    {

        //        var department = _departmentRepository.Get(Id);
        //        if (department != null)
        //        {
        //            // mapping
        //            department.Code = model.Code;
        //            department.Name = model.Name;
        //            department.CreateAt = model.CreateAt;

        //            var count = _departmentRepository.Update(department);
        //            if (count > 0)
        //            {
        //                return RedirectToAction(nameof(Index));
        //            }
        //        }
        //    }
        //    return View(model);
        //}

        public IActionResult Edit(int id) {

            var department = _departmentRepository.Get(id);
            var model = new CreateDepartmentDto()
            {
                Code = department.Code,
                Name = department.Name,
                CreateAt = department.CreateAt
            };
            return View(model);
        }



        [HttpPost]
        public IActionResult Edit2(int id , CreateDepartmentDto model)
        {
            
            var department = _departmentRepository.Get(id);
            
                // mapping
                department.Code = model.Code;
                department.Name = model.Name;
                department.CreateAt = model.CreateAt;

                var count = _departmentRepository.Update(department);
                return RedirectToAction(nameof(Index));
            
                    
                
            
         
        }

    }
}
