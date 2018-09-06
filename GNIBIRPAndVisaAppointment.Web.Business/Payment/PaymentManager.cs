using System;
using GNIBIRPAndVisaAppointment.Web.Business.Application;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Storage;
using GNIBIRPAndVisaAppointment.Web.Utility;
using Stripe;

namespace GNIBIRPAndVisaAppointment.Web.Business.Payment
{
    public class PaymentManager : IPaymentManager
    {
        readonly IDomainHub DomainHub;
        Table<DataAccess.Model.Storage.Payment> PaymentTable;
        public string PublishableKey { get; private set; }
        public string SecretKey { get; private set; }
        public PaymentManager(IDomainHub domainHub, IStorageProvider storageProvider, IApplicationSettings applicationSettings)
        {
            this.DomainHub = domainHub;
            PaymentTable = storageProvider.GetTable<DataAccess.Model.Storage.Payment>();
            PublishableKey = applicationSettings["StripeAPIKeyPublishable"];
            SecretKey = applicationSettings["StripeAPIKeySecret"];
        }

        const string EUR = "EUR";
        const string Stripe = "Stripe";

        public void Startup()
        {
            StripeConfiguration.SetApiKey(SecretKey);
        }

        public bool StripePay(string orderId, string stripeToken, string email)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();
            
            var order = applicationManager.GetOrder(orderId);
            
            var options = new StripeChargeCreateOptions
            {
                Amount = (int)(order.Amount * 100),
                Currency = EUR,
                SourceTokenOrExistingSourceId = stripeToken
            };

            var service = new StripeChargeService();
            var charge = service.Create(options);

            if (charge.Paid)
            {
                var payment = new DataAccess.Model.Storage.Payment
                {
                    PartitionKey = orderId,
                    RowKey = charge.Id,
                    Time = DateTime.UtcNow.AddHours(1),
                    Type = Stripe,
                    Currency = charge.Currency,
                    Amount = ((double)charge.Amount) / 100,
                    Payer = charge.Customer?.Email ?? email
                };
                
                PaymentTable.Insert(payment);
            }

            return charge.Paid;
        }
    }
}