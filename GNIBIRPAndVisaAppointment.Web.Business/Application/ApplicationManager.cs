using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GNIBIRPAndVisaAppointment.Web.Business.Configuration;
using GNIBIRPAndVisaAppointment.Web.Business.Payment;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Storage;
using GNIBIRPAndVisaAppointment.Web.Utility;

namespace GNIBIRPAndVisaAppointment.Web.Business.Application
{
    public class ApplicationManager : IApplicationManager
    {
        Table<DataAccess.Model.Storage.Application> ApplicationTable;
        Table<Order> OrderTable;
        Table<Assignment> AssignmentTable;
        Table<AppointmentLetter> AppointmentLetterTable;
        Table<AppointLog> AppointLogTable;
        IDomainHub DomainHub;

        public ApplicationManager(IStorageProvider storageProvider, IDomainHub domainHub)
        {
            ApplicationTable = storageProvider.GetTable<DataAccess.Model.Storage.Application>();
            OrderTable = storageProvider.GetTable<Order>();
            AssignmentTable = storageProvider.GetTable<Assignment>();
            AppointmentLetterTable = storageProvider.GetTable<AppointmentLetter>();
            AppointLogTable = storageProvider.GetTable<AppointLog>();
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
                application.Time = DateTime.Now;
                
                application.PartitionKey = application.Id;
                application.RowKey = New;

                ApplicationTable.Insert(application);

                return application.Id;
            }
        }
        
        public string CreateOrder(DataAccess.Model.Storage.Order order)
        {
            order.Id = order.ApplicationId;
            order.Time = DateTime.Now;
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
            if (GetAssignment(orderId) != null)
            {
                return;
            }

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

        public void Appoint(string orderId)
        {
            UpdataAssignmentStatus(orderId, AssignmentStatus.Accepted, AssignmentStatus.Appointed);
        }

        public void Reaccept(string orderId)
        {
            var assignment = GetAssignment(orderId);

            if (assignment.Status == AssignmentStatus.Appointed || assignment.Status == AssignmentStatus.Duplicated)
            {
                UpdataAssignmentStatus(orderId, assignment.Status, AssignmentStatus.Accepted);
            }
        }

        public void Duplicate(string orderId)
        {
            UpdataAssignmentStatus(orderId, AssignmentStatus.Accepted, AssignmentStatus.Duplicated);
        }

        public void Cancel(string orderId)
        {
            var assignment = GetAssignment(orderId);
            if (assignment.Status == AssignmentStatus.Appointed || assignment.Status == AssignmentStatus.Duplicated)
            {
                UpdataAssignmentStatus(assignment.Id, assignment.Status, AssignmentStatus.Cancelled);
            }
        }

        public void Complete(string orderId, string appointmentNo, DateTime time, string name, string category, string subCategory)
        {
            AppointmentLetterTable.Insert(new AppointmentLetter
            {
                PartitionKey = orderId,
                RowKey = appointmentNo,
                AppointmentNo = appointmentNo,
                Time = time,
                Name = name,
                Category = category,
                SubCategory = subCategory
            });

            UpdataAssignmentStatus(orderId, AssignmentStatus.Appointed, AssignmentStatus.Complete);
        }

        public void Close(string orderId)
        {
            UpdataAssignmentStatus(orderId, AssignmentStatus.Complete, AssignmentStatus.Closed);
        }

        protected void UpdataAssignmentStatus(string orderId, string fromStatus, string toStatus, string assignmentNo = null)
        {
            var assignment = AssignmentTable[fromStatus, orderId];
            AssignmentTable.Delete(assignment);

            assignment.ETag = "*";
            assignment.Status = toStatus;
            assignment.PartitionKey = assignment.Status;

            if (!string.IsNullOrEmpty(assignmentNo))
            {
                assignment.AppointmentNo = assignmentNo;
            }

            AssignmentTable.Insert(assignment);

            var trackedAssignment = GetAssignment(orderId);
            trackedAssignment.Status = toStatus;

            if (!string.IsNullOrEmpty(assignmentNo))
            {
                trackedAssignment.AppointmentNo = assignmentNo;
            }
            AssignmentTable.Replace(trackedAssignment);

            if (fromStatus == AssignmentStatus.Accepted || toStatus == AssignmentStatus.Accepted)
            {
                NotifyWorkerUpdateAsync();
            }
        }

        public Assignment GetAssignment(string orderId)
        {
            return AssignmentTable[orderId, AssignmentStatus.Tracked];
        }

        public List<Assignment> GetAssignments(string status, bool withDetails = false)
        {
            var assignments = AssignmentTable[status];

            if (withDetails)
            {
                foreach (var assignment in assignments)
                {
                    assignment.Application = ApplicationTable[assignment.Id, New];
                    assignment.Order = OrderTable[assignment.Id, Application];
                    assignment.AppointmentLetter = GetAppointmentLetter(assignment.Id);
                }
            }

            return assignments;
        }

        public AppointmentLetter GetAppointmentLetter(string orderId)
        {
            return AppointmentLetterTable[orderId].FirstOrDefault();
        }

        public void AppointLog(string orderId, bool success, string result)
        {
            AppointLogTable.Insert(new AppointLog
            {
                PartitionKey = orderId,
                RowKey = success ? "Success" : "Failed",
                Id = orderId,
                Success = success,
                Result = result
            });
        }

        public async Task NotifyWorkerUpdateAsync()
        {
            using (var httpClient = new HttpClient())
            {
                var configurationManager = DomainHub.GetDomain<IConfigurationManager>();
                var notifyWorkerUpdateApi = configurationManager["API", "NotifyWorkerUpdate"];
                var token = configurationManager["API", "Token"];

                await httpClient.PostAsync(notifyWorkerUpdateApi, new FormUrlEncodedContent(new []
                {
                    new KeyValuePair<string, string>("token", token)
                }));
            }
        }
    }
}