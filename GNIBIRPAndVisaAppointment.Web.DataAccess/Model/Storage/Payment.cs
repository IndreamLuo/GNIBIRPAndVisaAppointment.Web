using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage
{
    public class Payment : TableEntity
    {
        public string Id { get; set; }
        public string ChargeId { get; set; }
        public DateTime Time { get; set; }
        public string Type { get; set; }
        public string Currency { get; set; }
        public double Amount { get; set; }
        public string Payer { get; set; }
        public string Status { get; set; }
    }
}