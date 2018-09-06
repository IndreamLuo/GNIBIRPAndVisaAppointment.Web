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
        public bool SelectFrom { get; set; }
        public string From { get; set; }
        public bool SelectTo { get; set; }
        public string To { get; set; }
        public bool Rebook { get; set; }
        public bool NoCancelRebook { get; set; }
        public bool Emergency { get; set; }
    }
}