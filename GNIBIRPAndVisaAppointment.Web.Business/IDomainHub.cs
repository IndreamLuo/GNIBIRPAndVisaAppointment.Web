namespace GNIBIRPAndVisaAppointment.Web.Business
{
    public interface IDomainHub
    {
        TIDomain GetDomain<TIDomain>() where TIDomain : IDomain;
    }
}