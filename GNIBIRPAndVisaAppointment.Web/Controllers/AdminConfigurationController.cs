using GNIBIRPAndVisaAppointment.Web.Business;
using GNIBIRPAndVisaAppointment.Web.Business.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GNIBIRPAndVisaAppointment.Web.Controllers
{
    [Route("Admin/Configuration")]
    [Authorize(Roles="Admin")]
    public class AdminConfigurationController : Controller
    {
        readonly IDomainHub DomainHub;

        public AdminConfigurationController(IDomainHub domainHub)
        {
            DomainHub = domainHub;
        }

        public IActionResult Index()
        {
            var configurationManager = DomainHub.GetDomain<IConfigurationManager>();
            ViewBag.Configurations = configurationManager.GetAll();

            return View();
        }

        [Route("Add")]
        public IActionResult Add(string area, string key, string value)
        {
            if (!string.IsNullOrEmpty(area) && !string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(key))
            {
                var configurationManager = DomainHub.GetDomain<IConfigurationManager>();
                configurationManager[area, key] = value;

                return RedirectToAction("Index");
            }

            ViewBag.Area = area;
            ViewBag.Key = key;
            ViewBag.Value = value;

            return View();
        }

        [Route("Set/{area}/{key}")]
        public IActionResult Set(string area, string key, string value)
        {
            var configurationManager = DomainHub.GetDomain<IConfigurationManager>();
            
            if (!string.IsNullOrEmpty(value))
            {
                configurationManager[area, key] = value;

                return RedirectToAction("Index");
            }

            ViewBag.Area = area;
            ViewBag.Key = key;
            ViewBag.Value = configurationManager[area, key];

            return View("~/Views/AdminConfiguration/Set.cshtml");
        }

        [Route("Remove/{area}/{key}")]
        public IActionResult Remove(string area, string key, bool remove = false)
        {
            var configurationManager = DomainHub.GetDomain<IConfigurationManager>();
            
            if (remove)
            {
                configurationManager.Remove(area, key);

                return RedirectToAction("Configuration");
            }

            ViewBag.Area = area;
            ViewBag.Key = key;
            ViewBag.Value = configurationManager[area, key];

            return View();
        }
    }
}