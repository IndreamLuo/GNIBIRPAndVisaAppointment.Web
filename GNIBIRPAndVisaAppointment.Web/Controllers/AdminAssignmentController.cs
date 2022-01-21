using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GNIBIRPAndVisaAppointment.Web.Business;
using GNIBIRPAndVisaAppointment.Web.Business.Application;
using GNIBIRPAndVisaAppointment.Web.Business.AppointmnetLetter;
using GNIBIRPAndVisaAppointment.Web.Business.Payment;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage;
using GNIBIRPAndVisaAppointment.Web.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GNIBIRPAndVisaAppointment.Web.Controllers
{
    [Authorize(Roles="Admin,Manager")]
    [Route("Admin/Assignment")]
    public partial class AdminAssignmentController : Controller
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

            if (status == AssignmentStatus.Complete)
            {
                assignments = assignments.OrderBy(assignment => assignment.AppointmentLetter?.Time ?? DateTime.MaxValue).ToList();
            }
            else
            {
                assignments = assignments.OrderBy(assignment => assignment.Time).ToList();
            }

            ViewBag.Assignments = assignments;
            ViewBag.Status = status;

            var paymentManager = DomainHub.GetDomain<IPaymentManager>();
            var payments = new Dictionary<string, List<Payment>>();
            var isPaids = new Dictionary<string, bool>();
            foreach (var assignment in assignments)
            {
                payments.Add(assignment.Id, paymentManager.GetPayments(assignment.Id));
                isPaids.Add(assignment.Id, paymentManager.IsPaid(assignment.Id));
            }
            ViewBag.Payments = payments;
            ViewBag.IsPaids = isPaids;

            if (status == AssignmentStatus.Appointed)
            {
                var appointmentLetterManger = DomainHub.GetDomain<IAppointmentLetterManager>();
                var letters = appointmentLetterManger
                    .UnassignedLetters
                    .Where(letter => letter.Time > DateTime.Now.AddDays(-7))
                    .GroupBy(letter => letter.Name.Replace("\r", ""))
                    .ToDictionary(group => group.Key);
                var noLetter = new AppointmentLetter[0];
                ViewBag.AppointmentLetters = assignments
                    .ToDictionary(assignment => assignment.Id,
                        assignment =>
                        {
                            var application = applicationManager[assignment.Id];
                            var name = $"{application.GivenName}  {application.SurName}";
                            return letters.ContainsKey(name) ? letters[name].ToArray() : noLetter;
                        });
            }

            return View();
        }

        [Route("Pendings")]
        public IActionResult Pendings(string[] ids)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();

            foreach (var id in ids)
            {
                applicationManager.Pending(id);
            }

            return RedirectToAction("Index",
            new
            {
                Status = AssignmentStatus.Accepted
            });
        }

        [Route("Accepts")]
        public IActionResult Accepts(string[] ids)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();

            foreach (var id in ids)
            {
                applicationManager.Accept(id);
            }

            return Redirect("/Admin/Assignment/Pending");
        }

        [Route("Completes")]
        public IActionResult Completes(string[] letters)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();
            
            foreach (var letter in letters)
            {
                var keys = letter.Split("|");
                applicationManager.Complete(keys[0], keys[1]);
            }

            return Redirect("/Admin/Assignment/Appointed");
        }

        [Route("Pending/{id}")]
        public IActionResult Pending(string id, string returnUrl)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();
            applicationManager.Pending(id);
            return Redirect(returnUrl ?? "/Admin/Assignment/Pending");
        }

        [Route("Accept/{id}")]
        public IActionResult Accept(string id, string returnUrl)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();
            applicationManager.Accept(id);
            return Redirect(returnUrl ?? "/Admin/Assignment/Pending");
        }

        [Route("Reaccept/{id}")]
        public IActionResult Reaccept(string id, string returnUrl)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();
            applicationManager.Reaccept(id);
            return Redirect(returnUrl ?? "/Admin/Assignment/Appointed");
        }

        [Route("Duplicate/{id}")]
        public IActionResult Duplicate(string id, string returnUrl)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();
            applicationManager.Duplicate(id);
            return Redirect(returnUrl ?? "/Admin/Assignment/Accepted");
        }

        [Route("Cancel/{id}")]
        public IActionResult Cancel(string id, string returnUrl)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();
            applicationManager.Cancel(id);
            return Redirect(returnUrl ?? "/Admin/Assignment/Appointed");
        }

        [Route("Reject/{id}")]
        public IActionResult Reject(string id, string returnUrl)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();
            applicationManager.Reject(id);
            return Redirect(returnUrl ?? "/Admin/Assignment/Pending");
        }

        [Route("Appoint/{id}")]
        public IActionResult Appoint(string id, string returnUrl)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();
            applicationManager.Appoint(id);
            return Redirect(returnUrl ?? "/Admin/Assignment/Accepted");
        }

        [Route("Complete/{assignmentId}/{emailId?}")]
        public IActionResult Complete(AppointmentLetterModel model, string emailId, string returnUrl)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();
            var application = applicationManager[model.AssignmentId];
            var fullName = $"{application.GivenName.Trim()}  {application.SurName.Trim()}";

            var appointmentLetterManger = DomainHub.GetDomain<IAppointmentLetterManager>();
            var availableAppointmentLetters = appointmentLetterManger.FindByName(fullName);
            ViewBag.AvailableAppointmentLetters = availableAppointmentLetters;

            // if (model.Name != fullName)
            // {
            //     ModelState.AddModelError("Name", "Name doesn't match with applicant.");
            // }
            
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
                
                if (emailId != null)
                {
                    applicationManager.Complete(model.AssignmentId, emailId);
                }
                else
                {
                    applicationManager.Complete(model.AssignmentId, model.AppointmentNo, time, model.Name, model.Category, model.SubCategory);
                }
                return Redirect(returnUrl ?? "/Admin/Assignment/Appointed");
            }
            else if (!string.IsNullOrEmpty(model.Content))
            {
                var regex = new Regex(@"Name: (?<name>.*)\r\n.*Appointment Date: (?<time>.*)\r\n.*Registration Appointment Reference: (?<reference>.*)[\r\n.]*Category: (?<category>[^ ]*) \| (?<subCategory>[^\r]*)");
                var match = regex.Match(model.Content);
                model.Name = match.Groups["name"].Value;
                model.Time = match.Groups["time"].Value;
                model.AppointmentNo = match.Groups["reference"].Value;
                model.Category = match.Groups["category"].Value;
                model.SubCategory = match.Groups["subCategory"].Value;
            }
            else if (emailId != null)
            {
                var appointmentLetter = availableAppointmentLetters.First(letter => letter.EmailId == emailId);
                model.Name = appointmentLetter.Name;
                model.AppointmentNo = appointmentLetter.AppointmentNo;
                model.Time = appointmentLetter.Time.ToString("dd/MM/yyyy, HH:mm");
                model.Category = application.Category;
                model.SubCategory = application.SubCategory;
            }

            ViewBag.ReturnUrl = returnUrl;

            return View(model);
        }

        [Route("Close/{id}")]
        public IActionResult Close(string id, string returnUrl)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();
            applicationManager.Close(id);
            return Redirect(returnUrl ?? "/Admin/Assignment/Complete");
        }

        [Authorize(Roles="Admin,Manager")]
        [Route("Pay/{assignmentId}")]
        public IActionResult Pay(AssignmentPayModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var paymentManager = DomainHub.GetDomain<IPaymentManager>();
                paymentManager.AdminPay(model.AssignmentId, model.ChargeId, model.Type, model.Currency, model.Amount, model.Payer);
                return Redirect(returnUrl ?? "/Admin/Assignment/Complete");
            }

            return View(model);
        }

        [Route("Contact/{id}")]
        public IActionResult Contact(string id)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();
            var application = applicationManager[id];
            var contacts = new [] { application.Email };

            ViewBag.Contacts = contacts;

            return View();
        }
    }
}