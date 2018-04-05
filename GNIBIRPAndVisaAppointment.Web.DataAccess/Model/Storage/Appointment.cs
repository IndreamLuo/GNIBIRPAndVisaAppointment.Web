using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage
{
    public class Appointment : TableEntity
    {
        public string Type { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public DateTime Time { get; set; }
        public DateTime? Expiration { get; set; }
        public DateTime Published { get; set; }
        public DateTime? Appointed { get; set; }

        public static class Types
        {
            public const string IRP = "IRP";
            public const string Visa = "Visa";
        }

        public static class Categories
        {
            public const string Work = "Work";
            public const string Study = "Study";
            public const string Other = "Other";
            public const string Individual = "Individual";
            public const string Family = "Family";
            public const string Emergency = "Emergency";
        }

        public static class SubCategories
        {
            public const string New = "New";
            public const string Renewal = "Renewal";
        }
    }
}