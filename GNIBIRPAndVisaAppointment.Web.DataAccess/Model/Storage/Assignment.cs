using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage
{
    public class Assignment : TableEntity
    {
        public string Id { get; set; }
        public DateTime Time { get; set; }
        public string AppointmentNo { get; set; }
        public string Status { get; set; }
        public Application Application { get; set; }
        public Order Order { get; set; }
    }
}