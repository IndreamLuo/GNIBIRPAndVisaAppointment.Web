using GNIBIRPAndVisaAppointment.Web.Utility;
using Microsoft.WindowsAzure.Storage.Table;

namespace GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage
{
    public class Account : TableEntity
    {
        public string Id { get; set; }
        public string Password { get; internal set; }
        public void SetAndEncryptPassword(string password)
        {
            this.Password = password.ToMD5();
        }
    }
}