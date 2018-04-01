using System;

namespace GNIBIRPAndVisaAppointment.Web.Models
{
    public class InformationModel
    {
        public string Key { get; set; }
        public string Language { get; set; }
        public string Title { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
    }
}