using GNIBIRPAndVisaAppointment.Web.Utility;

namespace GNIBIRPAndVisaAppointment.Web.Business
{
    internal class DomainHub : IDomainHub
    {
        readonly IDIContainer DIContainer;

        public DomainHub(IDIContainer dIContainer)
        {
            DIContainer = dIContainer;
        }

        public ITDomain GetDomain<ITDomain>() where ITDomain : IDomain
        {
            return DIContainer.GetInstance<ITDomain>();
        }
    }
}