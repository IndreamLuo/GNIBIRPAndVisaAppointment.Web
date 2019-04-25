namespace GNIBIRPAndVisaAppointment.Web.Utility
{
    public class ExchangeHelper
    {
        public static double ToEUR(string currency, double amount)
        {
            currency = currency.ToLower();
            if (currency == "cny")
            {
                return amount / 7.5;
            }

            if (currency == "eur")
            {
                return amount;
            }

            throw new System.NotImplementedException();
        }
    }
}