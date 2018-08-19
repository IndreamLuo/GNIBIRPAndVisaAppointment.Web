using Microsoft.WindowsAzure.Storage.Table;

namespace GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage
{
    public class Order : TableEntity
    {
        public OrderStatus Status { get; set; }
        public string Currency { get; set; }
        public decimal Price { get; set; }
        public string PaymentId { get; set; }
        public string AppointmentType { get; set; }
        public string ApplicationPartitionKey { get; set; }
        public string ApplicationRowKey { get; set; }

        public enum OrderStatus
        {
            New,
            Waiting,
            Paid,
            Finished
        }
    }
}