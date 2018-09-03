using System;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Storage;

namespace GNIBIRPAndVisaAppointment.Web.Business.Application
{
    public class ApplicationManager : IApplicationManager
    {
        Table<DataAccess.Model.Storage.Application> ApplicationTable;

        public ApplicationManager(IStorageProvider storageProvider)
        {
            ApplicationTable = storageProvider.GetTable<DataAccess.Model.Storage.Application>();
        }

        public string CreateApplication(DataAccess.Model.Storage.Application application)
        {
            application.PartitionKey = DateTime.UtcNow.AddHours(1).Ticks.ToString();
            application.RowKey = "New";

            ApplicationTable.Insert(application);

            return application.PartitionKey;
        }
    }
}