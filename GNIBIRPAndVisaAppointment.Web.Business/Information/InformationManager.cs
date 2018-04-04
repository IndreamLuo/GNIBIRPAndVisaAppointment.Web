using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Storage;
using GNIBIRPAndVisaAppointment.Web.Utility;
using Microsoft.WindowsAzure.Storage.File;

namespace GNIBIRPAndVisaAppointment.Web.Business.Information
{
    public class InformationManager : SingleTableDomainBase<DataAccess.Model.Storage.Information>, IInformationManager
    {
        readonly CloudFileDirectory InformationDirectory;
        readonly IApplicationSettings ApplicationSettings;

        public InformationManager(IStorageProvider storageProvider, IApplicationSettings applicationSettings) : base(storageProvider)
        {
            ApplicationSettings = applicationSettings;

            var webRootDirectory = StorageProvider.WebFileShare.GetRootDirectoryReference();
            InformationDirectory = webRootDirectory.GetDirectoryReference("info");
            
            InformationDirectory.CreateIfNotExistsAsync().Wait();
        }

        public DataAccess.Model.Storage.Information this[string key, string language = Languages.English] => Table[key.ToLower(), language ?? Languages.English];

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
                    nameof(DataAccess.Model.Storage.Information.CreatedTime),
                    nameof(DataAccess.Model.Storage.Information.FacebookComment))
                .OrderBy(information => information.PartitionKey)
                .ThenBy(information => information.RowKey);
        }

        public void Add(string key, string title, string auther, string content, bool facebookComment = false)
        {
            Add(key, Languages.English, title, auther, content, facebookComment);
        }

        public void Add(string key, string language, string title, string auther, string content, bool facebookComment = false)
        {
            Table.Insert(new DataAccess.Model.Storage.Information
            {
                PartitionKey = key.ToLower(),
                RowKey = language,
                Title = title,
                Author = auther,
                CreatedTime = DateTime.Now,
                Content = content,
                FacebookComment = facebookComment
            });
        }

        public void Update(string key, string title, string auther, string content, bool facebookComment)
        {
            Update(key, Languages.English, title, auther, content, facebookComment);
        }

        public void Update(string key, string language, string title, string auther, string content, bool facebookComment)
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
                Content = content,
                FacebookComment = facebookComment
            });
        }

        public void Delete(string key, string language)
        {
            Table.Delete(key, language);
        }

        public string UploadFile(string fileName, string contentType, Stream fileStream)
        {
            var storeFileName = $"{fileStream.ToMD5()}{Path.GetExtension(fileName)}";
            var fileReference = InformationDirectory.GetFileReference(storeFileName);
            if (!fileReference.ExistsAsync().Result)
            {
                fileReference.CreateAsync(fileStream.Length).Wait();
                using (var writeStream = fileReference.OpenWriteAsync(fileStream.Length).Result)
                {
                    fileStream.Position = 0;
                    fileStream.CopyTo(writeStream);
                }
            }

            return $"{ApplicationSettings["UploadedFileRewritingURL"]}{storeFileName}";
        }

        public Stream LoadFile(string fileName)
        {
            var fileReference = InformationDirectory.GetFileReference(fileName);
            if (!fileReference.ExistsAsync().Result)
            {
                throw new ArgumentOutOfRangeException(fileName);
            }

            return fileReference.OpenReadAsync().Result;
        }
    }
}