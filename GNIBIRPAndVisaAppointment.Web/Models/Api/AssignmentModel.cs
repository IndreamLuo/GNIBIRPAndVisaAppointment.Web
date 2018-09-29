using System;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage;

namespace GNIBIRPAndVisaAppointment.Web.Models.Api
{
    public class AssignmentModel
    {
        public AssignmentModel(Assignment entity)
        {
            Id = entity.Id;
            
            Application = new ApplicationModel(entity.Application);
            Order = new OrderModel(entity.Order);
        }

        public string Id { get; set; }

        public ApplicationModel Application { get; set; }
        public OrderModel Order { get; set; }
    }
}