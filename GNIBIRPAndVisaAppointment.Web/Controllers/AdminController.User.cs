using System.Security.Claims;
using System.Threading.Tasks;
using GNIBIRPAndVisaAppointment.Web.Business.User;
using GNIBIRPAndVisaAppointment.Web.Identity;
using GNIBIRPAndVisaAppointment.Web.Models.Admin;
using Microsoft.AspNetCore.Mvc;

namespace GNIBIRPAndVisaAppointment.Web.Controllers
{
    public partial class AdminController
    {
        [Route("User")]
        public IActionResult User()
        {
            var userManager = DomainHub.GetDomain<IUserManager>();
            ViewBag.Users = userManager.GetAllUsers();

            return View();
        }
        
        [Route("User/Create")]
        public async Task<IActionResult> UserCreate(UserModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.CreateAsync(new ApplicationUser
                {
                    Id = model.Id,
                    UserName = model.Name,
                    PasswordHash = model.Password,
                    Role = model.Role
                }, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("User");
                }
            }

            return View();
        }

        [Route("User/Edit/{id}")]
        public async Task<IActionResult> UserEdit(UserModel model, bool isOld = false)
        {
            if (isOld)
            {
                if (ModelState.IsValid)
                {
                    var applicationUser = new ApplicationUser
                    {
                        Id = model.Id,
                        UserName = model.Name,
                        Role = model.Role
                    };
                    await UserManager.UpdateAsync(applicationUser);

                    if (!string.IsNullOrEmpty(model.Password))
                    {
                        await UserManager.ChangePasswordAsync(applicationUser, model.OldPassword, model.Password);
                    }

                    return RedirectToAction("User");
                }
            }
            else
            {
                model = new UserModel(await UserManager.GetUserAsync(base.User));
            }

            return View(model);
        }
    }
}