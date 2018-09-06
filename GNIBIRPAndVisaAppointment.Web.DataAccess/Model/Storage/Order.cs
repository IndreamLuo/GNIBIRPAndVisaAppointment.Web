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
        public double SelectFrom { get; set; }
        public string From { get; set; }
        public double SelectTo { get; set; }
        public string To { get; set; }
        public double Rebook { get; set; }
        public double NoCancelRebook { get; set; }
        public double Emergency { get; set; }
        public double Amount { get; set; }
    }
}