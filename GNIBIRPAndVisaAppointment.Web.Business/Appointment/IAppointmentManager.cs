namespace GNIBIRPAndVisaAppointment.Web.Business.Appointment
{
    public interface IAppointmentManager : IDomain
    {
        DataAccess.Model.Storage.Appointment[] GetLastAppointments();
    }
}