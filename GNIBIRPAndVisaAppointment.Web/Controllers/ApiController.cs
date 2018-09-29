using System;
using System.Linq;
using GNIBIRPAndVisaAppointment.Web.Business;
using GNIBIRPAndVisaAppointment.Web.Business.Api;
using GNIBIRPAndVisaAppointment.Web.Business.Application;
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
                    .GetAssignments(AssignmentStatus.Accepted)
                    .Select(assignment => new AssignmentModel(assignment));
                
                return Json(assignments);
            }
            
            return Json(null);
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

            return Json(null);
        }
    }
}