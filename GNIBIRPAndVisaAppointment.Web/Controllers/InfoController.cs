using System;
using GNIBIRPAndVisaAppointment.Web.Business;
using GNIBIRPAndVisaAppointment.Web.Business.Information;
using GNIBIRPAndVisaAppointment.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace GNIBIRPAndVisaAppointment.Web.Controllers
{
    [Route("Info")]
    public class InfoController : Controller
    {
        IDomainHub DomainHub;

        public InfoController(IDomainHub domainHub)
        {
            DomainHub = domainHub;
        }

        [Route("About/{name}")]
        public IActionResult About(string name)
        {
            throw new NotImplementedException();
        }

        [Route("Get/{key}/{language}")]
        public JsonResult Get(string key, string language = null)
        {
            var informationManager = DomainHub.GetDomain<IInformationManager>();
            var information = informationManager[key, language];

            if (information == null)
            {
                return null;
            }

            var informationModel = new InformationModel
            {
                Key = information.PartitionKey,
                Language = information.RowKey,
                Title = information.Title,
                Author = information.Author,
                CreatedTime = information.CreatedTime,
                Content = information.Content
            };

            return Json(informationModel);
        }
    }
}