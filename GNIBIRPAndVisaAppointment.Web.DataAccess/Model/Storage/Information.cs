using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage
{
    public class Information : TableEntity
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Content { get; set; }
        public bool FacebookComment { get; set; }
    }
}