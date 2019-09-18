using System;
using System.Linq;
using System.Threading.Tasks;
using GNIBIRPAndVisaAppointment.Web.Business.Application;
using GNIBIRPAndVisaAppointment.Web.Business.Configuration;
using GNIBIRPAndVisaAppointment.Web.Business.Payment;
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

        public async Task NotifyApplicationChangedAsync(string applicationId, string currentStatus)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();
            var application = applicationManager[applicationId];

            var configurationManager = DomainHub.GetDomain<IConfigurationManager>();
            var templateKey = $"Application{currentStatus}";
            var time = DateTime.Now;
            if (currentStatus == AssignmentStatus.Complete)
            {
                var order = applicationManager.GetOrder(applicationId);
                var paymentManager = DomainHub.GetDomain<IPaymentManager>();
                var payments = paymentManager.GetPayments(applicationId);
                if (payments.Sum(payment => payment.Amount) >= (order.Amount + order.Special))
                {
                    templateKey += "Paid";
                }
                else
                {
                    templateKey += "Unpaid";
                }

                var appointment = applicationManager.GetAppointmentLetter(applicationId);
                time = appointment.Time;
            }
            var templateId = configurationManager["MailjetTemplate", templateKey];

            if (templateId != null)
            {
                var mailjetUsername = configurationManager["Mailjet", "Username"];
                var mailjetPassword = configurationManager["Mailjet", "Password"];
                var client = new MailjetClient(mailjetUsername, mailjetPassword);

                var senderEmail = configurationManager["Mailjet", "SenderEmail"];
                var senderName = configurationManager["Mailjet", "SenderName"];
                var request = new MailjetRequest()
                {
                    Resource = Send.Resource
                }
                .Property(Send.FromEmail, senderEmail)
                .Property(Send.FromName, senderName)
                .Property(Send.To, $"{application.GivenName} {application.SurName} <{application.Email}>")
                .Property(Send.Bcc, $"Backup <{senderEmail}>")
                .Property(Send.MjTemplateID, templateId)
                .Property(Send.MjTemplateLanguage, "True")
                .Property(Send.Vars, new JObject
                {
                    { "name", application.GivenName },
                    { "id", application.Id },
                    { "time", time.ToString("dddd, dd MMMM") }
                });

                var response = await client.PostAsync(request);
            }
        }
    }
}