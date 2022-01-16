using System;
using System.ComponentModel.DataAnnotations;

namespace GNIBIRPAndVisaAppointment.Web.Models.Admin
{
    public class MergeModel
    {
        [Required]
        public string Id { get; set; }
        
        [Required]
        public string WithId { get; set; }

        public DateTime Time { get; set; }
        public string GNIBNo { get; set; }
        public string GNIBExDT { get; set; }
        
        [Required]
        public string Salutation { get; set; }

        [Required]
        public string GivenName { get; set; }

        public string MidName { get; set; }

        [Required]
        public string SurName { get; set; }
        
        [Required]
        public string DOB { get; set; }
        
        [Required]
        public string Nationality { get; set; }
        
        [Required]
        public string Email { get; set; }

        [Required]
        public string FamAppYN { get; set; }
        public string FamAppNo { get; set; }

        
        [Required]
        public string PPNoYN { get; set; }
        public string PPNo { get; set; }
        public string PPReason { get; set; }

        public string Comment { get; set; }
        
        public bool PickDate { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public bool Emergency { get; set; }
        public bool AnyCategory { get; set; }
        public string Mobile { get; set; }
        public double Special { get; set; }
        public string OrderComment { get; set; }

        public bool Save { get; set; }
    }
}