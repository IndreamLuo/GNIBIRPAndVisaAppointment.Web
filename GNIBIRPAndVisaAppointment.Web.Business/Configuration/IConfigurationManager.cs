namespace GNIBIRPAndVisaAppointment.Web.Business.Configuration
{
    public interface IConfigurationManager : IDomain
    {
        string this[string area, string key] { get; set; }
    }
}