using System;
using System.Collections.Generic;
using System.Linq;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Storage;

namespace GNIBIRPAndVisaAppointment.Web.Business.User
{
    public class UserManager : IUserManager
    {
        readonly IDomainHub DomainHub;
        readonly Table<DataAccess.Model.Storage.User> UserTable;

        public UserManager(IDomainHub domainHub, IStorageProvider storageProvider)
        {
            DomainHub = domainHub;
            UserTable = storageProvider.GetTable<DataAccess.Model.Storage.User>();
        }

        const string Normal = "Normal";

        public DataAccess.Model.Storage.User this[string id] => UserTable[id.Trim().ToUpper()].FirstOrDefault();
        
        public IEnumerable<DataAccess.Model.Storage.User> GetAllUsers()
        {
            return UserTable.GetAll();
        }

        public void Create(string id, string password, string name, string role)
        {
            id = id.Trim().ToUpper();

            var newUser = new DataAccess.Model.Storage.User
            {
                PartitionKey = id,
                RowKey = Normal,
                Id = id,
                Password = password,
                Name = name,
                Role = role
            };

            UserTable.Insert(newUser);
        }

        public bool Identify(string id, string password)
        {
            id = id.Trim().ToUpper();

            var user = UserTable[id].FirstOrDefault();

            return user != null && user.Password != password;
        }

        public void Update(string id, string password, string name, string role)
        {
            var user = UserTable[id, Normal];

            if (!string.IsNullOrEmpty(password))
            {
                user.Password = password;
            }
            
            user.Name = name;
            user.Role = role;

            UserTable.Replace(user);
        }
    }
}