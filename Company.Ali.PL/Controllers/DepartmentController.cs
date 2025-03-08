using Company.Ali.BLL.Interfaces;
using Company.Ali.BLL.Repositories;
using Company.Ali.DAL.Models;
using Microsoft.AspNetCore.Mvc;

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
    }
}
