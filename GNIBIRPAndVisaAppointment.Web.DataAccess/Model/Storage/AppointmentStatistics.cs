using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage
{
    public class AppointmentStatistics : TableEntity
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int ValidIRPWorkNew { get; set; }
        public int PublishIRPWorkNew { get; set; }
        public double TotalContinuousIRPWorkNew { get; set; }
        public int ValidIRPWorkRenew { get; set; }
        public int PublishIRPWorkRenew { get; set; }
        public double TotalContinuousIRPWorkRenew { get; set; }
        public int ValidIRPStudyNew { get; set; }
        public int PublishIRPStudyNew { get; set; }
        public double TotalContinuousIRPStudyNew { get; set; }
        public int ValidIRPStudyRenew { get; set; }
        public int PublishIRPStudyRenew { get; set; }
        public double TotalContinuousIRPStudyRenew { get; set; }
        public int ValidIRPOtherNew { get; set; }
        public int PublishIRPOtherNew { get; set; }
        public double TotalContinuousIRPOtherNew { get; set; }
        public int ValidIRPOtherRenew { get; set; }
        public int PublishIRPOtherRenew { get; set; }
        public double TotalContinuousIRPOtherRenew { get; set; }
        public int ValidVisaIndividual { get; set; }
        public int PublishVisaIndividual { get; set; }
        public double TotalContinuousVisaIndividual { get; set; }
        public int ValidVisaFamily { get; set; }
        public int PublishVisaFamily { get; set; }
        public double TotalContinuousVisaFamily { get; set; }
        public int ValidVisaEmergency { get; set; }
        public int PublishVisaEmergency { get; set; }
        public double TotalContinuousVisaEmergency { get; set; }
    }
}