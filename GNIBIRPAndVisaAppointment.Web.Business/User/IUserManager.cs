using System.Collections.Generic;

namespace GNIBIRPAndVisaAppointment.Web.Business.User
{
    public interface IUserManager : IDomain
    {
        DataAccess.Model.Storage.User this[string id] { get; }
        IEnumerable<DataAccess.Model.Storage.User> GetAllUsers();
        void Create(string id, string password, string name, string role);
        bool Identify(string id, string password);
        void Update(string id, string password, string name, string role);
    }
}