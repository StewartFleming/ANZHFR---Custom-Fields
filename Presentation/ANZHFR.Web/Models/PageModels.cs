using ANZHFR.Data.Models;
using ANZHFR.Services.Synonyms;
using ANZHFR.Web.Properties;
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
    public class BaseModel
    {
        public int Page { get; set; }

        public string FilterSearchName { get; set; }
        public string FilterSearchEmail { get; set; }
        public string FilterSearchState { get; set; }
        public string FilterSearchHospitalID { get; set; }
        public string FilterSearchYear { get; set; }

        public string ReturnUrl { get; set; }

        public string ErrorMessage { get; set; }
    }

    public class PatientModel : BaseModel
    {
        public int PatientID { get; set; }
        public long HospitalID { get; set; }
        public long TransferHospitalID { get; set; }

        [StringLength(20)]
        [Display(Name = "First Name")]
        public string Name { get; set; }

        [StringLength(20)]
        public string Surname { get; set; }

        //[Required(ErrorMessage = "MRN/URN must not be empty.")]
        [StringLength(10)]
        [Display(Name = "MRN/URN", Description = "This is mandatory and forms the unique identifier for the record.")]
        public string MRN { get; set; }

        [StringLength(15)]
        [Display(Name = "Contact Phone Number")]
        public string Phone { get; set; }

        [Display(Name = "Date of birth")]
        public string DOB { get; set; }

        public string Sex { get; set; }

        [Display(Name = "Indigenous Status")]
        public string Indig { get; set; }

        [Display(Name = "Ethnic Status")]
        public string Ethnic { get; set; }

        [Display(Name = "Post Code")]
        [StringLength(4)]
        public string PostCode { get; set; }

        [StringLength(11)]
        [Display(Name = "Medicare Number")]
        public string Medicare { get; set; }

        [Display(Name = "Patient Type")]
        public string PatientType { get; set; }

        [Display(Name = "Admission Via ED")]
        public string AdmissionViaED { get; set; }

        [Display(Name = "Usual place of residence")]
        public string UResidence { get; set; }

        [StringLength(10)]
        [Display(Name = "Transfer hospital")]
        public string TransferHospital { get; set; }

        [Display(Name = "Transfer hospital ED date & time")]
        public string TransferDateTime { get; set; }

        [Display(Name = "ED arrival date & time")]
        public string ArrivalDateTime { get; set; }

        [Display(Name = "ED departure date & time")]
        public string DepartureDateTime { get; set; }

        [Display(Name = "In-patient fracture date & time")]
        public string InHospFractureDateTime { get; set; }

        [Display(Name = "Ward Type")]
        public string WardType { get; set; }

        [Display(Name = "Pre-admission walking ability")]
        public string PreAdWalk { get; set; }

        [Display(Name = "Pre-operative AMTS")]
        public string AMTS { get; set; }

        [Display(Name = "Pre-admission cognitive state")]
        public string CognitiveState { get; set; }

        [Display(Name = "Bone medication at admission")]
        public string BoneMed { get; set; }

        [Display(Name = "Pre-operative medical assessment")]
        public string PreOpMedAss { get; set; }

        [Display(Name = "Fracture side")]
        public string FractureSide { get; set; }

        [Display(Name = "Atypical fracture")]
        public string AtypicalFracture { get; set; }

        [Display(Name = "Fracture Type")]
        public string FractureType { get; set; }

        [Display(Name = "ASA Grade")]
        public string ASAGrade { get; set; }

        public string Surgery { get; set; }

        [Display(Name = "Surgery date & time")]
        public string SurgeryDateTime { get; set; }

        [Display(Name = "Surgery delay")]
        public string SurgeryDelay { get; set; }

        [StringLength(250)]
        [Display(Name = "Surgery delay other")]
        public string SurgeryDelayOther { get; set; }

        [Display(Name = "Type of anaesthesia")]
        public string Anaesthesia { get; set; }

        [Display(Name = "Analgesia - nerve block")]
        public string Analgesia { get; set; }

        [Display(Name = "Consultant surgeon present")]
        public string ConsultantPresent { get; set; }

        [Display(Name = "Type of operation")]
        public string Operation { get; set; }

        [Display(Name = "Intra-operative fracture")]
        public string InterOpFracture { get; set; }

        [Display(Name = "Post-operative weight bearing status")]
        public string FullWeightBear { get; set; }

        [Display(Name = "Day 1 mobilisation")]
        public string Mobilisation { get; set; }

        [Display(Name = "First Day Walking")]
        public string FirstDayWalking { get; set; }

        [Display(Name = "New pressure ulcers")]
        public string PressureUlcers { get; set; }

        [Display(Name = "Malnutrition Assessment")]
        public string Malnutrition { get; set; }

        [Display(Name = "Assessed by geriatric medicine")]
        public string GeriatricAssessment { get; set; }

        [Display(Name = "Geriatric assessment date")]
        public string GeriatricAssDateTime { get; set; }

        [Display(Name = "Specialist falls assessment")]
        public string FallsAssessment { get; set; }

        [Display(Name = "Bone medication at discharge")]
        public string BoneMedDischarge { get; set; }

        [Display(Name = "Discharge date")]
        public string WardDischargeDate { get; set; }

        [Display(Name = "Discharge destination")]
        public string DischargeDest { get; set; }

        [Display(Name = "Discharge date")]
        public string HospitalDischargeDate { get; set; }

        [Display(Name = "Length of stay")]
        public Nullable<short> OLengthofStay { get; set; }

        [Display(Name = "Hospital length of stay")]
        public Nullable<short> HLengthofStay { get; set; }

        [Display(Name = "Discharge destination")]
        public string DischargeResidence { get; set; }

        [Display(Name = "30 day follow-up date")]
        public string FollowupDate30 { get; set; }

        [Display(Name = "Expected follow-up date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "dd-MMM-yyyy")]
        public DateTime? ExpectedFollowup30 { get; set; }

        public string ExpectedFollowup30String
        {
            get
            {
                return string.Format("{0:dd-MMM-yyyy}", this.ExpectedFollowup30);
            }
        }

        [Display(Name = "Expected follow-up date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "dd-MMM-yyyy")]
        public DateTime? ExpectedFollowup120 { get; set; }

        public string ExpectedFollowup120String
        {
            get
            {
                return string.Format("{0:dd-MMM-yyyy}", this.ExpectedFollowup120);
            }
        }

        [Display(Name = "Confirm health service discharge")]
        public string HealthServiceDischarge30 { get; set; }

        [Display(Name = "Survival at 30 days")]
        public string Survival30 { get; set; }

        [Display(Name = "Place of residence")]
        public string Residence30 { get; set; }

        [Display(Name = "Full weight bear")]
        public string WeightBear30 { get; set; }

        [Display(Name = "Walking ability")]
        public string WalkingAbility30 { get; set; }

        [Display(Name = "Bone medication")]
        public string BoneMed30 { get; set; }

        [Display(Name = "Re-operation within 30 days")]
        public string Reoperation30 { get; set; }

        [Display(Name = "120 follow-up date")]
        public string FollowupDate120 { get; set; }

        [Display(Name = "Confirm health service discharge")]
        public string HealthServiceDischarge120 { get; set; }

        [Display(Name = "Survival at 120 days")]
        public string Survival120 { get; set; }

        [Display(Name = "Place of residence")]
        public string Residence120 { get; set; }

        [Display(Name = "Full weight bear")]
        public string WeightBear120 { get; set; }

        [Display(Name = "Walking ability")]
        public string WalkingAbility120 { get; set; }

        [Display(Name = "Bone medication")]
        public string BoneMed120 { get; set; }

        [Display(Name = "Re-operation within 120 days")]
        public string Reoperation120 { get; set; }

        [Display(Name = "Incomplete")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal Completeness { get; set; }

        [Display(Name = "Completeness Exemption")]
        public string CompleteExemption { get; set; }

        [Display(Name = "Date Created")]
        public string Created { get; set; }

        [Display(Name = "Author")]
        public string Author { get; set; }

        [Display(Name = "Last Modified")]
        public string LastModified { get; set; }

        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }

        [Display(Name = "Pre-operative cognitive assessment")]
        public string CognitiveAssessment { get; set; }

        [Display(Name = "Pain assessment")]
        public string PainAssessment { get; set; }

        [Display(Name = "Pain management")]
        public string PainManagement { get; set; }

        [Display(Name = "Delirium Assessment")]
        public string DeleriumAssessment { get; set; }

        [Display(Name = "Start Date")]
        [Required(ErrorMessage = "Please add the ED Arrival or In-Patient Fall date")]
        public string StartDate { get; set; }

        [Display(Name = "Informed of registry inclusion")]
        public bool Informed { get; set; }

        [Display(Name = "Opted out of registry")]
        public bool OptedOut { get; set; }

        [Display(Name = "Cannot Followup")]
        public bool CannotFollowup { get; set; }

        [Display(Name = "Date of Death")]
        public string DeathDate { get; set; }

        [Display(Name = "Mobility")]
        public string EQ5D_Mobility { get; set; }

        [Display(Name = "Self-Care")]
        public string EQ5D_SelfCare { get; set; }

        [Display(Name = "Usual Activity")]
        public string EQ5D_UsualActivity { get; set; }

        [Display(Name = "Pain / Discomfort")]
        public string EQ5D_Pain { get; set; }

        [Display(Name = "Anxiety / Depression")]
        public string EQ5D_Anxiety { get; set; }

        [Display(Name = "Health Today")]
        [RegularExpression("^$|^([0-9]|([1-9][0-9])|100)$", ErrorMessage = "Must be 0 - 100")]
        public short? EQ5D_Health { get; set; }

        public List<Sex> SexList { get; set; }
        public List<Indig> IndigList { get; set; }
        public List<Ethnic> EthnicList { get; set; }
        public List<PatientType> PatientTypeList { get; set; }
        public List<AdmissionViaED> AdmissionViaEDList { get; set; }
        public List<Residence> ResidenceList { get; set; }
        public List<Hospital> HospitalList { get; set; }
        public List<TransferHospital> TransferHospitalList { get; set; }
        public List<WardType> WardTypeList { get; set; }
        public List<PreAdWalk> PreAdWalkList { get; set; }
        public List<AMTS> AMTSList { get; set; }
        public List<CognitiveState> CognitiveStateList { get; set; }
        public List<ASAGrade> ASAGradeList { get; set; }
        public List<BoneMed> BoneMedList { get; set; }
        public List<PreOpMedAss> PreOpMedAssList { get; set; }
        public List<FractureSide> FractureSideList { get; set; }
        public List<AtypicalFracture> AtypicalFractureList { get; set; }
        public List<FractureType> FractureTypeList { get; set; }
        public List<Surgery> SurgeryList { get; set; }
        public List<SurgeryDelay> SurgeryDelayList { get; set; }
        public List<Anaesthesia> AnaesthesiaList { get; set; }
        public List<Analgesia> AnalgesiaList { get; set; }
        public List<ConsultantPresent> ConsultantPresentList { get; set; }
        public List<Operation> OperationList { get; set; }
        public List<InterOpFracture> InterOpFractureList { get; set; }
        public List<PressureUlcer> PressureUlcersList { get; set; }
        public List<Malnutrition> MalnutritionList { get; set; }
        public List<Mobilisation> MobilisationList { get; set; }
        public List<GeriatricAssessment> GeriatricAssessmentList { get; set; }
        public List<FallsAssessment> FallsAssessmentList { get; set; }
        public List<DischargeDest> DischargeDestList { get; set; }
        public List<DischargeResidence> DischargeResidenceList { get; set; }
        public List<LengthofStay> LengthofStayList { get; set; }
        public List<Survival> SurvivalList { get; set; }
        public List<WeightBear> WeightBearList { get; set; }
        public List<WalkingAbility> WalkingAbilityList { get; set; }
        public List<Reoperation> ReoperationList { get; set; }
        public List<CognitiveAssessment> CognitiveAssessmentList { get; set; }
        public List<PainAssessment> PainAssessmentList { get; set; }
        public List<PainManagement> PainManagementList { get; set; }
        public List<DeleriumAssessment> DeleriumAssessmentList { get; set; }
        public List<FirstDayWalking> FirstDayWalkingList { get; set; }
        public List<EQ5DMobility> EQ5DMobilityList { get; set; }
        public List<EQ5DSelfCare> EQ5DSelfCareList { get; set; }
        public List<EQ5DUsualActivity> EQ5DUsualActivityList { get; set; }
        public List<EQ5DPain> EQ5DPainList { get; set; }
        public List<EQ5DAnxiety> EQ5DAnxietyList { get; set; }
    }
    public class QualityScoreResults : BaseModel
    {
        public int QualityScore { get; set; }
        public int QualityScore15 { get; set; }
        public int QualityScoreMismatch { get; set; }
        public string QualityScoreComments { get; set; }
    }

    public class QualityPatientModel : BaseModel
    {
        public int QualityID { get; set; }
        public int PatientID { get; set; }
        public long HospitalID { get; set; }
        public long TransferHospitalID { get; set; }

        [StringLength(20)]
        [Display(Name = "First Name")]
        public string Name { get; set; }

        [StringLength(20)]
        public string Surname { get; set; }

        //[Required(ErrorMessage = "MRN/URN must not be empty.")]
        [StringLength(10)]
        [Display(Name = "MRN/URN", Description = "This is mandatory and forms the unique identifier for the record.")]
        public string MRN { get; set; }

        [StringLength(15)]
        [Display(Name = "Contact Phone Number")]
        public string Phone { get; set; }

        [Display(Name = "Date of birth")]
        public string DOB { get; set; }
        public int QDOB { get; set; }
        public string Sex { get; set; }
        public int QSex { get; set; }

        [Display(Name = "Indigenous Status")]
        public string Indig { get; set; }
        public int QIndig { get; set; }
        [Display(Name = "Ethnic Status")]
        public string Ethnic { get; set; }
        public int QEthnic { get; set; }

        [Display(Name = "Post Code")]
        [StringLength(4)]
        public string PostCode { get; set; }

        [StringLength(11)]
        [Display(Name = "Medicare Number")]
        public string Medicare { get; set; }

        [Display(Name = "Patient Type")]
        public string PatientType { get; set; }
        public int QPatientType { get; set; }

        [Display(Name = "Admission Via ED")]
        public string AdmissionViaED { get; set; }
        public int QAdmissionViaED { get; set; }

        [Display(Name = "Usual place of residence")]
        public string UResidence { get; set; }
        public int QUResidence { get; set; }

        [StringLength(10)]
        [Display(Name = "Transfer hospital")]
        public string TransferHospital { get; set; }
        public int QTransferHospital { get; set; }

        [Display(Name = "Transfer hospital ED date & time")]
        public string TransferDateTime { get; set; }
        public int QTransferDateTime { get; set; }
        public int Q15TransferDateTime { get; set; }

        [Display(Name = "ED arrival date & time")]
        public string ArrivalDateTime { get; set; }
        public int QArrivalDateTime { get; set; }
        public int Q15ArrivalDateTime { get; set; }

        [Display(Name = "ED departure date & time")]
        public string DepartureDateTime { get; set; }
        public int QDepartureDateTime { get; set; }
        public int Q15DepartureDateTime { get; set; }

        [Display(Name = "In-patient fracture date & time")]
        public string InHospFractureDateTime { get; set; }
        public int QInHospFractureDateTime { get; set; }
        public int Q15InHospFractureDateTime { get; set; }

        [Display(Name = "Ward Type")]
        public string WardType { get; set; }
        public int QWardType { get; set; }

        [Display(Name = "Pre-admission walking ability")]
        public string PreAdWalk { get; set; }
        public int QPreAdWalk { get; set; }

        [Display(Name = "Pre-operative AMTS")]
        public string AMTS { get; set; }
        public int QAMTS { get; set; }

        [Display(Name = "Pre-admission cognitive state")]
        public string CognitiveState { get; set; }
        public int QCognitiveState { get; set; }

        [Display(Name = "Bone medication at admission")]
        public string BoneMed { get; set; }
        public int QBoneMed { get; set; }

        [Display(Name = "Pre-operative medical assessment")]
        public string PreOpMedAss { get; set; }
        public int QPreOpMedAss { get; set; }

        [Display(Name = "Fracture side")]
        public string FractureSide { get; set; }

        [Display(Name = "Atypical fracture")]
        public string AtypicalFracture { get; set; }
        public int QAtypicalFracture { get; set; }

        [Display(Name = "Fracture Type")]
        public string FractureType { get; set; }
        public int QFractureType { get; set; }

        [Display(Name = "ASA Grade")]
        public string ASAGrade { get; set; }
        public int QASAGrade { get; set; }

        public string Surgery { get; set; }
        public int QSurgery { get; set; }

        [Display(Name = "Surgery date & time")]
        public string SurgeryDateTime { get; set; }
        public int QSurgeryDateTime { get; set; }
        public int Q15SurgeryDateTime { get; set; }

        [Display(Name = "Surgery delay")]
        public string SurgeryDelay { get; set; }
        public int QSurgeryDelay { get; set; }

        [StringLength(250)]
        [Display(Name = "Surgery delay other")]
        public string SurgeryDelayOther { get; set; }

        [Display(Name = "Type of anaesthesia")]
        public string Anaesthesia { get; set; }
        public int QAnaesthesia { get; set; }

        [Display(Name = "Analgesia - nerve block")]
        public string Analgesia { get; set; }
        public int QAnalgesia { get; set; }

        [Display(Name = "Consultant surgeon present")]
        public string ConsultantPresent { get; set; }
        public int QConsultantPresent { get; set; }

        [Display(Name = "Type of operation")]
        public string Operation { get; set; }
        public int QOperation { get; set; }

        [Display(Name = "Intra-operative fracture")]
        public string InterOpFracture { get; set; }
        public int QInterOpFracture { get; set; }

        [Display(Name = "Post-operative weight bearing status")]
        public string FullWeightBear { get; set; }
        public int QFullWeightBear { get; set; }

        [Display(Name = "Day 1 mobilisation")]
        public string Mobilisation { get; set; }
        public int QMobilisation { get; set; }

        [Display(Name = "New pressure ulcers")]
        public string PressureUlcers { get; set; }
        public int QPressureUlcers { get; set; }

        [Display(Name = "Malnutrition Assessment")]
        public string Malnutrition { get; set; }
        public int QMalnutrition { get; set; }

        [Display(Name = "Assessed by geriatric medicine")]
        public string GeriatricAssessment { get; set; }
        public int QGeriatricAssessment { get; set; }

        [Display(Name = "Geriatric assessment date")]
        public string GeriatricAssDateTime { get; set; }
        public int QGeriatricAssDateTime { get; set; }

        [Display(Name = "Specialist falls assessment")]
        public string FallsAssessment { get; set; }
        public int QFallsAssessment { get; set; }

        [Display(Name = "Bone medication at discharge")]
        public string BoneMedDischarge { get; set; }
        public int QBoneMedDischarge { get; set; }

        [Display(Name = "Discharge date")]
        public string WardDischargeDate { get; set; }
        public int QWardDischargeDate { get; set; }

        [Display(Name = "Discharge destination")]
        public string DischargeDest { get; set; }
        public int QDischargeDest { get; set; }

        [Display(Name = "Discharge date")]
        public string HospitalDischargeDate { get; set; }
        public int QHospitalDischargeDate { get; set; }

        [Display(Name = "Length of stay")]
        public Nullable<short> OLengthofStay { get; set; }

        [Display(Name = "Hospital length of stay")]
        public Nullable<short> HLengthofStay { get; set; }

        [Display(Name = "Discharge destination")]
        public string DischargeResidence { get; set; }
        public int QDischargeResidence { get; set; }

        [Display(Name = "Incomplete")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal Completeness { get; set; }

        [Display(Name = "Completeness Exemption")]
        public string CompleteExemption { get; set; }

        [Display(Name = "Date Created")]
        public string Created { get; set; }

        [Display(Name = "Author")]
        public string Author { get; set; }

        [Display(Name = "Last Modified")]
        public string LastModified { get; set; }

        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }

        [Display(Name = "Pre-operative cognitive assessment")]
        public string CognitiveAssessment { get; set; }
        public int QCognitiveAssessment { get; set; }

        [Display(Name = "Pain assessment")]
        public string PainAssessment { get; set; }
        public int QPainAssessment { get; set; }

        [Display(Name = "Pain management")]
        public string PainManagement { get; set; }
        public int QPainManagement { get; set; }

        [Display(Name = "Delirium Assessment")]
        public string DeleriumAssessment { get; set; }
        public int QDeleriumAssessment { get; set; }

        [Display(Name = "Start Date")]
        public string StartDate { get; set; }

        [Display(Name = "Informed of registry inclusion")]
        public bool Informed { get; set; }

        [Display(Name = "Opted out of registry")]
        public bool OptedOut { get; set; }

        [Display(Name = "Cannot Followup")]
        public bool CannotFollowup { get; set; }

        [Display(Name = "Date of Death")]
        public string DeathDate { get; set; }
        public int QDeathDate { get; set; }

        [Display(Name = "First Day Walking")]
        public string FirstDayWalking { get; set; }
        public int QFirstDayWalking { get; set; }
        public int QScore { get; set; }
        public int QScore15 { get; set; }
        public int QScoreMM { get; set; }
        public string QScoreComments { get; set; }

        public List<Sex> SexList { get; set; }
        public List<Indig> IndigList { get; set; }
        public List<Ethnic> EthnicList { get; set; }
        public List<PatientType> PatientTypeList { get; set; }
        public List<AdmissionViaED> AdmissionViaEDList { get; set; }
        public List<Residence> ResidenceList { get; set; }
        public List<Hospital> HospitalList { get; set; }
        public List<TransferHospital> TransferHospitalList { get; set; }
        public List<WardType> WardTypeList { get; set; }
        public List<PreAdWalk> PreAdWalkList { get; set; }
        public List<AMTS> AMTSList { get; set; }
        public List<CognitiveState> CognitiveStateList { get; set; }
        public List<ASAGrade> ASAGradeList { get; set; }
        public List<BoneMed> BoneMedList { get; set; }
        public List<PreOpMedAss> PreOpMedAssList { get; set; }
        public List<FractureSide> FractureSideList { get; set; }
        public List<AtypicalFracture> AtypicalFractureList { get; set; }
        public List<FractureType> FractureTypeList { get; set; }
        public List<Surgery> SurgeryList { get; set; }
        public List<SurgeryDelay> SurgeryDelayList { get; set; }
        public List<Anaesthesia> AnaesthesiaList { get; set; }
        public List<Analgesia> AnalgesiaList { get; set; }
        public List<ConsultantPresent> ConsultantPresentList { get; set; }
        public List<Operation> OperationList { get; set; }
        public List<InterOpFracture> InterOpFractureList { get; set; }
        public List<PressureUlcer> PressureUlcersList { get; set; }
        public List<Malnutrition> MalnutritionList { get; set; }
        public List<Mobilisation> MobilisationList { get; set; }
        public List<FirstDayWalking> FirstDayWalkingList { get; set; }
        public List<GeriatricAssessment> GeriatricAssessmentList { get; set; }
        public List<FallsAssessment> FallsAssessmentList { get; set; }
        public List<DischargeDest> DischargeDestList { get; set; }
        public List<DischargeResidence> DischargeResidenceList { get; set; }
        public List<LengthofStay> LengthofStayList { get; set; }
        public List<Survival> SurvivalList { get; set; }
        public List<WeightBear> WeightBearList { get; set; }
        public List<WalkingAbility> WalkingAbilityList { get; set; }
        public List<Reoperation> ReoperationList { get; set; }
        public List<CognitiveAssessment> CognitiveAssessmentList { get; set; }
        public List<PainAssessment> PainAssessmentList { get; set; }
        public List<PainManagement> PainManagementList { get; set; }

        public List<DeleriumAssessment> DeleriumAssessmentList { get; set; }
    }

    public class UserModel : BaseModel
    {
        public long UserID { get; set; }

        [Required]
        [Display(Name = "Hospital")]
        public long HospitalID { get; set; }

        [Required]
        [Display(Name = "Transfer Hospital")]
        public long TransferHospitalID { get; set; }

        [Required]
        [Display(Name = "Access level")]
        public int AccessLevel { get; set; }

        [Required]
        [StringLength(80)]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required]
        [MaxLength(20)]
        [MinLength(6)]
        [DataType(DataType.Password)]
        [RegularExpression(@"(\S)+", ErrorMessage = "White space is not allowed.")]
        public string Password { get; set; }

        [StringLength(25)]
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [MaxLength(20)]
        [MinLength(6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [RegularExpression(@"(\S)+", ErrorMessage = "White space is not allowed.")]
        public string Password1 { get; set; }

        [StringLength(25)]
        [DataType(DataType.Password)]
        [Compare("Password1")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword1 { get; set; }

        public string Title { get; set; }

        [StringLength(50)]
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        public string Surname { get; set; }
        public string Mobile { get; set; }
        public string WorkPhone { get; set; }
        public string DateCreated { get; set; }
        public string LastAccessed { get; set; }
        public byte Active { get; set; }
        public string AdminNotes { get; set; }

        [Display(Name = "Position")]
        public string UPosition { get; set; }

        [Display(Name = "Active?")]
        public bool ActiveFlag
        {
            get
            {
                return this.Active == 1 ? true : false;
            }
            set
            {
                this.Active = value ? (byte)1 : (byte)0;
            }
        }

        public List<Hospital> HospitalList { get; set; }
        public List<TransferHospital> TransferHospitalList { get; set; }
        public List<UserPosition> PositionList { get; set; }
        public List<AccessLevel> AccessLevelList { get; set; }
    }

    public class HospitalModel : BaseModel
    {
        public long HospitalID { get; set; }

        public string Name { get; set; }

        [Display(Name = "Street Address 1")]
        public string StreetAddress1 { get; set; }

        [Display(Name = "Street Address 2")]
        public string StreetAddress2 { get; set; }

        public string Suburb { get; set; }

        public string City { get; set; }
        public string State { get; set; }

        [Display(Name = "Post code")]
        public string PostCode { get; set; }

        public string Country { get; set; }

        public string Phone { get; set; }

        [Display(Name = "Admin Email")]
        public string AdminEmail { get; set; }

        public List<string> StateList { get; set; }

        public string Location
        {
            get; set;
        }
    }

    public class TransferHospitalModel : BaseModel
    {
        public long TransferHospitalID { get; set; }

        public long HospitalID { get; set; }

        public string Name { get; set; }

        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }

        [Display(Name = "Street Address 2")]
        public string StreetAddress2 { get; set; }

        public string Suburb { get; set; }

        public string City { get; set; }

        [Display(Name = "Post code")]
        public string PostCode { get; set; }

        public string State { get; set; }
        public string Country { get; set; }

        public string Phone { get; set; }

        [Display(Name = "Admin Email")]
        public string AdminEmail { get; set; }
    }

    public class ContentModel : BaseModel
    {
        public long Content_ID { get; set; }

        [Display(Name = "Key")]
        public string Content_Key { get; set; }

        [Display(Name = "HTML")]
        public string Content_HTML { get; set; }

        [Display(Name = "Text")]
        public string Content_Text { get; set; }
    }

    public class SurveyModel : BaseModel
    {
        public string YearText { get; set; }
        public int SurveyID { get; set; }
        public long HospitalID { get; set; }
        public string HospitalName { get; set; }
        public int Year { get; set; }
        public int? EstimatedRecordsPerMonth { get; set; }

        [Required]
        [Display(Name = "Role name")]
        public string RoleName { get; set; }

        [Required]
        [Display(Name = "Estimated number of hip fractures")]
        public string EstimatedRecords { get; set; }

        [Required]
        [Display(Name = "Designated major trauma centre")]
        public string DesignatedMajorTraumaCentre { get; set; }

        [Required]
        [Display(Name = "Was there a formal orthogeriatric service in place")]
        public string FormalOrthogeriatricService { get; set; }

        [Required]
        [Display(Name = "Model of Care")]
        public string ModelOfCare { get; set; }

        [Display(Name = "Model of Care Other")]
        public string ModelCareOther { get; set; }

        [Required]
        [Display(Name = "For a suspected hip fracture, does your hospital have a protocol or pathway for access to CT / MRI for inconclusive plain imaging")]
        public string ProtocolAccessCTMRI { get; set; }

        [Required]
        [Display(Name = "Does your hospital have an agreed hip fracture pathway")]
        public string HipFracturePathway { get; set; }

        [Required]
        [Display(Name = "Does your hospital have a VTE protocol")]
        public string VTEProtocol { get; set; }

        [Required]
        [Display(Name = "Does your hospital have a protocol or pathway for pain in hip fracture patients")]
        public string ProtocolPain { get; set; }

        [Required]
        [Display(Name = "Does your hospital have a planned list / planned trauma list for hip fracture patients")]
        public string PlannedList { get; set; }

        [Required]
        [Display(Name = "Are hip fracture patients routinely offered a choice of anaesthesia")]
        public string ChoiceOfAnaesthesia { get; set; }

        [Required]
        [Display(Name = "Are hip fracture patients offered local nerve blocks as part of pain management prior to surgery")]
        public string LocalNerveBlocks { get; set; }

        [Required]
        [Display(Name = "Are local nerve blocks used at the time of surgery to help with postoperative pain")]
        public string LocalNerveBlocksPostoperative { get; set; }

        [Required]
        [Display(Name = "Does your hospital offer hip fracture patients routine access to therapy services at weekends")]
        public string WeekendTherapyServices { get; set; }

        [Required]
        [Display(Name = "Does your hospital routinely provide patients and/or family and carers with written information about treatment and care for a hip fracture")]
        public string WrittenCareInformation { get; set; }

        [Required]
        [Display(Name = "Access to in-patient rehabilitation")]
        public string InPatientRehabilitation { get; set; }

        [Required]
        [Display(Name = "Does your hospital have access to an early supported home based rehabilitation service (not the same as the Commonwealth funded transitional aged care program or community services)")]
        public string SupportedHomeBasedRehabilitation { get; set; }

        [Required]
        [Display(Name = "Does your service provide individualised *written* information to patients on discharge that includes recommendations for future falls and fracture prevention")]
        public string IndividualisedWrittenInformation { get; set; }

        [Required]
        [Display(Name = "Does your service have access to a Falls clinic (Public)")]
        public string FallsClinicPublic { get; set; }

        [Required]
        [Display(Name = "Does your service have access to an osteoporosis clinic (Public)")]
        public string OsteoporosisClinicPublic { get; set; }

        [Required]
        [Display(Name = "Does your service have access to a combined falls and bone health clinic (Public)")]
        public string CombinedFallsBoneHealthClinicPublic { get; set; }

        [Required]
        [Display(Name = "Does your service have access to an orthopaedic clinic (Public)")]
        public string OrthopaedicClinicPublic { get; set; }

        [Required]
        [Display(Name = "Do you have a fracture liaison service, whereby there is systematic identification of all fracture patients by a fracture liaison nurse, with a view to onward referrals and management of osteoporosis")]
        public string FractureLiaisonService { get; set; }

        [Required]
        [Display(Name = "Does your hospital routinely collect local hip fracture data")]
        public string CollectLocalHipFractureData { get; set; }

        [Required]
        [Display(Name = "Who currently collects the data")]
        public string WhoCollectsData { get; set; }

        [Required]
        [Display(Name = "Do you have any plans to alter any of your service provision for hip fracture patients over the next 12 months")]
        public string ServiceChanges { get; set; }

        [Display(Name = "If Yes, details of service changes planned")]
        public string ServiceChangesPlanned { get; set; }

        [Required]
        [Display(Name = "Are there identified barriers to any proposed service redesign")]
        public string IdentifiedBarriers { get; set; }

        [Display(Name = "If Yes, details of identified barriers")]
        public string IdentifiedBarrierDetails { get; set; }

        public int? CreatedUserID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedUserID { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}