using System;

namespace GNIBIRPAndVisaAppointment.Web.Business.Authentication
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
    public class AdminRequired : RoleRequiredAttribute
    {
        public AdminRequired() : base(Role.Admin) { }
    }
}