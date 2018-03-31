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

        readonly LazyLoader LazyLoader;

        public TableProvider(IApplicationSettings applicationSettings)
        {
            var cloudStorageAccount = CloudStorageAccount.Parse(applicationSettings[DataSourceKeys.AzureStorageConnectionString]);
            CloudTableClient = cloudStorageAccount.CreateCloudTableClient();

            LazyLoader = new LazyLoader();
        }

        public Table<Configuration> Configuration => LazyLoadTable<Configuration>();

        public Table<TTableEntity> LazyLoadTable<TTableEntity>([CallerMemberName]string tableName = null)
            where TTableEntity : TableEntity
        {
            RuntimeAssert.IsNotNull(tableName, nameof(tableName));

            return LazyLoader.LazyLoad(() => new Table<TTableEntity>(CloudTableClient.GetTableReference(tableName)));
        }
    }
}