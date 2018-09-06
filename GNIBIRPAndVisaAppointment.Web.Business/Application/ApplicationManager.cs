using System;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Storage;

namespace GNIBIRPAndVisaAppointment.Web.Business.Application
{
    public class ApplicationManager : IApplicationManager
    {
        Table<DataAccess.Model.Storage.Application> ApplicationTable;
        Table<DataAccess.Model.Storage.Order> OrderTable;

        public ApplicationManager(IStorageProvider storageProvider)
        {
            ApplicationTable = storageProvider.GetTable<DataAccess.Model.Storage.Application>();
            OrderTable = storageProvider.GetTable<DataAccess.Model.Storage.Order>();
        }

        const string New = "New";
        const string Application = "Application";

        public DataAccess.Model.Storage.Application this[string applicationId]
        {
            get
            {
                return ApplicationTable[applicationId, New];
            }
        }

        public string CreateApplication(DataAccess.Model.Storage.Application application)
        {
            application.Id = Guid.NewGuid().ToString();
            application.Time = DateTime.UtcNow.AddHours(1);
            
            application.PartitionKey = application.Id;
            application.RowKey = New;

            ApplicationTable.Insert(application);

            return application.Id;
        }
        
        public string CreateOrder(DataAccess.Model.Storage.Order order)
        {
            order.Id = order.ApplicationId;
            order.Time = DateTime.UtcNow.AddHours(1);
            order.Type = Application;
            
            order.PartitionKey = order.Id;
            order.RowKey = Application;

            order.Amount = order.Base
            + order.SelectFrom
            + order.SelectTo
            + order.Rebook
            + order.NoCancelRebook
            + order.Emergency;

            OrderTable.Replace(order);

            return order.Id;
        }

        public DataAccess.Model.Storage.Order GetOrder(string orderId)
        {
            return OrderTable[orderId, Application];
        }
    }
}