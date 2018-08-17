using System;

namespace GNIBIRPAndVisaAppointment.Web.Business.Appointment
{
    public interface IAppointmentManager : IDomain
    {
        DataAccess.Model.Storage.Appointment[] GetLastAppointments();
        DataAccess.Model.Storage.AppointmentStatistics[] GetStatistics();
        DataAccess.Model.Storage.AppointmentStatistics[] GetStatistics(DateTime from);
        DataAccess.Model.Storage.AppointmentStatistics[] GetStatistics(DateTime from, DateTime to);
    }
}