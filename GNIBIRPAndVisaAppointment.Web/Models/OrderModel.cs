using System;
using System.ComponentModel.DataAnnotations;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage;

namespace GNIBIRPAndVisaAppointment.Web.Models
{
    public class OrderModel
    {
        public OrderModel() { }
        
        public OrderModel(Order entity)
        {
            
        }
        public string Id { get; set; }
        
        [Required]
        public string ApplicationId { get; set; }
        public string Type { get { return "Application"; } }
        public bool PickDate { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public bool Emergency { get; set; }
        public bool AnyCategory { get; set; }
        public string Mobile { get; set; }
        public double Special { get; set; }
        public string Comment { get; set; }
    }
}