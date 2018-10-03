using System.Security.Claims;
using System.Threading.Tasks;
using GNIBIRPAndVisaAppointment.Web.Business;
using GNIBIRPAndVisaAppointment.Web.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GNIBIRPAndVisaAppointment.Web.Controllers
{
    [Route("User")]
    public class UserController : Controller
    {
        IDomainHub DomainHub;
        SignInManager<ApplicationUser> SignInManager;
        public UserController(IDomainHub domainHub, SignInManager<ApplicationUser> signInManager)
        {
            DomainHub = domainHub;
            SignInManager = signInManager;
        }

        [Authorize]
        public IActionResult Index()
        {
            return Redirect("/Admin");
        }

        [Route("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string id, string password, string returnUrl = "/Admin")
        {
            if (SignInManager.IsSignedIn(User))
            {
                return RedirectToAction("Logout");
            }

            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(password))
            {
                var signInResult = await SignInManager.PasswordSignInAsync(id, password, false, true);
                if (signInResult.Succeeded)
                {
                    return Redirect(returnUrl);
                }
            }

            ViewBag.Id = id;

            return View();
        }

        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}