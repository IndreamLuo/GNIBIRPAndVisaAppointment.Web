using GNIBIRPAndVisaAppointment.Web.DataAccess.Storage;
using StructureMap;

namespace GNIBIRPAndVisaAppointment.Web.DataAccess
{
    public class StructureMapRegistry : Registry
    {
        public StructureMapRegistry()
        {
            For<ITableProvider>().Singleton().Use<TableProvider>();
        }
    }
}