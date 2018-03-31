using GNIBIRPAndVisaAppointment.Web.DataAccess.Storage;
using GNIBIRPAndVisaAppointment.Web.Utility;

namespace GNIBIRPAndVisaAppointment.Web.Business.Configuration
{
    internal class ConfigurationManager : IConfigurationManager
    {
        readonly Table<DataAccess.Model.Storage.Configuration> Table;
        
        public ConfigurationManager(ITableProvider tableProvider)
        {
            Table = tableProvider.Configuration;
        }
        
        public string this[string area, string key]
        {
            get => Table[area, key].Value;
            set
            {
                var configuration = Table[area, key];
                configuration.Value = value;
                Table.Update(configuration);
            }
        }
    }
}