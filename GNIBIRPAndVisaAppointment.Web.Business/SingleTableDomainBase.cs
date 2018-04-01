using GNIBIRPAndVisaAppointment.Web.DataAccess.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace GNIBIRPAndVisaAppointment.Web.Business
{
    public abstract class SingleTableDomainBase<TTableEntity> : IDomain
        where TTableEntity : TableEntity, new()
    {
        protected readonly ITableProvider TableProvider;

        protected readonly string TableName;

        protected Table<TTableEntity> Table => TableProvider.GetTable<TTableEntity>(TableName);

        public SingleTableDomainBase(ITableProvider tableProvider, string tableName = null)
        {
            TableProvider = tableProvider;
            TableName = tableName;
        }
    }
}