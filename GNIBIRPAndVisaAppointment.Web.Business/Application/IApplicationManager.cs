using System;
using System.Collections.Generic;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage;

namespace GNIBIRPAndVisaAppointment.Web.Business.Application
{
    public interface IApplicationManager : IDomain
    {
        DataAccess.Model.Storage.Application this[string applicationId] { get; }
        string CreateApplication(DataAccess.Model.Storage.Application application);
        void ChangeGNIB(string id, bool hasGNIB, string gnibNo, string gnibExDT);
        string CreateOrder(DataAccess.Model.Storage.Order order);
        DataAccess.Model.Storage.Order GetOrder(string orderId);
        void Pending(string orderId);
        void Accept(string orderId);
        void Reject(string orderId);
        void Appoint(string orderId);
        void Reaccept(string orderId);
        void Duplicate(string orderId);
        void Unverify(string orderId);
        void Cancel(string orderId);
        void Complete(string orderId, string appointmentNo, DateTime time, string name, string category, string subCategory);
        void Complete(string orderId, string emailId);
        void Close(string orderId);
        Assignment GetAssignment(string orderId);
        List<Assignment> GetAssignments(string status, bool withDetails = false);
        Dictionary<string, List<Assignment>> CachedAssignments { get; }
        AppointmentLetter GetAppointmentLetter(string orderId);
        void AppointLog(string orderId, string slot, bool success, string result, double timeSpan);
    }
}