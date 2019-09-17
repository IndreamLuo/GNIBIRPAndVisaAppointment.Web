using System.Threading.Tasks;
using GNIBIRPAndVisaAppointment.Web.Business.Application;
using GNIBIRPAndVisaAppointment.Web.Business.Configuration;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;

namespace GNIBIRPAndVisaAppointment.Web.Business.Email
{
    public class EmailApplication : IEmailApplication
    {
        readonly IDomainHub DomainHub;

        public EmailApplication(IDomainHub domainHub)
        {
            DomainHub = domainHub;
        }

        public async Task NotifyApplicationChangedAsync(string applicationId, string currentStatus, string lastStatus = null)
        {
            if (currentStatus != lastStatus)
            {
                var applicationManager = DomainHub.GetDomain<IApplicationManager>();
                var application = applicationManager[applicationId];
                var configurationManager = DomainHub.GetDomain<IConfigurationManager>();
                
                var mailjetUsername = configurationManager["Mailjet", "Username"];
                var mailjetPassword = configurationManager["Mailjet", "Password"];
                var client = new MailjetClient(mailjetUsername, mailjetPassword);

                var senderEmail = configurationManager["Mailjet", "SenderEmail"];
                var senderName = configurationManager["Mailjet", "SenderName"];
                var templateId = configurationManager["MailjetTemplate", $"Application{currentStatus}"];
                var request = new MailjetRequest()
                {
                    Resource = Send.Resource
                }
                .Property(Send.FromEmail, senderEmail)
                .Property(Send.FromName, senderName)
                .Property(Send.Recipients, new JArray
                {
                    new JObject
                    {
                        { "Email", application.Email }
                    }
                })
                .Property(Send.MjTemplateID, templateId)
                .Property(Send.MjTemplateLanguage, "True")
                .Property(Send.Vars, new JObject
                {
                    { "name", application.GivenName },
                    { "id", application.Id }
                });

                var response = await client.PostAsync(request);
            }
        }
    }
}