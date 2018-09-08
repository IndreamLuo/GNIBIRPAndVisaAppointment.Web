using System;
using System.ComponentModel.DataAnnotations;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GNIBIRPAndVisaAppointment.Web.Models
{
    public class ApplicationModel
    {
        public ApplicationModel() { }

        public ApplicationModel(Application application)
        {

        }

        public string Id { get; set; }
        
        [Required]
        public string Category { get; set; }
        
        [Required]
        public string SubCategory { get; set; }
        public bool HasGNIB { get; set; }

        [MaxLength(20)]
        public string GNIBNo { get; set; }
        public string GNIBExDT { get; set; }
        
        [Required]
        public char UsrDeclaration { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string GivenName { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string SurName { get; set; }
        
        [Required]
        public string DOB { get; set; }
        
        [Required]
        public string Nationality { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Compare("Email")]
        public string EmailConfirm { get; set; }
        public bool IsFamily { get; set; }
        public string FamAppNo { get; set; }
        public bool HasPassport { get; set; }
        [MaxLength(30)]
        public string PPNo { get; set; }
        public string PPReason { get; set; }
        public string Comment { get; set; }
        
        [Required]
        public bool AuthorizeContact { get; set; }
    }
}