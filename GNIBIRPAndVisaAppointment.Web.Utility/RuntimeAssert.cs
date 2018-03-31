using System;

namespace GNIBIRPAndVisaAppointment.Web.Utility
{
    public static class RuntimeAssert
    {
        public static void IsNotNull<T>(T obj, string valueName = null, string message = null)
        {
            if (obj == null)
            {
                throw new NullReferenceException(message ?? $"{valueName ?? "Value"} cannot be NULL.");
            }
        }
    }
}