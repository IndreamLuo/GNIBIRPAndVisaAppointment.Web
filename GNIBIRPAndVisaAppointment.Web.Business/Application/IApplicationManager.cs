namespace GNIBIRPAndVisaAppointment.Web.Business.Application
{
    public interface IApplicationManager : IDomain
    {
        string CreateApplication(DataAccess.Model.Storage.Application application);
    }
}