using System;
using System.Collections.Generic;
using System.Linq;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Storage;
using GNIBIRPAndVisaAppointment.Web.Utility;

namespace GNIBIRPAndVisaAppointment.Web.Business.Information
{
    public class InformationManager : SingleTableDomainBase<DataAccess.Model.Storage.Information>, IInformationManager
    {
        public InformationManager(ITableProvider tableProvider) : base(tableProvider)
        {
            
        }

        public DataAccess.Model.Storage.Information this[string key, string language = Languages.English] => Table[key, language ?? Languages.English];

        public IDictionary<string, IEnumerable<string>> GetAllKeys()
        {
            return Table.GetAllKeys();
        }

        public IEnumerable<DataAccess.Model.Storage.Information> GetList()
        {
            return Table
                .GetAll(nameof(DataAccess.Model.Storage.Information.PartitionKey),
                    nameof(DataAccess.Model.Storage.Information.RowKey),
                    nameof(DataAccess.Model.Storage.Information.Title),
                    nameof(DataAccess.Model.Storage.Information.Author),
                    nameof(DataAccess.Model.Storage.Information.CreatedTime))
                .OrderBy(information => information.PartitionKey)
                .ThenBy(information => information.RowKey);
        }

        public void Add(string key, string title, string auther, string content)
        {
            Add(key, Languages.English, title, auther, content);
        }

        public void Add(string key, string language, string title, string auther, string content)
        {
            Table.Insert(new DataAccess.Model.Storage.Information
            {
                PartitionKey = key,
                RowKey = language,
                Title = title,
                Author = auther,
                CreatedTime = DateTime.Now,
                Content = content
            });
        }

        public void Update(string key, string title, string auther, string content)
        {
            Update(key, Languages.English, title, auther, content);
        }

        public void Update(string key, string language, string title, string auther, string content)
        {
            var oldInformation = this[key, language];

            if (oldInformation == null)
            {
                throw new System.InvalidOperationException("The information to be updated doesn't exist.");
            }

            Table.Replace(new DataAccess.Model.Storage.Information
            {
                PartitionKey = key,
                RowKey = language,
                Title = title,
                Author = auther,
                CreatedTime = oldInformation.CreatedTime,
                Content = content
            });
        }

        public void Delete(string key, string language)
        {
            Table.Delete(key, language);
        }
    }
}