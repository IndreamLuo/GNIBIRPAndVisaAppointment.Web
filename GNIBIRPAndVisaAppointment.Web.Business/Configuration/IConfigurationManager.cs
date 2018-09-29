using System.Collections.Generic;

namespace GNIBIRPAndVisaAppointment.Web.Business.Configuration
{
    public interface IConfigurationManager : IDomain
    {
        string this[string area, string key] { get; set; }
        void Remove(string area, string key);
        Dictionary<string, Dictionary<string, string>> GetAll();
    }
}