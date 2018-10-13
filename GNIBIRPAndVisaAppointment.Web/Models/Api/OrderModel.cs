using System;
using System.ComponentModel.DataAnnotations;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage;

namespace GNIBIRPAndVisaAppointment.Web.Models.Api
{
    public class OrderModel
    {
        public OrderModel() { }
        
        public OrderModel(Order entity)
        {
            From = entity.From;
            To = entity.To;
            Emergency = entity.Emergency > 0;
            AnyCategory = entity.AnyCategory != 0;
        }

        public string From { get; set; }
        public string To { get; set; }
        public bool Emergency { get; set; }
        public bool AnyCategory { get; set; }
    }
}