using System;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage;

namespace GNIBIRPAndVisaAppointment.Web.Models
{
    public class AppointmentModel
    {
        public AppointmentModel(Appointment entity)
        {
            Type = entity.Type;
            Category = entity.Category;
            SubCategory = entity.SubCategory;
            Time = entity.Time;
            Expiration = entity.Expiration;
            Published = entity.Published;
            Appointed = entity.Appointed;
        }

        public string Type { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public DateTime Time { get; set; }
        public DateTime? Expiration { get; set; }
        public DateTime Published { get; set; }
        public DateTime? Appointed { get; set; }
    }
}