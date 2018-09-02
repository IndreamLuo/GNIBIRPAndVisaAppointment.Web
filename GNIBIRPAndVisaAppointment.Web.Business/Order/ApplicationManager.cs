using System;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Storage;

namespace GNIBIRPAndVisaAppointment.Web.Business.Information
{
    public class ApplicationManager : IApplicationManager
    {
        Table<Application> ApplicationTable;

        public ApplicationManager(IStorageProvider storageProvider)
        {
            ApplicationTable = storageProvider.GetTable<Application>();
        }

        public string CreateApplication(Application application)
        {
            application.PartitionKey = DateTime.UtcNow.AddHours(1).Ticks.ToString();
            application.RowKey = "New";

            ApplicationTable.Insert(application);

            return application.PartitionKey;
        }
    }
}