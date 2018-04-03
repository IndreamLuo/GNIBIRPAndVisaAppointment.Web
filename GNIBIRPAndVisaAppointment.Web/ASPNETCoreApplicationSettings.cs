using GNIBIRPAndVisaAppointment.Web.Utility;
using Microsoft.Extensions.Configuration;

namespace GNIBIRPAndVisaAppointment.Web
{
    public class ASPNETCoreApplicationSettings : IApplicationSettings
    {
        readonly IConfiguration Configuration;
        public ASPNETCoreApplicationSettings(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string this[string key] => Configuration[key];

        public string GetConnectionString(string key) => Configuration.GetConnectionString(key);
    }
}