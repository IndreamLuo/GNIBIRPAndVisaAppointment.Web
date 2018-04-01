using System.Collections.Generic;
using GNIBIRPAndVisaAppointment.Web.Business.Authentication;
using GNIBIRPAndVisaAppointment.Web.Utility;

namespace GNIBIRPAndVisaAppointment.Web.Business.Information
{
    public interface IInformationManager : IDomain
    {
        DataAccess.Model.Storage.Information this[string key] { get; }

        [AdminRequired]
        IDictionary<string, IEnumerable<string>> GetAllKeys();

        [AdminRequired]
        void Add(string key, string title, string auther, string content);

        [AdminRequired]
        void Add(string key, string language, string title, string auther, string content);

        [AdminRequired]
        void Update(string key, string title, string auther, string content);

        [AdminRequired]
        void Update(string key, string language, string title, string auther, string content);
    }
}