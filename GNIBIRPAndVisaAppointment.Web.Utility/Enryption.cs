using System;
using System.Linq;

namespace GNIBIRPAndVisaAppointment.Web.Utility
{
    public static class Encryption
    {
        public static string ToMD5(this string input)
        {
            var md5 = System.Security.Cryptography.MD5.Create();
            var inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            var md5Bytes = md5.ComputeHash(inputBytes);
            return string.Join(string.Empty, md5Bytes.Select(md5Byte => md5Byte.ToString("X2")));
        }
    }
}
