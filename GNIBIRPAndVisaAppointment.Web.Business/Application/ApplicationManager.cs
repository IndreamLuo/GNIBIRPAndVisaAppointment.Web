using System;
using GNIBIRPAndVisaAppointment.Web.Business.Payment;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Storage;

namespace GNIBIRPAndVisaAppointment.Web.Business.Application
{
    public class ApplicationManager : IApplicationManager
    {
        Table<DataAccess.Model.Storage.Application> ApplicationTable;
        Table<DataAccess.Model.Storage.Order> OrderTable;
        IDomainHub DomainHub;

        public ApplicationManager(IStorageProvider storageProvider, IDomainHub domainHub)
        {
            ApplicationTable = storageProvider.GetTable<DataAccess.Model.Storage.Application>();
            OrderTable = storageProvider.GetTable<DataAccess.Model.Storage.Order>();
            DomainHub = domainHub;
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
            lock(ApplicationTable)
            {
                application.Id = Guid.NewGuid().ToString();
                application.Time = DateTime.UtcNow.AddHours(1);
                
                application.PartitionKey = application.Id;
                application.RowKey = New;

                ApplicationTable.Insert(application);

                return application.Id;
            }
        }
        
        public string CreateOrder(DataAccess.Model.Storage.Order order)
        {
            order.Id = order.ApplicationId;
            order.Time = DateTime.UtcNow.AddHours(1);
            order.Type = Application;
            
            order.PartitionKey = order.Id;
            order.RowKey = Application;

            order.Amount = order.Base
            + order.PickDate
            + order.Rebook
            + order.NoCancelRebook
            + order.Emergency;

            if (GetOrder(order.Id) == null)
            {
                OrderTable.Insert(order);
            }
            else
            {
                OrderTable.Replace(order);
            }

            return order.Id;
        }

        public DataAccess.Model.Storage.Order GetOrder(string orderId)
        {
            return OrderTable[orderId, Application];
        }

        public void Pending(string orderId)
        {
            var paymentManager = DomainHub.GetDomain<IPaymentManager>();
            if (paymentManager.IsPaid(orderId))
            {
                
            }

            throw new NotImplementedException();
        }

        public void Accept(string orderId)
        {
            throw new NotImplementedException();
        }

        public void Complete(string orderId, string appointmentNo)
        {
            throw new NotImplementedException();
        }

        public void Close(string orderId)
        {
            throw new NotImplementedException();
        }
    }
}