using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using GNIBIRPAndVisaAppointment.Web.Business;
using GNIBIRPAndVisaAppointment.Web.Business.Application;
using GNIBIRPAndVisaAppointment.Web.Business.Information;
using GNIBIRPAndVisaAppointment.Web.Business.Payment;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage;
using GNIBIRPAndVisaAppointment.Web.Models;
using GNIBIRPAndVisaAppointment.Web.Models.Admin;
using GNIBIRPAndVisaAppointment.Web.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GNIBIRPAndVisaAppointment.Web.Controllers
{
    [Route("Admin")]
    public class AdminController : Controller
    {
        IApplicationSettings ApplicationSettings;
        IDomainHub DomainHub;

        public AdminController(IApplicationSettings applicationSettings, IDomainHub domainHub)
        {
            ApplicationSettings = applicationSettings;
            DomainHub = domainHub;

            var adminAllowed = bool.Parse(applicationSettings["AdminAllowed"]);
            if (!adminAllowed)
            {
                throw new InvalidOperationException("AdminAllowed is set false is application settings.");
            }
        }
        
        public IActionResult Index()
        {
            return View();
        }

        [Route("BrowseImages")]
        public IActionResult BrowseImages()
        {
            throw new NotImplementedException();
        }

        [Route("Upload")]
        public IActionResult Upload(IFormFile upload)
        {
            var informationManager = DomainHub.GetDomain<IInformationManager>();
            var url = informationManager.UploadFile(upload.FileName, upload.ContentType, upload.OpenReadStream());

            return Ok(new
            {
                fileName = Path.GetFileNameWithoutExtension(url),
                uploaded = true,
                url = url
            });
        }

        [Route("Info")]
        public IActionResult Infos()
        {
            var informationManager = DomainHub.GetDomain<IInformationManager>();
            var informationList = informationManager.GetList();
            var models = informationList.Select(entity => new InformationModel(entity));

            return View(models);
        }

        [Route("Info/{operation}/{key}/{language}/")]
        public IActionResult Info(InformationModel model, string operation)
        {
            var informationManager = DomainHub.GetDomain<IInformationManager>();
            var information = informationManager[model.Key, model.Language];
            model = new InformationModel(information);
            ViewBag.operation = operation;

            return View(model);
        }

        [Route("Info/Add/")]
        public IActionResult AddInfo(InformationModel model)
        {
            if (model.Key != null && model.Language != null)
            {
                var informationManager = DomainHub.GetDomain<IInformationManager>();
                informationManager.Add(model.Key, model.Title, model.Author, model.Content);
                return Redirect($"/Admin/Info?_={DateTime.Now.Ticks}");
            }

            return View(model);
        }

        [Route("Info/Update/")]
        [HttpPost]
        public IActionResult UpdateInfo(InformationModel model)
        {
            var informationManager = DomainHub.GetDomain<IInformationManager>();
            informationManager.Update(model.Key, model.Language, model.Title, model.Author, model.Content, model.FacebookComment);
            return Redirect($"/Admin/Info?_={DateTime.Now.Ticks}");
        }

        [Route("Info/Delete/")]
        [HttpPost]
        public IActionResult DeleteInfo(InformationModel model)
        {
            var informationManager = DomainHub.GetDomain<IInformationManager>();
            informationManager.Delete(model.Key, model.Language);
            return Redirect($"/Admin/Info?_={DateTime.Now.Ticks}");
        }

        [Route("Assignment/{status?}")]
        public IActionResult Assignment(string status = AssignmentStatus.Pending)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();
            var assignments = applicationManager.GetAssignments(status);
            ViewBag.Assignments = assignments;
            ViewBag.Status = status;

            var paymentManager = DomainHub.GetDomain<IPaymentManager>();
            var payments = new Dictionary<string, Payment>();
            foreach (var assignment in assignments)
            {
                payments.Add(assignment.Id, paymentManager.GetPayment(assignment.Id));
            }
            ViewBag.Payments = payments;

            return View();
        }

        [Route("Assignment/Accept/{id}")]
        public IActionResult AssignmentAccept(string id)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();
            applicationManager.Accept(id);
            return RedirectToAction("Assignment");
        }

        [Route("Assignment/Reject/{id}")]
        public IActionResult AssignmentReject(string id)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();
            applicationManager.Reject(id);
            return RedirectToAction("Assignment");
        }

        [Route("Assignment/Appoint/{id}")]
        public IActionResult AssignmentAppoint(string id)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();
            applicationManager.Appoint(id);
            return Redirect("/Admin/Assignment/Accepted");
        }

        [Route("Assignment/Complete/{assignmentId}")]
        public IActionResult AssignmentComplete(AppointmentLetterModel model)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();
            
            if (ModelState.IsValid)
            {
                var regex = new Regex("(?<day>[0-9]{2})/(?<month>[0-9]{2})/(?<year>[0-9]{4}) (?<hour>[0-9]{2}):(?<minute>[0-9]{2})");
                var match = regex.Match(model.Time);
                var time = new DateTime(
                    int.Parse(match.Groups["year"].Value),
                    int.Parse(match.Groups["month"].Value),
                    int.Parse(match.Groups["day"].Value),
                    int.Parse(match.Groups["hour"].Value),
                    int.Parse(match.Groups["minute"].Value),
                    0
                );

                applicationManager.Complete(model.AssignmentId, model.AppointmentNo, time, model.Name, model.Category, model.SubCategory);
                return Redirect("/Admin/Assignment/Appointed");
            }

            return View(model);
        }

        [Route("Assignment/Close/{id}")]
        public IActionResult AssignmentClose(string id)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();
            applicationManager.Close(id);
            return Redirect("/Admin/Assignment/Complete");
        }

        [Route("Assignment/Pay/{assignmentId}")]
        public IActionResult AssignmentPay(AssignmentPayModel model)
        {
            if (ModelState.IsValid)
            {
                var paymentManager = DomainHub.GetDomain<IPaymentManager>();
                paymentManager.AdminPay(model.AssignmentId, model.ChargeId, model.Type, model.Currency, model.Amount, model.Payer);
                return Redirect("/Admin/Assignment/Complete");
            }

            return View(model);
        }
    }
}