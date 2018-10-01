using System.Security.Claims;
using System.Threading.Tasks;
using GNIBIRPAndVisaAppointment.Web.Business;
using GNIBIRPAndVisaAppointment.Web.Business.User;
using GNIBIRPAndVisaAppointment.Web.Identity;
using GNIBIRPAndVisaAppointment.Web.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GNIBIRPAndVisaAppointment.Web.Controllers
{
    [Route("Admin/User")]
    [Authorize(Roles="Admin")]
    public class AdminUserController : Controller
    {
        readonly IDomainHub DomainHub;
        readonly UserManager<ApplicationUser> UserManager;

        public AdminUserController(IDomainHub domainHub, UserManager<ApplicationUser> userManager)
        {
            DomainHub = domainHub;
            UserManager = userManager;
        }
        
        public IActionResult Index()
        {
            var userManager = DomainHub.GetDomain<IUserManager>();
            ViewBag.Users = userManager.GetAllUsers();

            return View();
        }
        
        [Route("Create")]
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
                    return RedirectToAction("Index");
                }
            }

            return View();
        }

        [Route("Edit/{id}")]
        public async Task<IActionResult> UserEdit(UserModel model, bool isOld = false)
        {
            var userEntity = await UserManager.FindByIdAsync(model.Id);

            if (isOld)
            {
                if (ModelState.IsValid)
                {
                    userEntity.UserName = model.Name;
                    userEntity.Role = model.Role;

                    if (!string.IsNullOrEmpty(model.Password))
                    {
                        userEntity.PasswordHash = UserManager.PasswordHasher.HashPassword(userEntity, model.Password);
                    }

                    await UserManager.UpdateAsync(userEntity);

                    return RedirectToAction("Index");
                }
            }
            else
            {
                model = new UserModel(userEntity);
            }

            return View(model);
        }
    }
}