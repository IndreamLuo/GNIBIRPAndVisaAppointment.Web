using System;

namespace GNIBIRPAndVisaAppointment.Web.Business.Authentication
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
    public class RoleRequiredAttribute : Attribute
    {
        public RoleRequiredAttribute(params Role[] roles)
        {
            Roles = roles;
        }

        public Role[] Roles { get; private set; }
    }

    public enum Role
    {
        Admin
    }
}