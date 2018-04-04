using System.Collections.Generic;
using System.IO;
using GNIBIRPAndVisaAppointment.Web.Business.Authentication;
using GNIBIRPAndVisaAppointment.Web.Utility;

namespace GNIBIRPAndVisaAppointment.Web.Business.Information
{
    public interface IInformationManager : IDomain
    {
        DataAccess.Model.Storage.Information this[string key, string language = Languages.English] { get; }

        [AdminRequired]
        IDictionary<string, IEnumerable<string>> GetAllKeys();

        [AdminRequired]
        IEnumerable<DataAccess.Model.Storage.Information> GetList();

        [AdminRequired]
        void Add(string key, string title, string auther, string content);

        [AdminRequired]
        void Add(string key, string language, string title, string auther, string content);

        [AdminRequired]
        void Update(string key, string title, string auther, string content);

        [AdminRequired]
        void Update(string key, string language, string title, string auther, string content);

        [AdminRequired]
        void Delete(string key, string language);

        [AdminRequired]
        string UploadFile(string fileName, string contentType, Stream fileStream);

        Stream LoadFile(string fileName);
    }
}