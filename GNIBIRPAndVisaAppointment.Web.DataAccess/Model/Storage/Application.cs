using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage
{
    public class Application : TableEntity
    {
        public string Id { get; set; }
        public DateTime Time { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string ConfirmGNIB { get; set; }
        public string GNIBNo { get; set; }
        public string GNIBExDT { get; set; }
        public char UsrDeclaration { get; set; }
        public string GivenName { get; set; }
        public string SurName { get; set; }
        public string DOB { get; set; }
        public string Nationality { get; set; }
        public string Email { get; set; }
        public string FamAppYN { get; set; }
        public string FamAppNo { get; set; }
        public string PPNoYN { get; set; }
        public string PPNo { get; set; }
        public string PPReason { get; set; }
        public string Comment { get; set; }
    }
}