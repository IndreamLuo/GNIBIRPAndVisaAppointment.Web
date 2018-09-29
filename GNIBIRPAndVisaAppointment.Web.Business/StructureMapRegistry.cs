using GNIBIRPAndVisaAppointment.Web.Business.Appointment;
using GNIBIRPAndVisaAppointment.Web.Business.Configuration;
using GNIBIRPAndVisaAppointment.Web.Business.Information;
using GNIBIRPAndVisaAppointment.Web.Business.Application;
using StructureMap;
using GNIBIRPAndVisaAppointment.Web.Business.Payment;
using GNIBIRPAndVisaAppointment.Web.Business.User;

namespace GNIBIRPAndVisaAppointment.Web.Business
{
    public class StructureMapRegistry : Registry
    {
        public StructureMapRegistry()
        {
            For<IDomainHub>().Singleton().Use<DomainHub>();
            For<IConfigurationManager>().Singleton().Use<ConfigurationManager>();
            For<IInformationManager>().Singleton().Use<InformationManager>();
            For<IAppointmentManager>().Use<AppointmentManager>();
            For<IApplicationManager>().Use<ApplicationManager>();
            For<IPaymentManager>().Use<PaymentManager>();
            For<IUserManager>().Use<UserManager>();
        }
    }
}