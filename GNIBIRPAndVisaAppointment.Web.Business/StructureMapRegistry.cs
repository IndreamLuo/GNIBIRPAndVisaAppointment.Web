using StructureMap;
using GNIBIRPAndVisaAppointment.Web.Business.Appointment;
using GNIBIRPAndVisaAppointment.Web.Business.Configuration;
using GNIBIRPAndVisaAppointment.Web.Business.Information;
using GNIBIRPAndVisaAppointment.Web.Business.Application;
using GNIBIRPAndVisaAppointment.Web.Business.Payment;
using GNIBIRPAndVisaAppointment.Web.Business.User;
using GNIBIRPAndVisaAppointment.Web.Business.Api;
using GNIBIRPAndVisaAppointment.Web.Business.AppointmnetLetter;
using GNIBIRPAndVisaAppointment.Web.Business.Email;

namespace GNIBIRPAndVisaAppointment.Web.Business
{
    public class StructureMapRegistry : Registry
    {
        public StructureMapRegistry()
        {
            For<IDomainHub>().Singleton().Use<DomainHub>();
            For<IConfigurationManager>().Singleton().Use<ConfigurationManager>();
            For<IEmailApplication>().Singleton().Use<EmailApplication>();
            For<IInformationManager>().Singleton().Use<InformationManager>();
            For<IAppointmentManager>().Singleton().Use<AppointmentManager>();
            For<IApplicationManager>().Singleton().Use<ApplicationManager>();
            For<IPaymentManager>().Singleton().Use<PaymentManager>();
            For<IUserManager>().Singleton().Use<UserManager>();
            For<IApiManager>().Singleton().Use<ApiManager>();
            For<IAppointmentLetterManager>().Singleton().Use<AppointmentLetterManager>();
        }
    }
}