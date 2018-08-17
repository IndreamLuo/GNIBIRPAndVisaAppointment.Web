using System;
using System.Linq;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Storage;

namespace GNIBIRPAndVisaAppointment.Web.Business.Appointment
{
    public class AppointmentManager : IAppointmentManager
    {
        readonly Table<DataAccess.Model.Storage.Appointment> AppointmentTable;
        readonly Table<DataAccess.Model.Storage.Appointment> LastAppointmentTable;
        readonly Table<DataAccess.Model.Storage.AppointmentStatistics> AppointmentStatistics;

        public AppointmentManager(IStorageProvider storageProvider)
        {
            AppointmentTable = storageProvider.GetTable<DataAccess.Model.Storage.Appointment>();
            LastAppointmentTable = storageProvider.GetTable<DataAccess.Model.Storage.Appointment>("LastAppointment");
            AppointmentStatistics = storageProvider.GetTable<DataAccess.Model.Storage.AppointmentStatistics>();
        }

        public DataAccess.Model.Storage.Appointment[] GetLastAppointments()
        {
            return LastAppointmentTable.GetAll().ToArray();
        }

        public AppointmentStatistics[] GetStatistics()
        {
            return GetStatistics(DateTime.Now.AddDays(-1).Date);
        }

        public AppointmentStatistics[] GetStatistics(DateTime from)
        {
            return GetStatistics(from, DateTime.Now);
        }

        public AppointmentStatistics[] GetStatistics(DateTime from, DateTime to)
        {
            var datesToRetrieve = Enumerable
                .Range(0, (int)(to.Date - from.Date).TotalDays + 1)
                .Select(index => from.Date.AddDays(index));

            var statisticses = datesToRetrieve
                .Select(date => AppointmentStatistics[$"{date.ToString("yyyyMMdd-24")}h"])
                .SelectMany(statistics => statistics)
                .ToArray();

            return statisticses;
        }
    }
}