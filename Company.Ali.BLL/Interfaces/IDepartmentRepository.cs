using Company.Ali.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Ali.BLL.Interfaces
{
    interface IDepartmentRepository
    {
        IEnumerable<Department> GetAll();
        Department? Get(int id);
        int Add(Department model);
        int Update(Department model);
        int Delete(Department model);
    }
}
