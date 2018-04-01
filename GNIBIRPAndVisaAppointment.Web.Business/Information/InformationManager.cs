using System.Collections.Generic;
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

        public DataAccess.Model.Storage.Information this[string key] => throw new System.NotImplementedException();

        public IDictionary<string, IEnumerable<string>> GetAllKeys()
        {
            return Table.GetAllKeys();
        }

        public void Add(string key, string title, string auther, string content)
        {
            Add(key, Languages.English, title, auther, content);
        }

        public void Add(string key, string language, string title, string auther, string content)
        {
            throw new System.NotImplementedException();
        }

        public void Update(string key, string title, string auther, string content)
        {
            Update(key, Languages.English, title, auther, content);
        }

        public void Update(string key, string language, string title, string auther, string content)
        {
            throw new System.NotImplementedException();
        }
    }
}