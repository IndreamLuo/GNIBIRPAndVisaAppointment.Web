using StructureMap;

namespace GNIBIRPAndVisaAppointment.Web.Utility
{
    public class StructureMapRegistry : Registry
    {
        public StructureMapRegistry()
        {
            For<IGlobalCache>().Singleton().Use<GlobalCache>();
            For<ICurrentObjectCache>().Transient().Use<CurrentObjectCache>();
            For<reCaptchaHelper>().Use<reCaptchaHelper>();
        }
    }
}