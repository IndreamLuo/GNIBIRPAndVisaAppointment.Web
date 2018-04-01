using GNIBIRPAndVisaAppointment.Web.Business.Configuration;
using GNIBIRPAndVisaAppointment.Web.Business.Information;
using StructureMap;

namespace GNIBIRPAndVisaAppointment.Web.Business
{
    public class StructureMapRegistry : Registry
    {
        public StructureMapRegistry()
        {
            For<IDomainHub>().Singleton().Use<DomainHub>();
            For<IConfigurationManager>().Singleton().Use<ConfigurationManager>();
            For<IInformationManager>().Singleton().Use<InformationManager>();
        }
    }
}