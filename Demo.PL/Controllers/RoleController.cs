using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly  RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        //Get , GetAll, Add, Update, Delete
        //Index, Details, Create, Edit, Delete


        //public RoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser>  userManager)
        //{
        //    _roleManager = roleManager;
        //    _userManager = userManager;
        //}

        public async Task<IActionResult> Index(string SearchInput)
        {
            var roles = Enumerable.Empty<RoleViewModel>();

            if (string.IsNullOrEmpty(SearchInput))
            {
                roles = await _roleManager.Roles.Select(R => new RoleViewModel()
                {
                    Id = R.Id,
                    RoleName = R.Name
                 
                }).ToListAsync();

            }
            else
            {
                roles = await _roleManager.Roles.Where(U => U.Name
                                  .ToLower()
                                  .Contains(SearchInput.ToLower()))
                                  .Select(R => new RoleViewModel()
                                  {
                                      Id = R.Id,
                                      RoleName = R.Name
                                  }).ToListAsync();
            }

            return View(roles);

        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole()
                {
                    Name = model.RoleName
                };

                 await _roleManager.CreateAsync(role);
                return RedirectToAction(nameof(Index));
            }


            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Details(string? id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest(); //400 


            var roleFromDb = await _roleManager.FindByIdAsync(id);
            if (roleFromDb is null)
               return NotFound(); //404

            var user = new RoleViewModel()
            {
                Id = roleFromDb.Id,
                RoleName = roleFromDb.Name
            };


            return View(ViewName, user);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {

            return await Details(id, ViewName: "Edit");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleViewModel model)
        {
            if (id != model.Id)

                return BadRequest(); //400
            if (ModelState.IsValid)
            {

              
            var roleFromDb = await _roleManager.FindByIdAsync(id);
            if (roleFromDb is null)
               return NotFound(); //404


                roleFromDb.Name = model.RoleName;
              

                await _roleManager.UpdateAsync(roleFromDb);
                return RedirectToAction(nameof(Index));


            }
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            return await Details(id, ViewName: "Delete");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete([FromRoute] string id, UserViewModel model)
        {

            if (id != model.Id)

                return BadRequest(); //400
            if (ModelState.IsValid)
            {


                var roleFromDb = await _roleManager.FindByIdAsync(id);
                if (roleFromDb is null)
                    return NotFound(); //404

                await _roleManager.DeleteAsync(roleFromDb);
                return RedirectToAction(nameof(Index));


            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId)
        {
        var role =  await  _roleManager.FindByIdAsync(roleId);
            if(role is null) return NotFound();

            var usersInRole = new List<UsersInRoleViewModel>();
            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var userInRole = new UsersInRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName
                }; 
                if(await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userInRole.IsSelected = true;
                }
                else
                {
                    userInRole.IsSelected = false;
                }
                usersInRole.Add(userInRole);


            }
                return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId , List<UsersInRoleViewModel> users)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null) return NotFound();

            ViewData[index:"RoleId"] = roleId;

            if(ModelState.IsValid)
            {
                foreach (var user in users)
                {
                    var appUser = await _userManager.FindByIdAsync(user.UserId);
                    if (appUser is not null)
                    {
                        if (user.IsSelected &&! await _userManager.IsInRoleAsync(appUser, role.Name))
                        {

                      await  _userManager.AddToRoleAsync(appUser, role.Name);
                        }
                    }else if (user.IsSelected && await _userManager.IsInRoleAsync(appUser, role.Name))
                    {
                       await _userManager.RemoveFromRoleAsync(appUser, role.Name);
                    }
                }

                return RedirectToAction(nameof(Edit),new {id = roleId});
            }
            return View(users);
        }


    }
}
