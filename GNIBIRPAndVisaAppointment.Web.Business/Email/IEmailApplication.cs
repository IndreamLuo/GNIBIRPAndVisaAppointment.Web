using System.Threading.Tasks;

namespace GNIBIRPAndVisaAppointment.Web.Business.Email
{
    public interface IEmailApplication : IDomain
    {
        Task NotifyApplicationChangedAsync(string applicationId, string currentStatus);
    }
}