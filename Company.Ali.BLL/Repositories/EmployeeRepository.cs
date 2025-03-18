using Company.Ali.BLL.Interfaces;
using Company.Ali.DAL.Data.Contexts;
using Company.Ali.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Company.Ali.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly CompanyDbContext _context;
        public EmployeeRepository(CompanyDbContext context) : base(context)
        {
            _context = context;
        }

        // Search Logic 
        public List<Employee>? GetByName(string name)
        {
           return _context.Employees.Include(E => E.Department).Where(E => E.Name.ToLower().Contains(name.ToLower())).ToList();
        }
    }
}

