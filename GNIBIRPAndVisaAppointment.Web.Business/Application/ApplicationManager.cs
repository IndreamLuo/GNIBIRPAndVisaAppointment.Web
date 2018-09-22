using System;
using System.Collections.Generic;
using GNIBIRPAndVisaAppointment.Web.Business.Payment;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Storage;

namespace GNIBIRPAndVisaAppointment.Web.Business.Application
{
    public class ApplicationManager : IApplicationManager
    {
        Table<DataAccess.Model.Storage.Application> ApplicationTable;
        Table<DataAccess.Model.Storage.Order> OrderTable;
        Table<DataAccess.Model.Storage.Assignment> AssignmentTable;
        IDomainHub DomainHub;

        public ApplicationManager(IStorageProvider storageProvider, IDomainHub domainHub)
        {
            ApplicationTable = storageProvider.GetTable<DataAccess.Model.Storage.Application>();
            OrderTable = storageProvider.GetTable<DataAccess.Model.Storage.Order>();
            AssignmentTable = storageProvider.GetTable<DataAccess.Model.Storage.Assignment>();
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
            
            var assignment = new Assignment
            {
                Id = orderId,
                Time = DateTime.Now,
                Status = AssignmentStatus.Pending
            };

            assignment.PartitionKey = assignment.Status;
            assignment.RowKey = assignment.Id;

            AssignmentTable.Insert(assignment);

            assignment.PartitionKey = assignment.RowKey;
            assignment.RowKey = AssignmentStatus.Tracked;
            AssignmentTable.Insert(assignment);
        }

        public void Accept(string orderId)
        {
            UpdataAssignmentStatus(orderId, AssignmentStatus.Pending, AssignmentStatus.Accepted);
        }

        public void Reject(string orderId)
        {
            UpdataAssignmentStatus(orderId, AssignmentStatus.Pending, AssignmentStatus.Rejected);
        }

        public void Complete(string orderId, string appointmentNo)
        {
            UpdataAssignmentStatus(orderId, AssignmentStatus.Accepted, AssignmentStatus.Complete);
        }

        public void Close(string orderId)
        {
            UpdataAssignmentStatus(orderId, AssignmentStatus.Complete, AssignmentStatus.Closed);
        }

        protected void UpdataAssignmentStatus(string orderId, string fromStatus, string toStatus)
        {
            var assignment = AssignmentTable[fromStatus, orderId];
            AssignmentTable.Delete(assignment);

            assignment.ETag = "*";
            assignment.Status = toStatus;
            assignment.PartitionKey = assignment.Status;
            AssignmentTable.Insert(assignment);

            var trackedAssignment = GetAssignment(orderId);
            trackedAssignment.Status = toStatus;
            AssignmentTable.Replace(trackedAssignment);
        }

        public Assignment GetAssignment(string orderId)
        {
            return AssignmentTable[orderId, AssignmentStatus.Tracked];
        }

        public List<Assignment> GetAssignments(string status)
        {
            return AssignmentTable[status];
        }
    }
}