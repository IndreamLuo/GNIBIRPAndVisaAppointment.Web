using System;
using System.Linq;
using GNIBIRPAndVisaAppointment.Web.Business;
using GNIBIRPAndVisaAppointment.Web.Business.Api;
using GNIBIRPAndVisaAppointment.Web.Business.Application;
using GNIBIRPAndVisaAppointment.Web.Business.Configuration;
using GNIBIRPAndVisaAppointment.Web.Models.Api;
using Microsoft.AspNetCore.Mvc;

namespace GNIBIRPAndVisaAppointment.Web.Controllers
{
    [Route("Api")]
    public class ApiController : Controller
    {
        readonly IDomainHub DomainHub;
        public ApiController(IDomainHub domainHub)
        {
            DomainHub = domainHub;
        }

        [HttpPost]
        [Route("Assignment/Accepted")]
        public IActionResult AssignmentAccepted(string token)
        {
            var apiManager = DomainHub.GetDomain<IApiManager>();
            if (apiManager.VerifyToken(token))
            {
                var applicationManager = DomainHub.GetDomain<IApplicationManager>();
                var assignments = applicationManager
                    .GetAssignments(AssignmentStatus.Accepted, true)
                    .Select(assignment => new AssignmentModel(assignment));
                
                return Json(assignments);
            }
            
            return Accepted();
        }

        [HttpPost]
        [Route("Assignment/Appoint")]
        public IActionResult AssignmentAppoint(string token, string id)
        {
            var apiManager = DomainHub.GetDomain<IApiManager>();
            if (apiManager.VerifyToken(token))
            {
                var applicationManager = DomainHub.GetDomain<IApplicationManager>();
                applicationManager.Appoint(id);
            }

            return Accepted();
        }

        [HttpPost]
        [Route("Assignment/Duplicate")]
        public IActionResult AssignmentDuplicate(string token, string id)
        {
            var apiManager = DomainHub.GetDomain<IApiManager>();
            if (apiManager.VerifyToken(token))
            {
                var applicationManager = DomainHub.GetDomain<IApplicationManager>();
                applicationManager.Duplicate(id);
            }

            return Accepted();
        }

        [HttpPost]
        [Route("Assignment/Unverify")]
        public IActionResult AssignmentUnverify(string token, string id)
        {
            var apiManager = DomainHub.GetDomain<IApiManager>();
            if (apiManager.VerifyToken(token))
            {
                var applicationManager = DomainHub.GetDomain<IApplicationManager>();
                applicationManager.Duplicate(id);
            }

            return Accepted();
        }

        [HttpPost]
        [Route("Assignment/Appoint/Log")]
        public IActionResult AssignmentAppointLog(string token, string id, string slot, bool success, string result, double timeSpan)
        {
            var apiManager = DomainHub.GetDomain<IApiManager>();
            if (apiManager.VerifyToken(token))
            {
                var applicationManager = DomainHub.GetDomain<IApplicationManager>();
                applicationManager.AppointLog(id, slot, success, result, timeSpan);
            }

            return Accepted();
        }

        [HttpPost]
        [Route("Configuration/Get")]
        public IActionResult ConfigurationGet(string token, string area, string key)
        {
            var apiManager = DomainHub.GetDomain<IApiManager>();
            if (apiManager.VerifyToken(token))
            {
                var configurationManager = DomainHub.GetDomain<IConfigurationManager>();
                var result = configurationManager[area, key];

                return Content(result);
            }

            return Accepted();
        }

        [HttpPost]
        [Route("Configuration/Set")]
        public IActionResult ConfigurationSet(string token, string area, string key, string value)
        {
            var apiManager = DomainHub.GetDomain<IApiManager>();
            if (apiManager.VerifyToken(token))
            {
                var configurationManager = DomainHub.GetDomain<IConfigurationManager>();
                var result = configurationManager[area, key] = value;

                return Accepted();
            }

            return BadRequest();
        }
    }
}