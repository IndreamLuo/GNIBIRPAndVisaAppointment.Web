using Microsoft.AspNetCore.Mvc;

namespace GNIBIRPAndVisaAppointment.Web.Controllers
{
    public class ApplicationAdminToolViewComponent : ViewComponent
    {
        public ApplicationAdminToolViewComponent()
        {

        }

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}