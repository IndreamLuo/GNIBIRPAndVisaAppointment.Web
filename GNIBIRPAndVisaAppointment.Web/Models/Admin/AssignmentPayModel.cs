using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GNIBIRPAndVisaAppointment.Web.Models.Admin
{
    public class AssignmentPayModel
    {
        [Required]
        public string AssignmentId { get; set; }
        
        [Required]
        [RegularExpression("[^ ]*")]
        public string ChargeId { get; set; }
        
        [Required]
        public string Type { get; set; }

        [Required]
        public string Currency { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required]
        public string Payer { get; set; }
    }
}