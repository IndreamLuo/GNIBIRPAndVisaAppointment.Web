using System.ComponentModel.DataAnnotations;

namespace GNIBIRPAndVisaAppointment.Web.Models.Application
{
    public class ChangeGNIBModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public bool HasGNIB { get; set; }

        public string GNIBNo { get; set; }

        public string GNIBExDT { get; set; }

        public bool IsInitialazed { get; set; }
    }
}