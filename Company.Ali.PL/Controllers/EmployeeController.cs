using AutoMapper;
using Company.Ali.BLL.Interfaces;
using Company.Ali.DAL.Models;
using Company.Ali.PL.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Company.Ali.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepositroy;
        IMapper _mapper;

        public EmployeeController(
            IEmployeeRepository employeeRepository ,
            IDepartmentRepository departmentRepository,
            IMapper mapper
            )
        {
            _employeeRepository = employeeRepository;
            _departmentRepositroy = departmentRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                employees = _employeeRepository.GetAll();
            }
            else
            {
                employees = _employeeRepository.GetByName(SearchInput);
            }

                // Dictionary 
                // 1.ViewDate : Transfer Extra Information From Controller (Action) To View 
                ViewData["Message"] = "Hello From ViewDate";
            // 2.ViewBag : Transfer Extra Information From Controller (Action) To View 
            ViewBag.Name = "Ali Ahmed";
            
            return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var departments = _departmentRepositroy.GetAll();
            ViewData["departments"] = departments;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateEmployeeDto model)
        {
            var departments = _departmentRepositroy.GetAll();
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
                var employee = _mapper.Map<Employee>(model);
                var count = _employeeRepository.Add(employee);
                if (count > 0)
                {
                    TempData["Message"] = "Employee is Created!";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }



        [HttpGet]
        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");
            var employee = _employeeRepository.Get(id.Value);
            if (employee is null) return NotFound(new { statusCode = 404, message = $"Employee With Id: {id} is not found" });
            return View(viewName, employee);
        }


        [HttpGet]
        public IActionResult Edit(int? id)
        {
            //return Details(id, "Edit");
            var departments = _departmentRepositroy.GetAll();
            ViewData["departments"] = departments;
            if (id is null) return BadRequest("Invalid Id");
            var employee = _employeeRepository.Get(id.Value);
            if (employee is null) return NotFound(new { statusCode = 404, message = $"Employee With Id: {id} is not found" });
            //var model = new CreateEmployeeDto()
            //{
            //    Name = employee.Name,
            //    Age = employee.Age,
            //    Address = employee.Address,
            //    Phone = employee.Phone,
            //    Salary = employee.Salary,
            //    CreateAt = employee.CreateAt,
            //    Email = employee.Email,
            //    HiringDate = employee.HiringDate,
            //    IsActive = employee.IsActive,
            //    IsDeleted = employee.IsDeleted,
            //    DepartmentId = employee.DepartmentId
            //};

            // Auto Mapper 
            var model = _mapper.Map<CreateEmployeeDto>(employee);

            return View( model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, CreateEmployeeDto model)
        {
            var departments = _departmentRepositroy.GetAll();
            ViewData["departments"] = departments;
            if (ModelState.IsValid)
            {
                //if (id != model.Id) return BadRequest(); //

                var employee = new Employee()
                {
                    Id = id,
                    Name = model.Name,
                    Age = model.Age,
                    Address = model.Address,
                    Phone = model.Phone,
                    Salary = model.Salary,
                    CreateAt = model.CreateAt,
                    Email = model.Email,
                    HiringDate = model.HiringDate,
                    IsActive = model.IsActive,
                    IsDeleted = model.IsDeleted,
                    DepartmentId = model.DepartmentId
                };
                var count = _employeeRepository.Update(employee);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }


        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, Employee employee)
        {
            if (ModelState.IsValid)
            {
                if (ModelState.IsValid)
                {
                    if (id != employee.Id) return BadRequest("The Id Is not Valid");
                    var count = _employeeRepository.Delete(employee);
                    if (count > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            return View(employee);
        }





    }
}
