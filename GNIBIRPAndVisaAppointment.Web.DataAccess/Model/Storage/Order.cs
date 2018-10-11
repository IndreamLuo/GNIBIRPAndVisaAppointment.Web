using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage
{
    public class Order : TableEntity
    {
        public string Id { get; set; }
        public DateTime Time { get; set; }
        public string ApplicationId { get; set; }
        public string Type { get; set; }
        public double Base { get; set; }
        public double PickDate { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public double Emergency { get; set; }
        public double AnyCategory { get; set; }
        public double Special { get; set; }
        public string Comment { get; set; }
        public double Amount { get; set; }
    }
}