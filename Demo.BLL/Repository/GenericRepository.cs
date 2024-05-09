using Demo.BLL.Interfaces;
using Demo.DAL.Data;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity 
    {

        private protected AppDbContext _context; //NULL


        public GenericRepository(AppDbContext context) // Ask CLR to create object from AppDbcontext
        {
            // _context = new AppDbContext();
            _context = context;
        }



        public void Add(T entity)
        {
             // _context.Set<T>().Add(entity);

            _context.Add(entity);   

           // return _context.SaveChanges();
        }

        public void Delete(T entity)
        {

           //  _context.Set<T>().Remove(entity);

            _context.Remove(entity);
           
        }
        public void Update(T entity)
        {
            _context.Update(entity);

        }
        public T Get(int id)
        {
            var entity = _context.Set<T>().Find(id);

            return entity;
        }

        public IEnumerable<T> GetAll()
        {
            //var result = _context.Set<T>().ToList();

            //return result;


            if (typeof(T) == typeof(Employee) )
            {
                return (IEnumerable<T>) _context.Employees.Include(E=>E.Department).ToList();   
            }
            else
            {
                return _context.Set<T>().ToList();
            }

        }

       
    }
}
