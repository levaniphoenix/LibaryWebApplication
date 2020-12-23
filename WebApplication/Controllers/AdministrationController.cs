using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using WebApplication.Models.ViewModels;

namespace WebApplication.Controllers
{
    [Authorize]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;
        public AdministrationController(RoleManager<IdentityRole> roleManager,UserManager<IdentityUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateNewRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };

                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }
        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }
        public IActionResult Users()
        {
            var users = userManager.Users;
            return View(users);
        }
        //[Authorize(Roles ="admin")]
        public async Task<IActionResult> EditUser(string id)
        {
            IdentityUser user = await userManager.FindByIdAsync(id);
            if(user!=null)
            {
                EditUserRoleModel model = new EditUserRoleModel();
                model.userId = user.Id;
                model.roles= roleManager.Roles.Select(x => x.Name).ToList();
                model.userRoles =(List<string>) await userManager.GetRolesAsync(user);
                model.userName = user.Email;
                return View(model);
            }
            else
            {
                return RedirectToAction("index");
            }
        }  
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserRoleModel model)
        {
            if(ModelState.IsValid)
            {
                IdentityUser user = await userManager.FindByIdAsync(model.userId);
                if(user!=null)
                {
                    //user.Id = model.userId;
                    //user.Email = model.userName;
                    //IdentityResult result=await 
                    var roles = await userManager.GetRolesAsync(user);
                    foreach(var role in roles)
                    {
                        if(model.userRoles==null || (model.userRoles!=null && !model.userRoles.Contains(role)))
                        {
                            await userManager.RemoveFromRoleAsync(user, role);
                        }
                    }
                    if(model.userRoles!=null)
                    {
                        foreach(var role in model.userRoles)
                        {
                            if(!roles.Contains(role))
                            {
                                await userManager.AddToRoleAsync(user, role);
                            }
                        }
                    }
                    

                }
            }
            else
            {
                ModelState.AddModelError("", "could not find user");
            }
            return RedirectToAction("Users");
        }
    }
}
