using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GNIBIRPAndVisaAppointment.Web.Utility
{
    public class reCaptchaHelper
    {
        public readonly string reCaptchaUserCode;
        readonly string reCaptchaSystemCode;
        public reCaptchaHelper(IApplicationSettings applicationSettings)
        {
            reCaptchaUserCode = applicationSettings["reCaptchaUserCode"];
            reCaptchaSystemCode = applicationSettings["reCaptchaSystemCode"];
        }

        public async Task<bool> VerifyAsync(string g_recaptcha_response, string userIP)
        {
            using (var httpClient = new HttpClient())
            {
                var postResponse = await httpClient.PostAsync($"https://www.google.com/recaptcha/api/siteverify?secret={reCaptchaSystemCode}&response={g_recaptcha_response}&remoteip{userIP}", null);

                var resultContent = await postResponse.Content.ReadAsStringAsync();

                dynamic response = JsonConvert.DeserializeObject(resultContent);
                bool result = response.success;
                return result;
            }
        }
    }
}