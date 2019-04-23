using System;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage;

namespace GNIBIRPAndVisaAppointment.Web.Models
{
    public class AppointmentStatisticsModel
    {
        public AppointmentStatisticsModel() { }
        
        public AppointmentStatisticsModel(AppointmentStatistics appointmentStatistics)
        {
            StartTime = appointmentStatistics.StartTime;
            EndTime = appointmentStatistics.EndTime;

            ValidIRPAllNew = appointmentStatistics.ValidIRPAllNew;
            PublishIRPAllNew = appointmentStatistics.PublishIRPAllNew;
            TotalContinuousIRPAllNew  = appointmentStatistics.TotalContinuousIRPAllNew;
            ValidIRPAllRenew = appointmentStatistics.ValidIRPAllRenew;
            PublishIRPAllRenew = appointmentStatistics.PublishIRPAllRenew;
            TotalContinuousIRPAllRenew = appointmentStatistics.TotalContinuousIRPAllRenew;

            ValidIRPWorkNew = appointmentStatistics.ValidIRPWorkNew;
            PublishIRPWorkNew = appointmentStatistics.PublishIRPWorkNew;
            TotalContinuousIRPWorkNew = appointmentStatistics.TotalContinuousIRPWorkNew;
            ValidIRPWorkRenew = appointmentStatistics.ValidIRPWorkRenew;
            PublishIRPWorkRenew = appointmentStatistics.PublishIRPWorkRenew;
            TotalContinuousIRPWorkRenew = appointmentStatistics.TotalContinuousIRPWorkRenew;
            ValidIRPStudyNew = appointmentStatistics.ValidIRPStudyNew;
            PublishIRPStudyNew = appointmentStatistics.PublishIRPStudyNew;
            TotalContinuousIRPStudyNew = appointmentStatistics.TotalContinuousIRPStudyNew;
            ValidIRPStudyRenew = appointmentStatistics.ValidIRPStudyRenew;
            PublishIRPStudyRenew = appointmentStatistics.PublishIRPStudyRenew;
            TotalContinuousIRPStudyRenew = appointmentStatistics.TotalContinuousIRPStudyRenew;
            ValidIRPOtherNew = appointmentStatistics.ValidIRPOtherNew;
            PublishIRPOtherNew = appointmentStatistics.PublishIRPOtherNew;
            TotalContinuousIRPOtherNew = appointmentStatistics.TotalContinuousIRPOtherNew;
            ValidIRPOtherRenew = appointmentStatistics.ValidIRPOtherRenew;
            PublishIRPOtherRenew = appointmentStatistics.PublishIRPOtherRenew;
            TotalContinuousIRPOtherRenew = appointmentStatistics.TotalContinuousIRPOtherRenew;
            ValidVisaIndividual = appointmentStatistics.ValidVisaIndividual;
            PublishVisaIndividual = appointmentStatistics.PublishVisaIndividual;
            TotalContinuousVisaIndividual = appointmentStatistics.TotalContinuousVisaIndividual;
            ValidVisaFamily = appointmentStatistics.ValidVisaFamily;
            PublishVisaFamily = appointmentStatistics.PublishVisaFamily;
            TotalContinuousVisaFamily = appointmentStatistics.TotalContinuousVisaFamily;
            ValidVisaEmergency = appointmentStatistics.ValidVisaEmergency;
            PublishVisaEmergency = appointmentStatistics.PublishVisaEmergency;
            TotalContinuousVisaEmergency = appointmentStatistics.TotalContinuousVisaEmergency;
        }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        #region All
        public int ValidIRPAllNew { get; set; }
        public int PublishIRPAllNew { get; set; }
        public double TotalContinuousIRPAllNew { get; set; }
        public int ValidIRPAllRenew { get; set; }
        public int PublishIRPAllRenew { get; set; }
        public double TotalContinuousIRPAllRenew { get; set; }
        #endregion
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