using Demo.DAL.Data.Configurations;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Data
{

    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {

        public AppDbContext(    DbContextOptions    < AppDbContext >  options ) :   base (   options )
        {
            
        }       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.ApplyConfiguration(new DepartmentCofiguration());

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


            base.OnModelCreating(modelBuilder); 
        }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server = . ;Database = AppMVCDB04 ; Trusted_Connection = True ");
        //}



        public DbSet<Department> Department { get; set; } 
       
        public DbSet<Employee> Employees { get; set; }

        //public DbSet<IdentityUser> Users { get;}
        //public DbSet<IdentityRole> Roles { get; set; }

    }
}
