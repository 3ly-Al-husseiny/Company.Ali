using Company.Ali.BLL.Interfaces;
using Company.Ali.DAL.Data.Contexts;
using Company.Ali.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Ali.BLL.Repositories
{
     public class DepartmentRepository : IDepartmentRepository
        {
        private readonly CompanyDbContext _context; // Null 
        // Ask CLR Create Object From CompanyDbContext

        public DepartmentRepository(CompanyDbContext context) 
        {
            _context = context;
        }
        public int Add(Department model)
        {
            _context.Departments.Add(model);
            return _context.SaveChanges();
        }

        public int Delete(Department model)
        {
            _context.Departments.Remove(model);
            return _context.SaveChanges();
        }

        public Department? Get(int id)
        {
            return _context.Departments.Find(id);
        }

        public IEnumerable<Department> GetAll()
        {
            return _context.Departments.ToList();
        }

        public int Update(Department model)
        {
            _context.Departments.Update(model);
            return _context.SaveChanges();
        }
    }
}
