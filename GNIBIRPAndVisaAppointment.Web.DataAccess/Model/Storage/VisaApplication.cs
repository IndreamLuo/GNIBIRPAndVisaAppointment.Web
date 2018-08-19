using Microsoft.WindowsAzure.Storage.Table;

namespace GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage
{
    public class VisaApplication : TableEntity
    {
        public bool btCom { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public string DOB { get; set; }
        public string Email { get; set; }
        public string AppointType { get; set; }
        public int AppsNum { get; set; }
        public string EmReason { get; set; }
        public string EmContactNum { get; set; }
        public string PassportNo { get; set; }
        public string Nationality { get; set; }
        public string VisaType { get; set; }
        public string GNIBNo { get; set; }
    }
}