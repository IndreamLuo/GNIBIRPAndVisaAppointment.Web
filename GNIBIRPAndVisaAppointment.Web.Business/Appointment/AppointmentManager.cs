using System.Linq;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Storage;

namespace GNIBIRPAndVisaAppointment.Web.Business.Appointment
{
    public class AppointmentManager : IAppointmentManager
    {
        readonly Table<DataAccess.Model.Storage.Appointment> AppointmentTable;
        readonly Table<DataAccess.Model.Storage.Appointment> LastAppointmentTable;

        public AppointmentManager(IStorageProvider storageProvider)
        {
            AppointmentTable = storageProvider.GetTable<DataAccess.Model.Storage.Appointment>();
            LastAppointmentTable = storageProvider.GetTable<DataAccess.Model.Storage.Appointment>("LastAppointment");
        }

        public DataAccess.Model.Storage.Appointment[] GetLastAppointments()
        {
            return LastAppointmentTable.GetAll().ToArray();
        }
    }
}