using GNIBIRPAndVisaAppointment.Web.Business;
using Microsoft.AspNetCore.Mvc;

namespace GNIBIRPAndVisaAppointment.Web.Controllers
{
    public class InfoContentViewComponent : ViewComponent
    {
        IDomainHub DomainHub;

        public InfoContentViewComponent(IDomainHub domainHub)
        {
            DomainHub = domainHub;
        }

        public IViewComponentResult Invoke(string key, string language)
        {
            return View(InfoController.GetInformationModel(DomainHub, key, language));
        }
    }
}