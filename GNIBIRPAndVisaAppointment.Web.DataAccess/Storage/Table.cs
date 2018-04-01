using System.Collections.Generic;
using GNIBIRPAndVisaAppointment.Web.Utility;
using Microsoft.WindowsAzure.Storage.Table;

namespace GNIBIRPAndVisaAppointment.Web.DataAccess.Storage
{
    public class Table<TTableEntity>
        where TTableEntity : TableEntity, new()
    {
        readonly CloudTable CloudTable;

        readonly LazyLoader lazyLoader = new LazyLoader();

        public Table(CloudTable cloudTable)
        {
            CloudTable = cloudTable;
        }

        public static IList<string> Keys = new List<string> { "PartitionKey", "RowKey" };

        public List<TTableEntity> this[string partitionKey]
        {
            get
            {
                var query = new TableQuery<TTableEntity>().Select(Keys);
                var result = new List<TTableEntity>();
                TableContinuationToken token = null;

                do
                {
                    var response = CloudTable.ExecuteQuerySegmentedAsync(query, token).Result;

                    foreach (var entity in response.Results)
                    {
                        result.Add(entity);
                    }

                    token = response.ContinuationToken;
                } while (token != null);

                return result;
            }
        }

        public TTableEntity this[string partitionKey, string rowKey] => (TTableEntity)CloudTable.ExecuteAsync(TableOperation.Retrieve(partitionKey, rowKey)).Result.Result;

        public IDictionary<string, IEnumerable<string>> GetAllKeys()
        {
            var query = new TableQuery<TTableEntity>().Select(Keys);
            var result = new Dictionary<string, IEnumerable<string>>();
            TableContinuationToken token = null;

            do
            {
                var response = CloudTable.ExecuteQuerySegmentedAsync(query, token).Result;

                foreach (var entity in response.Results)
                {
                    IEnumerable<string> rowKeys;
                    if (!result.TryGetValue(entity.PartitionKey, out rowKeys))
                    {
                        rowKeys = new List<string>();
                        result.Add(entity.PartitionKey, rowKeys);
                    }
                    (rowKeys as List<string>).Add(entity.RowKey);
                }

                token = response.ContinuationToken;
            } while (token != null);

            return result;
        }

        public async void Replace(TTableEntity update) => await CloudTable.ExecuteAsync(TableOperation.Replace(update));

        public async void Insert(TTableEntity entity) => await CloudTable.ExecuteAsync(TableOperation.Insert(entity));

        public async void Delete(TTableEntity entity) => await CloudTable.ExecuteAsync(TableOperation.Delete(entity));

        public async void Delete(string partitionKey, string rowKey) => await CloudTable.ExecuteAsync(TableOperation.Delete(this[partitionKey, rowKey]));
    }
}