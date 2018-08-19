using Microsoft.WindowsAzure.Storage.Table;

namespace GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage
{
    public class IRPApplication : TableEntity
    {
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public bool ConfirmGNIB { get; set; }
        public string GNIBNo { get; set; }
        public string GNIBExDT { get; set; }
        public bool UsrDeclaration { get; set; }
        public string GivenName { get; set; }
        public string SurName { get; set; }
        public string DOB { get; set; }
        public string Nationality { get; set; }
        public string Email { get; set; }
        public bool FamAppYN { get; set; }
        public int FamAppNo { get; set; }
        public bool PPNoYN { get; set; }
        public string PPNo { get; set; }
        public string PPReason { get; set; }
    }
}