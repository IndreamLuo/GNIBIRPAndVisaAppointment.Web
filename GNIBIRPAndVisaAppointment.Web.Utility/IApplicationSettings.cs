namespace GNIBIRPAndVisaAppointment.Web.Utility
{
    public interface IApplicationSettings
    {
        string this[string key] { get; }
    }
}