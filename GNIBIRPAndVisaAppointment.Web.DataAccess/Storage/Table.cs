using Microsoft.WindowsAzure.Storage.Table;

namespace GNIBIRPAndVisaAppointment.Web.DataAccess.Storage
{
    public class Table<T>
        where T : TableEntity
    {
        readonly CloudTable CloudTable;

        public Table(CloudTable cloudTable)
        {
            CloudTable = cloudTable;
        }

        public T this[string partitionKey, string rowKey] => throw new System.NotImplementedException();

        public async void Update(T update)
        {
            var updateOperation = TableOperation.Replace(update);
            await CloudTable.ExecuteAsync(updateOperation);
        }
    }
}