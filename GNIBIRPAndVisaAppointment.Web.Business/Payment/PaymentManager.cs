using System;
using System.Collections.Generic;
using System.Linq;
using GNIBIRPAndVisaAppointment.Web.Business.Application;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage;
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
                    ChargeId = charge.Id,
                    RowKey = charge.Id,
                    Time = DateTime.Now,
                    Type = Stripe,
                    Currency = charge.Currency,
                    Amount = ((double)charge.Amount) / 100,
                    Payer = charge.Customer?.Email ?? email
                };
                
                PaymentTable.Insert(payment);
            }

            return charge.Paid;
        }

        public void AdminPay(string orderId, string chargeId, string type, string currency, double amount, string payer)
        {
            var payment = new DataAccess.Model.Storage.Payment
            {
                PartitionKey = orderId,
                ChargeId = chargeId,
                RowKey = chargeId,
                Time = DateTime.Now,
                Type = type,
                Currency = currency,
                Amount = amount,
                Payer = payer
            };

            PaymentTable.Insert(payment);
        }

        public List<DataAccess.Model.Storage.Payment> GetPayments(string orderId)
        {
            return PaymentTable[orderId];
        }

        public bool IsPaid(string orderId)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();
            var order = applicationManager.GetOrder(orderId);
            var payments = PaymentTable[orderId];
            var amount = payments
                .Sum(payment => ExchangeHelper.ToEUR(payment.Currency, payment.Amount));

            return amount == order.Amount;
        }
    }
}