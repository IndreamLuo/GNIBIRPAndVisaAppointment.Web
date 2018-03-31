using System;
using GNIBIRPAndVisaAppointment.Web.Business;
using Microsoft.AspNetCore.Mvc;

namespace GNIBIRPAndVisaAppointment.Web.Controllers
{
    public class AdminController : Controller
    {
        public AdminController(IDomainHub domainHub)
        {

        }
        
        public IActionResult Index()
        {
            throw new NotImplementedException();
        }
    }
}