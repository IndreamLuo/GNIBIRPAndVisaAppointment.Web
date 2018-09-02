using System;
using GNIBIRPAndVisaAppointment.Web.Business;
using GNIBIRPAndVisaAppointment.Web.Business.Information;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage;
using GNIBIRPAndVisaAppointment.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace GNIBIRPAndVisaAppointment.Web.Controllers
{
    [Route("Application")]
    public class ApplicationController : Controller
    {
        IDomainHub DomainHub;

        public ApplicationController(IDomainHub domainHub)
        {
            DomainHub = domainHub;
        }

        public IActionResult Index(ApplicationModel model)
        {
            if (ModelState.IsValid)
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
                
                applicationManager.CreateApplication(application);
            }

            return View(model);
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