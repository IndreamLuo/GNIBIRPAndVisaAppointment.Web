using GNIBIRPAndVisaAppointment.Web.Business.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace GNIBIRPAndVisaAppointment.Web.Controllers
{
    public partial class AdminController
    {
        [Route("Configuration")]
        public IActionResult Configuration()
        {
            
            var configurationManager = DomainHub.GetDomain<IConfigurationManager>();
            ViewBag.Configurations = configurationManager.GetAll();

            return View();
        }

        [Route("Configuration/Add")]
        public IActionResult ConfigurationAdd(string area, string key, string value)
        {
            if (!string.IsNullOrEmpty(area) && !string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(key))
            {
                var configurationManager = DomainHub.GetDomain<IConfigurationManager>();
                configurationManager[area, key] = value;

                return RedirectToAction("Configuration");
            }

            ViewBag.Area = area;
            ViewBag.Key = key;
            ViewBag.Value = value;

            return View();
        }

        [Route("Configuration/Set/{area}/{key}")]
        public IActionResult ConfigurationSet(string area, string key, string value)
        {
            var configurationManager = DomainHub.GetDomain<IConfigurationManager>();
            
            if (!string.IsNullOrEmpty(value))
            {
                configurationManager[area, key] = value;

                return RedirectToAction("Configuration");
            }

            ViewBag.Area = area;
            ViewBag.Key = key;
            ViewBag.Value = configurationManager[area, key];

            return View("~/Views/Admin/ConfigurationSet.cshtml");
        }

        [Route("Configuration/Remove/{area}/{key}")]
        public IActionResult ConfigurationRemove(string area, string key, bool remove = false)
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

            return View("~/Views/Admin/ConfigurationRemove.cshtml");
        }
    }
}