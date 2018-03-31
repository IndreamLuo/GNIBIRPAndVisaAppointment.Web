namespace GNIBIRPAndVisaAppointment.Web.Business
{
    public interface IDomainHub
    {
        ITDomain GetDomain<ITDomain>() where ITDomain : IDomain;
    }
}