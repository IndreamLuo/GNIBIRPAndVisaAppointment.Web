using GNIBIRPAndVisaAppointment.Web.Business.Configuration;
using StructureMap;

namespace GNIBIRPAndVisaAppointment.Web.Business
{
    public class StructureMapRegistry : Registry
    {
        public StructureMapRegistry()
        {
            For<IDomainHub>().Singleton().Use<DomainHub>();
            For<IConfigurationManager>().Singleton().Use<ConfigurationManager>();
        }
    }
}