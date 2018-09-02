using GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage;

namespace GNIBIRPAndVisaAppointment.Web.Business.Information
{
    public interface IApplicationManager : IDomain
    {
        string CreateApplication(Application application);
    }
}