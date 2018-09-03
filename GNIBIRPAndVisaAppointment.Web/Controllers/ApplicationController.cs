using System;
using System.Threading.Tasks;
using GNIBIRPAndVisaAppointment.Web.Business;
using GNIBIRPAndVisaAppointment.Web.Business.Application;
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
            if (ModelState.IsValid && await reCaptchaHelper.VerifyAsync(reCaptchaResponse, HttpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString()))
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
                    Message = model.Message
                };
                
                var applicationId = applicationManager.CreateApplication(application);
                return Redirect($"/Order/{applicationId}");
            }

            return View(model);
        }

        [Route("Order/{applicationId}")]
        public IActionResult Order(string applicationId)
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