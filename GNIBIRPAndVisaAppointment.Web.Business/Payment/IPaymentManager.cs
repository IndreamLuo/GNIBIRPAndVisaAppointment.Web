namespace GNIBIRPAndVisaAppointment.Web.Business.Payment
{
    public interface IPaymentManager : IDomain
    {
        string PublishableKey { get; }
        string SecretKey { get; }
        void Startup();
        bool StripePay(string orderId, string stripeToken, string email);
    }
}