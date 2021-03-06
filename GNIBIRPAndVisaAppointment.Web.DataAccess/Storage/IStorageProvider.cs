﻿using Microsoft.WindowsAzure.Storage.File;
using Microsoft.WindowsAzure.Storage.Table;

namespace GNIBIRPAndVisaAppointment.Web.DataAccess.Storage
{
    public interface IStorageProvider
    {
        Table<TTableEntity> GetTable<TTableEntity>(string tableName = null) where TTableEntity : TableEntity, new();

        CloudFileShare WebFileShare { get; }
    }
}