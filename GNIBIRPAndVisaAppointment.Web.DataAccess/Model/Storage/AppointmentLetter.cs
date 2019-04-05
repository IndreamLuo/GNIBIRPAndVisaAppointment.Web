using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage
{
    public class AppointmentLetter : TableEntity
    {
        public string EmailId { get; set; }
        public string AppointmentNo { get; set; }
        public DateTime Time { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
    }
}