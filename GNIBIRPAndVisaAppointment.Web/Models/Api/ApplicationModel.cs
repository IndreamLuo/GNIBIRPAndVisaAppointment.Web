using System;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage;

namespace GNIBIRPAndVisaAppointment.Web.Models.Api
{
    public class ApplicationModel
    {
        public ApplicationModel(Application entity)
        {
            Time = entity.Time;
            Category = entity.Category;
            SubCategory = entity.SubCategory;
            ConfirmGNIB = entity.ConfirmGNIB;
            GNIBNo = entity.GNIBNo;
            GNIBExDT = entity.GNIBExDT;
            UsrDeclaration = entity.UsrDeclaration;
            GivenName = entity.GivenName;
            SurName = entity.SurName;
            DOB = entity.DOB;
            Nationality = entity.Nationality;
            Email = entity.Email;
            FamAppYN = entity.FamAppYN;
            FamAppNo = entity.FamAppNo;
            PPNoYN = entity.PPNoYN;
            PPNo = entity.PPNo;
            PPReason = entity.PPReason;
        }

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
    }
}