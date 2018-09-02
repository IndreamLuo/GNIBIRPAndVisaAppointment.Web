using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GNIBIRPAndVisaAppointment.Web.Models
{
    public class ApplicationModel
    {
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

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Compare("Email")]
        public string EmailConfirm { get; set; }
        public string FamAppYN { get; set; }
        public int FamAppNo { get; set; }
        public string PPNoYN { get; set; }
        public string PPNo { get; set; }
        public string PPReason { get; set; }
        public string Message { get; set; }
        public bool AuthorizeContact { get; set; }
    }
}