﻿using ANZHFR.Data.Models;
using ANZHFR.Services.Synonyms;
using ANZHFR.Web.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace ANZHFR.Web.Models
{
    public class ReportQueryModel
    {
        public DateTime? DOB { get; set; }
        public long HospitalID { get; set; }
        public string Hospital { get; set; }
        public DateTime? ArrivalDateTime { get; set; }
        public DateTime? SurgeryDateTime { get; set; }
        public DateTime? DischargeDate { get; set; }
        public DateTime? WardDischargeDate { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public int? Age { get; set; }
        public string Sex { get; set; }
        public string FractureTypeID { get; set; }
        public string SurgeryDelayID { get; set; }
        public string FractureType { get; set; }
        public string SurgeryDelay { get; set; }
        public double LengthOfStay { get; set; }
        public string DischargeResID { get; set; }
        public string DischargeResName { get; set; }
        public int EstimatedRecords { get; set; }
        public string AdmissionViaED { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? InHospFractureDateTime { get; set; }
        public DateTime? TransferDateTime { get; set; }
        public DateTime? DepartureDateTime { get; set; }
        public DateTime? FollowupDate30 { get; set; }
        public string Survival30 { get; set; }
        public string CognitiveState { get; set; }
        public string Analgesia { get; set; }
        public string PreOpMedAss { get; set; }
        public string PainAssessment { get; set; }
        public string PainManagement { get; set; }
        public string Mobilisation { get; set; }
        public string FullWeightBear { get; set; }
        public string PressureUlcers { get; set; }
        public string BoneMedDischarge { get; set; }
        public string Reoperation30 { get; set; }
        public decimal? Completeness { get; set; }
        public DateTime? MyStart { get; set; }
        public DateTime? StartDate
        {
            get
            {
                if (this.AdmissionViaED == "1" && this.ArrivalDateTime.HasValue)
                {
                    return this.ArrivalDateTime;
                }
                else if (this.AdmissionViaED == "3" && this.InHospFractureDateTime.HasValue)
                {
                    return this.InHospFractureDateTime;
                }
                else if (this.AdmissionViaED == "3" && !this.InHospFractureDateTime.HasValue)
                {
                    return this.ArrivalDateTime;
                }
                else if (this.AdmissionViaED == "2" && this.ArrivalDateTime.HasValue)
                {
                    return this.ArrivalDateTime;
                }
                else if (this.AdmissionViaED == "2" && this.DepartureDateTime.HasValue)
                {
                    return this.DepartureDateTime;
                }
                else if (this.AdmissionViaED == "2" && this.TransferDateTime.HasValue)
                {
                    return this.TransferDateTime;
                }
                else if (this.AdmissionViaED == "2" && this.TransferDateTime.HasValue)
                {
                    return this.TransferDateTime;
                }
                else if (this.ArrivalDateTime.HasValue)
                {
                    return this.ArrivalDateTime;
                }
                else if (this.InHospFractureDateTime.HasValue)
                {
                    return this.InHospFractureDateTime;
                }

                return this.ArrivalDateTime;
            }
        }
    }

    public class ReportModel
    {
        public string Label { get; set; }
        public List<object> Data { get; set; }
    }
}