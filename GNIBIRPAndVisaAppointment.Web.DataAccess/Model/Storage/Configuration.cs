using Microsoft.WindowsAzure.Storage.Table;

namespace GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage
{
    public class Configuration : TableEntity
    {
        public string Value { get; set; }
    }
}