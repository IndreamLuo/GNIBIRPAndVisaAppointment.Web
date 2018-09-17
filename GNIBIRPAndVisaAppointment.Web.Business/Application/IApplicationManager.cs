namespace GNIBIRPAndVisaAppointment.Web.Business.Application
{
    public interface IApplicationManager : IDomain
    {
        DataAccess.Model.Storage.Application this[string applicationId] { get; }
        string CreateApplication(DataAccess.Model.Storage.Application application);
        string CreateOrder(DataAccess.Model.Storage.Order order);
        DataAccess.Model.Storage.Order GetOrder(string orderId);
        void Pending(string orderId);
        void Accept(string orderId);
        void Complete(string orderId, string appointmentNo);
        void Close(string orderId);
    }
}