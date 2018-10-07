using GNIBIRPAndVisaAppointment.Web.Utility;
using Microsoft.WindowsAzure.Storage.Table;

namespace GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage
{
    public class AppointLog : TableEntity
    {
        public string Id { get; set; }
        public string Slot { get; set; }
        public bool Success { get; set; }
        public string Result { get; set; }
        public double TimeSpan { get; set; }
    }
}