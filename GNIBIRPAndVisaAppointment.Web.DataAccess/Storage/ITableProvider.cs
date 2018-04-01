using Microsoft.WindowsAzure.Storage.Table;

namespace GNIBIRPAndVisaAppointment.Web.DataAccess.Storage
{
    public interface ITableProvider
    {
        Table<TTableEntity> GetTable<TTableEntity>(string tableName = null) where TTableEntity : TableEntity, new();
    }
}