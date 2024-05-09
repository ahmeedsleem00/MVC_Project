using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repository;
using Demo.DAL.Models;
using Demo.PL.Helper;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.Linq;

namespace Demo.PL.Controllers
{

	[Authorize]

	public class EmployeeController : Controller
    {

        //Employee/Index

        //private IEmployeeRepository _employeeRepository;
        //private readonly IDepartmentRepository _departmentRepository;

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;


        public EmployeeController (IUnitOfWork unitOfWork/*IEmployeeRepository employeeRepository*/,IMapper mapper/*,IDepartmentRepository departmentRepository*/)
        {
            //  _employeeRepository = employeeRepository;   //Ask CLR  Create Object from DepartmentRepository  
            // _departmentRepository = departmentRepository;


            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        //Get All
        //Department/Index
        public IActionResult Index(string SearchInput)
        {
            var employees = Enumerable.Empty<Employee>();

            //1.Add
            //2.Update
            //3.Delete


            if (string.IsNullOrEmpty(SearchInput))
            {
                //employees = _employeeRepository.GetAll();
               
                employees = _unitOfWork.EmployeeRepository.GetAll();


            }
            else
            {
                //employees = _employeeRepository.GetByName(SearchInput.ToLower());
              
                employees = _unitOfWork.EmployeeRepository.GetByName(SearchInput.ToLower());

            }

            var result= _mapper.Map<IEnumerable<EmployeeViewModel>>(employees);

            //View 's Dictionary : Transfer Extra Information from action to view (one way)(key,value)

            // 1 - ViewData : Property inherted from controller class,Dictionary
           
            //ViewData["Message"] = "Hello ViewData";

            // 2 - ViewBag :  Property inherted from controller class,Dynamic
           
            ViewBag.Message = "Hello ViewBag";

            // 3 - ViewTemp : Property inherted from controller class,Like ViewData
            
            //Transfer information from request to another 



            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
           // ViewData["Departments"] = _unitOfWork.DepartmentRepository.GetAll();   //All DEpartments

            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeViewModel model)
        {
            if (ModelState.IsValid)//Server Side Validation
            {

                model.ImageName = DocumentSettings.UploadFile(model.Image, "images");


                var employee = _mapper.Map<Employee>(model);


                //Employee employee = new Employee()
                //{
                //    Id=model.Id,
                //    Name=model.Name,
                //    HireDate=model.HireDate,
                //    Salary=model.Salary,
                //    Address=model.Address,
                //    PhoneNumber=model.Phone,
                //    DateOfCreation=model.DateOfCreation,
                //    DepartmentId=model.DepartmentId,
                //    Department=model.Department,
                //    IsDeleted=model.IsDeleted
                //};


                //var count = _employeeRepository.Add(employee);
               
                _unitOfWork.EmployeeRepository.Add(employee);
               
                var count = _unitOfWork.Complete();
               
                if (count > 0)
                {
                    TempData["Message"] = "Employee Added!!";

                }
                else
                {
                    TempData["Message"] = "Employee Not Added!!";

                }
                return RedirectToAction("Index");

            }
            return View(model);
        }

        public IActionResult Details(int? id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest(); //400 

            //var employee = _employeeRepository.Get(id.Value);
          
            var employee = _unitOfWork.EmployeeRepository.Get(id.Value);


            //EmployeeViewModel employeeViewModel = new EmployeeViewModel()
            //{
            //    Id = employee.Id,
            //    Name = employee.Name,
            //    HireDate = employee.HireDate,
            //    Salary = employee.Salary,
            //    Address = employee.Address,
            //    PhoneNumber = employee.Phone,
            //    DateOfCreation = employee.DateOfCreation,
            //    DepartmentId = employee.DepartmentId,
            //    Department = employee.Department,
            //    IsDeleted = employee.IsDeleted
            //};

            var employeeViewModel = _mapper.Map<EmployeeViewModel>(employee);

            if (employee is null)
              
                return NotFound(); //404

            return View(ViewName, employeeViewModel);
        }


        [HttpGet]
        public IActionResult Edit(int? id)
        {
            //ViewData["Departments"] = _departmentRepository.GetAll();   //All DEpartments

           //  ViewData["Departments"] = _unitOfWork.DepartmentRepository.GetAll();

            return Details(id, "Edit");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int? id, EmployeeViewModel model)
        {
            if (id != model.Id)

                return BadRequest();//400


            if (model.ImageName is not null)
            {
                DocumentSettings.DeleteFile(model.ImageName, "images");
            }

              model.ImageName = DocumentSettings.UploadFile(model.Image,"images");
            



            var employee = _mapper.Map<Employee>(model);
           
            
                //var count = _employeeRepository.Update(employee);
               
                _unitOfWork.EmployeeRepository.Update(employee);
              
                int count = _unitOfWork.Complete();

                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
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
        public IActionResult Delete([FromRoute] int? id, EmployeeViewModel model)
        {
            if (id != model.Id)
                return BadRequest();//400


            
            var employee = _mapper.Map<Employee>(model);

             _unitOfWork.EmployeeRepository.Delete(employee);
           
            
             int count = _unitOfWork.Complete();

            
                if (count > 0)
                {
                    DocumentSettings.DeleteFile(model.ImageName,"images");
                   
                    return RedirectToAction(nameof(Index));
                }
            

            return View(model);
        }
    }
}
