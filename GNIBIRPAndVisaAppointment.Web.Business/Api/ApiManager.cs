using GNIBIRPAndVisaAppointment.Web.Business.Configuration;

namespace GNIBIRPAndVisaAppointment.Web.Business.Api
{
    public class ApiManager : IApiManager
    {
        readonly IDomainHub DomainHub;

        public ApiManager(IDomainHub domainHub)
        {
            DomainHub = domainHub;
        }

        const string API = "API";
        const string Token = "Token";

        public bool VerifyToken(string token)
        {
            var configurationManager = DomainHub.GetDomain<IConfigurationManager>();
            var setToken = configurationManager[API, Token];

            return token == setToken;
        }
    }
}