using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GNIBIRPAndVisaAppointment.Web.Models.Admin
{
    public class AppointmentLetterModel
    {
        [Required]
        public string AssignmentId { get; set; }

        [Required]
        [RegularExpression("EXTW-.{9}")]
        public string AppointmentNo { get; set; }
        
        [Required]
        [RegularExpression("[0-9]{2}/[0-9]{2}/[0-9]{4}, [0-9]{2}:[0-9]{2}")]
        public string Time { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Category { get; set; }
        
        [Required]
        public string SubCategory { get; set; }

        public string Content { get; set; }
    }
}