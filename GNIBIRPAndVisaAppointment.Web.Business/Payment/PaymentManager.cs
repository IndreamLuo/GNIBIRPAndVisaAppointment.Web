using System;
using System.Collections.Generic;
using System.Linq;
using GNIBIRPAndVisaAppointment.Web.Business.Application;
using GNIBIRPAndVisaAppointment.Web.Business.Email;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Storage;
using GNIBIRPAndVisaAppointment.Web.Utility;
using Stripe;
using Stripe.Checkout;

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
        const string paid = "paid";

        public void Startup()
        {
            StripeConfiguration.ApiKey = SecretKey;
        }

        public bool StripePay(string orderId, string stripeToken, string email)
        {
            throw new InvalidOperationException("Abandoned method.");
            // var applicationManager = DomainHub.GetDomain<IApplicationManager>();
            
            // var order = applicationManager.GetOrder(orderId);
            
            // var options = new StripeChargeCreateOptions
            // {
            //     Amount = (int)(order.Amount * 100),
            //     Currency = EUR,
            //     SourceTokenOrExistingSourceId = stripeToken
            // };

            // var service = new StripeChargeService();
            // var charge = service.Create(options);

            // if (charge.Paid)
            // {
            //     var payment = new DataAccess.Model.Storage.Payment
            //     {
            //         PartitionKey = orderId,
            //         ChargeId = charge.Id,
            //         RowKey = charge.Id,
            //         Time = DateTime.Now,
            //         Type = Stripe,
            //         Currency = charge.Currency,
            //         Amount = ((double)charge.Amount) / 100,
            //         Payer = charge.Customer?.Email ?? email
            //     };
                
            //     PaymentTable.Insert(payment);

            //     var assignment = applicationManager.GetAssignment(orderId);

            //     if (assignment.Status == AssignmentStatus.Complete)
            //     {
            //         var emailApplication = DomainHub.GetDomain<IEmailApplication>();
            //         emailApplication.NotifyApplicationChangedAsync(orderId, assignment.Status);
            //     }
            // }

            // return charge.Paid;
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
                Payer = payer,
                Status = paid
            };

            PaymentTable.Insert(payment);
        }

        public Session CreateStripePaySession(string orderId, string cancelledUrl, string succeededUrl)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();
            var application = applicationManager[orderId];
            var unpaidAmount = this.GetUnpaidAmount(orderId);

            var currency = "eur";
            var amount = (long)(100 * unpaidAmount);
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                  "card"
                },
                LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = amount,
                        Currency = currency,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "GNIB/IRP Appointment Service",
                        },
                    },
                    Quantity = 1,
                  },
                },
                Mode = "payment",
                SuccessUrl = succeededUrl,
                CancelUrl = cancelledUrl,
            };
            var service = new SessionService();
            Session session = service.Create(options);

            var payment = new DataAccess.Model.Storage.Payment
            {
                PartitionKey = orderId,
                ChargeId = session.PaymentIntentId,
                RowKey = session.PaymentIntentId,
                Time = DateTime.Now,
                Type = Stripe,
                Currency = currency,
                Amount = ((double)session.AmountTotal) / 100,
                Payer = application.Email,
                Status = session.PaymentStatus
            };
            
            PaymentTable.Insert(payment);

            return session;
        }
        
        public bool ConfirmPayment(string orderId)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();
            var order = applicationManager.GetOrder(orderId);
            var payments = this.GetPayments(orderId);

            var service = new SessionService();
            foreach (var payment in payments)
            {
                if (payment.Type == Stripe && payment.Status != "succeeded")
                {
                    var sessions = service.List(new SessionListOptions
                    {
                        PaymentIntent = payment.ChargeId
                    });
                    foreach (var session in sessions)
                    {
                        if (session.PaymentStatus != payment.Status)
                        {
                            payment.Status = session.PaymentStatus;
                            payment.Payer = session.CustomerEmail ?? payment.Payer;
                            PaymentTable.Replace(payment);
                        }
                    }
                }
            }

            return order.Amount - this.GetPaidAmount(payments) <= 0;
        }

        public List<DataAccess.Model.Storage.Payment> GetPayments(string orderId)
        {
            return PaymentTable[orderId];
        }

        public bool IsPaid(string orderId)
        {
            return this.GetUnpaidAmount(orderId) <= 0;
        }

        public double GetUnpaidAmount(string orderId)
        {
            var applicationManager = DomainHub.GetDomain<IApplicationManager>();
            var order = applicationManager.GetOrder(orderId);
            var payments = PaymentTable[orderId];

            var amount = this.GetPaidAmount(payments);

            return order.Amount - amount;
        }

        double GetPaidAmount(IEnumerable<DataAccess.Model.Storage.Payment> payments)
        {
            return payments
                .Where(payment => payment.Status == null || payment.Status == paid)
                .Sum(payment => ExchangeHelper.ToEUR(payment.Currency, payment.Amount));
        }
    }
}