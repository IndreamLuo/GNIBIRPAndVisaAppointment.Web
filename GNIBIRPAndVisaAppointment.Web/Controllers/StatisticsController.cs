
using System;
using System.Linq;
using GNIBIRPAndVisaAppointment.Web.Business;
using GNIBIRPAndVisaAppointment.Web.Business.Appointment;
using GNIBIRPAndVisaAppointment.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace GNIBIRPAndVisaAppointment.Web.Controllers
{
    [Route("Statistics")]
    public class StatisticsController : Controller
    {
        IDomainHub DomainHub;

        public StatisticsController(IDomainHub domainHub)
        {
            DomainHub = domainHub;
        }

        [Route("Appointment/{date?}")]
        public ActionResult Appointment(string date = "yesterday")
        {
            if (date.ToLower() == "yesterday")
            {
                date = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
            }

            var assignmentManager = DomainHub.GetDomain<IAppointmentManager>();
            var statisticsDate = date == null
                ? DateTime.Now.Date
                : DateTime.ParseExact(date, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);

            var Statistics = assignmentManager.GetStatistics(statisticsDate, statisticsDate);

            ViewBag.Date = statisticsDate;

            return View(Statistics.Select(statistics => new AppointmentStatisticsModel(statistics)));
        }
    }
}