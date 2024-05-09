using Demo.BLL.Interfaces;
using Demo.BLL.Repository;
using Demo.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        private Lazy<IDepartmentRepository> departmentRepository; //NULL
        
        private Lazy<IEmployeeRepository> employeeRepository;     //NULL
       
        
        public UnitOfWork(AppDbContext context)   //Ask CLR create object from AppDbContext
        {
            _context = context;
           
            departmentRepository = new Lazy<IDepartmentRepository>(new DepartmentRepository(_context));
            
            employeeRepository = new Lazy<IEmployeeRepository>(new EmployeeRepository(_context));
        }


        public IDepartmentRepository DepartmentRepository => departmentRepository.Value;
        public IEmployeeRepository EmployeeRepository { get { return employeeRepository.Value; } }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
