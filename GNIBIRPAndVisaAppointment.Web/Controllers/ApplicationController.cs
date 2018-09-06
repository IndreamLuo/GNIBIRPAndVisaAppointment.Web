using System;
using System.Threading.Tasks;
using GNIBIRPAndVisaAppointment.Web.Business;
using GNIBIRPAndVisaAppointment.Web.Business.Application;
using GNIBIRPAndVisaAppointment.Web.Business.Payment;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage;
using GNIBIRPAndVisaAppointment.Web.Models;
using GNIBIRPAndVisaAppointment.Web.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GNIBIRPAndVisaAppointment.Web.Controllers
{
    [Route("Application")]
    public class ApplicationController : Controller
    {
        IDomainHub DomainHub;
        IHttpContextAccessor HttpContextAccessor;
        reCaptchaHelper reCaptchaHelper;

        public ApplicationController(IDomainHub domainHub, IHttpContextAccessor httpContextAccessor, reCaptchaHelper reCaptchaHelper)
        {
            DomainHub = domainHub;
            HttpContextAccessor = httpContextAccessor;
            this.reCaptchaHelper = reCaptchaHelper;
        }

        public async Task<IActionResult> Index(ApplicationModel model, string reCaptchaResponse)
        {
            if (ModelState.IsValid && model.AuthorizeContact && await reCaptchaHelper.VerifyAsync(reCaptchaResponse, HttpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString()))
            {
                var applicationManager = DomainHub.GetDomain<IApplicationManager>();
                var application = new Application
                {
                    Category = model.Category,
                    SubCategory = model.SubCategory,
                    ConfirmGNIB = model.ConfirmGNIB,
                    GNIBNo = model.GNIBNo,
                    GNIBExDT = model.GNIBExDT,
                    UsrDeclaration = model.UsrDeclaration,
                    GivenName = model.GivenName,
                    SurName = model.SurName,
                    DOB = model.DOB,
                    Nationality = model.Nationality,
                    Email = model.Email,
                    FamAppYN = model.FamAppYN,
                    FamAppNo = model.FamAppNo,
                    PPNoYN = model.PPNoYN,
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

            ViewBag.reCaptchaUserCode = reCaptchaHelper.reCaptchaUserCode;

            return View(model);
        }

        [Route("Order/{applicationId}")]
        public IActionResult Order(OrderModel model, bool isOld = false)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();

            if (isOld && ModelState.IsValid)
            {
                var order = new Order
                {
                    ApplicationId = model.ApplicationId,
                    Base = 33,
                    SelectFrom = model.SelectFrom ? 3 : 0,
                    From = model.From,
                    SelectTo = model.SelectTo ? 3 : 0,
                    To = model.To,
                    Rebook = model.Rebook ? 20 : 0,
                    NoCancelRebook = model.NoCancelRebook ? 33 : 0,
                    Emergency = model.Emergency ? 40 : 0
                };

                var orderId = applicationManager.CreateOrder(order);

                return RedirectToAction("Checkout", new
                {
                    orderId = orderId
                });
            }

            ViewBag.Application = applicationManager[model.ApplicationId];

            return View();
        }

        [Route("Checkout/{orderId}/{isFaild?}")]
        public IActionResult Checkout(string orderId, bool isFailed = false)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();
            var paymentManager = DomainHub.GetDomain<IPaymentManager>();

            ViewBag.Order = applicationManager.GetOrder(orderId);
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
                failed = true
            });
        }

        [Route("Paid/{orderId}")]
        public IActionResult Paid(string orderId)
        {
            return View();
        }


        // [Route("PlaceOrder")]
        // public IActionResult PlaceOrder()
        // {
        //     return View();
        // }

        // [Route("Pay")]
        // public IActionResult Pay()
        // {
        //     return View();
        // }

        // [Route("ConfirmPayment")]
        // public IActionResult ConfirmPayment()
        // {
        //     return View();
        // }

        // [Route("Status")]
        // public IActionResult Status()
        // {
        //     return View();
        // }
    }
}