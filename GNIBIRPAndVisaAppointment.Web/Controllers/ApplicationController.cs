using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GNIBIRPAndVisaAppointment.Web.Business;
using GNIBIRPAndVisaAppointment.Web.Business.Application;
using GNIBIRPAndVisaAppointment.Web.Business.Information;
using GNIBIRPAndVisaAppointment.Web.Business.Payment;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage;
using GNIBIRPAndVisaAppointment.Web.Identity;
using GNIBIRPAndVisaAppointment.Web.Models;
using GNIBIRPAndVisaAppointment.Web.Models.Application;
using GNIBIRPAndVisaAppointment.Web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GNIBIRPAndVisaAppointment.Web.Controllers
{
    [Route("Application")]
    public class ApplicationController : Controller
    {
        readonly IDomainHub DomainHub;
        readonly IHttpContextAccessor HttpContextAccessor;
        readonly IHostingEnvironment HostingEnvironment;
        readonly reCaptchaHelper reCaptchaHelper;
        readonly SignInManager<ApplicationUser> SignInManager;

        public ApplicationController(IDomainHub domainHub, IHttpContextAccessor httpContextAccessor, IHostingEnvironment hostingEnvironment, reCaptchaHelper reCaptchaHelper, SignInManager<ApplicationUser> signInManager)
        {
            DomainHub = domainHub;
            HttpContextAccessor = httpContextAccessor;
            HostingEnvironment = hostingEnvironment;
            this.reCaptchaHelper = reCaptchaHelper;
            SignInManager = signInManager;
        }

        public async Task<IActionResult> Index(ApplicationModel model, string reCaptchaResponse)
        {
            if (model.IsInitialized)
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

                if (!string.IsNullOrEmpty(model.DOB))
                {
                    if (!IsFormattedDate(model.DOB))
                    {
                        ModelState.AddModelError("DOB", "Date of Birth is formatting wrong, should be DD/MM/YYYY.");
                    }
                    else
                    {
                        var dob = DateTime.ParseExact(model.DOB, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        if (dob >= DateTime.Now)
                        {
                            ModelState.AddModelError(nameof(model.DOB), "Date of Birth should be at least before today.");
                        }
                    }
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
                            ConfirmGNIB = model.HasGNIB ? "Renewal" : "New",
                            GNIBNo = model.GNIBNo,
                            GNIBExDT = model.GNIBExDT,
                            UsrDeclaration = model.UsrDeclaration ? 'Y' : 'N',
                            Salutation = model.Salutation,
                            GivenName = model.GivenName,
                            MidName = model.MidName,
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
                        model.HasGNIB = true;
                    }
                }
            }
            else
            {
                ModelState.Clear();
            }

            ViewBag.reCaptchaUserCode = reCaptchaHelper.reCaptchaUserCode;
            ViewBag.HostingEnvironment = HostingEnvironment;

            return View(model);
        }

        bool IsFormattedDate(string dateString)
        {
            DateTime notUsed;
            return IsFormattedDate(dateString, out notUsed);
        }
        bool IsFormattedDate(string dateString, out DateTime time)
        {
            var dateRegex = new Regex(@"\d{2}/\d{2}/\d{4}");
            if (dateRegex.IsMatch(dateString))
            {
                var numbers = dateString
                    .Split("/");

                return DateTime.TryParse($"{numbers[2]}-{numbers[1]}-{numbers[0]} 00:00:00", out time);
            }

            time = DateTime.MaxValue;

            return false;
        }

        [Route("Order/{applicationId}")]
        public async Task<IActionResult> Order(OrderModel model, bool isOld = false)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();
            var isSignedIn = SignInManager.IsSignedIn(User);
            ViewBag.IsSignedIn = isSignedIn;

            var assignment = applicationManager.GetAssignment(model.ApplicationId);
            if (assignment != null && assignment.Status != AssignmentStatus.Pending && !isSignedIn)
            {
                return RedirectToAction("Status", new
                {
                    orderId = model.ApplicationId
                });
            }

            if (isOld)
            {
                if (model.PickDate)
                {
                    if (string.IsNullOrEmpty(model.From) && string.IsNullOrEmpty(model.To))
                    {
                        ModelState.AddModelError("From", "You need to specify the date range you want for appointment.");
                    }
                    else
                    {
                        DateTime from = DateTime.MinValue;
                        if (!string.IsNullOrEmpty(model.From) && !IsFormattedDate(model.From, out from))
                        {
                            ModelState.AddModelError("From", "Date format wrong, shoud be as 30/11/2018 in DD/MM/YYYY.");
                        }

                        DateTime to = DateTime.MaxValue;
                        if (!string.IsNullOrEmpty(model.To) && !IsFormattedDate(model.To, out to))
                        {
                            ModelState.AddModelError("To", "Date format wrong, shoud be as 30/11/2018 in DD/MM/YYYY.");
                        }

                        if (from > to)
                        {
                            ModelState.AddModelError("To", "Date of 'To' can only be after 'From'.");
                        }
                    }
                }
                else if (model.Emergency)
                {
                    ModelState.AddModelError("PickDate", "Emergency service has to select with target date range.");
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
                        AnyCategory = model.AnyCategory ? 1 : 0,
                        Emergency = model.Emergency ? 10 : 0,
                        Special = model.Special,
                        Comment = model.Comment
                    };

                    var orderId = applicationManager.CreateOrder(order);

                    return RedirectToAction("PayAfter", new
                    {
                        orderId = orderId
                    });
                }
            }
            else
            {
                var order = applicationManager.GetOrder(model.ApplicationId);
                if (order != null)
                {
                    model.PickDate = order.PickDate > 0;
                    model.From = order.From;
                    model.To = order.To;
                    model.AnyCategory = order.AnyCategory != 0;
                    model.Emergency = order.Emergency > 0;
                    model.Special = order.Special;
                    model.Comment = order.Comment;
                }
                else
                {
                    model.PickDate = true;
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
            ViewBag.UnpaidAmount = paymentManager.GetUnpaidAmount(orderId);
            ViewBag.StripeKey = paymentManager.PublishableKey;

            return View();
        }

        [Route("CreatePayment/{orderId}")]
        public IActionResult CreatePayment(string orderId)
        {
            var paymentManager = DomainHub.GetDomain<IPaymentManager>();
            var domain = $"{Request.Scheme}://{Request.Host.Value}";

            var session = paymentManager.CreateStripePaySession(orderId, $"{domain}/Application/Checkout/{orderId}", $"{domain}/Application/PaymentSuccess/{orderId}");            

            return Json(session.Id);
        }

        [Route("PaymentSuccess/{orderId}")]
        public IActionResult PaymentSuccess(string orderId)
        {
            var paymentManager = DomainHub.GetDomain<IPaymentManager>();
            var isPaid = paymentManager.ConfirmPayment(orderId);

            if (!isPaid)
            {
                return RedirectToAction("Checkout", new {
                    orderId = orderId
                });
            }
            
            return RedirectToAction("Status", new {
                orderId = orderId
            });
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
            var isSignedIn = SignInManager.IsSignedIn(User);
            ViewBag.IsSignedIn = isSignedIn;

            var applicationManager = DomainHub.GetDomain<IApplicationManager>();

            var assignment = applicationManager.GetAssignment(orderId);
            ViewBag.Assignment = assignment;
            ViewBag.Order = applicationManager.GetOrder(orderId);

            var application = applicationManager[orderId];
            if ((assignment.Status == AssignmentStatus.Closed
                    || assignment.Status == AssignmentStatus.Cancelled
                    || assignment.Status == AssignmentStatus.Rejected)
                && !isSignedIn)
            {
                application.Salutation = "Closed";
                application.GivenName = "Closed";
                application.MidName = "Closed";
                application.SurName = "Closed";
                application.DOB = "Closed";
                application.GNIBNo = "Closed";
                application.GNIBExDT = "Closed";
                application.PPNo = "Closed";
                application.PPReason = "Closed";
                application.Nationality = "Closed";
                application.Comment = "Closed";
                application.Email = "Closed";
                application.Category = "Closed";
                application.SubCategory = "Closed";
                application.FamAppYN = "Yes";
                application.FamAppNo = "Closed";
            }

            ViewBag.Application = application;

            var paymentManager = DomainHub.GetDomain<IPaymentManager>();
            ViewBag.Payments = paymentManager.GetPayments(orderId);
            ViewBag.IsPaid = paymentManager.IsPaid(orderId);

            if (ViewBag.Payment != null)
            {
                ViewBag.AppointmentLetter = applicationManager.GetAppointmentLetter(orderId);
            }

            return View();
        }

        [Route("Appointment/{orderId}")]
        public async Task<IActionResult> Appointment(string orderId)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();
            var order = applicationManager.GetOrder(orderId);
            var paymentManager = DomainHub.GetDomain<IPaymentManager>();

            if (!SignInManager.IsSignedIn(User) && !paymentManager.IsPaid(orderId))
            {
                return RedirectToAction(nameof(this.Checkout), new
                {
                    orderId = orderId
                });
            }

            ViewBag.ApplicationId = orderId;

            ViewBag.Appointment = applicationManager.GetAppointmentLetter(orderId);

            var informationManager = DomainHub.GetDomain<IInformationManager>();
            var letterTemplate = informationManager["appointment-letter"];
            ViewBag.AppointmentLetterTemplate = letterTemplate.Content;

            return View();
        }

        [Route("Download/{orderId}")]
        public async Task<IActionResult> Download(string orderId)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();
            var order = applicationManager.GetOrder(orderId);
            var paymentManager = DomainHub.GetDomain<IPaymentManager>();

            if (!SignInManager.IsSignedIn(User) && !paymentManager.IsPaid(orderId))
            {
                throw new InvalidOperationException("Not paid.");
            }

            var appointmentLetter = applicationManager.GetAppointmentLetter(orderId);

            var informationManager = DomainHub.GetDomain<IInformationManager>();
            var letterTemplate = informationManager["appointment-letter"];

            var appointmentLetterText = WebUtility
                    .HtmlDecode(Regex
                        .Replace(letterTemplate
                        .Content
                        .Replace("{AppointmentNo}", appointmentLetter.AppointmentNo)
                        .Replace("{Time}", appointmentLetter.Time.ToString("dd/MM/yyyy HH:mm"))
                        .Replace("{Name}", appointmentLetter.Name)
                        .Replace("{Category}", appointmentLetter.Category)
                        .Replace("{SubCategory}", appointmentLetter.SubCategory),
                    "<.*?>",
                    string.Empty));

            return File(Encoding.UTF8.GetBytes(appointmentLetterText),
                "text/plain",
                $"{appointmentLetter.Name} - INIS IRP Appointment Letter.txt");
        }

        [Authorize(Roles="Admin,Manager")]
        [Route("Contact/{orderId}")]
        public IActionResult Contact(string orderId)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();
            var application = applicationManager[orderId];
            var assignment = applicationManager.GetAssignment(orderId);
            var appointment = applicationManager.GetAppointmentLetter(orderId);
            var informationManager = DomainHub.GetDomain<InformationManager>();
            string emailTitle = string.Empty;
            string emailContent = string.Empty;

            if (assignment.Status == AssignmentStatus.Complete)
            {
                var email = informationManager["contact-email-complete"];
                emailTitle = email.Title;
                emailContent = email
                    .Content
                    .Replace("{data}", appointment.Time.ToString("dddd, dd MMMM"))
                    .Replace("{id}", orderId);
            }

            var model = new List<ContactModel>();
            
            model.Add(new ContactModel
            {
                Contact = application.Email,
                Title = emailTitle,
                Content = emailContent,
                IsHtml = true
            });

            return View(model);
        }

        [Route("ChangeGNIB/{Id}")]
        public IActionResult ChangeGNIB(ChangeGNIBModel model)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();

            if (!model.IsInitialazed)
            {
                var application = applicationManager[model.Id];
                model.HasGNIB = application.ConfirmGNIB == "Renewal";
                model.GNIBNo = application.GNIBNo;
                model.GNIBExDT = application.GNIBExDT;
            }
            else
            {
                if (model.HasGNIB)
                {
                    if (string.IsNullOrEmpty(model.GNIBNo))
                    {
                        ModelState.AddModelError("GNIBNo", "GNIB No is required.");
                    }

                    if (!IsFormattedDate(model.GNIBExDT))
                    {
                        ModelState.AddModelError("GNIBExDT", "GNIB expired date format wrong, should be DD/MM/YYYY.");
                    }
                }
                
                if (ModelState.IsValid)
                {
                    applicationManager.ChangeGNIB(model.Id, model.HasGNIB, model.GNIBNo, model.GNIBExDT);

                    return RedirectToAction(nameof(Status),
                    new
                    {
                        orderId = model.Id
                    });
                }
            }

            model.IsInitialazed = true;

            return View(model);
        }
    }
}