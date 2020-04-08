using ANZHFR.Data.Models;
using ANZHFR.Services.Synonyms;
using ANZHFR.Web.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace ANZHFR.Web.Models
{
    public class ExportPatientModel
    {
        private ANZHFREntities Entity = new ANZHFREntities();
        //private SynonymChildServices synonymService = new SynonymChildServices();

        #region Public Properties

        public string Name { get; set; }
        public string Surname { get; set; }
        public string MRN { get; set; }
        public string PostCode { get; set; }
        public string Phone { get; set; }
        public string DOB { get; set; }
        public string Sex { get; set; }
        public string Indig { get; set; }
        public int Age { get; set; }
        public string Medicare { get; set; }   
        public string PatientType { get; set; }
        public string UResidence { get; set; }
        public string AdmissionViaED { get; set; }
        public string TransferHospital { get; set; }
        public string TransferDate { get; set; }
        public string TransferTime { get; set; }
        public string ArrivalDate { get; set; }
        public string ArrivalTime { get; set; }
        public string DepartureDate { get; set; }
        public string DepartureTime { get; set; }
        public string InHospFractureDate { get; set; }
        public string InHospFractureTime { get; set; }
        public string WardType { get; set; }
        public string PreAdWalk { get; set; }
        public string PainAssessment { get; set; }
        public string PainManagement { get; set; }
        public string CognitiveAssessment { get; set; }
        public string CognitiveState { get; set; }
        public string BoneMed { get; set; }
        public string PreOpMedAss { get; set; }
        public string FractureSide { get; set; }
        public string AtypicalFracture { get; set; } 
        public string FractureType { get; set; }
        public string ASAgrade { get; set; }
        public string Surgery { get; set; }
        public string SurgeryDate { get; set; }
        public string SurgeryTime { get; set; }
        public string SurgeryDelay { get; set; }
        public string SurgeryDelayOther { get; set; }
        public string Anaesthesia { get; set; }
        public string Analgesia { get; set; }
        public string ConsultantPresent { get; set; }
        public string Operation { get; set; }
        public string InterOpFracture { get; set; }
        public string FullWeightBear { get; set; }
        public string Mobilisation { get; set; }
        public string FirstDayWalking { get; set; }
        public string DeleriumAssessment { get; set; }
        public string PressureUlcers { get; set; }
        public string Malnutrition { get; set; }
        public string GeriatricAssessment { get; set; }
        public string GeriatricAssDate { get; set; }
        public string FallsAssessment { get; set; }
        public string BoneMedDischarge { get; set; }
        public string WardDischargeDate { get; set; }
        public string DischargeDest { get; set; }
        public string HospitalDischargeDate { get; set; }
        public string DischargeResidence { get; set; }
        public int OLengthofStay { get; set; }
        public int HLengthofStay { get; set; }
        public string FollowupDate30 { get; set; }
        public string HealthServiceDischarge30 { get; set; }
        public string Survival30 { get; set; }      
        public string Residence30 { get; set; } 
        public string WeightBear30 { get; set; }
        public string WalkingAbility30 { get; set; }
        public string BoneMed30 { get; set; }
        public string Reoperation30 { get; set; }
        public string FollowupDate120 { get; set; }
        public string HealthServiceDischarge120 { get; set; }
        public string Survival120 { get; set; }
        public string Residence120 { get; set; }
        public string WeightBear120 { get; set; }
        public string WalkingAbility120 { get; set; }
        public string BoneMed120 { get; set; }
        public string Reoperation120 { get; set; }
        public string DeathDate { get; set; }

        //public decimal? Completeness { get; set; }
        //public string CompleteExemption { get; set; }
        //public string Created { get; set; }
        //public string Author { get; set; }
        //public string LastModified { get; set; }

        public string StartDate { get; set; }
        public bool Informed { get; set; }

        //public bool OptedOut { get; set; }
        public bool CannotFollowup { get; set; }

        #endregion Public Properties
    }

    public class ImportPatientModel
    {
        private ANZHFREntities Entity = new ANZHFREntities();
        public long HospitalID { get; set; }


        #region Table Properties

        public string Name { get; set; }
        public string Surname { get; set; }
        public string MRN { get; set; }
        public string PostCode { get; set; }
        public string Phone { get; set; }
        private string _DOB;

        public string DOB
        {
            get
            {
                double d;
                DateTime _date;
                if (double.TryParse(_DOB, out d))
                {
                    DateTime conv = DateTime.FromOADate(d);
                    return conv.ToString("yyyy/MM/dd");
                }
                else if (DateTime.TryParse(_DOB, out _date))
                {
                    return _date.ToString("yyyy/MM/dd");
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                _DOB = value;
            }
        }

        private string _sex;

        public string Sex
        {
            get
            {
                string s = "";
                var sex = Entity.Sexes.SingleOrDefault(x => x.Name == _sex);

                if (sex == null)
                {
                    _sex = _sex.Synonym();
                    sex = Entity.Sexes.SingleOrDefault(x => x.Name == _sex);
                }

                s = (sex == null ? _sex : sex.SexID.ToString());
                return s;
            }
            set
            {
                _sex = (value == null ? string.Empty : value);
            }
        }

        public string _Indig;

        public string Indig
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var indig = Entity.Indigs.SingleOrDefault(x => x.Name == _Indig);

                    if (indig == null)
                    {
                        _Indig = _Indig.Synonym();
                        indig = Entity.Indigs.SingleOrDefault(x => x.Name == _Indig);
                    }

                    s = (indig == null ? _Indig : indig.IndigID.ToString());
                }
                return s;
            }
            set
            {
                _Indig = (value == null ? string.Empty : value);
            }
        }

        public string _Ethnic;

        public string Ethnic
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var ethnic = Entity.Ethnics.SingleOrDefault(x => x.Name == _Ethnic);

                    if (ethnic == null)
                    {
                        _Ethnic = _Ethnic.Synonym();
                        ethnic = Entity.Ethnics.SingleOrDefault(x => x.Name == _Ethnic);
                    }

                    s = (ethnic == null ? _Ethnic : ethnic.EthnicID.ToString());
                }
                return s;
            }
            set
            {
                _Ethnic = (value == null ? string.Empty : value);
            }
        }

        public int Age { get; set; }
        public string Medicare { get; set; }

        private string _PatientType;

        public string PatientType
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var patientType = Entity.PatientTypes.SingleOrDefault(x => x.Name == _PatientType);
                    if (patientType == null)
                    {
                        _PatientType = _PatientType.Synonym();
                        patientType = Entity.PatientTypes.SingleOrDefault(x => x.Name == _PatientType);
                    }
                    s = (patientType == null ? _PatientType : patientType.PatientTypeID.ToString());
                }
                return s;
            }
            set
            {
                _PatientType = (value == null ? string.Empty : value);
            }
        }

        private string _AdmissionViaED;

        public string AdmissionViaED
        {
            get
            {
                string s = "";
                var AdmissionViaED = Entity.AdmissionViaEDs.SingleOrDefault(x => x.Name == _AdmissionViaED);
                if (AdmissionViaED == null)
                {
                    _AdmissionViaED = _AdmissionViaED.Synonym();
                    AdmissionViaED = Entity.AdmissionViaEDs.SingleOrDefault(x => x.Name == _AdmissionViaED);
                }
                s = (AdmissionViaED == null ? _AdmissionViaED : AdmissionViaED.AdmissionViaEDID.ToString()).ToString();
                return s;
            }
            set
            {
                _AdmissionViaED = (value == null ? string.Empty : value);
            }
        }

        private string _UResidence;
        public string UResidence
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var r = Entity.Residences.SingleOrDefault(x => x.Address == _UResidence);
                    if (r == null)
                    {
                        _UResidence = _UResidence.Synonym();
                        r = Entity.Residences.SingleOrDefault(x => x.Address == _UResidence);
                    }
                    s = (r == null ? _UResidence : r.ResidenceID.ToString());
                }
                return s;
            }
            set
            {
                _UResidence = (value == null ? string.Empty : value);
            }
        }

        private string _TransferHospital;
        public string TransferHospital
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var hospital = Entity.TransferHospitals.SingleOrDefault(x => x.Name == _TransferHospital && x.HospitalID == this.HospitalID);

                    s = (hospital == null ? _TransferHospital : hospital.TransferHospitalID.ToString());
                }
                return s;
            }
            set
            {
                _TransferHospital = (value == null ? string.Empty : value);
            }
        }

        private string _TransferDate;

        public string TransferDate
        {
            get
            {
                double d;
                DateTime _date;
                if (double.TryParse(_TransferDate, out d))
                {
                    DateTime conv = DateTime.FromOADate(d);
                    return conv.ToString("yyyy/MM/dd");
                }
                else if (DateTime.TryParse(_TransferDate, out _date))
                {
                    return _date.ToString("yyyy/MM/dd");
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                _TransferDate = value;
            }
        }

        private string _TransferTime;

        public string TransferTime
        {
            get
            {
                DateTime _time;
                if (DateTime.TryParse(_TransferTime, out _time))
                {
                    return _time.ToString("hh:mm tt");
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                _TransferTime = value;
            }
        }

        private string _ArrivalDate;

        public string ArrivalDate
        {
            get
            {
                double d;
                DateTime _date;
                if (double.TryParse(_ArrivalDate, out d))
                {
                    DateTime conv = DateTime.FromOADate(d);
                    return conv.ToString("yyyy/MM/dd");
                }
                else if (DateTime.TryParse(_ArrivalDate, out _date))
                {
                    return _date.ToString("yyyy/MM/dd");
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                _ArrivalDate = value;
            }
        }

        private string _ArrivalTime;

        public string ArrivalTime
        {
            get
            {
                DateTime _time;
                if (DateTime.TryParse(_ArrivalTime, out _time))
                {
                    return _time.ToString("hh:mm tt");
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                _ArrivalTime = value;
            }
        }

        private string _DepartureDate;

        public string DepartureDate
        {
            get
            {
                double d;
                DateTime _date;
                if (double.TryParse(_DepartureDate, out d))
                {
                    DateTime conv = DateTime.FromOADate(d);
                    return conv.ToString("yyyy/MM/dd");
                }
                else if (DateTime.TryParse(_DepartureDate, out _date))
                {
                    return _date.ToString("yyyy/MM/dd");
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                _DepartureDate = value;
            }
        }

        private string _DepartureTime;

        public string DepartureTime
        {
            get
            {
                DateTime _time;
                if (DateTime.TryParse(_DepartureTime, out _time))
                {
                    return _time.ToString("hh:mm tt");
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                _DepartureTime = value;
            }
        }

        private string _InHospFractureDate;

        public string InHospFractureDate
        {
            get
            {
                double d;
                DateTime _date;
                if (double.TryParse(_InHospFractureDate, out d))
                {
                    DateTime conv = DateTime.FromOADate(d);
                    return conv.ToString("yyyy/MM/dd");
                }
                else if (DateTime.TryParse(_InHospFractureDate, out _date))
                {
                    return _date.ToString("yyyy/MM/dd");
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                _InHospFractureDate = value;
            }
        }

        private string _InHospFractureTime;

        public string InHospFractureTime
        {
            get
            {
                DateTime _time;
                if (DateTime.TryParse(_InHospFractureTime, out _time))
                {
                    return _time.ToString("hh:mm tt");
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                _InHospFractureTime = value;
            }
        }

        private string _WardType;

        public string WardType
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var w = Entity.WardTypes.SingleOrDefault(x => x.Name == _WardType);
                    if (w == null)
                    {
                        _WardType = _WardType.Synonym();
                        w = Entity.WardTypes.SingleOrDefault(x => x.Name == _WardType);
                    }
                    s = (w == null ? _WardType : w.WardTypeID.ToString());
                }
                return s;
            }
            set
            {
                _WardType = (value == null ? string.Empty : value);
            }
        }

        private string _PreAdWalk;

        public string PreAdWalk
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var p = Entity.PreAdWalks.SingleOrDefault(x => x.Name == _PreAdWalk);
                    if (p == null)
                    {
                        _PreAdWalk = _PreAdWalk.Synonym();
                        p = Entity.PreAdWalks.SingleOrDefault(x => x.Name == _PreAdWalk);
                    }
                    s = (p == null ? _PreAdWalk : p.PreAdWalkID.ToString());
                }
                return s;
            }
            set
            {
                _PreAdWalk = (value == null ? string.Empty : value);
            }
        }

        private string _CognitiveAssessment;

        public string CognitiveAssessment
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var c = Entity.CognitiveAssessments.SingleOrDefault(x => x.Name == _CognitiveAssessment);
                    if (c == null)
                    {
                        _CognitiveAssessment = _CognitiveAssessment.Synonym();
                        c = Entity.CognitiveAssessments.SingleOrDefault(x => x.Name == _CognitiveAssessment);
                    }
                    s = (c == null ? _CognitiveAssessment : c.CognitiveAssessmentID.ToString());
                }
                return s;
            }
            set
            {
                _CognitiveAssessment = (value == null ? string.Empty : value);
            }
        }

        private string _PainAssessment;

        public string PainAssessment
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var c = Entity.PainAssessments.SingleOrDefault(x => x.Name == _PainAssessment);
                    if (c == null)
                    {
                        _PainAssessment = _PainAssessment.Synonym();
                        c = Entity.PainAssessments.SingleOrDefault(x => x.Name == _PainAssessment);
                    }
                    s = (c == null ? _PainAssessment : c.PainAssessmentID.ToString());
                }
                return s;
            }
            set
            {
                _PainAssessment = (value == null ? string.Empty : value);
            }
        }

        private string _PainManagement;

        public string PainManagement
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var c = Entity.PainManagements.SingleOrDefault(x => x.Name == _PainManagement);
                    if (c == null)
                    {
                        _PainManagement = _PainManagement.Synonym();
                        c = Entity.PainManagements.SingleOrDefault(x => x.Name == _PainManagement);
                    }
                    s = (c == null ? _PainManagement : c.PainManagementID.ToString());
                }
                return s;
            }
            set
            {
                _PainManagement = (value == null ? string.Empty : value);
            }
        }

        private string _CognitiveState;

        public string CognitiveState
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var c = Entity.CognitiveStates.SingleOrDefault(x => x.Name == _CognitiveState);
                    if (c == null)
                    {
                        _CognitiveState = _CognitiveState.Synonym();
                        c = Entity.CognitiveStates.SingleOrDefault(x => x.Name == _CognitiveState);
                    }
                    s = (c == null ? _CognitiveState : c.CognitiveStateID.ToString());
                }
                return s;
            }
            set
            {
                _CognitiveState = (value == null ? string.Empty : value);
            }
        }

        private string _BoneMed;

        public string BoneMed
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var b = Entity.BoneMeds.SingleOrDefault(x => x.Name == _BoneMed);
                    if (b == null)
                    {
                        _BoneMed = _BoneMed.Synonym();
                        b = Entity.BoneMeds.SingleOrDefault(x => x.Name == _BoneMed);
                    }
                    s = (b == null ? _BoneMed : b.BoneMedID.ToString());
                }
                return s;
            }
            set
            {
                _BoneMed = (value == null ? string.Empty : value);
            }
        }

        private string _PreOpMedAss;

        public string PreOpMedAss
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var p = Entity.PreOpMedAsses.SingleOrDefault(x => x.Name == _PreOpMedAss);
                    if (p == null)
                    {
                        _PreOpMedAss = _PreOpMedAss.Synonym();
                        p = Entity.PreOpMedAsses.SingleOrDefault(x => x.Name == _PreOpMedAss);
                    }
                    s = (p == null ? _PreOpMedAss : p.PreOpMedAssID.ToString());
                }
                return s;
            }
            set
            {
                _PreOpMedAss = (value == null ? string.Empty : value);
            }
        }

        private string _FractureSide;

        public string FractureSide
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var f = Entity.FractureSides.SingleOrDefault(x => x.Name == _FractureSide);
                    if (f == null)
                    {
                        _FractureSide = _FractureSide.Synonym();
                        f = Entity.FractureSides.SingleOrDefault(x => x.Name == _FractureSide);
                    }
                    if (f == null)
                    {
                        _FractureSide = "Left";
                        f = Entity.FractureSides.SingleOrDefault(x => x.Name == _FractureSide);
                    }
                    s = (f == null ? _FractureSide : f.FractureSideID.ToString());
                }
                return s;
            }
            set
            {
                _FractureSide = (value == null ? string.Empty : value);
            }
        }

        private string _AtypicalFracture;

        public string AtypicalFracture
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var a = Entity.AtypicalFractures.SingleOrDefault(x => x.Name == _AtypicalFracture);
                    if (a == null)
                    {
                        _AtypicalFracture = _AtypicalFracture.Synonym();
                        a = Entity.AtypicalFractures.SingleOrDefault(x => x.Name == _AtypicalFracture);
                    }
                    s = (a == null ? _AtypicalFracture : a.AtypicalFractureID.ToString());
                }
                return s;
            }
            set
            {
                _AtypicalFracture = (value == null ? string.Empty : value);
            }
        }

        private string _FractureType;

        public string FractureType
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var f = Entity.FractureTypes.SingleOrDefault(x => x.Name == _FractureType);
                    if (f == null)
                    {
                        _FractureType = _FractureType.Synonym();
                        f = Entity.FractureTypes.SingleOrDefault(x => x.Name == _FractureType);
                    }
                    s = (f == null ? _FractureType : f.FractureTypeID.ToString());
                }
                return s;
            }
            set
            {
                _FractureType = (value == null ? string.Empty : value);
            }
        }

        private string _ASAGrade;

        public string ASAgrade
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var f = Entity.ASAGrades.SingleOrDefault(x => x.Name == _ASAGrade);
                    if (f == null)
                    {
                        ASAgrade = _ASAGrade.Synonym();
                        f = Entity.ASAGrades.SingleOrDefault(x => x.Name == _ASAGrade);
                    }
                    s = (f == null ? _ASAGrade : f.ASAGradeID.ToString());
                }
                return s;
            }
            set
            {
                _ASAGrade = (value == null ? string.Empty : value);
            }
        }

        private string _Surgery;

        public string Surgery
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var f = Entity.Surgeries.SingleOrDefault(x => x.Name == _Surgery);
                    if (f == null)
                    {
                        Surgery = _Surgery.Synonym();
                        f = Entity.Surgeries.SingleOrDefault(x => x.Name == _Surgery);
                    }
                    s = (f == null ? _Surgery : f.SurgeryID.ToString());
                }
                return s;
            }
            set
            {
                _Surgery = (value == null ? string.Empty : value);
            }
        }

        private string _SurgeryDate;

        public string SurgeryDate
        {
            get
            {
                double d;
                DateTime _date;
                if (double.TryParse(_SurgeryDate, out d))
                {
                    DateTime conv = DateTime.FromOADate(d);
                    return conv.ToString("yyyy/MM/dd");
                }
                else if (DateTime.TryParse(_SurgeryDate, out _date))
                {
                    return _date.ToString("yyyy/MM/dd");
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                _SurgeryDate = value;
            }
        }

        private string _SurgeryTime;

        public string SurgeryTime
        {
            get
            {
                DateTime _time;
                if (DateTime.TryParse(_SurgeryTime, out _time))
                {
                    return _time.ToString("hh:mm tt");
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                _SurgeryTime = value;
            }
        }

        private string _SurgeryDelay;

        public string SurgeryDelay
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var f = Entity.SurgeryDelays.SingleOrDefault(x => x.Name == _SurgeryDelay);
                    if (f == null)
                    {
                        _SurgeryDelay = _SurgeryDelay.Synonym();
                        f = Entity.SurgeryDelays.SingleOrDefault(x => x.Name == _SurgeryDelay);
                    }
                    s = (f == null ? _SurgeryDelay : f.SurgeryDelayID.ToString());
                }
                return s;
            }
            set
            {
                _SurgeryDelay = (value == null ? string.Empty : value);
            }
        }

        public string SurgeryDelayOther { get; set; }

        private string _Anaesthesia;

        public string Anaesthesia
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var a = Entity.Anaesthesias.SingleOrDefault(x => x.Name == _Anaesthesia);
                    if (a == null)
                    {
                        _Anaesthesia = _Anaesthesia.Synonym();
                        a = Entity.Anaesthesias.SingleOrDefault(x => x.Name == _Anaesthesia);
                    }
                    s = (a == null ? _Anaesthesia : a.AnaesthesiaID.ToString());
                }
                return s;
            }
            set
            {
                _Anaesthesia = (value == null ? string.Empty : value);
            }
        }

        private string _Analgesia;

        public string Analgesia
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var a = Entity.Analgesias.SingleOrDefault(x => x.Name == _Analgesia);
                    if (a == null)
                    {
                        _Analgesia = _Analgesia.Synonym();
                        a = Entity.Analgesias.SingleOrDefault(x => x.Name == _Analgesia);
                    }
                    s = (a == null ? _Analgesia : a.AnalgesiaID.ToString());
                }
                return s;
            }
            set
            {
                _Analgesia = (value == null ? string.Empty : value);
            }
        }

        private string _ConsultantPresent;

        public string ConsultantPresent
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var a = Entity.ConsultantPresents.SingleOrDefault(x => x.Name == _ConsultantPresent);
                    if (a == null)
                    {
                        _ConsultantPresent = _ConsultantPresent.Synonym();
                        a = Entity.ConsultantPresents.SingleOrDefault(x => x.Name == _ConsultantPresent);
                    }
                    s = (a == null ? _ConsultantPresent : a.ConsultantPresentID.ToString());
                }
                return s;
            }
            set
            {
                _ConsultantPresent = (value == null ? string.Empty : value);
            }
        }

        private string _Operation;

        public string Operation
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var a = Entity.Operations.SingleOrDefault(x => x.Name == _Operation);
                    if (a == null)
                    {
                        _Operation = _Operation.Synonym();
                        a = Entity.Operations.SingleOrDefault(x => x.Name == _Operation);
                    }
                    s = (a == null ? _Operation : a.OperationID.ToString());
                }
                return s;
            }
            set
            {
                _Operation = (value == null ? string.Empty : value);
            }
        }

        private string _InterOpFracture;

        public string InterOpFracture
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var a = Entity.InterOpFractures.SingleOrDefault(x => x.Name == _InterOpFracture);
                    if (a == null)
                    {
                        _InterOpFracture = _InterOpFracture.Synonym();
                        a = Entity.InterOpFractures.SingleOrDefault(x => x.Name == _InterOpFracture);
                    }
                    s = (a == null ? _InterOpFracture : a.InterOpFractureID.ToString());
                }
                return s;
            }
            set
            {
                _InterOpFracture = (value == null ? string.Empty : value);
            }
        }

        private string _FullWeightBear;

        public string FullWeightBear
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var a = Entity.WeightBears.SingleOrDefault(x => x.Name == _FullWeightBear);
                    if (a == null)
                    {
                        _FullWeightBear = _FullWeightBear.Synonym();
                        a = Entity.WeightBears.SingleOrDefault(x => x.Name == _FullWeightBear);
                    }
                    s = (a == null ? _FullWeightBear : a.WeightBearID.ToString());
                }
                return s;
            }
            set
            {
                _FullWeightBear = (value == null ? string.Empty : value);
            }
        }

        private string _Mobilisation;

        public string Mobilisation
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var a = Entity.Mobilisations.SingleOrDefault(x => x.Name == _Mobilisation);
                    if (a == null)
                    {
                        _Mobilisation = _Mobilisation.Synonym();
                        a = Entity.Mobilisations.SingleOrDefault(x => x.Name == _Mobilisation);
                    }
                    s = (a == null ? _Mobilisation : a.MobilisationID.ToString());
                }
                return s;
            }
            set
            {
                _Mobilisation = (value == null ? string.Empty : value);
            }
        }

        private string _FirstDaywalking;

        public string FirstDayWalking
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var a = Entity.FirstDayWalkings.SingleOrDefault(x => x.Name == _FirstDaywalking);
                    if (a == null)
                    {
                        _FirstDaywalking = _FirstDaywalking.Synonym();
                        a = Entity.FirstDayWalkings.SingleOrDefault(x => x.Name == _FirstDaywalking);
                    }
                    s = (a == null ? _FirstDaywalking : a.FirstDayWalkingID.ToString());
                }
                return s;
            }
            set
            {
                _FirstDaywalking = (value == null ? string.Empty : value);
            }
        }

        private string _PressureUlcers;

        public string PressureUlcers
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var a = Entity.PressureUlcers.SingleOrDefault(x => x.Name == _PressureUlcers);
                    if (a == null)
                    {
                        _PressureUlcers = _PressureUlcers.Synonym();
                        a = Entity.PressureUlcers.SingleOrDefault(x => x.Name == _PressureUlcers);
                    }
                    s = (a == null ? _PressureUlcers : a.PressureUlcersID.ToString());
                }
                return s;
            }
            set
            {
                _PressureUlcers = (value == null ? string.Empty : value);
            }
        }

        private string _Malnutrition;

        public string Malnutrition
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var a = Entity.Malnutritions.SingleOrDefault(x => x.Name == _Malnutrition);
                    if (a == null)
                    {
                        _Malnutrition = _Malnutrition.Synonym();
                        a = Entity.Malnutritions.SingleOrDefault(x => x.Name == _Malnutrition);
                    }
                    s = (a == null ? _Malnutrition : a.MalnutritionID.ToString());
                }
                return s;
            }
            set
            {
                _Malnutrition = (value == null ? string.Empty : value);
            }
        }

        private string _GeriatricAssessment;

        public string GeriatricAssessment
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var a = Entity.GeriatricAssessments.SingleOrDefault(x => x.Name == _GeriatricAssessment);
                    if (a == null)
                    {
                        _GeriatricAssessment = _GeriatricAssessment.Synonym();
                        a = Entity.GeriatricAssessments.SingleOrDefault(x => x.Name == _GeriatricAssessment);
                    }
                    s = (a == null ? _GeriatricAssessment : a.GeriatricAssessmentID.ToString());
                }
                return s;
            }
            set
            {
                _GeriatricAssessment = (value == null ? string.Empty : value);
            }
        }

        private string _GeriatricAssDate;

        public string GeriatricAssDate
        {
            get
            {
                double d;
                DateTime _date;
                if (double.TryParse(_GeriatricAssDate, out d))
                {
                    DateTime conv = DateTime.FromOADate(d);
                    return conv.ToString("yyyy/MM/dd");
                }
                else if (DateTime.TryParse(_GeriatricAssDate, out _date))
                {
                    return _date.ToString("yyyy/MM/dd");
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                _GeriatricAssDate = value;
            }
        }

        //private string _GeriatricAssTime;
        //public string GeriatricAssTime
        //{
        //    get
        //    {
        //        DateTime _time;
        //        if (DateTime.TryParse(_GeriatricAssTime, out _time))
        //        {
        //            return _time.ToString("hh:mm tt");
        //        }
        //        else
        //        {
        //            return string.Empty;
        //        }
        //    }
        //    set
        //    {
        //        _GeriatricAssTime = value;
        //    }
        //}

        private string _FallsAssessment;

        public string FallsAssessment
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var a = Entity.FallsAssessments.SingleOrDefault(x => x.Name == _FallsAssessment);
                    if (a == null)
                    {
                        _FallsAssessment = _FallsAssessment.Synonym();
                        a = Entity.FallsAssessments.SingleOrDefault(x => x.Name == _FallsAssessment);
                    }
                    s = (a == null ? _FallsAssessment : a.FallsAssessmentID.ToString());
                }
                return s;
            }
            set
            {
                _FallsAssessment = (value == null ? string.Empty : value);
            }
        }

        private string _BoneMedDischarge;

        public string BoneMedDischarge
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var b = Entity.BoneMeds.SingleOrDefault(x => x.Name == _BoneMedDischarge);
                    if (b == null)
                    {
                        _BoneMedDischarge = _BoneMedDischarge.Synonym();
                        b = Entity.BoneMeds.SingleOrDefault(x => x.Name == _BoneMedDischarge);
                    }
                    s = (b == null ? _BoneMedDischarge : b.BoneMedID.ToString());
                }
                return s;
            }
            set
            {
                _BoneMedDischarge = (value == null ? string.Empty : value);
            }
        }

        private string _WardDischargeDate;

        public string WardDischargeDate
        {
            get
            {
                double d;
                DateTime _date;
                if (double.TryParse(_WardDischargeDate, out d))
                {
                    DateTime conv = DateTime.FromOADate(d);
                    return conv.ToString("yyyy/MM/dd");
                }
                else if (DateTime.TryParse(_WardDischargeDate, out _date))
                {
                    return _date.ToString("yyyy/MM/dd");
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                _WardDischargeDate = value;
            }
        }

        private string _DischargeDest;

        public string DischargeDest
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var a = Entity.DischargeDests.SingleOrDefault(x => x.Name == _DischargeDest);
                    if (a == null)
                    {
                        _DischargeDest = _DischargeDest.Synonym();
                        a = Entity.DischargeDests.SingleOrDefault(x => x.Name == _DischargeDest);
                    }
                    s = (a == null ? _DischargeDest : a.DischargeDestID.ToString());
                }
                return s;
            }
            set
            {
                _DischargeDest = (value == null ? string.Empty : value);
            }
        }

        private string _HospitalDischargeDate;

        public string HospitalDischargeDate
        {
            get
            {
                double d;
                DateTime _date;
                if (double.TryParse(_HospitalDischargeDate, out d))
                {
                    DateTime conv = DateTime.FromOADate(d);
                    return conv.ToString("yyyy/MM/dd");
                }
                else if (DateTime.TryParse(_HospitalDischargeDate, out _date))
                {
                    return _date.ToString("yyyy/MM/dd");
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                _HospitalDischargeDate = value;
            }
        }

        public int OLengthofStay { get; set; }
        public int HLengthofStay { get; set; }

        private string _DischargeResidence;

        public string DischargeResidence
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var a = Entity.DischargeResidences.SingleOrDefault(x => x.Address == _DischargeResidence);
                    if (a == null)
                    {
                        _DischargeResidence = _DischargeResidence.Synonym();
                        a = Entity.DischargeResidences.SingleOrDefault(x => x.Address == _DischargeResidence);
                    }
                    s = (a == null ? _DischargeResidence : a.DischargeResidenceID.ToString());
                }
                return s;
            }
            set
            {
                _DischargeResidence = (value == null ? string.Empty : value);
            }
        }

        private string _FollowupDate30;

        public string FollowupDate30
        {
            get
            {
                double d;
                DateTime _date;
                if (double.TryParse(_FollowupDate30, out d))
                {
                    DateTime conv = DateTime.FromOADate(d);
                    return conv.ToString("yyyy/MM/dd");
                }
                else if (DateTime.TryParse(_FollowupDate30, out _date))
                {
                    return _date.ToString("yyyy/MM/dd");
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                _FollowupDate30 = value;
            }
        }

        public string HealthServiceDischarge30 { get; set; }

        private string _Survival30;

        public string Survival30
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var a = Entity.Survivals.SingleOrDefault(x => x.Name == _Survival30);
                    if (a == null)
                    {
                        _Survival30 = _Survival30.Synonym();
                        a = Entity.Survivals.SingleOrDefault(x => x.Name == _Survival30);
                    }
                    s = (a == null ? _Survival30 : a.SurvivalID.ToString());
                }
                return s;
            }
            set
            {
                _Survival30 = (value == null ? string.Empty : value);
            }
        }

        private string _Residence30;

        public string Residence30
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var a = Entity.DischargeDests.SingleOrDefault(x => x.Name == _Residence30);
                    if (a == null)
                    {
                        _Residence30 = _Residence30.Synonym();
                        a = Entity.DischargeDests.SingleOrDefault(x => x.Name == _Residence30);
                    }
                    s = (a == null ? _Residence30 : a.DischargeDestID.ToString());
                }
                return s;
            }
            set
            {
                _Residence30 = (value == null ? string.Empty : value);
            }
        }

        private string _WeightBear30;

        public string WeightBear30
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var a = Entity.WeightBears.SingleOrDefault(x => x.Name == _WeightBear30);
                    if (a == null)
                    {
                        _WeightBear30 = _WeightBear30.Synonym();
                        a = Entity.WeightBears.SingleOrDefault(x => x.Name == _WeightBear30);
                    }
                    s = (a == null ? _WeightBear30 : a.WeightBearID.ToString());
                }
                return s;
            }
            set
            {
                _WeightBear30 = (value == null ? string.Empty : value);
            }
        }

        private string _WalkingAbility30;

        public string WalkingAbility30
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var a = Entity.WalkingAbilities.SingleOrDefault(x => x.Name == _WalkingAbility30);
                    if (a == null)
                    {
                        _WalkingAbility30 = _WalkingAbility30.Synonym();
                        a = Entity.WalkingAbilities.SingleOrDefault(x => x.Name == _WalkingAbility30);
                    }
                    s = (a == null ? _WalkingAbility30 : a.WalkingAbilityID.ToString());
                }
                return s;
            }
            set
            {
                _WalkingAbility30 = (value == null ? string.Empty : value);
            }
        }

        private string _BoneMed30;

        public string BoneMed30
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var a = Entity.BoneMeds.SingleOrDefault(x => x.Name == _BoneMed30);
                    if (a == null)
                    {
                        _BoneMed30 = _BoneMed30.Synonym();
                        a = Entity.BoneMeds.SingleOrDefault(x => x.Name == _BoneMed30);
                    }
                    s = (a == null ? _BoneMed30 : a.BoneMedID.ToString());
                }
                return s;
            }
            set
            {
                _BoneMed30 = (value == null ? string.Empty : value);
            }
        }

        private string _Reoperation30;

        public string Reoperation30
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var a = Entity.Reoperations.SingleOrDefault(x => x.Name == _Reoperation30);
                    if (a == null)
                    {
                        _Reoperation30 = _Reoperation30.Synonym();
                        a = Entity.Reoperations.SingleOrDefault(x => x.Name == _Reoperation30);
                    }
                    s = (a == null ? _Reoperation30 : a.ReoperationID.ToString());
                }
                return s;
            }
            set
            {
                _Reoperation30 = (value == null ? string.Empty : value);
            }
        }

        private string _FollowupDate120;

        public string FollowupDate120
        {
            get
            {
                double d;
                DateTime _date;
                if (double.TryParse(_FollowupDate120, out d))
                {
                    DateTime conv = DateTime.FromOADate(d);
                    return conv.ToString("yyyy/MM/dd");
                }
                else if (DateTime.TryParse(_FollowupDate120, out _date))
                {
                    return _date.ToString("yyyy/MM/dd");
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                _FollowupDate120 = value;
            }
        }

        public string HealthServiceDischarge120 { get; set; }

        private string _Survival120;

        public string Survival120
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var a = Entity.Survivals.SingleOrDefault(x => x.Name == _Survival120);
                    if (a == null)
                    {
                        _Survival120 = _Survival120.Synonym();
                        a = Entity.Survivals.SingleOrDefault(x => x.Name == _Survival120);
                    }
                    s = (a == null ? _Survival120 : a.SurvivalID.ToString());
                }
                return s;
            }
            set
            {
                _Survival120 = (value == null ? string.Empty : value);
            }
        }

        private string _Residence120;

        public string Residence120
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var a = Entity.DischargeDests.SingleOrDefault(x => x.Name == _Residence120);
                    if (a == null)
                    {
                        _Residence120 = _Residence120.Synonym();
                        a = Entity.DischargeDests.SingleOrDefault(x => x.Name == _Residence120);
                    }
                    s = (a == null ? _Residence120 : a.DischargeDestID.ToString());
                }
                return s;
            }
            set
            {
                _Residence120 = (value == null ? string.Empty : value);
            }
        }

        private string _WeightBear120;

        public string WeightBear120
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var a = Entity.WeightBears.SingleOrDefault(x => x.Name == _WeightBear120);
                    if (a == null)
                    {
                        _WeightBear120 = _WeightBear120.Synonym();
                        a = Entity.WeightBears.SingleOrDefault(x => x.Name == _WeightBear120);
                    }
                    s = (a == null ? _WeightBear120 : a.WeightBearID.ToString());
                }
                return s;
            }
            set
            {
                _WeightBear120 = (value == null ? string.Empty : value);
            }
        }

        private string _WalkingAbility120;

        public string WalkingAbility120
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var a = Entity.WalkingAbilities.SingleOrDefault(x => x.Name == _WalkingAbility120);
                    if (a == null)
                    {
                        _WalkingAbility120 = _WalkingAbility120.Synonym();
                        a = Entity.WalkingAbilities.SingleOrDefault(x => x.Name == _WalkingAbility120);
                    }
                    s = (a == null ? _WalkingAbility120 : a.WalkingAbilityID.ToString());
                }
                return s;
            }
            set
            {
                _WalkingAbility120 = (value == null ? string.Empty : value);
            }
        }

        private string _BoneMed120;

        public string BoneMed120
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var a = Entity.BoneMeds.SingleOrDefault(x => x.Name == _BoneMed120);
                    if (a == null)
                    {
                        _BoneMed120 = _BoneMed120.Synonym();
                        a = Entity.BoneMeds.SingleOrDefault(x => x.Name == _BoneMed120);
                    }
                    s = (a == null ? _BoneMed120 : a.BoneMedID.ToString());
                }
                return s;
            }
            set
            {
                _BoneMed120 = (value == null ? string.Empty : value);
            }
        }

        private string _Reoperation120;

        public string Reoperation120
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var a = Entity.Reoperations.SingleOrDefault(x => x.Name == _Reoperation120);
                    if (a == null)
                    {
                        _Reoperation120 = _Reoperation120.Synonym();
                        a = Entity.Reoperations.SingleOrDefault(x => x.Name == _Reoperation120);
                    }
                    s = (a == null ? _Reoperation120 : a.ReoperationID.ToString());
                }
                return s;
            }
            set
            {
                _Reoperation120 = (value == null ? string.Empty : value);
            }
        }
        private string _DeathDate;
        public string DeathDate
        {
            get
            {
                double d;
                DateTime _date;
                if (double.TryParse(_DeathDate, out d))
                {
                    DateTime conv = DateTime.FromOADate(d);
                    return conv.ToString("yyyy/MM/dd");
                }
                else if (DateTime.TryParse(_DeathDate, out _date))
                {
                    return _date.ToString("yyyy/MM/dd");
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                _DeathDate = value;
            }
        }
        public decimal Completeness { get; set; }
        public string CompleteExemption { get; set; }
        public string Created { get; set; }
        public string Author { get; set; }
        public string LastModified { get; set; }
        public string StartDate { get; set; }
        private string _DeleriumAssessment;

        public string DeleriumAssessment
        {
            get
            {
                string s = "";
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var a = Entity.DeleriumAssessments.SingleOrDefault(x => x.Name == _DeleriumAssessment);
                    if (a == null)
                    {
                        _DeleriumAssessment = _DeleriumAssessment.Synonym();
                        a = Entity.DeleriumAssessments.SingleOrDefault(x => x.Name == _DeleriumAssessment);
                    }
                    s = (a == null ? _DeleriumAssessment : a.DeleriumAssessmentID.ToString());
                }
                return s;
            }
            set
            {
                _DeleriumAssessment = (value == null ? string.Empty : value);
            }
        }

        public bool Informed { get; set; }
        public bool OptedOut { get; set; }
        public bool CannotFollowup { get; set; }
        public string EQ5D_Mobility { get; set; }
        public string EQ5D_SelfCare { get; set; }
        public string EQ5D_UsualActivity { get; set; }
        public string EQ5D_Pain{ get; set; }
        public string EQ5D_Anxiety { get; set; }
        public short EQ5D_Health { get; set; }

        #endregion Table Properties

        public int Line { get; set; }
        public string Column { get; set; }
        public List<string> Messages { get; set; }

        public bool IsValid
        {
            get
            {
                Messages = new List<string>();

                DateTime dateOfBirth;
                if (DateTime.TryParse(DOB, out dateOfBirth))
                 
                {
                    if (dateOfBirth.Year < DateTime.Today.AddYears(-120).Year || dateOfBirth.Year > DateTime.Today.AddYears(-49).Year)
                                            {
                        if (dateOfBirth.AddYears(-100).Year < DateTime.Today.AddYears(-120).Year || dateOfBirth.AddYears(-100).Year > DateTime.Today.AddYears(-49).Year)
                        {
                            Messages.Add(string.Format("Date of birth ({0:dd-MMM-yyyy}) is outside allowable date range of between {2} and {3} in line {1} - {4}.", dateOfBirth, Line, DateTime.Today.AddYears(-120).Year, DateTime.Today.AddYears(-49).Year, Name + " " + Surname));
                        }
                        else
                        {
                            dateOfBirth = dateOfBirth.AddYears(-100);
                        }
                    }
                }

                short a = 0;
                if (Sex != "0" && short.TryParse(Sex, out a))
                {
                    var IDs = Entity.Sexes.Select(x => x.SexID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column Sex.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(Sex))
                {
                    Messages.Add(string.Format("Value {0} is not the correct type in line {1} column Sex - {2}.", Sex, Line, Name + " " + Surname));
                }

                if (Indig != "0" && short.TryParse(Indig, out a))
                {
                    var IDs = Entity.Indigs.Select(x => x.IndigID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column Indig.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(Indig))
                {
                    Messages.Add(string.Format("Value {0} is not the correct type in line {1} column Indigenous - {2}.", Indig, Line, Name + " " + Surname));
                }

                int postCode = 0;
                if (!string.IsNullOrWhiteSpace(PostCode) && (PostCode.Length > 4 || !int.TryParse(PostCode, out postCode)))
                {
                    Messages.Add(string.Format("Value {0} is not the correct type in line {1} column PostCode - {2}.", PostCode, Line, Name + " " + Surname));
                }

                if (!string.IsNullOrEmpty(Medicare) && Medicare.Length != Regex.Matches(Medicare, @"[0-9 ]").Count)
                {
                    Messages.Add(string.Format("Value of {0} is not the correct type in line {1} column Medicare - {2}.", Medicare, Line, Name + " " + Surname));
                }

                if (PatientType != "0" && short.TryParse(PatientType, out a))
                {
                    var IDs = Entity.PatientTypes.Select(x => x.PatientTypeID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column PatientTypes.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(PatientType))
                {
                    Messages.Add(string.Format("Value {0} is not the correct type in line {1} column PatientType - {2}.", PatientType, Line, Name + " " + Surname));
                }

                if (AdmissionViaED != "0" && short.TryParse(AdmissionViaED, out a))
                {
                    var IDs = Entity.AdmissionViaEDs.Select(x => x.AdmissionViaEDID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column AdmissionViaED.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(AdmissionViaED))
                {
                    Messages.Add(string.Format("Value {0} is not the correct type in line {1} column AdmissionViaED - {2}.", AdmissionViaED, Line, Name + " " + Surname));
                }

                if (UResidence != "0" && short.TryParse(UResidence, out a))
                {
                    var IDs = Entity.Residences.Select(x => x.ResidenceID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column UResidence.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(UResidence))
                {
                    Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column UResidence - {2}.", UResidence, Line, Name + " " + Surname));
                }

                if (TransferHospital != "0" && short.TryParse(TransferHospital, out a))
                {
                    var IDs = Entity.TransferHospitals.Select(x => x.TransferHospitalID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column TransferHospital.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(TransferHospital))
                {
                    Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column TransferHospital - {2}.", TransferHospital, Line, Name + " " + Surname));
                }

                if (!string.IsNullOrEmpty(ArrivalDate) && !string.IsNullOrEmpty(ArrivalTime) && !string.IsNullOrEmpty(DepartureDate) && !string.IsNullOrEmpty(DepartureTime))
                {
                    DateTime arrival = new DateTime();
                    DateTime departure = new DateTime();

                    if (DateTime.TryParse(ArrivalDate + " " + ArrivalTime, out arrival) == true &&
                        DateTime.TryParse(DepartureDate + " " + DepartureTime, out departure) == true)
                    {
                        if (arrival >= departure)
                        {
                            Messages.Add("ED Arrival Date (" + arrival.ToString("dd/MM/yyyy hh:mm") + ") must be earlier than ED Departure Date (" + departure.ToString("dd/MM/yyyy hh:mm") + ") in line " + Line + " - " + Name + " " + Surname);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(InHospFractureDate) && !string.IsNullOrEmpty(InHospFractureTime) && !string.IsNullOrEmpty(DepartureDate) && !string.IsNullOrEmpty(DepartureTime))
                {
                    DateTime inHospFracture = new DateTime();
                    DateTime departure = new DateTime();

                    if (DateTime.TryParse(InHospFractureDate + " " + InHospFractureTime, out inHospFracture) == true &&
                        DateTime.TryParse(DepartureDate + " " + DepartureTime, out departure) == true)
                    {
                        if (inHospFracture < departure)
                        {
                            Messages.Add("In Hospital Fracture Date (" + inHospFracture.ToString("dd/MM/yyyy hh:mm") + ") must be later than Departure Date (" + departure.ToString("dd/MM/yyyy hh:mm") + ") in line " + Line + " - " + Name + " " + Surname);
                        }
                    }
                }

                if (WardType != "0" && short.TryParse(WardType, out a))
                {
                    var IDs = Entity.WardTypes.Select(x => x.WardTypeID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column WardType.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(WardType))
                {
                    Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column WardType - {2}.", WardType, Line, Name + " " + Surname));
                }

                if (PreAdWalk != "0" && short.TryParse(PreAdWalk, out a))
                {
                    var IDs = Entity.PreAdWalks.Select(x => x.PreAdWalkID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column PreAdWalk.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(PreAdWalk))
                {
                    Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column PreAdWalk - {2}.", PreAdWalk, Line, Name + " " + Surname));
                }

                if (PainAssessment != "0" && short.TryParse(PainAssessment, out a))
                {
                    var IDs = Entity.PainAssessments.Select(x => x.PainAssessmentID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column PainAssessment.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(PainAssessment))
                {
                    Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column PainAssessment - {2}.", PainAssessment, Line, Name + " " + Surname));
                }

                if (PainManagement != "0" && short.TryParse(PainManagement, out a))
                {
                    var IDs = Entity.PainManagements.Select(x => x.PainManagementID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column PainManagement.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(PainManagement))
                {
                    Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column PainManagement - {2}.", PainManagement, Line, Name + " " + Surname));
                }

                if (CognitiveAssessment != "0" && short.TryParse(CognitiveAssessment, out a))
                {
                    var IDs = Entity.CognitiveAssessments.Select(x => x.CognitiveAssessmentID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column CognitiveAssessment.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(CognitiveAssessment))
                {
                    Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column CognitiveAssessment - {2}.", CognitiveAssessment, Line, Name + " " + Surname));
                }

                if (CognitiveState != "0" && short.TryParse(CognitiveState, out a))
                {
                    var IDs = Entity.CognitiveStates.Select(x => x.CognitiveStateID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column CognitiveState.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(CognitiveState))
                {
                    Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column CognitiveState - {2}.", CognitiveState, Line, Name + " " + Surname));
                }

                if (BoneMed != "" && short.TryParse(BoneMed, out a))
                {
                    var IDs = Entity.BoneMeds.Select(x => x.BoneMedID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column BoneMed.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(BoneMed))
                {
                    Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column BoneMed - {2}.", BoneMed, Line, Name + " " + Surname));
                }

                if (PreOpMedAss != "" && short.TryParse(PreOpMedAss, out a))
                {
                    var IDs = Entity.PreOpMedAsses.Select(x => x.PreOpMedAssID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column PreOpMedAss.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(PreOpMedAss))
                {
                    Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column PreOpMedAss - {2}.", PreOpMedAss, Line, Name + " " + Surname));
                }

                if (short.TryParse(FractureSide, out a))
                {
                    var IDs = Entity.FractureSides.Select(x => x.FractureSideID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column FractureSide.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(FractureSide))
                {
                    Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column FractureSide - {2}.", FractureSide, Line, Name + " " + Surname));
                }

                if (AtypicalFracture != "" && short.TryParse(AtypicalFracture, out a))
                {
                    var IDs = Entity.AtypicalFractures.Select(x => x.AtypicalFractureID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column AtypicalFracture.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(AtypicalFracture))
                {
                    Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column AtypicalFracture - {2}.", AtypicalFracture, Line, Name + " " + Surname));
                }

                if (FractureType != "0" && short.TryParse(FractureType, out a))
                {
                    var IDs = Entity.FractureTypes.Select(x => x.FractureTypeID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column FractureType.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(FractureType))
                {
                    Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column FractureType - {2}.", FractureType, Line, Name + " " + Surname));
                }

                if (ASAgrade != "0" && short.TryParse(ASAgrade, out a))
                {
                    var IDs = Entity.ASAGrades.Select(x => x.ASAGradeID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column ASA Grade.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(ASAgrade))
                {
                    Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column ASAGrade - {2}.", ASAgrade, Line, Name + " " + Surname));
                }

                if (Surgery != "0" && short.TryParse(Surgery, out a))
                {
                    var IDs = Entity.Surgeries.Select(x => x.SurgeryID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column Surgery Performed.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(Surgery))
                {
                    Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column Surgery - {2}.", Surgery, Line, Name + " " + Surname));
                }

                if (!string.IsNullOrEmpty(SurgeryDate) && !string.IsNullOrEmpty(SurgeryTime) && !string.IsNullOrEmpty(DepartureDate) && !string.IsNullOrEmpty(DepartureTime))
                {
                    DateTime surgeryDate = new DateTime();
                    DateTime departure = new DateTime();

                    if (DateTime.TryParse(SurgeryDate + " " + SurgeryTime, out surgeryDate) == true &&
                        DateTime.TryParse(DepartureDate + " " + DepartureTime, out departure) == true)
                    {
                        if (surgeryDate <= departure)
                        {
                            Messages.Add("Departure Date (" + departure.ToString("dd/MM/yyyy hh:mm") + ") must be earlier than Surgery Date (" + surgeryDate.ToString("dd/MM/yyyy hh:mm") + ") in line " + Line + " - " + Name + " " + Surname);
                        }
                    }
                }

                if (SurgeryDelay != "0" && short.TryParse(SurgeryDelay, out a))
                {
                    var IDs = Entity.SurgeryDelays.Select(x => x.SurgeryDelayID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column SurgeryDelay.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(SurgeryDelay))
                {
                    Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column SurgeryDelay - {2}.", SurgeryDelay, Line, Name + " " + Surname));
                }

                if (SurgeryDelayOther != "0" && SurgeryDelayOther.Length > 250)
                {
                    Messages.Add("SurgeryDelayOther is more than 250 characters in line " + Line);
                }

                if (Anaesthesia != "0" && short.TryParse(Anaesthesia, out a))
                {
                    var IDs = Entity.Anaesthesias.Select(x => x.AnaesthesiaID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column Anaesthesia.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(Anaesthesia))
                {
                    Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column Anaesthesia - {2}.", Anaesthesia, Line, Name + " " + Surname));
                }

                if (Analgesia != "0" && short.TryParse(Analgesia, out a))
                {
                    var IDs = Entity.Analgesias.Select(x => x.AnalgesiaID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column Analgesia.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(Analgesia))
                {
                    Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column Analgesia - {2}.", Analgesia, Line, Name + " " + Surname));
                }

                if (ConsultantPresent != "" && short.TryParse(ConsultantPresent, out a))
                {
                    var IDs = Entity.ConsultantPresents.Select(x => x.ConsultantPresentID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value {0} is not the correct type in line " + Line + " column ConsultantPresent.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(ConsultantPresent))
                {
                    Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column Consultant Present - {2}.", ConsultantPresent, Line, Name + " " + Surname));
                }

                if (Operation != "0" && short.TryParse(Operation, out a))
                {
                    var IDs = Entity.Operations.Select(x => x.OperationID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column Operation.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(Operation))
                {
                    Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column Operation - {2}.", Operation, Line, Name + " " + Surname));
                }

                if (InterOpFracture != "0" && short.TryParse(InterOpFracture, out a))
                {
                    var IDs = Entity.InterOpFractures.Select(x => x.InterOpFractureID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column Inter OpFracture.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(InterOpFracture))
                {
                    Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column InterOpFracture - {2}.", InterOpFracture, Line, Name + " " + Surname));
                }

                if (FullWeightBear != "" && short.TryParse(FullWeightBear, out a))
                {
                    var IDs = Entity.WeightBears.Select(x => x.WeightBearID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column Full Weight Bear.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(FullWeightBear))
                {
                    Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column Full Weight Bear - {2}.", FullWeightBear, Line, Name + " " + Surname));
                }

                if (Mobilisation != "" && short.TryParse(Mobilisation, out a))
                {
                    var IDs = Entity.Mobilisations.Select(x => x.MobilisationID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column Day 1 Mobilisation.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(Mobilisation))
                {
                    Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column Day 1 Mobilisation - {2}.", Mobilisation, Line, Name + " " + Surname));
                }

                if (FirstDayWalking != "" && short.TryParse(FirstDayWalking, out a))
                {
                    var IDs = Entity.FirstDayWalkings.Select(x => x.FirstDayWalkingID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column First Day Walking.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(FirstDayWalking))
                {
                    Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column First Day Walking - {2}.", FirstDayWalking, Line, Name + " " + Surname));
                }

                if (DeleriumAssessment != "0" && short.TryParse(DeleriumAssessment, out a))
                {
                    var IDs = Entity.DeleriumAssessments.Select(x => x.DeleriumAssessmentID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add(string.Format("Value {0} is not the correct type in line {1} column Delerium Assessment - {2}.", DeleriumAssessment, Line, Name + " " + Surname));
                    }
                }
                else if (!string.IsNullOrWhiteSpace(DeleriumAssessment))
                {
                    Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column Delerium Assessment - {2}.", DeleriumAssessment, Line, Name + " " + Surname));
                }

                if (PressureUlcers != "" && short.TryParse(PressureUlcers, out a))
                {
                    var IDs = Entity.PressureUlcers.Select(x => x.PressureUlcersID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column Pressure Ulcers.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(PressureUlcers))
                {
                    Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column PressureUlcers - {2}.", PressureUlcers, Line, Name + " " + Surname));
                }

                if (Malnutrition != "" && short.TryParse(Malnutrition, out a))
                {
                    var IDs = Entity.Malnutritions.Select(x => x.MalnutritionID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column Malnutrition.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(Malnutrition))
                {
                    Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column Malnutrition - {2}.", Malnutrition, Line, Name + " " + Surname));
                }

                if (GeriatricAssessment != "" && short.TryParse(GeriatricAssessment, out a))
                {
                    var IDs = Entity.GeriatricAssessments.Select(x => x.GeriatricAssessmentID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column GeriatricAssessment.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(GeriatricAssessment))
                {
                    Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column GeriatricAssessment - {2}.", GeriatricAssessment, Line, Name + " " + Surname));
                }

                if (!string.IsNullOrEmpty(GeriatricAssDate) && !string.IsNullOrEmpty(GeriatricAssDate))
                {
                    bool dpt = true, arr = true;

                    DateTime geriatricAssessment = new DateTime();

                    if (DateTime.TryParse(GeriatricAssDate + " " + GeriatricAssDate, out geriatricAssessment) == true)
                    {
                        if (!string.IsNullOrEmpty(DepartureDate) && !string.IsNullOrEmpty(DepartureTime))
                        {
                            DateTime departure = new DateTime();

                            if (DateTime.TryParse(DepartureDate + " " + DepartureTime, out departure) == true)
                            {
                                if (geriatricAssessment <= departure)
                                {
                                    dpt = false;
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(ArrivalDate) && !string.IsNullOrEmpty(ArrivalTime))
                        {
                            DateTime arrival = new DateTime();

                            if (DateTime.TryParse(ArrivalDate + " " + ArrivalTime, out arrival) == true)
                            {
                                if (geriatricAssessment <= arrival)
                                {
                                    arr = false;
                                }
                            }
                        }

                        if (dpt == true || arr == true) { }
                        else
                        {
                            Messages.Add("GeriatricAssDateTime must be later than ArrivalDateTime or DepartureDateTime in line " + Line + " - " + Name + " " + Surname);

                        }
                    }
                }

                if (FallsAssessment != "" && short.TryParse(FallsAssessment, out a))
                {
                    var IDs = Entity.FallsAssessments.Select(x => x.FallsAssessmentID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column FallsAssessment.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(FallsAssessment))
                {
                    Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column FallsAssessment - {2}.", FallsAssessment, Line, Name + " " + Surname));
                }

                if (BoneMed != "" && short.TryParse(BoneMed, out a))
                {
                    var IDs = Entity.BoneMeds.Select(x => x.BoneMedID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column BoneMed.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(BoneMed))
                {
                    Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column BoneMed - {2}.", BoneMed, Line, Name + " " + Surname));
                }

                if (BoneMedDischarge != "" && short.TryParse(BoneMedDischarge, out a))
                {
                    var IDs = Entity.BoneMeds.Select(x => x.BoneMedID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column Bone Med Discharge.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(BoneMedDischarge))
                {
                    Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column BoneMedDischarge - {2}.", BoneMedDischarge, Line, Name + " " + Surname));
                }

                if (!string.IsNullOrEmpty(WardDischargeDate))
                {
                    bool c = true, b = true;

                    DateTime wardDischarge = new DateTime();

                    if (DateTime.TryParse(WardDischargeDate, out wardDischarge) == true)
                    {
                        if (!string.IsNullOrEmpty(SurgeryDate))
                        {
                            DateTime surgery = new DateTime();

                            if (DateTime.TryParse(SurgeryDate, out surgery) == true)
                            {
                                if (wardDischarge <= surgery)
                                {
                                    c = false;
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(ArrivalDate))
                        {
                            DateTime arrival = new DateTime();

                            if (DateTime.TryParse(ArrivalDate, out arrival) == true)
                            {
                                if (wardDischarge <= arrival)
                                {
                                    b = false;
                                }
                            }
                        }

                        if (c == true || b == true) { }
                        else
                        {
                            Messages.Add("Ward Discharge Date (" + wardDischarge.ToString("dd/MM/yyyy") + ") must be later than Surgery Date (" + SurgeryDate + ") or Arrival Date (" + ArrivalDate + ") in line " + Line + " - " + Name + " " + Surname);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(HospitalDischargeDate))
                {
                    bool c = true, b = true;

                    DateTime hospitalDischarge = new DateTime();

                    if (DateTime.TryParse(HospitalDischargeDate, out hospitalDischarge) == true)
                    {
                        if (!string.IsNullOrEmpty(SurgeryDate))
                        {
                            DateTime surgery = new DateTime();

                            if (DateTime.TryParse(SurgeryDate, out surgery) == true)
                            {
                                if (hospitalDischarge <= surgery)
                                {
                                    c = false;
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(ArrivalDate))
                        {
                            DateTime arrival = new DateTime();

                            if (DateTime.TryParse(ArrivalDate, out arrival) == true)
                            {
                                if (hospitalDischarge <= arrival)
                                {
                                    b = false;
                                }
                            }
                        }

                        if (c == true || b == true) { }
                        else
                        {
                            DateTime surgery = new DateTime();
                            DateTime.TryParse(SurgeryDate, out surgery);
                            DateTime arrival = new DateTime();
                            DateTime.TryParse(ArrivalDate, out arrival);

                            Messages.Add("HospitalDischargeDate (" + hospitalDischarge.ToString("dd/MM/yyyy") + ") must be later than SurgeryDateTime (" + surgery.ToString("dd/MM/yyyy") + ") or ArrivalDateTime (" + arrival.ToString("dd/MM/yyyy") + ") in line " + Line + " - " + Name + " " + Surname);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(HealthServiceDischarge30))
                {
                    bool c = true, b = true;

                    DateTime healthServiceDischarge = new DateTime();
                    DateTime hospitalDischarge = new DateTime();
                    DateTime wardDischarge = new DateTime();

                    if (DateTime.TryParse(HealthServiceDischarge30, out healthServiceDischarge) == true)
                    {
                        if (!string.IsNullOrEmpty(HospitalDischargeDate))
                        {
                            if (DateTime.TryParse(HospitalDischargeDate, out hospitalDischarge) == true)
                            {
                                if (healthServiceDischarge != hospitalDischarge)
                                {
                                    c = false;
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(WardDischargeDate))
                        {
                            if (DateTime.TryParse(WardDischargeDate, out wardDischarge) == true)
                            {
                                if (healthServiceDischarge != wardDischarge)
                                {
                                    b = false;
                                }
                            }
                        }

                        if (c == false && b == false)
                        {
                            //Messages.Add("Health Service Discharge 30 (" + healthServiceDischarge.ToString("dd/MM/yyyy") + ") must equal Hospital Discharge Date (" + hospitalDischarge.ToString("dd/MM/yyyy") + ") or Ward Discharge Date (" + wardDischarge.ToString("dd/MM/yyyy") + ") in line " + Line);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(FollowupDate30) && !string.IsNullOrEmpty(HospitalDischargeDate))
                {
                    DateTime followup = new DateTime();
                    DateTime hospitalDischarge = new DateTime();

                    if (DateTime.TryParse(FollowupDate30, out followup) == true &&
                        DateTime.TryParse(HospitalDischargeDate, out hospitalDischarge) == true)
                    {
                        if (followup <= hospitalDischarge)
                        {
                            //  Messages.Add("FollowupDate30 of " + followup.ToString("dd/MM/yyyy") + " must be later than HospitalDischargeDate of " + hospitalDischarge.ToString("dd/MM/yyyy") + " in line " + Line + ". Record not inserted/updated.");
                        }
                    }
                }

                if (!string.IsNullOrEmpty(FollowupDate120) && !string.IsNullOrEmpty(FollowupDate30))
                {
                    DateTime followup120 = new DateTime();
                    DateTime followup30 = new DateTime();

                    if (DateTime.TryParse(FollowupDate120, out followup120) == true &&
                        DateTime.TryParse(FollowupDate30, out followup30) == true)
                    {
                        if (followup120 < followup30)
                        {
                            Messages.Add("FollowupDate120 must be later than FollowupDate30 in line " + Line);
                        }
                    }
                }

                if (DischargeDest != "0" && short.TryParse(DischargeDest, out a))
                {
                    var IDs = Entity.DischargeDests.Select(x => x.DischargeDestID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column Ward Discharge Destination.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(DischargeDest))
                {
                    Messages.Add(string.Format("Value (" + DischargeDest.ToString() + ") is not the correct type in line {1} column Ward Discharge Destination - {2}.", DischargeDest, Line, Name + " " + Surname));
                }

                if (DischargeResidence != "0" && short.TryParse(DischargeResidence, out a))
                {
                    var IDs = Entity.DischargeResidences.Select(x => x.DischargeResidenceID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value (" + DischargeResidence + ") is not the correct type in line " + Line + " column Hospital Discharge Residence..");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(DischargeResidence))
                {
                    Messages.Add(string.Format("Value (" + DischargeResidence.ToString() + ") is not the correct type in line {1} column Hospital Discharge Residence - {2}.", DischargeResidence, Line, Name + " " + Surname));
                }

                if (Survival30 != "0" && short.TryParse(Survival30, out a))
                {
                    var IDs = Entity.Survivals.Select(x => x.SurvivalID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column Survival30.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(Survival30))
                {
                    Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column Survival30 - {2}.", Survival30, Line, Name + " " + Surname));
                }

                if (Residence30 != "0" && short.TryParse(Residence30, out a))
                {
                    var IDs = Entity.DischargeDests.Select(x => x.DischargeDestID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column Residence30.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(Residence30))
                {
                    Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column Residence30 - {2}.", Residence30, Line, Name + " " + Surname));
                }

                if (WeightBear30 != "0" && short.TryParse(WeightBear30, out a))
                {
                    var IDs = Entity.WeightBears.Select(x => x.WeightBearID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column WeightBear30.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(WeightBear30))
                {
                    Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column WeightBear30 - {2}.", WeightBear30, Line, Name + " " + Surname));
                }

                if (WalkingAbility30 != "0" && short.TryParse(WalkingAbility30, out a))
                {
                    var IDs = Entity.WalkingAbilities.Select(x => x.WalkingAbilityID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column WalkingAbility30.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(WalkingAbility30))
                {
                    Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column WalkingAbility30 - {2}.", WalkingAbility30, Line, Name + " " + Surname));
                }

                if (BoneMed30 != "0" && short.TryParse(BoneMed30, out a))
                {
                    var IDs = Entity.BoneMeds.Select(x => x.BoneMedID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column BoneMed30.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(BoneMed30))
                {
                    Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column BoneMed30 - {2}.", BoneMed30, Line, Name + " " + Surname));
                }

                if (Reoperation30 != "0" && short.TryParse(Reoperation30, out a))
                {
                    var IDs = Entity.Reoperations.Select(x => x.ReoperationID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column Reoperation30.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(Reoperation30))
                {
                    Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column Reoperation30 - {2}.", Reoperation30, Line, Name + " " + Surname));
                }

                if (Survival120 != "" && short.TryParse(Survival120, out a))
                {
                    var IDs = Entity.Survivals.Select(x => x.SurvivalID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column Survival120.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(Survival120))
                {
                    Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column Survival120 - {2}.", Survival120, Line, Name + " " + Surname));
                }

                if (Residence120 != "0" && short.TryParse(Residence120, out a))
                {
                    var IDs = Entity.DischargeDests.Select(x => x.DischargeDestID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value (" + Residence120 + ") is not the correct type in line " + Line + " column Residence120.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(Residence120))
                {
                    Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column Residence120 - {2}.", Residence120, Line, Name + " " + Surname));
                }

                if (WeightBear120 != "0" && short.TryParse(WeightBear120, out a))
                {
                    var IDs = Entity.WeightBears.Select(x => x.WeightBearID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column WeightBear120.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(WeightBear120) && WeightBear120 != "0")
                {
                  Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column WeightBear120 - {2}.", WeightBear120, Line, Name + " " + Surname));
                }

                if (WalkingAbility120 != "0" && short.TryParse(WalkingAbility120, out a))
                {
                    var IDs = Entity.WalkingAbilities.Select(x => x.WalkingAbilityID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column WalkingAbility120.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(WalkingAbility120))
                {
                    Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column WalkingAbility120 - {2}.", WalkingAbility120, Line, Name + " " + Surname));
                }

                if (BoneMed120 != "" && short.TryParse(BoneMed120, out a))
                {
                    var IDs = Entity.BoneMeds.Select(x => x.BoneMedID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column BoneMed120.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(BoneMed120))
                {
                    Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column BoneMed120 - {2}.", BoneMed120, Line, Name + " " + Surname));
                }

                if (Reoperation120 != "" && short.TryParse(Reoperation120, out a))
                {
                    var IDs = Entity.Reoperations.Select(x => x.ReoperationID).ToList();
                    if (IDs != null && !IDs.Contains(a))
                    {
                        Messages.Add("Value is not the correct type in line " + Line + " column Reoperation120.");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(Reoperation120))
                {
                    Messages.Add(string.Format("Value '{0}' is not the correct type in line {1} column Reoperation120 - {2}.", Reoperation120, Line, Name + " " + Surname));
                }

                if (!string.IsNullOrEmpty(DeathDate))
                {
                    bool c = true, b = true;

                    DateTime deathDate = new DateTime();

                    if (DateTime.TryParse(DeathDate, out deathDate) == true)
                    {
                        if (!string.IsNullOrEmpty(SurgeryDate))
                        {
                            DateTime surgery = new DateTime();

                            if (DateTime.TryParse(SurgeryDate, out surgery) == true)
                            {
                                if (deathDate <= surgery)
                                {
                                    c = false;
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(ArrivalDate))
                        {
                            DateTime arrival = new DateTime();

                            if (DateTime.TryParse(ArrivalDate, out arrival) == true)
                            {
                                if (deathDate <= arrival)
                                {
                                    b = false;
                                }
                            }
                        }

                        if (c == true || b == true) { }
                        else
                        {
                            Messages.Add("Death Date (" + deathDate.ToString("dd/MM/yyyy") + ") must be later than Surgery Date (" + SurgeryDate + ") or Arrival Date (" + ArrivalDate + ") in line " + Line + " - " + Name + " " + Surname);
                        }
                    }
                }

                if (Messages.Count > 0)
                {
                    return false;
                }

                return true;
            }
        }
    }

    public class ImportStatusModel
    {
        public List<string> success { get; set; }
        public List<string> errors { get; set; }
    }
}