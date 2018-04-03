using System;
using System.ComponentModel.DataAnnotations;

namespace GNIBIRPAndVisaAppointment.Web.Models
{
    public class InformationModel
    {
        [Required]
        public string Key { get; set; }

        [Required]
        public string Language { get; set; }

        public string Title { get; set; }

        public DateTime CreatedTime { get; set; }

        public string Author { get; set; }
        
        public string Content { get; set; }
    }
}