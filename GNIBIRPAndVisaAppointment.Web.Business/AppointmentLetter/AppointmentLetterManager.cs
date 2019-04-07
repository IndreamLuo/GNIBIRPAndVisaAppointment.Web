using System;
using System.Collections.Generic;
using System.Linq;
using GNIBIRPAndVisaAppointment.Web.Business.Application;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Storage;

namespace GNIBIRPAndVisaAppointment.Web.Business.AppointmnetLetter
{
    public class AppointmentLetterManager : IAppointmentLetterManager
    {
        readonly IDomainHub DomainHub;

        readonly Table<AppointmentLetter> AppointmentLetterTable;
        
        public AppointmentLetterManager(IStorageProvider storageProvider, IDomainHub domainHub)
        {
            AppointmentLetterTable = storageProvider.GetTable<AppointmentLetter>();
            DomainHub = domainHub;
        }

        const string Unassigned = "Unassigned";

        static List<AppointmentLetter> Cache = null;

        void EnsureCacheLoaded()
        {
            if (Cache == null)
            {
                Cache = AppointmentLetterTable[Unassigned];
            }
        }

        public AppointmentLetter this[string emailId]
        {
            get
            {
                EnsureCacheLoaded();
                return Cache.FirstOrDefault(letter => letter.EmailId == emailId);
            }
        }

        public List<AppointmentLetter> UnassignedLetters
        {
            get
            {
                EnsureCacheLoaded();
                return Cache;
            }
        }

        public void SubmitLetter(string id, string appointmentNo, string name, DateTime time, string category, string subCategory)
        {
            var appointmentLetter = AppointmentLetterTable[Unassigned, id];

            if (appointmentLetter == null)
            {
                appointmentLetter = new AppointmentLetter
                {
                    PartitionKey = Unassigned,
                    RowKey = id,
                    EmailId = id,
                    AppointmentNo = appointmentNo,
                    Name = name,
                    Time = time,
                    Category = category,
                    SubCategory = subCategory
                };

                AppointmentLetterTable.Insert(appointmentLetter);

                EnsureCacheLoaded();
                Cache.Add(appointmentLetter);
            }
        }

        public AppointmentLetter[] FindByName(string name)
        {
            EnsureCacheLoaded();
            return Cache.Where(appointmentLetter => appointmentLetter.Name == name).ToArray();
        }

        public void Assign(string id, string applicationId)
        {
            var appointmentLetter = AppointmentLetterTable[Unassigned, id];

            var applicationManager = DomainHub.GetDomain<IApplicationManager>();

            applicationManager.Complete(applicationId,
                appointmentLetter.AppointmentNo,
                appointmentLetter.Time,
                appointmentLetter.Name,
                appointmentLetter.Category,
                appointmentLetter.SubCategory);

            AppointmentLetterTable.Delete(appointmentLetter);
            
            EnsureCacheLoaded();
            var cached = Cache.FirstOrDefault(letter => letter.EmailId == id);
            if (cached != null)
            {
                Cache.Remove(cached);
            }
        }
    }
}