using System.Collections.Generic;
using System.Linq;
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

                if (configuration == null)
                {
                    configuration = new DataAccess.Model.Storage.Configuration
                    {
                        PartitionKey = area,
                        RowKey = key,
                        Value = value
                    };

                    Table.Insert(configuration);
                }
                else
                {
                    configuration.Value = value;
                    Table.Replace(configuration);
                }
            }
        }

        public void Remove(string area, string key)
        {
            Table.Delete(Table[area, key]);
        }
        
        public Dictionary<string, Dictionary<string, string>> GetAll()
        {
            return Table
                .GetAll()
                .GroupBy(item => item.PartitionKey)
                .ToDictionary(group => group.Key,
                    group => group
                        .ToDictionary(item => item.RowKey, item => item.Value));
        }
    }
}