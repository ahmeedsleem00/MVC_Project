using Microsoft.AspNetCore.Mvc;
using Demo.BLL.Repository;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Authorization;


namespace Demo.PL.Controllers
{
	[Authorize]


	public class DepartmentController : Controller
    {
        //Department/Index

        //private IDepartmentRepository _departmentRepository;
       
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(IUnitOfWork unitOfWork/*DepartmentRepository departmentRepository*/)
        {
           // _departmentRepository = departmentRepository;   //Ask CLR  Create Object from DepartmentRepository  
           
            _unitOfWork = unitOfWork;
        }


        //Get All
        //Department/Index
        public IActionResult Index()
        {
            var departments = _unitOfWork.DepartmentRepository.GetAll();
            return View(departments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Department model)
        {
           if(ModelState.IsValid) //Server Side Validation
            {
                 _unitOfWork.DepartmentRepository.Add(model);
               
                var count = _unitOfWork.Complete();
              
                if (count > 0)
                {
                    return RedirectToAction("Index");
                }
            }
           
            return View(model);
        }

        public IActionResult Details(int?id,string ViewName= "Details")
        {
            if (id is null)
             
                return BadRequest(); //400 

            var department = _unitOfWork.DepartmentRepository.Get(id.Value);

            if (department is null)
              
                return NotFound(); //404

            return View(ViewName,department);
        }


        [HttpGet]
        public IActionResult Edit(int? id)
        {
            return Details(id, "Edit");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute]int? id,Department model)
        {
            if (id != model.Id)
                return BadRequest();  //400

            if (ModelState.IsValid)   //Server Side Validation
            {
                _unitOfWork.DepartmentRepository.Update(model);
               
                var count = _unitOfWork.Complete();

                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
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
        public IActionResult Delete([FromRoute]int?id ,Department model)
        {
            if (id != model.Id)
                return BadRequest();    //400

            if (ModelState.IsValid)    //Server Side Validation
            {
                 _unitOfWork.DepartmentRepository.Delete(model);
               
                var count = _unitOfWork.Complete();

                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }
    }
}
