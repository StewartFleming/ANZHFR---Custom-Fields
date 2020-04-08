using ANZHFR.Data.Models;
using ANZHFR.Services.Patients;
using ANZHFR.Web.ExtensionMethods;
using ANZHFR.Web.Helpers;
using ANZHFR.Web.Models;
using Excel;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace ANZHFR.Web.Controllers
{
    public class ImExController : BaseController
    {
        private readonly PatientServices _patientService = null;

        public ImExController()
        {
            _patientService = new PatientServices();
        }

        #region Export

        [Authorize]
        public ActionResult Export()
        {
            ViewBag.MenuExport = "active";
            ViewBag.Hospital = CurrentHospitalId();
            return View();
        }
        [Authorize]
        public JsonResult Export_Count()
        {
            long hospitalId = CurrentHospitalId();
            var from_date = Request["From"];
            var to_date = Request["To"];

            var patients = _patientService.GetExportCount(hospitalId, from_date, to_date);
            ResultString = patients.Count.ToString();
            //Return result
            return Json(ResultString, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DoExport(string fileFormat, string from_date, string to_date)
        {
            long hospitalId = CurrentHospitalId();
            if (ConfigurationManager.AppSettings["Location"] == "Australian")
            {
                List<ExportPatientModel> resultList = _patientService.GetPatientsForExport(hospitalId, from_date, to_date).Select(p => new ExportPatientModel
                {
                    AdmissionViaED = p.AdmissionViaED,
                    Anaesthesia = p.Anaesthesia,
                    Analgesia = p.Analgesia,
                    ArrivalDate = ExportHelper.getDate(p.ArrivalDateTime),
                    ArrivalTime = ExportHelper.getTime(p.ArrivalDateTime),
                    ASAgrade = p.ASAGrade,
                    PatientType = p.PatientType,
                    Operation = p.Operation,
                    Phone = p.Phone,
                    PostCode = p.PostCode,
                    PreAdWalk = p.PreAdWalk,
                    PreOpMedAss = p.PreOpMedAss,
                    PressureUlcers = p.PressureUlcers,
                    ConsultantPresent = p.ConsultantPresent,
                    DepartureDate = ExportHelper.getDate(p.DepartureDateTime),
                    DepartureTime = ExportHelper.getTime(p.DepartureDateTime),
                    Reoperation120 = p.Reoperation120,
                    Reoperation30 = p.Reoperation30,
                    AtypicalFracture = p.AtypicalFracture,
                    BoneMed = p.BoneMed,
                    BoneMedDischarge = p.BoneMedDischarge,
                    BoneMed120 = p.BoneMed120,
                    BoneMed30 = p.BoneMed30,
                    CognitiveAssessment = p.CognitiveAssessment,
                    CognitiveState = p.CognitiveState,
                    DeleriumAssessment = p.DeleriumAssessment,
                    DischargeDest = p.DischargeDest,
                    DischargeResidence = p.DischargeResidence,
                    DOB = ExportHelper.getDate(p.DOB),
                    DeathDate = ExportHelper.getDate(p.DeathDate),
                    FallsAssessment = p.FallsAssessment,
                    FirstDayWalking = p.FirstDayWalking,
                    FollowupDate120 = ExportHelper.getDate(p.FollowupDate120),
                    FollowupDate30 = ExportHelper.getDate(p.FollowupDate30),
                    FractureSide = p.FractureSide,
                    FractureType = p.FractureType,
                    FullWeightBear = p.FullWeightBear,
                    GeriatricAssDate = ExportHelper.getDate(p.GeriatricAssDateTime),
                    GeriatricAssessment = p.GeriatricAssessment,
                    HealthServiceDischarge120 = ExportHelper.getDate(p.HealthServiceDischarge120),
                    HealthServiceDischarge30 = ExportHelper.getDate(p.HealthServiceDischarge30),
                    Indig = p.Indig,
                    Malnutrition = p.Malnutrition,
                    Medicare = p.Medicare,
                    Mobilisation = p.Mobilisation,
                    MRN = p.MRN,
                    Name = p.Name,
                    PainAssessment = p.PainAssessment,
                    PainManagement = p.PainManagement,
                    Surname = p.Surname,
                    Surgery = p.Surgery,
                    SurgeryDelay = p.SurgeryDelay,
                    SurgeryDelayOther = p.SurgeryDelayOther,
                    Survival120 = p.Survival120,
                    Survival30 = p.Survival30,
                    UResidence = p.UsualResidence,
                    SurgeryDate = ExportHelper.getDate(p.SurgeryDateTime),
                    SurgeryTime = ExportHelper.getTime(p.SurgeryDateTime),
                    InHospFractureDate = ExportHelper.getDate(p.InHospFractureDateTime),
                    InHospFractureTime = ExportHelper.getTime(p.InHospFractureDateTime),
                    InterOpFracture = p.InterOpFracture,
                    Residence120 = p.Residence120,
                    Residence30 = p.Residence30,
                    HospitalDischargeDate = ExportHelper.getDate(p.HospitalDischargeDate),
                    TransferHospital = p.TransferHospital,
                    TransferDate = ExportHelper.getDate(p.TransferDateTime),
                    TransferTime = ExportHelper.getTime(p.TransferDateTime),
                    WalkingAbility120 = p.WalkingAbility120,
                    WalkingAbility30 = p.WalkingAbility30,
                    Sex = p.Sex,
                    WeightBear120 = p.WeightBear120,
                    WeightBear30 = p.WeightBear30,
                    WardType = p.WardType,
                    WardDischargeDate = ExportHelper.getDate(p.WardDischargeDate),
                    Age = ExportHelper.getAge(p.DOB, p.ArrivalDateTime, p.InHospFractureDateTime, p.TransferDateTime),
                    OLengthofStay = ExportHelper.getOHLength(p.WardDischargeDate, p.ArrivalDateTime),
                    HLengthofStay = ExportHelper.getOHLength(p.HospitalDischargeDate, p.ArrivalDateTime),
                    Informed = p.Informed ?? false,
                    CannotFollowup = p.CannotFollowup ?? false
                }).ToList<ExportPatientModel>();

                ExportHelper.Export(fileFormat, resultList, Response);
            }
            else
            {
                List<ExportPatientModel> resultList = _patientService.GetPatientsForExport(hospitalId, from_date, to_date).Select(p => new ExportPatientModel
                {
                    AdmissionViaED = p.AdmissionViaED,
                    Anaesthesia = p.Anaesthesia,
                    Analgesia = p.Analgesia,
                    ArrivalDate = ExportHelper.getDate(p.ArrivalDateTime),
                    ArrivalTime = ExportHelper.getTime(p.ArrivalDateTime),
                    ASAgrade = p.ASAGrade,
                    Operation = p.Operation,
                    Phone = p.Phone,
                    PostCode = p.PostCode,
                    PreAdWalk = p.PreAdWalk,
                    PreOpMedAss = p.PreOpMedAss,
                    PressureUlcers = p.PressureUlcers,
                    ConsultantPresent = p.ConsultantPresent,
                    DepartureDate = ExportHelper.getDate(p.DepartureDateTime),
                    DepartureTime = ExportHelper.getTime(p.DepartureDateTime),
                    Reoperation120 = p.Reoperation120,
                    Reoperation30 = p.Reoperation30,
                    AtypicalFracture = p.AtypicalFracture,
                    BoneMed = p.BoneMed,
                    BoneMedDischarge = p.BoneMedDischarge,
                    BoneMed120 = p.BoneMed120,
                    BoneMed30 = p.BoneMed30,
                    CognitiveAssessment = p.CognitiveAssessment,
                    CognitiveState = p.CognitiveState,
                    DeleriumAssessment = p.DeleriumAssessment,
                    DischargeDest = p.DischargeDest,
                    DischargeResidence = p.DischargeResidence,
                    DOB = ExportHelper.getDate(p.DOB),
                    DeathDate = ExportHelper.getDate(p.DeathDate),
                    FallsAssessment = p.FallsAssessment,
                    FirstDayWalking = p.FirstDayWalking,
                    FollowupDate120 = ExportHelper.getDate(p.FollowupDate120),
                    FollowupDate30 = ExportHelper.getDate(p.FollowupDate30),
                    FractureSide = p.FractureSide,
                    FractureType = p.FractureType,
                    FullWeightBear = p.FullWeightBear,
                    GeriatricAssDate = ExportHelper.getDate(p.GeriatricAssDateTime),
                    GeriatricAssessment = p.GeriatricAssessment,
                    HealthServiceDischarge120 = ExportHelper.getDate(p.HealthServiceDischarge120),
                    HealthServiceDischarge30 = ExportHelper.getDate(p.HealthServiceDischarge30),
                    Indig = p.Ethnic,
                    Malnutrition = p.Malnutrition,
                    Medicare = p.Medicare,
                    Mobilisation = p.Mobilisation,
                    MRN = p.MRN,
                    Name = p.Name,
                    PainAssessment = p.PainAssessment,
                    PainManagement = p.PainManagement,
                    Surname = p.Surname,
                    Surgery = p.Surgery,
                    SurgeryDelay = p.SurgeryDelay,
                    SurgeryDelayOther = p.SurgeryDelayOther,
                    Survival120 = p.Survival120,
                    Survival30 = p.Survival30,
                    UResidence = p.UsualResidence,
                    SurgeryDate = ExportHelper.getDate(p.SurgeryDateTime),
                    SurgeryTime = ExportHelper.getTime(p.SurgeryDateTime),
                    InHospFractureDate = ExportHelper.getDate(p.InHospFractureDateTime),
                    InHospFractureTime = ExportHelper.getTime(p.InHospFractureDateTime),
                    InterOpFracture = p.InterOpFracture,
                    Residence120 = p.Residence120,
                    Residence30 = p.Residence30,
                    HospitalDischargeDate = ExportHelper.getDate(p.HospitalDischargeDate),
                    TransferHospital = p.TransferHospital,
                    TransferDate = ExportHelper.getDate(p.TransferDateTime),
                    TransferTime = ExportHelper.getTime(p.TransferDateTime),
                    WalkingAbility120 = p.WalkingAbility120,
                    WalkingAbility30 = p.WalkingAbility30,
                    Sex = p.Sex,
                    WeightBear120 = p.WeightBear120,
                    WeightBear30 = p.WeightBear30,
                    WardType = p.WardType,
                    WardDischargeDate = ExportHelper.getDate(p.WardDischargeDate),
                    Age = ExportHelper.getAge(p.DOB, p.ArrivalDateTime, p.InHospFractureDateTime, p.TransferDateTime),
                    OLengthofStay = ExportHelper.getOHLength(p.WardDischargeDate, p.ArrivalDateTime),
                    HLengthofStay = ExportHelper.getOHLength(p.HospitalDischargeDate, p.ArrivalDateTime),
                    Informed = p.Informed ?? false,
                    CannotFollowup = p.CannotFollowup ?? false,
                    StartDate = ExportHelper.getDate(p.StartDate)
                }).ToList<ExportPatientModel>();

                ExportHelper.Export(fileFormat, resultList, Response);
            }

            ViewBag.Message = "Export successful.";
            return View();
        }

        #endregion

        #region Import

        [Authorize]
        public ActionResult Import()
        {
            ViewBag.MenuImport = "active";
            ViewBag.Hospital = CurrentHospitalId();
            return View();
        }

        public JsonResult Upload()
        {
            HttpPostedFileBase file = Request.Files["Filedata"];

            string targetDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings.Get("UploadDirectory"));
            string fileName = DateTime.Now.ToString("yyyy_M_d_H_m_s_") + file.FileName;
            string targetFilePath = Path.Combine(targetDirectory, fileName);

            string fileFormat = "2018";

            file.SaveAs(targetFilePath);

            long hospitalId = long.Parse(Request.Form["hospitalId"]);

            return Json(new { hospitalId = hospitalId, fileName = fileName, fileFormat = fileFormat });
        }

        [HttpPost]
        public JsonResult ImportData(long hospitalId, string fileName, string fileFormat)
        {
            string targetDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings.Get("UploadDirectory"));
            string targetFilePath = Path.Combine(targetDirectory, fileName);

            List<ImportPatientModel> patients = new List<ImportPatientModel>();

            FileStream stream = System.IO.File.Open(targetFilePath, FileMode.Open, FileAccess.Read);

            string extension = Path.GetExtension(targetFilePath);

            List<string> messages = new List<string>();
            ImportStatusModel importStats = new ImportStatusModel();

            if (stream != null)
            {
                var dataTable = ImportHelper.ParseUsingExceldataReader(stream, extension);
                bool patientExists = false, errorOccurred = false;
                List<string> error_messages = new List<string>();
                int insertedRecords = 0, updatedRecords = 0, skippedRecords = 0, line = int.Parse(ConfigurationManager.AppSettings["DataRow"]);
                
                if (fileFormat == "2019" && ConfigurationManager.AppSettings["Location"] == "Australian" && dataTable[0].Count != 68)
                {
                    error_messages.Add("<strong>Wrong amount of columns detected. You should have 68 but have " + dataTable[0].Count + ". </strong>");
                }
                else if (fileFormat == "2018" && ConfigurationManager.AppSettings["Location"] == "Australian" && dataTable[0].Count != 75)
                {
                    error_messages.Add("<strong>Wrong amount of columns detected. You should have 75 but have " + dataTable[0].Count + ". </strong>");
                }
                else if (fileFormat == "2017" && ConfigurationManager.AppSettings["Location"] == "Australian" && dataTable[0].Count != 73)
                {
                    error_messages.Add("<strong>Wrong amount of columns detected. You should have 73 but have " + dataTable[0].Count + ". </strong>");
                }
                else if (fileFormat == "2019" && ConfigurationManager.AppSettings["Location"] != "Australian" && dataTable[0].Count != 67)
                {
                    error_messages.Add("<strong>Wrong amount of columns detected. You should have 67 but have " + dataTable[0].Count + ". </strong>");
                }
                else if (fileFormat == "2018" && ConfigurationManager.AppSettings["Location"] != "Australian" && dataTable[0].Count != 74)
                {
                    error_messages.Add("<strong>Wrong amount of columns detected. You should have 74 but have " + dataTable[0].Count + ". </strong>");
                }
                else if (fileFormat == "2017" && ConfigurationManager.AppSettings["Location"] != "Australian" && dataTable[0].Count != 72)
                {
                    error_messages.Add("<strong>Wrong amount of columns detected. You should have 72 but have " + dataTable[0].Count + ". </strong>");
                }
                else
                {
                    patients = dataTable.GetPatientModel(fileFormat);

                    
                    foreach (ImportPatientModel model in patients)
                    {
                        model.HospitalID = hospitalId;
                        model.Line = line;
                        if (string.IsNullOrEmpty(model.MRN) || string.IsNullOrEmpty(model.FractureSide))
                        {
                            //Skip Record
                            skippedRecords++;
                            error_messages.Add("Skipping record on line " + line + " with MRN '" + model.MRN + "' and FractureSide " + model.FractureSide);
                        }
                        else if (model.MRN.Length > 20)
                        {
                            //Skip Record
                            skippedRecords++;
                            error_messages.Add("Skipping record on line " + line + " with MRN '" + model.MRN + "' - more than 40 characters");
                        }
                        else if (string.IsNullOrEmpty(model.ArrivalDate) && string.IsNullOrEmpty(model.InHospFractureDate) && string.IsNullOrEmpty(model.TransferDate))
                        {
                            //Skip Record
                            skippedRecords++;
                            error_messages.Add("Skipping record on line " + line + " with no valid ED Admission, In Patient Fall or Transfer date.");
                        }
                        else if (model.IsValid == false)
                        {
                            error_messages.AddRange(model.Messages);
                            skippedRecords++;
                        }
                        else
                        {
                            patientExists = false;
                            errorOccurred = false;

                            DateTime? TransferDateTime = ConvertToDate((model.TransferDate + " " + model.TransferTime), false);
                            DateTime? ArrivalDateTime = ConvertToDate((model.ArrivalDate + " " + model.ArrivalTime), false);
                            DateTime? DepartureDateTime = ConvertToDate((model.DepartureDate + " " + model.DepartureTime), false);
                            DateTime? InHospFractureDateTime = ConvertToDate((model.InHospFractureDate + " " + model.InHospFractureTime), false);
                            DateTime? SurgeryDateTime = ConvertToDate((model.SurgeryDate + " " + model.SurgeryTime), false);

                            DateTime? DOBDate = ConvertToDate(model.DOB);
                            DateTime? GeriatricAssDateTime = ConvertToDate(model.GeriatricAssDate);
                            DateTime? HospitalDischargeDate = ConvertToDate(model.HospitalDischargeDate);
                            DateTime? WardDischargeDate = ConvertToDate(model.WardDischargeDate);
                            DateTime? FollowupDate30 = ConvertToDate(model.FollowupDate30);
                            DateTime? HealthServiceDischarge30 = ConvertToDate(model.HealthServiceDischarge30);
                            DateTime? FollowupDate120 = ConvertToDate(model.FollowupDate120);
                            DateTime? HealthServiceDischarge120 = ConvertToDate(model.HealthServiceDischarge120);
                            DateTime? Created = ConvertToDate(model.Created);
                            DateTime? LastModified = ConvertToDate(model.LastModified);
                            DateTime? DeathDate = ConvertToDate(model.DeathDate);

                            model.StartDate = CalculateStartDate(model.AdmissionViaED, ArrivalDateTime, TransferDateTime, InHospFractureDateTime);

                            short oLengthOfStay = ExportHelper.getOHLength(ConvertToDate(model.WardDischargeDate), ArrivalDateTime);
                            short hLengthOfStay = ExportHelper.getOHLength(ConvertToDate(model.HospitalDischargeDate), ArrivalDateTime);

                            if (!string.IsNullOrEmpty(model.MRN) && !string.IsNullOrEmpty(model.FractureSide))
                            {
                                var patient = _patientService.GetUniquePatient(hospitalId, model.MRN, model.FractureSide);
                                var modifiedBy = CurrentUser();

                                if (patient != null)
                                {
                                    try
                                    {
                                        patientExists = true;
                                        var completeness = CalculateCompleteness2(model);

                                        if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["ImportUpdateDB"]) &&
                                            Convert.ToBoolean(ConfigurationManager.AppSettings["ImportUpdateDB"]))
                                        {
                                            LastModified = DateTime.Now;
                                            patient = _patientService.Update(patient.ANZHFRID, model.Name, model.Surname, model.MRN,
                                               model.Phone, DOBDate, model.Sex, model.Indig, model.Ethnic, model.PostCode, model.Medicare,
                                               model.PatientType, model.AdmissionViaED, model.UResidence, model.TransferHospital, TransferDateTime,
                                               ArrivalDateTime, DepartureDateTime, InHospFractureDateTime,
                                               model.WardType, model.PreAdWalk, model.CognitiveState, model.BoneMed, model.PreOpMedAss, model.FractureSide,
                                               model.AtypicalFracture, model.FractureType, model.Surgery, SurgeryDateTime, model.SurgeryDelay,
                                               model.SurgeryDelayOther, model.Anaesthesia, model.Analgesia, model.ConsultantPresent, model.Operation, model.InterOpFracture,
                                               model.FullWeightBear, model.Mobilisation, model.PressureUlcers, model.GeriatricAssessment, GeriatricAssDateTime,
                                               model.FallsAssessment, model.BoneMedDischarge, WardDischargeDate, model.DischargeDest,
                                               HospitalDischargeDate, model.DischargeResidence, oLengthOfStay, hLengthOfStay,
                                               FollowupDate30, HealthServiceDischarge30, model.Survival30, model.Residence30,
                                               model.WeightBear30, model.WalkingAbility30, model.BoneMed30, model.Reoperation30, FollowupDate120,
                                               HealthServiceDischarge120, model.Survival120, model.Residence120,
                                               model.WeightBear120, model.WalkingAbility120, model.BoneMed120, model.Reoperation120, model.ASAgrade,
                                               completeness, model.CompleteExemption, LastModified ?? default(DateTime), modifiedBy,
                                               model.CognitiveAssessment, model.PainAssessment, model.PainManagement,
                                               model.DeleriumAssessment, ConvertToDate(model.StartDate, false), model.Informed, model.OptedOut, model.CannotFollowup,
                                               model.Malnutrition,
                                               ConvertToDate(model.DeathDate, false), model.FirstDayWalking,
                                               model.EQ5D_Mobility, model.EQ5D_SelfCare, model.EQ5D_UsualActivity, model.EQ5D_Pain, model.EQ5D_Anxiety, model.EQ5D_Health
                                               );

                                            if (patient != null)
                                            {
                                                updatedRecords++;
                                            }
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        error_messages.Add("Failed to Update patient in line " + line + ": " + e);
                                    }
                                }
                            }
                            else
                            {
                                errorOccurred = true;
                                skippedRecords++;

                                //if (ArrivalDateTime == null)
                                //	error_messages.Add("Arrival date is empty in line " + line);

                                if (string.IsNullOrEmpty(model.MRN))
                                    error_messages.Add("MRN is empty in line " + line);

                                if (string.IsNullOrEmpty(model.FractureSide))
                                    error_messages.Add("Fracture side is empty in line " + line);
                            }

                            if (!patientExists && !errorOccurred)
                            {
                                var author = CurrentUser();
                                var modifiedBy = CurrentUser();
                                var completeness = CalculateCompleteness2(model);
                                Created = DateTime.Now;
                                try
                                {
                                    if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["ImportUpdateDB"]) &&
                                              Convert.ToBoolean(ConfigurationManager.AppSettings["ImportUpdateDB"]))
                                    {
                                        var patient = _patientService.Insert(hospitalId, model.Name, model.Surname, model.MRN,
                                        model.Phone, DOBDate, model.Sex, model.Indig, model.Ethnic, model.PostCode, model.Medicare,
                                        model.PatientType, model.AdmissionViaED, model.UResidence, model.TransferHospital,
                                        TransferDateTime, ArrivalDateTime, DepartureDateTime, InHospFractureDateTime,
                                        model.WardType, model.PreAdWalk, model.CognitiveState, model.BoneMed, model.PreOpMedAss, model.FractureSide,
                                        model.AtypicalFracture, model.FractureType, model.Surgery, SurgeryDateTime, model.SurgeryDelay,
                                        model.SurgeryDelayOther, model.Anaesthesia, model.Analgesia, model.ConsultantPresent, model.Operation, model.InterOpFracture,
                                        model.FullWeightBear, model.Mobilisation, model.PressureUlcers, model.GeriatricAssessment, GeriatricAssDateTime,
                                        model.FallsAssessment, model.BoneMedDischarge, ConvertToDate(model.WardDischargeDate), model.DischargeDest,
                                        ConvertToDate(model.HospitalDischargeDate), model.DischargeResidence, oLengthOfStay, hLengthOfStay,
                                        ConvertToDate(model.FollowupDate30), ConvertToDate(model.HealthServiceDischarge30), model.Survival30, model.Residence30,
                                        model.WeightBear30, model.WalkingAbility30, model.BoneMed30, model.Reoperation30, ConvertToDate(model.FollowupDate120),
                                        ConvertToDate(model.HealthServiceDischarge120), model.Survival120, model.Residence120,
                                        model.WeightBear120, model.WalkingAbility120, model.BoneMed120, model.Reoperation120, model.ASAgrade,
                                        completeness, model.CompleteExemption, Created, author, DateTime.Now, modifiedBy,
                                        model.CognitiveAssessment, model.PainAssessment, model.PainManagement,
                                        model.DeleriumAssessment, ConvertToDate(model.StartDate), model.Informed, model.OptedOut, model.CannotFollowup,
                                        model.Malnutrition
                                        );

                                        if (patient != null)
                                        {
                                            insertedRecords++;
                                        }
                                        else
                                        {
                                            skippedRecords++;
                                            error_messages.Add("Failed to Insert patient on INSERT in line " + line);
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    error_messages.Add("Failed to Insert patient in line " + line + ": " + e);
                                    skippedRecords++;
                                }
                            }
                        }

                        line++;
                    }
                }
                messages.Insert(0, "Number of records failed: " + skippedRecords);
                messages.Insert(0, "Number of records updated: " + updatedRecords);
                messages.Insert(0, "Number of records inserted: " + insertedRecords);
                messages.Insert(0, "<strong>Total Number of records: " + (insertedRecords + updatedRecords + skippedRecords) + "</strong>");

                importStats.success = messages;
                importStats.errors = error_messages;
            }

            return Json(importStats);
        }

        public string CalculateStartDate(string admissionViaED, DateTime? arrivalDateTime, DateTime? transferDateTime, DateTime? inHospFractureDateTime)
        {
            if (admissionViaED == "1" || admissionViaED == "4")
            {
                return string.Format("{0}", arrivalDateTime);
            }
            else if (admissionViaED == "2")
            {
                return string.Format("{0}", transferDateTime);
            }
            else if (admissionViaED == "3")
            {
                return string.Format("{0}", inHospFractureDateTime);
            }

            return "";
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
                        if (dateOnly)
                            return DateTime.ParseExact(temp.ToString("yyyy/MM/dd"), "yyyy/MM/dd", CultureInfo.InvariantCulture);
                        else
                            // Changed to 24 hour time.
                            return DateTime.ParseExact(temp.ToString("yyyy/MM/dd hh:mm tt"), "yyyy/MM/dd hh:mm tt", CultureInfo.InvariantCulture);
                    }
                }
                catch { }
            }

            return null;
        }

        public decimal CalculateCompleteness2(ImportPatientModel model)
        {
            double fieldCount;
            double filledfields;
            double Completeness = 0.00;

            fieldCount = 31;
            filledfields = 0;
            try
            {
                //Calculate the percentage of completeness for a given record.
                if (!string.IsNullOrEmpty(model.Name)) ++filledfields;
                if (!string.IsNullOrEmpty(model.Surname)) ++filledfields;
                if (!string.IsNullOrEmpty(model.Medicare)) ++filledfields;
                if (!string.IsNullOrEmpty(model.PostCode)) ++filledfields;
                if (!string.IsNullOrEmpty(model.DOB)) ++filledfields;
                if (!string.IsNullOrEmpty(model.Sex) && model.Sex != "0") ++filledfields;
                if (!string.IsNullOrEmpty(model.Phone)) ++filledfields;
                
                if (ConfigurationManager.AppSettings["Location"] == "Australian")
                {
                    if (!string.IsNullOrEmpty(model.Indig) && model.Indig != "0") ++filledfields;
                    if (!string.IsNullOrEmpty(model.PatientType) && model.PatientType != "0") ++filledfields;
                }
                else
                {
                    if (!string.IsNullOrEmpty(model.Ethnic) && model.Ethnic != "0") ++filledfields;
                }

                if (!string.IsNullOrEmpty(model.UResidence) && model.UResidence != "0") ++filledfields;
                if (!string.IsNullOrEmpty(model.WardType) && model.WardType != "0") ++filledfields;
                if (!string.IsNullOrEmpty(model.PreAdWalk) && model.PreAdWalk != "0") ++filledfields;
                if (!string.IsNullOrEmpty(model.ASAgrade) && model.ASAgrade != "0") ++filledfields;
                if (!string.IsNullOrEmpty(model.CognitiveAssessment) && model.CognitiveAssessment != "0") ++filledfields;
                if (!string.IsNullOrEmpty(model.PainAssessment) && model.PainAssessment != "0") ++filledfields;
                if (!string.IsNullOrEmpty(model.PainManagement) && model.PainManagement != "0") ++filledfields;
                if (!string.IsNullOrEmpty(model.CognitiveState) && model.CognitiveState != "0") ++filledfields;
                if (!string.IsNullOrEmpty(model.BoneMed) && model.BoneMed != "0") ++filledfields;
                if (!string.IsNullOrEmpty(model.PreOpMedAss) && model.PreOpMedAss != "0") ++filledfields;
                if (!string.IsNullOrEmpty(model.AtypicalFracture) && model.AtypicalFracture != "0") ++filledfields;
                if (!string.IsNullOrEmpty(model.FractureType) && model.FractureType != "0") ++filledfields;
                if (!string.IsNullOrEmpty(model.Surgery) && model.Surgery != "0") ++filledfields;
                if (!string.IsNullOrEmpty(model.Analgesia) && model.Analgesia != "0") ++filledfields;
                if (!string.IsNullOrEmpty(model.PressureUlcers) && model.PressureUlcers != "0") ++filledfields;
                if (!string.IsNullOrEmpty(model.Malnutrition) && model.Malnutrition != "0") ++filledfields;
                if (!string.IsNullOrEmpty(model.GeriatricAssessment) && model.GeriatricAssessment != "0") ++filledfields;
                if (!string.IsNullOrEmpty(model.FallsAssessment) && model.FallsAssessment != "0") ++filledfields;
                if (!string.IsNullOrEmpty(model.BoneMedDischarge) && model.BoneMedDischarge != "0") ++filledfields;
                if (!string.IsNullOrEmpty(model.DischargeDest) && model.DischargeDest != "0") ++filledfields;
                if (!string.IsNullOrEmpty(model.WardDischargeDate)) ++filledfields;
                if (!string.IsNullOrEmpty(model.DischargeResidence) && model.DischargeResidence != "0") ++filledfields;
                if (!string.IsNullOrEmpty(model.HospitalDischargeDate)) ++filledfields;

                // Add correct date options
                if (!string.IsNullOrEmpty(model.AdmissionViaED) && model.AdmissionViaED.Equals("2"))
                {
                    // Transfer from another hospital
                    fieldCount = fieldCount + 4;
                    if (!string.IsNullOrEmpty(model.TransferHospital) && model.TransferHospital != "0") ++filledfields;
                    if (!string.IsNullOrEmpty(model.TransferDate)) ++filledfields;
                    if (!string.IsNullOrEmpty(model.ArrivalDate)) ++filledfields;
                    if (!string.IsNullOrEmpty(model.DepartureDate)) ++filledfields;
                }
                else if (!string.IsNullOrEmpty(model.AdmissionViaED) && model.AdmissionViaED.Equals("3"))
                {
                    // Inpatient fall
                    fieldCount = fieldCount + 1;
                    if (!string.IsNullOrEmpty(model.InHospFractureDate)) ++filledfields;
                }
                else if (!string.IsNullOrEmpty(model.AdmissionViaED))
                {
                    // Admitted via ED
                    fieldCount = fieldCount + 2;
                    if (!string.IsNullOrEmpty(model.ArrivalDate)) ++filledfields;
                    if (!string.IsNullOrEmpty(model.DepartureDate)) ++filledfields;
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
                    if (!string.IsNullOrEmpty(model.SurgeryDate)) ++filledfields;
                    if (!string.IsNullOrEmpty(model.Anaesthesia) && model.Anaesthesia != "0") ++filledfields;
                    if (!string.IsNullOrEmpty(model.Analgesia) && model.Analgesia != "0") ++filledfields;
                    if (!string.IsNullOrEmpty(model.Operation) && model.Operation != "0") ++filledfields;
                    if (!string.IsNullOrEmpty(model.FullWeightBear) && model.FullWeightBear != "0") ++filledfields;
                    if (!string.IsNullOrEmpty(model.Mobilisation) && model.Mobilisation != "0") ++filledfields;
                }
                // Include 30 Followup fields if started
                if (!string.IsNullOrEmpty(model.FollowupDate30) && !string.IsNullOrEmpty(model.Survival30) && !model.Survival30.Equals("1"))
                {
                    fieldCount = fieldCount + 6;
                    if (!string.IsNullOrEmpty(model.HealthServiceDischarge30)) ++filledfields;
                    if (!string.IsNullOrEmpty(model.Residence30) && model.Residence30 != "0") ++filledfields;
                    if (!string.IsNullOrEmpty(model.WeightBear30) && model.WeightBear30 != "0") ++filledfields;
                    if (!string.IsNullOrEmpty(model.WalkingAbility30) && model.WalkingAbility30 != "0") ++filledfields;
                    if (!string.IsNullOrEmpty(model.BoneMed30) && model.BoneMed30 != "0") ++filledfields;
                    if (!string.IsNullOrEmpty(model.Reoperation30) && model.Reoperation30 != "0") ++filledfields;
                }

                // Include 120 Followup fields if started
                if (!string.IsNullOrEmpty(model.FollowupDate120) && !string.IsNullOrEmpty(model.Survival120) && !model.Survival120.Equals("1"))
                {
                    fieldCount = fieldCount + 6;
                    if (!string.IsNullOrEmpty(model.HealthServiceDischarge120)) ++filledfields;
                    if (!string.IsNullOrEmpty(model.Residence120) && model.Residence120 != "0") ++filledfields;
                    if (!string.IsNullOrEmpty(model.WeightBear120) && model.WeightBear120 != "0") ++filledfields;
                    if (!string.IsNullOrEmpty(model.WalkingAbility120) && model.WalkingAbility120 != "0") ++filledfields;
                    if (!string.IsNullOrEmpty(model.BoneMed120) && model.BoneMed120 != "0") ++filledfields;
                    if (!string.IsNullOrEmpty(model.Reoperation120) && model.Reoperation120 != "0") ++filledfields;
                }

                //Completeness = (filledfields / fieldCount) * 100;
                Completeness = fieldCount - filledfields;
                return Convert.ToDecimal(Completeness);
            }
            catch { }
            return 0;
        }

        private PatientModel PreparePatientModel(Patient patient)
        {
            var model = new PatientModel();
            model.PatientID = patient.ANZHFRID;
            model.Name = patient.Name;
            model.Surname = patient.Surname;
            model.MRN = patient.MRN;
            model.Phone = patient.Phone;
            //model.DOB = string.Format("{0:yyyy/MM/dd}", patient.DOB);
            model.DOB = string.Format("{0:dd/MM/yyyy}", patient.DOB);
            model.Sex = patient.Sex;
            model.Indig = patient.Indig;
            model.Ethnic = patient.Ethnic;
            model.PostCode = patient.PostCode;
            model.Medicare = patient.Medicare;
            model.PatientType = patient.PatientType;
            model.AdmissionViaED = patient.AdmissionViaED;
            model.UResidence = patient.UResidence;
            model.TransferHospital = patient.TransferHospital;
            model.TransferDateTime = string.Format("{0:dd/MM/yyyy HH:mm}", patient.TransferDateTime);
            model.ArrivalDateTime = string.Format("{0:dd/MM/yyyy HH:mm}", patient.ArrivalDateTime);
            model.DepartureDateTime = string.Format("{0:dd/MM/yyyy HH:mm}", patient.DepartureDateTime);
            model.InHospFractureDateTime = string.Format("{0:dd/MM/yyyy HH:mm}", patient.InHospFractureDateTime);
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
            model.SurgeryDateTime = string.Format("{0:dd/MM/yyyy HH:mm}", patient.SurgeryDateTime);
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
            model.Malnutrition = patient.Malnutrition;
            model.GeriatricAssessment = patient.GeriatricAssessment;
            model.GeriatricAssDateTime = string.Format("{0:dd/MM/yyyy}", patient.GeriatricAssDateTime);
            model.FallsAssessment = patient.FallsAssessment;
            model.BoneMedDischarge = patient.BoneMedDischarge;
            model.WardDischargeDate = string.Format("{0:dd/MM/yyyy}", patient.WardDischargeDate);
            model.DischargeDest = patient.DischargeDest;
            model.HospitalDischargeDate = string.Format("{0:dd/MM/yyyy}", patient.HospitalDischargeDate);
            model.OLengthofStay = patient.OLengthofStay;
            model.HLengthofStay = patient.HLengthofStay;
            model.DischargeResidence = patient.DischargeResidence;
            model.FollowupDate30 = string.Format("{0:dd/MM/yyyy}", patient.FollowupDate30);
            model.HealthServiceDischarge30 = string.Format("{0:dd/MM/yyyy}", patient.HealthServiceDischarge30);
            model.Survival30 = patient.Survival30;
            model.Residence30 = patient.Residence30;
            model.WeightBear30 = patient.WeightBear30;
            model.WalkingAbility30 = patient.WalkingAbility30;
            model.BoneMed30 = patient.BoneMed30;
            model.Reoperation30 = patient.Reoperation30;
            model.FollowupDate120 = string.Format("{0:dd/MM/yyyy}", patient.FollowupDate120);
            model.HealthServiceDischarge120 = string.Format("{0:dd/MM/yyyy}", patient.HealthServiceDischarge120);
            model.Survival120 = patient.Survival120;
            model.Residence120 = patient.Residence120;
            model.WeightBear120 = patient.WeightBear120;
            model.WalkingAbility120 = patient.WalkingAbility120;
            model.BoneMed120 = patient.BoneMed120;
            model.Reoperation120 = patient.Reoperation120;
            model.Completeness = patient.Completeness.GetValueOrDefault();
            model.CompleteExemption = patient.CompleteExemption;
            model.Created = string.Format("{0:dd/MM/yyyy}", patient.Created);
            model.Author = patient.Author;
            model.LastModified = string.Format("{0:dd/MM/yyyy}", patient.LastModified);
            if (string.IsNullOrEmpty(patient.ModifiedBy))
            {
                model.ModifiedBy = "ANZHFR Admin";
            }
            else
            {
                model.ModifiedBy = patient.ModifiedBy;
            }
            model.DeleriumAssessment = patient.DeleriumAssessment;
            model.StartDate = string.Format("{0:dd/MM/yyyy HH:mm}", patient.StartDate);
            model.Informed = patient.Informed.Value;
            model.OptedOut = patient.OptedOut.Value;
            model.CannotFollowup = patient.CannotFollowup.Value;
            return model;
        }

        #endregion
    }
}