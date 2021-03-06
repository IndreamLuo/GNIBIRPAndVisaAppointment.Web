using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GNIBIRPAndVisaAppointment.Web.Business;
using GNIBIRPAndVisaAppointment.Web.Business.Api;
using GNIBIRPAndVisaAppointment.Web.Business.Application;
using GNIBIRPAndVisaAppointment.Web.Business.AppointmnetLetter;
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
        [Route("Assignment/{status}")]
        public IActionResult AssignmentInStatus(string token, string status)
        {
            switch (status)
            {
                case AssignmentStatus.Pending:
                case AssignmentStatus.Accepted:
                case AssignmentStatus.Appointed:
                case AssignmentStatus.Duplicated:
                    var apiManager = DomainHub.GetDomain<IApiManager>();
                    if (apiManager.VerifyToken(token))
                    {
                        var applicationManager = DomainHub.GetDomain<IApplicationManager>();
                        var assignments = applicationManager
                            .GetAssignments(status, true)
                            .Select(assignment =>
                            {
                                var assignmentModel = new AssignmentModel(assignment);
                                assignmentModel.Order.AnyCategory = true;
                                return assignmentModel;
                            });
                        
                        return Json(assignments);
                    }
                    break;
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
            Task.Run(async () =>
            {
                await Task.Delay(60 * 1000);

                var apiManager = DomainHub.GetDomain<IApiManager>();
                if (apiManager.VerifyToken(token))
                {
                    var applicationManager = DomainHub.GetDomain<IApplicationManager>();
                    applicationManager.Duplicate(id);
                }
            });

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

        [HttpPost]
        [Route("AppointmentLetter/Submit")]
        public IActionResult AppointmentLetterSubmit(string token, string id, string message)
        {
            var apiManager = DomainHub.GetDomain<IApiManager>();

            if (apiManager.VerifyToken(token))
            {
                var regex = new Regex("Name: (?<firstName>.*)  (?<secondName>.*)(\r\n|\n\n|\n)Appointment Date: (?<day>[0-9]{2})/(?<month>[0-9]{2})/(?<year>[0-9]{4}), (?<hour>[0-9]{2}):(?<minute>[0-9]{2})(\r\n|\n\n|\n)Registration Appointment Reference: (?<appointmentNo>.*)(\r\n|\n\n|\n)Category: (?<category>[a-zA-Z]*) \\| (?<subCategory>[a-zA-Z]*)(\r\n|\n\n|\n)", RegexOptions.CultureInvariant);
                var match = regex.Match(message);
                var deserializedTime = new DateTime(
                    int.Parse(match.Groups["year"].Value),
                    int.Parse(match.Groups["month"].Value),
                    int.Parse(match.Groups["day"].Value),
                    int.Parse(match.Groups["hour"].Value),
                    int.Parse(match.Groups["minute"].Value),
                    0
                );
                
                var appointmentLetterManager = DomainHub.GetDomain<IAppointmentLetterManager>();
                appointmentLetterManager.SubmitLetter(id, match.Groups["appointmentNo"].Value, $"{match.Groups["firstName"].Value}  {match.Groups["secondName"].Value}", deserializedTime, match.Groups["category"].Value, match.Groups["subCategory"].Value);

                return Accepted();
            }

            return BadRequest();
        }
    }
}