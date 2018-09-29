namespace GNIBIRPAndVisaAppointment.Web.Business.Api
{
    public interface IApiManager : IDomain
    {
        bool VerifyToken(string token);
    }
}