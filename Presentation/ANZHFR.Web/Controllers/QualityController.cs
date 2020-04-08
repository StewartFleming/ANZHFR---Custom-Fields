using ANZHFR.Data.Models;
using ANZHFR.Services.Auth;
using ANZHFR.Services.Patients;
using ANZHFR.Web.Helpers;
using ANZHFR.Web.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ANZHFR.Web.Controllers
{
    public class QualityController : BaseController
    {
        private readonly UserServices _userServices;
        private readonly PatientServices _patientServices;
        private readonly HospitalServices _hospitalServices;
        private readonly TransferHospitalServices _transferhospitalServices;

        public QualityController()
        {
            _userServices = new UserServices();
            _patientServices = new PatientServices();
            _hospitalServices = new HospitalServices();
            _transferhospitalServices = new TransferHospitalServices();
        }

        [Authorize]
        public ActionResult Index(int? page, string search, string message)
        {
            long hopitalId = CurrentHospitalId();
            if (search == "undefined") { search = ""; }
            var results = _patientServices.GetQuality(hopitalId, search);

            ViewBag.MenuQuality = "active";
            ViewBag.HospitalName = _hospitalServices.GetHospitalNameById(hopitalId);
            ViewBag.FilterSearch = search;
            ViewBag.Message = message;
            int pageNumber = (page ?? 1);
            return View(results.OrderBy(x => x.Name).ToPagedList(pageNumber, 25));
        }

        [Authorize]
        public ActionResult Delete(int id = 0, int page = 1, string search = "")
        {
            string message = "Error in deleting!";
            if (_patientServices.QDelete(id))
            {
                message = "Quality Record deleted";
            }

            return RedirectToAction("index", new { page = page, search = search, message = message });
        }
        [Authorize]
        public ActionResult Edit(int id = 0, int page = 1, string search = "", string returnURL = "")
        {
            var patient = _patientServices.GetQualityPatientById(id);

            if (patient == null || _userServices.GetUserRole(CurrentUserID()) == 0)
            {
                return RedirectToAction("../home/Restricted");
            }
            else
            {
                var model = PrepareQualityPatientModel(patient);
                model.QualityID = id;
                model.Page = page;
                model.FilterSearchName = search;
                model.ReturnUrl = returnURL;
                model = LoadLookUpList(model);

                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Edit(QualityPatientModel model, string submit, FormCollection coll)
        {
            bool isValid = true;
            var compare = submit;

            //Model State errors?
            var errors = ModelState
    .Where(x => x.Value.Errors.Count > 0)
    .Select(x => new { x.Key, x.Value.Errors })
    .ToArray();


            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.MRN))
                {
                    isValid = false;
                    ViewBag.Message = "MRN/URN must not be empty.";
                }

                if (isValid)
                {
                    long hospitalID = CurrentHospitalId();
                    if (model.ArrivalDateTime != null)
                    {
                        DateTime? ArrivalDateTime = ConvertToDate(model.ArrivalDateTime, false);
                    }
                   
                    var user = _patientServices.GetUniqueQualityPatient(hospitalID, model.MRN, model.FractureSide);
                    var modifiedBy = CurrentUser();
                    var completeness = CalculateCompleteness(model);
                    model.StartDate = CalculateStartDate(model);

                    if (user == null || user.QualityID == model.QualityID)
                    {
                        var results = QualityCompare(model);
                        user = _patientServices.QUpdate(model.QualityID, model.PatientID, model.Name, model.Surname, model.MRN,
                            model.Phone, ConvertToDate(model.DOB), model.Sex, model.Indig, model.Ethnic, model.PostCode, model.Medicare,
                            model.PatientType, model.AdmissionViaED, model.UResidence, model.TransferHospital, ConvertToDate(model.TransferDateTime, false),
                            ConvertToDate(model.ArrivalDateTime, false), ConvertToDate(model.DepartureDateTime, false), ConvertToDate(model.InHospFractureDateTime, false),
                            model.WardType, model.PreAdWalk, model.CognitiveState, model.BoneMed, model.PreOpMedAss, model.FractureSide,
                            model.AtypicalFracture, model.FractureType, model.Surgery, ConvertToDate(model.SurgeryDateTime, false), model.SurgeryDelay,
                            model.SurgeryDelayOther, model.Anaesthesia, model.Analgesia, model.ConsultantPresent, model.Operation, model.InterOpFracture,
                            model.FullWeightBear, model.Mobilisation, model.PressureUlcers, model.GeriatricAssessment, ConvertToDate(model.GeriatricAssDateTime),
                            model.FallsAssessment, model.BoneMedDischarge, ConvertToDate(model.WardDischargeDate), model.DischargeDest,
                            ConvertToDate(model.HospitalDischargeDate), model.DischargeResidence, model.OLengthofStay, model.HLengthofStay,
                            model.ASAGrade, completeness.GetValueOrDefault(0), model.CompleteExemption, DateTime.Now, modifiedBy,
                            model.CognitiveAssessment, model.PainAssessment, model.PainManagement,
                            model.DeleriumAssessment, ConvertToDate(model.StartDate, false), model.Informed, model.OptedOut, model.CannotFollowup, model.Malnutrition,
                            ConvertToDate(model.DeathDate), model.FirstDayWalking,
                    
                            model.QDOB, model.QSex, model.QIndig, model.QEthnic, 
                            model.QPatientType, model.QAdmissionViaED, model.QUResidence, model.QTransferHospital, model.QTransferDateTime,
                            model.QArrivalDateTime, model.QDepartureDateTime, model.QInHospFractureDateTime,
                            model.Q15ArrivalDateTime, model.Q15DepartureDateTime, model.Q15InHospFractureDateTime,
                            model.QWardType, model.QPreAdWalk, model.QCognitiveState, model.QBoneMed, model.QPreOpMedAss,
                            model.QAtypicalFracture, model.QFractureType, model.QSurgery, model.QSurgeryDateTime, model.Q15SurgeryDateTime, model.QSurgeryDelay,
                            model.QAnaesthesia, model.QAnalgesia, model.QConsultantPresent, model.QOperation, model.QInterOpFracture,
                            model.QFullWeightBear, model.QMobilisation, model.QPressureUlcers, model.QGeriatricAssessment, model.QGeriatricAssDateTime,
                            model.QFallsAssessment, model.QBoneMedDischarge, model.QWardDischargeDate, model.QDischargeDest,
                            model.QHospitalDischargeDate, model.QDischargeResidence,
                            model.QASAGrade,
                            model.QCognitiveAssessment, model.QPainAssessment, model.QPainManagement,
                            model.QDeleriumAssessment, model.QMalnutrition,
                            model.QDeathDate, model.QFirstDayWalking,

                            results.QualityScore, results.QualityScore15, results.QualityScoreMismatch, results.QualityScoreComments
                            );

                        if (user != null)
                            if (compare.Equals("Save & Compare"))
                            {
                                //var matches = QualityCompare(model);
                                ViewBag.Message = results.QualityScoreComments;  
                            }
                            else
                                return RedirectToAction(model.ReturnUrl, new { page = model.Page, search = model.FilterSearchName, message = "Quality Record (" + model.Name + " " + model.Surname + ") updated." });
                        else
                            model.ErrorMessage = "Error in updating patient!";
                    }
                    else
                    {
                        ViewBag.Message = "Duplicate found!";
                    }
                }
            }

            model = LoadLookUpList(model);

            return View(model);
        }

        private QualityScoreResults QualityCompare(QualityPatientModel q)
        {
            var matchCount = 0;
            var match30Count = 0;
            var mismatchCount = 0;
            var missing = "";
            var matches = "";
            var QResults = new QualityScoreResults();
            //Get original patient record
            var patientID = q.PatientID;
            Patient p = new Patient();
            p = _patientServices.GetPatientById(patientID);

            //Compare records
            var properties = TypeDescriptor.GetProperties(typeof(QualityPatientModel));
            var pproperties = TypeDescriptor.GetProperties(typeof(Patient));

            foreach (PropertyDescriptor property in properties)
            {
                if (property.Name == "Sex" || property.Name == "DOB" || property.Name == "Indig" || property.Name == "Ethinc" || property.Name == "AdmissionViaED" || property.Name == "UResidence" ||
                    property.Name == "TransferDateTime" || property.Name == "ArrivalDateTime" || property.Name == "DepartureDateTime" || property.Name == "InHospFractureDateTime" || property.Name == "PatientType" ||
                    property.Name == "TransferHospital" || property.Name == "WardType" || property.Name == "PreAdWalk" || property.Name == "PainAssessment" || property.Name == "PainManagement" ||
                    property.Name == "CognitiveAssessment" || property.Name == "CognitiveState" || property.Name == "BoneMed" || property.Name == "PreOpMedAss" || property.Name == "AtypicalFracture" ||
                    property.Name == "FractureType" || property.Name == "Surgery" || property.Name == "SurgeryDelay" || property.Name == "ASAGrade" || property.Name == "Anaesthesia" || property.Name == "Analgesia" ||
                    property.Name == "ConsultantPresent" || property.Name == "Operation" || property.Name == "InterOpFracture" || property.Name == "FullWeightBear" || property.Name == "Mobilisation" || 
                    property.Name == "PressureUlcers" || property.Name == "DeleriumAssessment" || property.Name == "Malnutrition" || property.Name == "GeriatricAssessment" || property.Name == "FallsAssessment" || 
                    property.Name == "BoneMedDischarge" || property.Name == "WardDischargeDate" || property.Name == "HospitalDischargeDate" || property.Name == "DischargeDest" || property.Name == "DischargeResidence" || 
                    property.Name == "DeathDate" || property.Name == "FirstDayWalking" || property.Name == "GeriatricAssDateTime")

                {
                    var qitem = property.GetValue(q);
                    try
                    {
                        if (property.Name == "DOB" || property.Name == "HospitalDischargeDate" || property.Name == "WardDischargeDate" || property.Name == "GeriatricAssDateTime")
                        {
                            var pitem = string.Format("{0:dd-MMM-yyyy}", pproperties.Find(property.Name, true).GetValue(p));
                            if (qitem is null && pitem == "")
                            {
                                //No values on either record, count as matched
                                properties.Find("Q" + property.Name, true).SetValue(q, 0);
                                matchCount++;
                                match30Count++;
                            }
                            else if (pitem is null)
                            {
                                //False Positive (3)
                                missing += AddMissing(property.Name, pitem, mismatchCount);
                                mismatchCount++;
                                properties.Find("Q" + property.Name, true).SetValue(q,3);
                            }
                            else if (!(qitem is null) && qitem.Equals(pitem))
                            {
                                //Matches (1)
                                matchCount++;
                                match30Count++;
                                properties.Find("Q" + property.Name, true).SetValue(q, 1);
                            }
                            else
                            {
                                //No Match (2)
                                missing += AddMissing(property.Name, pitem, mismatchCount);
                                mismatchCount++;
                                properties.Find("Q" + property.Name, true).SetValue(q, 2);
                            }
                        }
                        else if (property.Name == "ArrivalDateTime" || property.Name == "DepartureDateTime" || property.Name == "TransferDateTime" || property.Name == "SurgeryDateTime")
                        {
                            var pitem = string.Format("{0:dd-MMM-yyyy HH:mm}", pproperties.Find(property.Name, true).GetValue(p));
                            if (qitem is null && pitem == "")
                            {
                                //No values on either side, so matched. 
                                matchCount++;
                                match30Count++;
                                properties.Find("Q" + property.Name, true).SetValue(q, 0);
                            }
                            else if (pitem is null)
                            {
                                //False Positive (3)
                                missing += AddMissing(property.Name, pitem, mismatchCount);
                                mismatchCount++;
                                properties.Find("Q" + property.Name, true).SetValue(q, 3);
                                properties.Find("Q15" + property.Name, true).SetValue(q, 3);
                            }
                            else if (!(qitem is null) && qitem.Equals(pitem))
                            {
                                //Matches exactly (1)
                                matchCount++;
                                match30Count++;
                                properties.Find("Q" + property.Name, true).SetValue(q, 1);
                                properties.Find("Q15" + property.Name, true).SetValue(q, 1);
                            }
                            else if (Within30min(p.InHospFractureDateTime, ConvertToDate(q.InHospFractureDateTime)))
                            {
                                //Matches Within 30 minutes
                                match30Count++;
                                properties.Find("Q" + property.Name, true).SetValue(q, 2);
                                properties.Find("Q15" + property.Name, true).SetValue(q, 1);
                            }
                            else
                            {
                                //No Match (2)
                                missing += AddMissing(property.Name, pitem.ToString(), mismatchCount);
                                mismatchCount++;
                                properties.Find("Q" + property.Name, true).SetValue(q, 2);
                                properties.Find("Q15" + property.Name, true).SetValue(q, 2);
                            };
                        }
                        else
                        {
                            var pitem = pproperties.Find(property.Name, true).GetValue(p);
                            if (qitem is null && pitem is null)
                            {
                                //No values in either record, count as matched
                                matchCount++;
                                match30Count++;
                                properties.Find("Q" + property.Name, true).SetValue(q, 0);
                            }
                            else if (pitem is null)
                            {
                                //False Positive (3)
                                mismatchCount++;
                                missing += AddMissing(property.Name + " (null expected)", qitem.ToString(), mismatchCount);
                                properties.Find("Q" + property.Name, true).SetValue(q, 3);
                            }
                            else if (!(qitem is null) && qitem.Equals(pitem))
                            {
                                //Matches (1)
                                matchCount++;
                                match30Count++;
                                properties.Find("Q" + property.Name, true).SetValue(q, 1);
                            }
                            else
                            {
                                //No Match (2)
                                missing += AddMissing(property.Name, pitem.ToString(), mismatchCount);
                                mismatchCount++;
                                properties.Find("Q" + property.Name, true).SetValue(q, 2);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (qitem is null)
                        {
                            missing += AddMissing(property.Name, "", mismatchCount);
                            mismatchCount++;
                        }
                        else
                        {
                            matches = matches + " + " + property.Name;
                            mismatchCount++;
                        }
                    }
                }
            }
            QResults.QualityScore = matchCount;
            QResults.QualityScore15 = match30Count;
            QResults.QualityScoreMismatch = mismatchCount;
            QResults.QualityScoreComments = "Matched " + matchCount.ToString() + " with " + mismatchCount.ToString() + " mismatches including (" + missing + ")";
            return QResults;
            throw new NotImplementedException();
        }
        private string AddMissing(string field, string val, int count)
        {
            if (count == 0)
            {
                return field + " (" + val + ")";
            }
            return " : " + field + " (" + val + ")";
        }
        private bool Within30min(DateTime? ptime, DateTime? qtime)
        {
            if (ptime is null || qtime is null) return false;
            DateTime endTime = qtime.Value.AddMinutes(15);
            DateTime startTime = qtime.Value.AddMinutes(-15);
            return (ptime <= endTime && ptime >= startTime);
        }

        
        [NonAction]
        public QualityPatientModel PrepareQualityPatientModel(QualityPatient patient)
        {
            var model = new QualityPatientModel();
            model.PatientID = patient.ANZHFRID;
            model.HospitalID = patient.HospitalID;
            model.Name = patient.Name;
            model.Surname = patient.Surname;
            model.MRN = patient.MRN;
            model.Phone = patient.Phone;
            //model.DOB = string.Format("{0:yyyy/MM/dd}", patient.DOB);
            model.DOB = string.Format("{0:dd-MMM-yyyy}", patient.DOB);
            model.QDOB = patient.QDOB ?? 0;

            model.Sex = patient.Sex;
            model.QSex = patient.QSex ?? 0;

            model.Indig = patient.Indig;
            model.QIndig = patient.QIndig ?? 0;

            model.Ethnic = patient.Ethnic;
            model.QEthnic = patient.QEthnic ?? 0;

            model.PostCode = patient.PostCode;
            model.Medicare = patient.Medicare;
            model.PatientType = patient.PatientType;
            model.QPatientType = patient.QPatientType ?? 0;

            model.AdmissionViaED = patient.AdmissionViaED;
            model.UResidence = patient.UResidence;
            model.TransferHospital = patient.TransferHospital;
            model.TransferDateTime = string.Format("{0:dd-MMM-yyyy HH:mm}", patient.TransferDateTime);
            model.ArrivalDateTime = string.Format("{0:dd-MMM-yyyy HH:mm}", patient.ArrivalDateTime);
            model.DepartureDateTime = string.Format("{0:dd-MMM-yyyy HH:mm}", patient.DepartureDateTime);
            model.InHospFractureDateTime = string.Format("{0:dd-MMM-yyyy HH:mm}", patient.InHospFractureDateTime);
            model.WardType = patient.WardType;
            model.PreAdWalk = patient.PreAdWalk;
            model.ASAGrade = patient.ASAGrade;
            model.CognitiveAssessment = patient.CognitiveAssessment;
            model.CognitiveState = patient.CognitiveState;
            model.PainAssessment = patient.PainAssessment;
            model.PainManagement = patient.PainManagement;
            model.BoneMed = patient.BoneMed;
            model.PreOpMedAss = patient.PreOpMedAss;
            model.FractureSide = patient.FractureSide;
            model.AtypicalFracture = patient.AtypicalFracture;
            model.FractureType = patient.FractureType;
            model.Surgery = patient.Surgery;
            model.SurgeryDateTime = string.Format("{0:dd-MMM-yyyy HH:mm}", patient.SurgeryDateTime);
            model.SurgeryDelay = patient.SurgeryDelay;
            model.SurgeryDelayOther = patient.SurgeryDelayOther;
            model.Anaesthesia = patient.Anaesthesia;
            model.Analgesia = patient.Analgesia;
            model.ConsultantPresent = patient.ConsultantPresent;
            model.Operation = patient.Operation;
            model.InterOpFracture = patient.InterOpFracture;
            model.FullWeightBear = patient.FullWeightBear;
            model.Mobilisation = patient.Mobilisation;
            model.PressureUlcers = patient.PressureUlcers;
            model.GeriatricAssessment = patient.GeriatricAssessment;
            model.GeriatricAssDateTime = string.Format("{0:dd-MMM-yyyy}", patient.GeriatricAssDateTime);
            model.FallsAssessment = patient.FallsAssessment;
            model.BoneMedDischarge = patient.BoneMedDischarge;
            model.WardDischargeDate = string.Format("{0:dd-MMM-yyyy}", patient.WardDischargeDate);
            model.DischargeDest = patient.DischargeDest;
            model.HospitalDischargeDate = string.Format("{0:dd-MMM-yyyy}", patient.HospitalDischargeDate);
            model.OLengthofStay = patient.OLengthofStay;
            model.HLengthofStay = patient.HLengthofStay;
            model.DischargeResidence = patient.DischargeResidence;
            model.Completeness = patient.Completeness.GetValueOrDefault();
            model.CompleteExemption = patient.CompleteExemption;
            model.Created = string.Format("{0:dd-MMM-yyyy}", patient.Created);
            model.Author = patient.Author;
            model.LastModified = string.Format("{0:dd-MMM-yyyy}", patient.LastModified);
            model.ModifiedBy = patient.ModifiedBy;
            model.DeleriumAssessment = patient.DeleriumAssessment;
            model.StartDate = string.Format("{0:dd-MMM-yyyy HH:mm}", patient.StartDate);
            model.Informed = patient.Informed ?? false;
            model.OptedOut = patient.OptedOut ?? false;
            model.CannotFollowup = patient.CannotFollowup ?? false;
            model.Malnutrition = patient.Malnutrition;
            model.DeathDate = string.Format("{0:dd-MMM-yyyy}", patient.DeathDate);

            return model;
        }

        private QualityPatientModel LoadLookUpList(QualityPatientModel model)
        {
            model.SexList = _patientServices.GetSex();
            model.IndigList = _patientServices.GetIndig();
            model.EthnicList = _patientServices.GetEthnic();
            model.PatientTypeList = _patientServices.GetPatientType();
            model.AdmissionViaEDList = _patientServices.GetAdmissionViaED();
            model.ResidenceList = _patientServices.GetResidence();
            model.HospitalList = _patientServices.GetHospitals();
            model.WardTypeList = _patientServices.GetWardType();
            model.PreAdWalkList = _patientServices.GetPreAdWalk();
            model.CognitiveStateList = _patientServices.GetCognitiveState();
            model.ASAGradeList = _patientServices.GetASAGrade();
            model.BoneMedList = _patientServices.GetBoneMed();
            model.AMTSList = _patientServices.GetAMTS();
            model.PreOpMedAssList = _patientServices.GetPreOpMedAss();
            model.FractureSideList = _patientServices.GetFractureSide();
            model.AtypicalFractureList = _patientServices.GetAtypicalFracture();
            model.FractureTypeList = _patientServices.GetFractureType();
            model.SurgeryList = _patientServices.GetSurgery();
            model.SurgeryDelayList = _patientServices.GetSurgeryDelay();
            model.AnaesthesiaList = _patientServices.GetAnaesthesia();
            model.AnalgesiaList = _patientServices.GetAnalgesia();
            model.ConsultantPresentList = _patientServices.GetConsultantPresent();
            model.OperationList = _patientServices.GetOperation();
            model.InterOpFractureList = _patientServices.GetInterOpFracture();
            model.PressureUlcersList = _patientServices.GetPressureUlcers();
            model.MobilisationList = _patientServices.GetMobilisation();
            model.GeriatricAssessmentList = _patientServices.GetGeriatricAssessment();
            model.FallsAssessmentList = _patientServices.GetFallsAssessment();
            model.DischargeDestList = _patientServices.GetDischargeDest();
            model.DischargeResidenceList = _patientServices.GetDischargeResidence();
            model.LengthofStayList = _patientServices.GetLengthofStay();
            model.SurvivalList = _patientServices.GetSurvival();
            model.WeightBearList = _patientServices.GetWeightBear();
            model.WalkingAbilityList = _patientServices.GetWalkingAbility();
            model.ReoperationList = _patientServices.GetReoperation();
            model.CognitiveAssessmentList = _patientServices.GetCognitiveAssessment();
            model.PainAssessmentList = _patientServices.GetPainAssessment();
            model.PainManagementList = _patientServices.GetPainManagement();
            model.TransferHospitalList = _transferhospitalServices.Get(CurrentHospitalId(), "");
            model.DeleriumAssessmentList = _patientServices.GetDeleriumAssessment();
            model.MalnutritionList = _patientServices.GetMalnutrition();

            return model;
        }

        private DateTime? ConvertToDate(string date, bool dateOnly = true)
        {
            DateTime temp;

            if (!string.IsNullOrEmpty(date))
            {
                try
                {
                    if (DateTime.TryParse(date, out temp))
                    {
                        //if (dateOnly)
                        //    return DateTime.ParseExact(date, new string[] { "d/M/yyyy", "dd/MM/yyyy", "dd-MMM-yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None);
                        //else
                        //    return DateTime.ParseExact(date, new string[] { "dd/MM/yyyy HH:mm", "dd-MMM-yyyy HH:mm" }, CultureInfo.InvariantCulture, DateTimeStyles.None);

                        return temp;
                    }
                }
                catch { }
            }

            return null;
        }

        public string CalculateStartDate(QualityPatientModel model)
        {
            if (model.AdmissionViaED == "1" || model.AdmissionViaED == "4")
            {
                return model.ArrivalDateTime;
            }
            else if (model.AdmissionViaED == "2")
            {
                return model.TransferDateTime;
            }
            else if (model.AdmissionViaED == "3")
            {
                return model.InHospFractureDateTime;
            }

            return "";
        }

        public decimal? CalculateCompleteness(QualityPatientModel model)
        {
            double fieldCount;
            double missingfields;
            double Completeness = 0.00;

            fieldCount = 30;
            missingfields = 0;
            try
            {
                //Calculate the number of missing fields for a given record.
                if (string.IsNullOrEmpty(model.Name)) ++missingfields;
                if (string.IsNullOrEmpty(model.Surname)) ++missingfields;
                if (string.IsNullOrEmpty(model.Medicare)) ++missingfields;
                if (string.IsNullOrEmpty(model.PostCode)) ++missingfields;
                if (string.IsNullOrEmpty(model.DOB)) ++missingfields;
                if (string.IsNullOrEmpty(model.Sex)) ++missingfields;
                if (string.IsNullOrEmpty(model.Phone)) ++missingfields;
                if (ConfigurationManager.AppSettings["Location"] == "Australian")
                {
                    if (string.IsNullOrEmpty(model.Indig)) ++missingfields;
                    if (string.IsNullOrEmpty(model.PatientType)) ++missingfields;
                }
                else
                {
                    if (string.IsNullOrEmpty(model.Ethnic)) ++missingfields;
                }

                if (string.IsNullOrEmpty(model.UResidence)) ++missingfields;
                if (string.IsNullOrEmpty(model.WardType)) ++missingfields;
                if (string.IsNullOrEmpty(model.PreAdWalk)) ++missingfields;
                if (string.IsNullOrEmpty(model.CognitiveAssessment)) ++missingfields;
                if (string.IsNullOrEmpty(model.CognitiveState)) ++missingfields;
                if (string.IsNullOrEmpty(model.PainAssessment)) ++missingfields;
                if (string.IsNullOrEmpty(model.PainManagement)) ++missingfields;
                if (string.IsNullOrEmpty(model.DeleriumAssessment)) ++missingfields;
                if (string.IsNullOrEmpty(model.BoneMed)) ++missingfields;
                if (string.IsNullOrEmpty(model.PreOpMedAss)) ++missingfields;
                if (string.IsNullOrEmpty(model.AtypicalFracture)) ++missingfields;
                if (string.IsNullOrEmpty(model.FractureType)) ++missingfields;
                if (string.IsNullOrEmpty(model.Surgery)) ++missingfields;
                if (string.IsNullOrEmpty(model.Analgesia)) ++missingfields;
                if (string.IsNullOrEmpty(model.PressureUlcers)) ++missingfields;
                if (string.IsNullOrEmpty(model.GeriatricAssessment)) ++missingfields;
                if (string.IsNullOrEmpty(model.FallsAssessment)) ++missingfields;
                if (string.IsNullOrEmpty(model.BoneMedDischarge)) ++missingfields;
                if (string.IsNullOrEmpty(model.DischargeDest)) ++missingfields;
                if (string.IsNullOrEmpty(model.WardDischargeDate)) ++missingfields;
                if (string.IsNullOrEmpty(model.DischargeResidence)) ++missingfields;
                if (string.IsNullOrEmpty(model.HospitalDischargeDate)) ++missingfields;

                // Add correct date options
                if (!string.IsNullOrEmpty(model.AdmissionViaED) && model.AdmissionViaED.Equals("2"))
                {
                    // Transfer from another hospital
                    fieldCount = fieldCount + 4;
                    if (string.IsNullOrEmpty(model.TransferHospital)) ++missingfields;
                    if (string.IsNullOrEmpty(model.TransferDateTime)) ++missingfields;
                    if (string.IsNullOrEmpty(model.ArrivalDateTime)) ++missingfields;
                    if (string.IsNullOrEmpty(model.DepartureDateTime)) ++missingfields;
                }
                else if (!string.IsNullOrEmpty(model.AdmissionViaED) && model.AdmissionViaED.Equals("3"))
                {
                    // Inpatient fall
                    fieldCount = fieldCount + 1;
                    if (string.IsNullOrEmpty(model.InHospFractureDateTime)) ++missingfields;
                }
                else if (!string.IsNullOrEmpty(model.AdmissionViaED))
                {
                    // Admitted via ED
                    fieldCount = fieldCount + 2;
                    if (string.IsNullOrEmpty(model.ArrivalDateTime)) ++missingfields;
                    if (string.IsNullOrEmpty(model.DepartureDateTime)) ++missingfields;
                }
                else
                {
                    // Cannot be complete unless Arrival from ED is completed
                    fieldCount = fieldCount + 1;
                }

                //Include Surgery fields if necessary.
                if (!string.IsNullOrEmpty(model.Surgery) && model.Surgery.Equals("2"))
                {
                    fieldCount = fieldCount + 6;
                    if (string.IsNullOrEmpty(model.SurgeryDateTime)) ++missingfields;
                    if (string.IsNullOrEmpty(model.Anaesthesia)) ++missingfields;
                    if (string.IsNullOrEmpty(model.ASAGrade)) ++missingfields;
                    if (string.IsNullOrEmpty(model.Operation)) ++missingfields;
                    if (string.IsNullOrEmpty(model.FullWeightBear)) ++missingfields;
                    if (string.IsNullOrEmpty(model.Mobilisation)) ++missingfields;
                }
                
                if (model.CannotFollowup == true || model.OptedOut == true)
                {
                    missingfields = 0;
                }
                Completeness = missingfields;
                return Convert.ToDecimal(missingfields);
            }
            catch { }
            return null;
        }
    }
}
