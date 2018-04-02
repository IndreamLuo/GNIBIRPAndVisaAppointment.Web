using System.Collections.Generic;
using System.Linq;
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

        public static string[] Keys = { "PartitionKey", "RowKey" };

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

        public TTableEntity this[string partitionKey, string rowKey] => (TTableEntity)CloudTable.ExecuteAsync(TableOperation.Retrieve<TTableEntity>(partitionKey, rowKey)).Result.Result;

        public IDictionary<string, IEnumerable<string>> GetAllKeys()
        {
            return this.GetAll(Keys)
                .GroupBy(entity => entity.PartitionKey)
                .ToDictionary(group => group.Key, group => group.Select(entity => entity.RowKey));
        }

        public IEnumerable<TTableEntity> GetAll(params string[] columns)
        {
            var query = new TableQuery<TTableEntity>().Select(columns);
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

        public async void Replace(TTableEntity update)
        {
            update.ETag = "*";
            await CloudTable.ExecuteAsync(TableOperation.Replace(update));
        }

        public async void Insert(TTableEntity entity) => await CloudTable.ExecuteAsync(TableOperation.Insert(entity));

        public async void Delete(TTableEntity entity) => await CloudTable.ExecuteAsync(TableOperation.Delete(entity));

        public async void Delete(string partitionKey, string rowKey) => await CloudTable.ExecuteAsync(TableOperation.Delete(this[partitionKey, rowKey]));
    }
}