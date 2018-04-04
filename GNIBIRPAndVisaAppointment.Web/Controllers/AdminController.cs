using System;
using System.IO;
using System.Linq;
using GNIBIRPAndVisaAppointment.Web.Business;
using GNIBIRPAndVisaAppointment.Web.Business.Information;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage;
using GNIBIRPAndVisaAppointment.Web.Models;
using GNIBIRPAndVisaAppointment.Web.Models.Admin;
using Microsoft.AspNetCore.Http;
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

        public IActionResult BrowseImages()
        {
            throw new NotImplementedException();
        }

        [Route("Upload")]
        public IActionResult Upload(IFormFile upload)
        {
            var informationManager = DomainHub.GetDomain<IInformationManager>();
            var url = informationManager.UploadFile(upload.FileName, upload.ContentType, upload.OpenReadStream());

            return Ok(new
            {
                fileName = Path.GetFileNameWithoutExtension(url),
                uploaded = true,
                url = url
            });
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

        [Route("Info/{operation}/{key}/{language}/")]
        public IActionResult Info(InformationModel model, string operation)
        {
            var informationManager = DomainHub.GetDomain<IInformationManager>();
            var information = informationManager[model.Key, model.Language];
            model = new InformationModel
            {
                Key = information.PartitionKey,
                Language = information.RowKey,
                Title = information.Title,
                Author = information.Author,
                CreatedTime = information.CreatedTime,
                Content = information.Content
            };
            ViewBag.operation = operation;

            return View(model);
        }

        [Route("Info/Add/")]
        public IActionResult AddInfo(InformationModel model)
        {
            if (model.Key != null && model.Language != null)
            {
                var informationManager = DomainHub.GetDomain<IInformationManager>();
                informationManager.Add(model.Key, model.Title, model.Author, model.Content);
                return Redirect("/Admin/Info");
            }

            return View(model);
        }

        [Route("Info/Update/")]
        [HttpPost]
        public IActionResult UpdateInfo(InformationModel model)
        {
            var informationManager = DomainHub.GetDomain<IInformationManager>();
            informationManager.Update(model.Key, model.Language, model.Title, model.Author, model.Content);
            return Redirect("/Admin/Info");
        }

        [Route("Info/Delete/")]
        [HttpPost]
        public IActionResult DeleteInfo(InformationModel model)
        {
            var informationManager = DomainHub.GetDomain<IInformationManager>();
            informationManager.Delete(model.Key, model.Language);
            return Redirect("/Admin/Info");
        }
    }
}