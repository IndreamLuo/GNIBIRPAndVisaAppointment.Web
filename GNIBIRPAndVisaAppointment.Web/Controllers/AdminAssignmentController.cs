using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GNIBIRPAndVisaAppointment.Web.Business;
using GNIBIRPAndVisaAppointment.Web.Business.Application;
using GNIBIRPAndVisaAppointment.Web.Business.Payment;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage;
using GNIBIRPAndVisaAppointment.Web.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GNIBIRPAndVisaAppointment.Web.Controllers
{
    [Authorize(Roles="Admin,Manager")]
    [Route("Admin/Assignment")]
    public class AdminAssignmentController : Controller
    {
        readonly IDomainHub DomainHub;
        
        public AdminAssignmentController(IDomainHub domainHub)
        {
            DomainHub = domainHub;
        }

        [Route("{status?}")]
        public IActionResult Index(string status = AssignmentStatus.Pending)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();
            var assignments = applicationManager.GetAssignments(status, true);
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

        [Route("Accept/{id}")]
        public IActionResult Accept(string id)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();
            applicationManager.Accept(id);
            return RedirectToAction("Index");
        }

        [Route("Reject/{id}")]
        public IActionResult Reject(string id)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();
            applicationManager.Reject(id);
            return RedirectToAction("Index");
        }

        [Route("Appoint/{id}")]
        public IActionResult Appoint(string id)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();
            applicationManager.Appoint(id);
            return Redirect("/Admin/Assignment/Accepted");
        }

        [Route("Complete/{assignmentId}")]
        public IActionResult Complete(AppointmentLetterModel model)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();
            
            if (ModelState.IsValid)
            {
                var regex = new Regex("(?<day>[0-9]{2})/(?<month>[0-9]{2})/(?<year>[0-9]{4}), (?<hour>[0-9]{2}):(?<minute>[0-9]{2})");
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

        [Route("Close/{id}")]
        public IActionResult Close(string id)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();
            applicationManager.Close(id);
            return Redirect("/Admin/Assignment/Complete");
        }

        [Authorize(Roles="Admin,Manager")]
        [Route("Pay/{assignmentId}")]
        public IActionResult Pay(AssignmentPayModel model)
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