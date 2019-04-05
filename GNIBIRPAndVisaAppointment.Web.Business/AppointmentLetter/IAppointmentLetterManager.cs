using System;
using System.Collections.Generic;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage;

namespace GNIBIRPAndVisaAppointment.Web.Business.AppointmnetLetter
{
    public interface IAppointmentLetterManager : IDomain
    {
        List<AppointmentLetter> UnassignedLetters { get; }
        void SubmitLetter(string id, string appointmentNo, string name, DateTime time, string category, string subCategory);
        AppointmentLetter FindByName(string name);
        void Assign(string id, string applicationId);
    }
}