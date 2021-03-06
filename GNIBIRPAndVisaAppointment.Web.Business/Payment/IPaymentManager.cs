using Stripe.Checkout;
using System.Collections.Generic;

namespace GNIBIRPAndVisaAppointment.Web.Business.Payment
{
    public interface IPaymentManager : IDomain
    {
        string PublishableKey { get; }
        string SecretKey { get; }
        void Startup();
        bool StripePay(string orderId, string stripeToken, string email);
        void AdminPay(string orderId, string chargeId, string type, string currency, double amount, string payer);
        Session CreateStripePaySession(string orderId, string cancelledUrl, string succeededUrl);
        bool ConfirmPayment(string orderId);
        List<DataAccess.Model.Storage.Payment> GetPayments(string orderId);
        bool IsPaid(string orderId);
        double GetUnpaidAmount(string orderId);
    }
}