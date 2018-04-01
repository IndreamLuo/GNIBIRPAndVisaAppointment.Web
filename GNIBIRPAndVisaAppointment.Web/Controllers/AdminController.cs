using System;
using System.Linq;
using GNIBIRPAndVisaAppointment.Web.Business;
using GNIBIRPAndVisaAppointment.Web.Business.Information;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage;
using GNIBIRPAndVisaAppointment.Web.Models;
using GNIBIRPAndVisaAppointment.Web.Models.Admin;
using Microsoft.AspNetCore.Mvc;

namespace GNIBIRPAndVisaAppointment.Web.Controllers
{
    [Route("Admin")]
    public class AdminController : Controller
    {
        IDomainHub DomainHub;

        public AdminController(IDomainHub domainHub)
        {
            DomainHub = domainHub;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        [Route("Info")]
        public IActionResult Infos()
        {
            var informationManager = DomainHub.GetDomain<IInformationManager>();
            var informationList = informationManager.GetList();
            var models = informationList.Select(entity => new InformationModel
            {
                Key = entity.PartitionKey,
                Language = entity.RowKey,
                Title = entity.Title,
                Author = entity.Author,
                CreatedTime = entity.CreatedTime
            });

            return View(models);
        }

        [Route("Info/{key}/{language?}")]
        public IActionResult Info(Information model)
        {
            var informationManager = DomainHub.GetDomain<IInformationManager>();
            return View();
        }
    }
}