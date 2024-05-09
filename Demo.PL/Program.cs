using Demo.BLL;
using Demo.BLL.Interfaces;
using Demo.BLL.Repository;
using Demo.DAL.Data;
using Demo.DAL.Models;
using Demo.PL.Helper;
using Demo.PL.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL
{
    public class Program
    {
        //Entry Point
        public static void Main(string[] args)
        {
            var Builder = WebApplication.CreateBuilder(args);

            #region Configure Services That Allow Dependancy Injection
            Builder.Services.AddControllersWithViews(); // Register Built in MVC Services to the container
            Builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();     //  Allow DI For DepartmentRepository

            Builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();     //  Allow DI For EmployeeRepository


            Builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Builder.Configuration.GetConnectionString("DefaultConnection"));

            });



            Builder.Services.AddScoped<DepartmentRepository>();

            Builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();



            Builder.Services.AddScoped<IScopedService, ScopedService>(); //Per Request

            Builder.Services.AddTransient<ITransientService, TransientService>(); //Per Operation

            Builder.Services.AddSingleton<ISingeltonService, SingeltonService>(); //Per App


            Builder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));


            Builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();


            Builder.Services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Account/SignIn";
                config.AccessDeniedPath = "/Account/AccessDenied";
                //config.AccessDeniedPath = "/Home/Error";


            });
            #endregion

            var app = Builder.Build();

            #region Configure HTTP Request Pipline Or Middlewares

            if (app.Environment.IsDevelopment())
            {

                app.UseDeveloperExceptionPage();

            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            #endregion

            app.Run();


        }


    }
}
