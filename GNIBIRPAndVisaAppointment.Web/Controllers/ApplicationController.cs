using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GNIBIRPAndVisaAppointment.Web.Business;
using GNIBIRPAndVisaAppointment.Web.Business.Application;
using GNIBIRPAndVisaAppointment.Web.Business.Information;
using GNIBIRPAndVisaAppointment.Web.Business.Payment;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage;
using GNIBIRPAndVisaAppointment.Web.Models;
using GNIBIRPAndVisaAppointment.Web.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GNIBIRPAndVisaAppointment.Web.Controllers
{
    [Route("Application")]
    public class ApplicationController : Controller
    {
        IDomainHub DomainHub;
        IHttpContextAccessor HttpContextAccessor;
        IHostingEnvironment HostingEnvironment;
        reCaptchaHelper reCaptchaHelper;

        public ApplicationController(IDomainHub domainHub, IHttpContextAccessor httpContextAccessor, IHostingEnvironment hostingEnvironment, reCaptchaHelper reCaptchaHelper)
        {
            DomainHub = domainHub;
            HttpContextAccessor = httpContextAccessor;
            HostingEnvironment = hostingEnvironment;
            this.reCaptchaHelper = reCaptchaHelper;
        }

        public async Task<IActionResult> Index(ApplicationModel model, string reCaptchaResponse)
        {
            if (model.HasGNIB)
            {
                if (string.IsNullOrEmpty(model.GNIBNo))
                {
                    ModelState.AddModelError("GNIBNo", "GNIB number is required.");
                }
                else if (!new Regex(@"\d+").IsMatch(model.GNIBNo)) 
                {
                    ModelState.AddModelError("GNIBNo", "GNIB number should all be digits.");
                }
                
                if (string.IsNullOrEmpty(model.GNIBExDT))
                {
                    ModelState.AddModelError("GNIBExDT", "GNIB expired date required.");
                }
                else if (!IsFormattedDate(model.GNIBExDT))
                {
                    ModelState.AddModelError("GNIBExDT", "GNIB expired date format wrong, should be DD/MM/YYYY.");
                }
            }

            if (!string.IsNullOrEmpty(model.DOB) && !IsFormattedDate(model.DOB))
            {
                ModelState.AddModelError("DOB", "Date of Birth is formatting wrong, should be DD/MM/YYYY.");
            }

            if (!model.UsrDeclaration)
            {
                ModelState.AddModelError("UsrDeclaration", "You need to confirm to continue.");
            }

            if (model.IsFamily && string.IsNullOrEmpty(model.FamAppNo))
            {
                ModelState.AddModelError("FamAppNo", "Family number is not selected.");
            }

            if (model.HasPassport)
            {
                if (string.IsNullOrEmpty(model.PPNo))
                {
                    ModelState.AddModelError("PPNo", "Passport Number required.");
                }
            }
            else if (string.IsNullOrEmpty(model.PPReason))
            {
                ModelState.AddModelError("PPReason", "Passport Number or No Passport Reason required.");
            }

            ViewBag.reCaptchaVerified = null;
            
            if (ModelState.IsValid
                && model.AuthorizeDataUsage)
            {
                var reCaptchaVerified = (HostingEnvironment.IsDevelopment() ||
                        await reCaptchaHelper.VerifyAsync(reCaptchaResponse, HttpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString()));

                if (reCaptchaVerified)
                {
                    var applicationManager = DomainHub.GetDomain<IApplicationManager>();
                    var application = new Application
                    {
                        Category = model.Category,
                        SubCategory = model.SubCategory,
                        ConfirmGNIB = model.HasGNIB ? "Yes" : "No",
                        GNIBNo = model.GNIBNo,
                        GNIBExDT = model.GNIBExDT,
                        UsrDeclaration = model.UsrDeclaration ? 'Y' : 'N',
                        GivenName = model.GivenName,
                        SurName = model.SurName,
                        DOB = model.DOB,
                        Nationality = model.Nationality,
                        Email = model.Email,
                        FamAppYN = model.IsFamily ? "Yes" : "No",
                        FamAppNo = model.FamAppNo,
                        PPNoYN = model.HasPassport ? "Yes" : "No",
                        PPNo = model.PPNo,
                        PPReason = model.PPReason,
                        Comment = model.Comment
                    };
                    
                    var applicationId = applicationManager.CreateApplication(application);

                    return RedirectToAction("Order",
                    new
                    {
                        applicationId = applicationId
                    });
                }
                else
                {
                    ViewBag.reCaptchaVerified = reCaptchaVerified;
                }
            }

            ViewBag.reCaptchaUserCode = reCaptchaHelper.reCaptchaUserCode;
            ViewBag.HostingEnvironment = HostingEnvironment;

            return View(model);
        }

        bool IsFormattedDate(string dateString)
        {
            var dateRegex = new Regex(@"\d{2}/\d{2}/\d{4}");
            if (dateRegex.IsMatch(dateString))
            {
                var numbers = dateString
                    .Split("/");

                DateTime value;

                return DateTime.TryParse($"{numbers[2]}-{numbers[1]}-{numbers[0]} 00:00:00", out value);
            }

            return false;
        }

        [Route("Order/{applicationId}")]
        public async Task<IActionResult> Order(OrderModel model, bool isOld = false)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();

            if (isOld)
            {
                if (!IsFormattedDate(model.From))
                {
                    ModelState.AddModelError("From", "Date format wrong, shoud be as 30/11/2018 in DD/MM/YYYY.");
                }

                if (!string.IsNullOrEmpty(model.To) && !IsFormattedDate(model.To))
                {
                    ModelState.AddModelError("To", "Date format wrong, shoud be as 30/11/2018 in DD/MM/YYYY.");
                }

                if (ModelState.IsValid)
                {
                    var order = new Order
                    {
                        ApplicationId = model.ApplicationId,
                        Base = 20,
                        PickDate = model.PickDate ? 10 : 0,
                        From = model.From,
                        To = model.To,
                        Emergency = model.Emergency ? 20 : 0
                    };

                    var orderId = applicationManager.CreateOrder(order);

                    return RedirectToAction("Checkout", new
                    {
                        orderId = orderId
                    });
                }
            }

            ViewBag.Application = applicationManager[model.ApplicationId];

            if (ViewBag.Application == null)
            {
                await Task.Delay(1000);
                ViewBag.Application = applicationManager[model.ApplicationId];
            }

            return View(model);
        }

        [Route("Checkout/{orderId}/{isFailed?}")]
        public IActionResult Checkout(string orderId, bool isFailed = false)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();
            var paymentManager = DomainHub.GetDomain<IPaymentManager>();

            ViewBag.Order = applicationManager.GetOrder(orderId);
            ViewBag.Assignment = applicationManager.GetAssignment(orderId);
            ViewBag.StripeKey = paymentManager.PublishableKey;

            return View();
        }

        [Route("StripePay/{orderId}")]
        public IActionResult StripePay(string orderId, string stripeToken, string stripeEmail)
        {
            var paymentManager = DomainHub.GetDomain<IPaymentManager>();
            
            if (paymentManager.StripePay(orderId, stripeToken, stripeEmail))
            {
                return RedirectToAction("Paid", new
                {
                    orderId = orderId
                });
            }

            return RedirectToAction("Checkout", new
            {
                orderId = orderId,
                isFailed = true
            });
        }

        [Route("PayAfter/{orderId}")]
        public IActionResult PayAfter(string orderId)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();

            if (applicationManager.GetAssignment(orderId) == null)
            {
                applicationManager.Pending(orderId);
            }

            return RedirectToAction("Status", new
            {
                orderId = orderId
            });
        }

        [Route("Paid/{orderId}")]
        public IActionResult Paid(string orderId)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();
            applicationManager.Pending(orderId);

            return RedirectToAction("Status", new
            {
                orderId = orderId
            });
        }

        [Route("Status/{orderId}")]
        public IActionResult Status(string orderId)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();

            ViewBag.Assignment = applicationManager.GetAssignment(orderId);
            ViewBag.Order = applicationManager.GetOrder(orderId);
            ViewBag.Application = applicationManager[orderId];

            var paymentManager = DomainHub.GetDomain<IPaymentManager>();
            ViewBag.Payment = paymentManager.GetPayment(orderId);

            if (ViewBag.Payment != null)
            {
                ViewBag.AppointmentLetter = applicationManager.GetAppointmentLetter(orderId);
            }

            return View();
        }

        [Route("Appointment/{orderId}")]
        public IActionResult Appointment(string orderId)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();
            ViewBag.Appointment = applicationManager.GetAppointmentLetter(orderId);

            var informationManager = DomainHub.GetDomain<IInformationManager>();
            var letterTemplate = informationManager["appointment-letter"];
            ViewBag.AppointmentLetterTemplate = letterTemplate.Content;

            return View();
        }
    }
}