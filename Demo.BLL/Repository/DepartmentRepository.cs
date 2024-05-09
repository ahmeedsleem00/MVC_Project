using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.DAL.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata.Ecma335;

namespace Demo.BLL.Repository
{
    public class DepartmentRepository :  GenericRepository<Department> , IDepartmentRepository
    {
        public DepartmentRepository( AppDbContext  context) : base(context) // ASK CLR To Create Object From AppDbContext
        {
        }
      
    }
}
