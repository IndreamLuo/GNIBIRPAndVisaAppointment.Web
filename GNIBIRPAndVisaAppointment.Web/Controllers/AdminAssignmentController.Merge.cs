using System;
using System.Threading.Tasks;
using GNIBIRPAndVisaAppointment.Web.Business.Application;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage;
using GNIBIRPAndVisaAppointment.Web.Models.Admin;
using Microsoft.AspNetCore.Mvc;

namespace GNIBIRPAndVisaAppointment.Web.Controllers
{
    public partial class AdminAssignmentController
    {
        [HttpGet]
        [Route("Merge/{id}")]
        public async Task<IActionResult> Merge(string id)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();

            return View(new MergeModel
            {
                Id = id
            });
        }

        [HttpPost]
        [Route("Merge/{id}")]
        public async Task<IActionResult> Merge(string id, string withId)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();

            return Redirect($"{id}/With/{withId}");
        }

        [Route("Merge/{id}/With/{withId}")]
        public async Task<IActionResult> Merge(MergeModel model)
        {
            if (model.Save)
            {
                throw new NotImplementedException();
            }
            else
            {
                var applicationManager = DomainHub.GetDomain<IApplicationManager>();

                var application = applicationManager[model.Id];
                var order = applicationManager.GetOrder(model.Id);
                var withApplication = applicationManager[model.WithId];
                var withOrder = applicationManager.GetOrder(model.WithId);

                SetModel(model, application, order);

                var merging = new MergeModel();
                SetModel(merging, withApplication, withOrder);
                ViewBag.Merging = merging;

                return View(model);
            }
        }

        public void SetModel(MergeModel model, Application application, Order order)
        {
            model.Time = application.Time;
            model.GNIBNo = model.GNIBNo ?? application.GNIBNo;
            model.GNIBExDT = model.GNIBExDT ?? application.GNIBExDT;
            model.Salutation = model.Salutation ?? application.Salutation;
            model.GivenName = model.GivenName ?? application.GivenName;
            model.MidName = model.MidName ?? application.MidName;
            model.SurName = model.SurName ?? application.SurName;
            model.DOB = model.DOB ?? application.DOB;
            model.Nationality = model.Nationality ?? application.Nationality;
            model.Email = model.Email ?? application.Email;
            model.FamAppYN = model.FamAppYN ?? application.FamAppYN;
            model.FamAppNo = model.FamAppNo ?? application.FamAppNo;
            model.PPNoYN = model.PPNoYN ?? application.PPNoYN;
            model.PPNo = model.PPNo ?? application.PPNo;
            model.PPReason = model.PPReason ?? application.PPReason;
            model.Comment = model.Comment ?? application.Comment;
        }
    }
}