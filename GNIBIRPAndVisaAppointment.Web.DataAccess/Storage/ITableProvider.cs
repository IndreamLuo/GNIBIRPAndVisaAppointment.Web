using GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage;

namespace GNIBIRPAndVisaAppointment.Web.DataAccess.Storage
{
    public interface ITableProvider
    {
        Table<Configuration> Configuration { get; }
    }
}