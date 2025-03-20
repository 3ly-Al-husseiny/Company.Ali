using AutoMapper;
using Company.Ali.BLL.Interfaces;
using Company.Ali.DAL.Models;
using Company.Ali.PL.DTOs;
using Company.Ali.PL.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Company.Ali.PL.Controllers
{
    public class EmployeeController : Controller
    {
        //private readonly IEmployeeRepository _employeeRepository;
        //private readonly IDepartmentRepository _departmentRepositroy;
        private readonly IUnitOfWork _unitOfWork;
        IMapper _mapper;

        public EmployeeController(
            //IEmployeeRepository employeeRepository ,
            //IDepartmentRepository departmentRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            //_employeeRepository = employeeRepository;
            //_departmentRepositroy = departmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
            }
            else
            {
                employees = await _unitOfWork.EmployeeRepository.GetByNameAsync(SearchInput);
            }

                // Dictionary 
                // 1.ViewDate : Transfer Extra Information From Controller (Action) To View 
                ViewData["Message"] = "Hello From ViewDate";
            // 2.ViewBag : Transfer Extra Information From Controller (Action) To View 
            ViewBag.Name = "Ali Ahmed";
            
            return View(employees);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["departments"] = departments;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEmployeeDto model)
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["departments"] = departments;
            if (ModelState.IsValid)
            {
                //var employee = new Employee()
                //{
                //    Name = model.Name,
                //    Address = model.Address,
                //    Age = model.Age,
                //    CreateAt = model.CreateAt,
                //    HiringDate = model.HiringDate,
                //    Email = model.Email,
                //    IsActive = model.IsActive,
                //    IsDeleted = model.IsDeleted,
                //    Phone = model.Phone,
                //    Salary = model.Salary,
                //    DepartmentId = model.DepartmentId
                //};

                if (model.Image is not null)
                {
                   model.ImageName = DocumentSettings.UploadFile(model.Image, "images");
                }

                var employee = _mapper.Map<Employee>(model);
                _unitOfWork.EmployeeRepository.Update(employee);
                var count = await _unitOfWork.Complete();
                if (count > 0)
                {
                    TempData["Message"] = "Employee is Created!";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");
            var employee = await _unitOfWork.EmployeeRepository.GetAsync(id.Value);
            if (employee is null) return NotFound(new { statusCode = 404, message = $"Employee With Id: {id} is not found" });
            return View(viewName, employee);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            //return Details(id, "Edit");
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["departments"] = departments;

            if (id is null) return BadRequest("Invalid Id");
            var employee = await _unitOfWork.EmployeeRepository.GetAsync(id.Value);
            if (employee is null) return NotFound(new { statusCode = 404, message = $"Employee With Id: {id} is not found" });
            var model = new CreateEmployeeDto()
            {
                Name = employee.Name,
                Age = employee.Age,
                Address = employee.Address,
                Phone = employee.Phone,
                Salary = employee.Salary,
                CreateAt = employee.CreateAt,
                Email = employee.Email,
                HiringDate = employee.HiringDate,
                IsActive = employee.IsActive,
                IsDeleted = employee.IsDeleted,
                DepartmentId = employee.DepartmentId,
                ImageName = employee.ImageName
                
            };

            // Auto Mapper 
            //var model = _mapper.Map<CreateEmployeeDto>(employee);

            return View( model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, CreateEmployeeDto model)
        {
            var departments = _unitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["departments"] = departments;

            if (ModelState.IsValid)
            {
                //if (id != model.Id) return BadRequest(); //

                //var employee = new Employee()
                //{
                //    Id = id,
                //    Name = model.Name,
                //    Age = model.Age,
                //    Address = model.Address,
                //    Phone = model.Phone,
                //    Salary = model.Salary,
                //    CreateAt = model.CreateAt,
                //    Email = model.Email,
                //    HiringDate = model.HiringDate,
                //    IsActive = model.IsActive,
                //    IsDeleted = model.IsDeleted,
                //    DepartmentId = model.DepartmentId
                //};

                if (model.ImageName is not null && model.Image is not null)
                {
                    DocumentSettings.DeleteFile(model.ImageName, "images");
                }

                if (model.Image is not null)
                {
                   model.ImageName = DocumentSettings.UploadFile(model.Image, "images");
                }


                var employee = _mapper.Map<Employee>(model);
                employee.Id = id;
                _unitOfWork.EmployeeRepository.Update(employee);
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
            return Details(id, "Delete");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, Employee employee)
        {
            _unitOfWork.EmployeeRepository.Delete(employee);
            var count = await _unitOfWork.Complete();
            if (count > 0)
            {
                if (employee.ImageName is not null)
                {
                    DocumentSettings.DeleteFile(employee.ImageName, "images");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }





    }
}
