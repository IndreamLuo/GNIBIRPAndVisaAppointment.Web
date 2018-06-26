using System.Runtime.CompilerServices;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage;
using GNIBIRPAndVisaAppointment.Web.Utility;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;
using Microsoft.WindowsAzure.Storage.Table;

namespace GNIBIRPAndVisaAppointment.Web.DataAccess.Storage
{
    public class StorageProvider : IStorageProvider
    {
        readonly CloudTableClient CloudTableClient;
        readonly CloudFileClient CloudFileClient;
        readonly IGlobalCache GlobalCache;

        public StorageProvider(IApplicationSettings applicationSettings, IGlobalCache globalCache)
        {
            var cloudStorageAccount = CloudStorageAccount.Parse(applicationSettings.GetConnectionString(ConnectionStringKeys.AzureStorage));
            CloudTableClient = cloudStorageAccount.CreateCloudTableClient();
            CloudFileClient = cloudStorageAccount.CreateCloudFileClient();

            GlobalCache = globalCache;
        }

        public Table<TTableEntity> GetTable<TTableEntity>(string tableName = null) where TTableEntity : TableEntity, new()
        {
            tableName = tableName ?? typeof(TTableEntity).Name;
            return GlobalCache.Cached($"table:{tableName}", () => new Table<TTableEntity>(CloudTableClient.GetTableReference(tableName)));
        }

        public CloudFileShare WebFileShare => GlobalCache.Cached(() => CloudFileClient.GetShareReference("web"));
    }
}