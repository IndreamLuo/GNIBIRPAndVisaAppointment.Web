using System.Runtime.CompilerServices;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage;
using GNIBIRPAndVisaAppointment.Web.Utility;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace GNIBIRPAndVisaAppointment.Web.DataAccess.Storage
{
    public class TableProvider : ITableProvider
    {
        readonly CloudTableClient CloudTableClient;

        readonly LazyLoader LazyLoader = new LazyLoader();

        public TableProvider(IApplicationSettings applicationSettings)
        {
            var cloudStorageAccount = CloudStorageAccount.Parse(applicationSettings.GetConnectionString(ConnectionStringKeys.AzureStorage));
            CloudTableClient = cloudStorageAccount.CreateCloudTableClient();
        }

        public Table<TTableEntity> GetTable<TTableEntity>(string tableName = null) where TTableEntity : TableEntity, new()
        {
            return LazyLoader.LazyLoad(() => new Table<TTableEntity>(CloudTableClient.GetTableReference(tableName ?? typeof(TTableEntity).Name)));
        }
    }
}