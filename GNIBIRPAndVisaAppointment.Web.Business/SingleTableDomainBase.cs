using GNIBIRPAndVisaAppointment.Web.DataAccess.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace GNIBIRPAndVisaAppointment.Web.Business
{
    public abstract class SingleTableDomainBase<TTableEntity> : IDomain
        where TTableEntity : TableEntity, new()
    {
        protected readonly IStorageProvider StorageProvider;

        protected readonly string TableName;

        protected Table<TTableEntity> Table => StorageProvider.GetTable<TTableEntity>(TableName);

        public SingleTableDomainBase(IStorageProvider tableProvider, string tableName = null)
        {
            StorageProvider = tableProvider;
            TableName = tableName;
        }
    }
}