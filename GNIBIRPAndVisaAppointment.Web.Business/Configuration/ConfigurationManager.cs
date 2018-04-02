using GNIBIRPAndVisaAppointment.Web.DataAccess.Storage;
using GNIBIRPAndVisaAppointment.Web.Utility;

namespace GNIBIRPAndVisaAppointment.Web.Business.Configuration
{
    internal class ConfigurationManager : SingleTableDomainBase<DataAccess.Model.Storage.Configuration>, IConfigurationManager
    {        
        public ConfigurationManager(IStorageProvider storageProvider) : base(storageProvider) { }
        
        public string this[string area, string key]
        {
            get => Table[area, key].Value;
            set
            {
                var configuration = Table[area, key];
                configuration.Value = value;
                Table.Replace(configuration);
            }
        }
    }
}