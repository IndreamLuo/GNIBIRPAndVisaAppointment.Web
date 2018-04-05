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

        public TIDomain GetDomain<TIDomain>() where TIDomain : IDomain
        {
            return DIContainer.GetInstance<TIDomain>();
        }
    }
}