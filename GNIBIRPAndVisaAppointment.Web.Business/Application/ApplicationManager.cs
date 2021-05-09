using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GNIBIRPAndVisaAppointment.Web.Business.AppointmnetLetter;
using GNIBIRPAndVisaAppointment.Web.Business.Configuration;
using GNIBIRPAndVisaAppointment.Web.Business.Email;
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

            CachedAssignments = new Dictionary<string, List<Assignment>>();
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

        public void ChangeGNIB(string id, bool hasGNIB, string gnibNo, string gnibExDT)
        {
            var application = this[id];
            
            var changed = application.ConfirmGNIB != (hasGNIB ? "Renewal" : "New")
                || application.GNIBNo != gnibNo
                || application.GNIBExDT != gnibExDT;

            application.ETag = "*";
            application.ConfirmGNIB = hasGNIB ? "Renewal" : "New";
            application.GNIBNo = gnibNo;
            application.GNIBExDT = gnibExDT;

            ApplicationTable.Replace(application);

            var assignment = GetAssignment(id);
            if (changed && assignment.Status == AssignmentStatus.Accepted)
            {
                NotifyWorkerUpdateAsync();
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
            //+ order.AnyCategory
            + order.Emergency
            + order.Special;

            if (GetOrder(order.Id) == null)
            {
                OrderTable.Insert(order);
            }
            else
            {
                OrderTable.Replace(order);
            }

            Task.Run(async () =>
            {
                await Task.Delay(5 * 60 * 1000);
                this.AutoAccept(order.Id);
            });

            return order.Id;
        }

        public DataAccess.Model.Storage.Order GetOrder(string orderId)
        {
            return OrderTable[orderId, Application];
        }

        public void Pending(string orderId)
        {
            var currentAssignment = GetAssignment(orderId);
            if (currentAssignment != null)
            {
                if (currentAssignment.Status == AssignmentStatus.Accepted)
                {
                    this.UpdataAssignmentStatus(orderId, currentAssignment.Status, AssignmentStatus.Pending);
                }
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

            var emailApplication = DomainHub.GetDomain<IEmailApplication>();
            emailApplication.NotifyApplicationChangedAsync(orderId, null, assignment.Status).Wait();
        }

        public void AutoAccept(string orderId)
        {
            var assignment = this.GetAssignment(orderId);
            if (assignment.Status != AssignmentStatus.Pending)
            {
                return;
            }

            var application = this[orderId];
            
            var configurationManager = DomainHub.GetDomain<IConfigurationManager>();
            var acceptedDaysString = application.ConfirmGNIB == "Renewal"
            ? configurationManager["Appointment", "AutoAcceptRenewalDays"]
            : configurationManager["Appointment", "AutoAcceptNewDays"];

            if (string.IsNullOrEmpty(acceptedDaysString))
            {
                return;
            }

            var order = this.GetOrder(orderId);
            var acceptedDays = Convert.ToInt32(acceptedDaysString);

            var to = string.IsNullOrEmpty(order.To)
            ? DateTime.MaxValue
            : DateTime.ParseExact(order.To, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

            var daysFromNow = (to - DateTime.Now.Date).TotalDays;

            if (daysFromNow >= acceptedDays)
            {
                this.Accept(orderId);
            }
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
            var assignment = GetAssignment(orderId);

            if (assignment.Status == AssignmentStatus.Accepted || assignment.Status == AssignmentStatus.Duplicated)
            {
                UpdataAssignmentStatus(orderId, assignment.Status, AssignmentStatus.Appointed);
            }
        }

        public void Reaccept(string orderId)
        {
            var assignment = GetAssignment(orderId);

            if (assignment.Status == AssignmentStatus.Accepted || assignment.Status == AssignmentStatus.Appointed || assignment.Status == AssignmentStatus.Duplicated)
            {
                UpdataAssignmentStatus(orderId, assignment.Status, AssignmentStatus.Accepted);
            }
        }

        public void Duplicate(string orderId)
        {
            UpdataAssignmentStatus(orderId, AssignmentStatus.Accepted, AssignmentStatus.Duplicated);
        }

        public void Unverify(string orderId)
        {
            UpdataAssignmentStatus(orderId, AssignmentStatus.Accepted, AssignmentStatus.Unverify);
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
                RowKey = appointmentNo.Replace("\r", ""),
                AppointmentNo = appointmentNo.Replace("\r", ""),
                Time = TimeZoneInfo.ConvertTimeFromUtc(time, Localization.DublinTimeZoneInfo),
                Name = name.Replace("\r", ""),
                Category = category,
                SubCategory = subCategory
            });

            UpdataAssignmentStatus(orderId, AssignmentStatus.Appointed, AssignmentStatus.Complete);
        }

        public void Complete(string orderId, string emailId)
        {
            var appointmentLetterManager = DomainHub.GetDomain<IAppointmentLetterManager>();
            appointmentLetterManager.Assign(emailId, orderId);
        }

        public void Close(string orderId)
        {
            UpdataAssignmentStatus(orderId, AssignmentStatus.Complete, AssignmentStatus.Closed);
        }

        static Dictionary<string, DateTime> UpdateAssignmentBuffer = new Dictionary<string, DateTime>();
        const int UpdateTaskDelaySeconds = 2;

        protected async Task UpdataAssignmentStatus(string orderId, string fromStatus, string toStatus)
        {
            lock(UpdateAssignmentBuffer)
            {
                if (UpdateAssignmentBuffer.ContainsKey(orderId) && (DateTime.Now - UpdateAssignmentBuffer[orderId]).TotalSeconds < UpdateTaskDelaySeconds)
                {
                    Task.Run(async () =>
                    {
                        await Task.Delay(UpdateTaskDelaySeconds * 1000);
                        UpdataAssignmentStatus(orderId, fromStatus, toStatus);
                    }).Start();
                    
                    return;
                }

                UpdateAssignmentBuffer[orderId] = DateTime.Now;
            }

            var assignment = AssignmentTable[fromStatus, orderId];

            if (assignment != null)
            {
                AssignmentTable.Delete(assignment);

                assignment.ETag = "*";
                assignment.Status = toStatus;
                assignment.PartitionKey = assignment.Status;

                AssignmentTable.Insert(assignment);

                var trackedAssignment = GetAssignment(orderId);
                trackedAssignment.Status = toStatus;
                
                AssignmentTable.Replace(trackedAssignment);

                if (fromStatus == AssignmentStatus.Accepted || toStatus == AssignmentStatus.Accepted)
                {
                    NotifyWorkerUpdateAsync();
                }

                var emailApplication = DomainHub.GetDomain<IEmailApplication>();
                emailApplication.NotifyApplicationChangedAsync(orderId, fromStatus, assignment.Status);
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

            CachedAssignments[status] = assignments;

            return assignments;
        }

        public Dictionary<string, List<Assignment>> CachedAssignments { get; private set; }

        static DateTime TimeZoneAdjusted = new DateTime(2018, 10, 22);
        public AppointmentLetter GetAppointmentLetter(string orderId)
        {
            return AppointmentLetterTable[orderId].FirstOrDefault();
        }

        public void AppointLog(string orderId, string slot, bool success, string result, double timeSpan)
        {
            AppointLogTable.Insert(new AppointLog
            {
                PartitionKey = orderId,
                RowKey = $"{slot}-{DateTime.UtcNow.Ticks}",
                Id = orderId,
                Success = success,
                Result = result,
                TimeSpan = timeSpan
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