using Microsoft.AspNetCore.Identity;

namespace GNIBIRPAndVisaAppointment.Web.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string Role { get; set; }
    }
}