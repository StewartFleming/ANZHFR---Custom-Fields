using ANZHFR.Data.Models;
using ANZHFR.Services.Auth;
using ANZHFR.Services.Patients;
using ANZHFR.Web.Helpers;
using ANZHFR.Web.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ANZHFR.Web.Controllers
{
    [Authorize]
    public class PatientController : BaseController
    {
        private readonly UserServices _userServices;
        private readonly PatientServices _patientServices;
        private readonly HospitalServices _hospitalServices;
        private readonly TransferHospitalServices _transferhospitalServices;

        public PatientController()
        {
            _userServices = new UserServices();
            _patientServices = new PatientServices();
            _hospitalServices = new HospitalServices();
            _transferhospitalServices = new TransferHospitalServices();
       //     _qualityServices = new QualityServices();
        }

        [Authorize]
        public ActionResult index(int? page, string search, string message)
        {
            if (_userServices.GetUserRole(CurrentUserID()) == 0)
            {
                return RedirectToAction("../home/Restricted");
            }
            else
            {
                long hopitalId = CurrentHospitalId();
                if (search == "undefined") { search = ""; }
                var results = _patientServices.Get(hopitalId, search);

                ViewBag.MenuPatients = "active";
                ViewBag.HospitalName = _hospitalServices.GetHospitalNameById(hopitalId);
                ViewBag.FilterSearch = search;
                ViewBag.Message = message;
                int pageNumber = (page ?? 1);
                return View(results.OrderBy(x => x.Name)); //.ToPagedList(pageNumber, PageSize));
            }
        }

        [Authorize]
        public ActionResult Create(int page = 1, string search = "")
        {
            var model = new PatientModel();
            model.Page = page;
            model.FilterSearchName = search;
            model.HospitalID = CurrentHospitalId();
            model.Author = CurrentUser();

            model = LoadLookUpList(model);

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(PatientModel model)
        {
            if (string.IsNullOrEmpty(model.MRN))
            {
                ViewBag.Message = "WARNING: Record was not saved. MRN/URN must not be empty.";
            }
            else if (string.IsNullOrEmpty(model.FractureSide))
            {
                ViewBag.Message = "WARNING: Record was not saved. Fracture Side must be selected. Look under Assessment.";
            }
            else if (string.IsNullOrEmpty(model.StartDate))
            {
                ViewBag.Message = "WARNING: Record was not saved. Enter the ED Arrival Date or InPatient Fall Date.";
            }
            else if (ModelState.IsValid)
            {
                DateTime? ArrivalDateTime = ConvertToDate(model.ArrivalDateTime, false);
                var patient = _patientServices.GetUniquePatient(model.HospitalID, model.MRN, model.FractureSide);
                var author = CurrentUser();
                var modifiedBy = CurrentUser();
                var completeness = CalculateCompleteness(model);
                model.StartDate = CalculateStartDate(model);

                if (patient == null)
                {
                    patient = _patientServices.Insert(model.HospitalID, model.Name, model.Surname, model.MRN,
                        model.Phone, ConvertToDate(model.DOB), model.Sex, model.Indig, model.Ethnic, model.PostCode, model.Medicare,
                        model.PatientType, model.AdmissionViaED, model.UResidence, model.TransferHospital, ConvertToDate(model.TransferDateTime, false),
                        ConvertToDate(model.ArrivalDateTime, false), ConvertToDate(model.DepartureDateTime, false), ConvertToDate(model.InHospFractureDateTime, false),
                        model.WardType, model.PreAdWalk, model.CognitiveState, model.BoneMed, model.PreOpMedAss, model.FractureSide,
                        model.AtypicalFracture, model.FractureType, model.Surgery, ConvertToDate(model.SurgeryDateTime, false), model.SurgeryDelay,
                        model.SurgeryDelayOther, model.Anaesthesia, model.Analgesia, model.ConsultantPresent, model.Operation, model.InterOpFracture,
                        model.FullWeightBear, model.Mobilisation, model.PressureUlcers, model.GeriatricAssessment, ConvertToDate(model.GeriatricAssDateTime),
                        model.FallsAssessment, model.BoneMedDischarge, ConvertToDate(model.WardDischargeDate), model.DischargeDest,
                        ConvertToDate(model.HospitalDischargeDate), model.DischargeResidence, model.OLengthofStay, model.HLengthofStay,
                        ConvertToDate(model.FollowupDate30), ConvertToDate(model.HealthServiceDischarge30), model.Survival30, model.Residence30,
                        model.WeightBear30, model.WalkingAbility30, model.BoneMed30, model.Reoperation30, ConvertToDate(model.FollowupDate120),
                        ConvertToDate(model.HealthServiceDischarge120), model.Survival120, model.Residence120,
                        model.WeightBear120, model.WalkingAbility120, model.BoneMed120, model.Reoperation120, model.ASAGrade,
                        completeness.GetValueOrDefault(0), model.CompleteExemption, DateTime.Now, author, DateTime.Now, modifiedBy,
                        model.CognitiveAssessment, model.PainAssessment, model.PainManagement,
                        model.DeleriumAssessment, ConvertToDate(model.StartDate, false), model.Informed, model.OptedOut, model.CannotFollowup, model.Malnutrition);

                    if (patient != null)
                        return RedirectToAction("Index", new { page = model.Page, search = model.FilterSearchName, message = "Patient (" + model.Name + " " + model.Surname + ") created" });
                    else
                        model.ErrorMessage = "Error in creating patient!";
                }
                else
                    ViewBag.Message = "Duplicate found!";
            }

            model = LoadLookUpList(model);

            return View(model);
        }

        [Authorize]
        public ActionResult Edit(int id = 0, int page = 1, string search = "", string returnURL = "")
        {
            var patient = _patientServices.GetPatientById(id);
            var qpatient = _patientServices.GetQualityPatientByPatientId(id);

            if (patient == null || _userServices.GetUserRole(CurrentUserID()) == 0)
            {
                return RedirectToAction("../home/Restricted");
            }
            else if (qpatient != null && qpatient.QualityScore == null)
            {
                return RedirectToAction("../home/QualityLock", new
                {
                    QID = qpatient.QualityID
                    });
            }
            else
            {
                var model = PreparePatientModel(patient);
                model.PatientID = id;
                model.Page = page;
                model.FilterSearchName = search;
                model.ReturnUrl = returnURL;
                model = LoadLookUpList(model);

                return View(model);
            }

           
        }

        [Authorize]
        public ActionResult View(int id = 0, int page = 1, string search = "")
        {
            var patient = _patientServices.GetPatientById(id);

            if (patient == null || _userServices.GetUserRole(CurrentUserID()) == 0 )
            {
                return HttpNotFound();
            }
            else
            {
            
            var model = PrepearViewPatientModel(patient);
            model.PatientID = id;
            model.Page = page;
            model.FilterSearchName = search;
            model = LoadLookUpList(model);

            return View(model);
            }
        }

        [HttpPost]
        public ActionResult Edit(PatientModel model, string Command, FormCollection coll)
        {
            bool isValid = true;
            var item = coll["Continue"];

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
                    DateTime? ArrivalDateTime = ConvertToDate(model.ArrivalDateTime, false);
                    var user = _patientServices.GetUniquePatient(hospitalID, model.MRN, model.FractureSide);
                    var modifiedBy = CurrentUser();
                    var completeness = CalculateCompleteness(model);
                    model.StartDate = CalculateStartDate(model);

                    if (user == null || user.ANZHFRID == model.PatientID)
                    {
                        user = _patientServices.Update(model.PatientID, model.Name, model.Surname, model.MRN,
                            model.Phone, ConvertToDate(model.DOB), model.Sex, model.Indig, model.Ethnic, model.PostCode, model.Medicare,
                            model.PatientType, model.AdmissionViaED, model.UResidence, model.TransferHospital, ConvertToDate(model.TransferDateTime, false),
                            ConvertToDate(model.ArrivalDateTime, false), ConvertToDate(model.DepartureDateTime, false), ConvertToDate(model.InHospFractureDateTime, false),
                            model.WardType, model.PreAdWalk, model.CognitiveState, model.BoneMed, model.PreOpMedAss, model.FractureSide,
                            model.AtypicalFracture, model.FractureType, model.Surgery, ConvertToDate(model.SurgeryDateTime, false), model.SurgeryDelay,
                            model.SurgeryDelayOther, model.Anaesthesia, model.Analgesia, model.ConsultantPresent, model.Operation, model.InterOpFracture,
                            model.FullWeightBear, model.Mobilisation, model.PressureUlcers, model.GeriatricAssessment, ConvertToDate(model.GeriatricAssDateTime),
                            model.FallsAssessment, model.BoneMedDischarge, ConvertToDate(model.WardDischargeDate), model.DischargeDest,
                            ConvertToDate(model.HospitalDischargeDate), model.DischargeResidence, model.OLengthofStay, model.HLengthofStay,
                            ConvertToDate(model.FollowupDate30), ConvertToDate(model.HealthServiceDischarge30), model.Survival30, model.Residence30,
                            model.WeightBear30, model.WalkingAbility30, model.BoneMed30, model.Reoperation30, ConvertToDate(model.FollowupDate120),
                            ConvertToDate(model.HealthServiceDischarge120), model.Survival120, model.Residence120,
                            model.WeightBear120, model.WalkingAbility120, model.BoneMed120, model.Reoperation120, model.ASAGrade,
                            completeness.GetValueOrDefault(0), model.CompleteExemption, DateTime.Now, modifiedBy,
                            model.CognitiveAssessment, model.PainAssessment, model.PainManagement,
                            model.DeleriumAssessment, ConvertToDate(model.StartDate, false), model.Informed, model.OptedOut, model.CannotFollowup, model.Malnutrition,
                            ConvertToDate(model.DeathDate), model.FirstDayWalking, 
                            model.EQ5D_Mobility, model.EQ5D_SelfCare, model.EQ5D_UsualActivity, model.EQ5D_Pain, model.EQ5D_Anxiety, model.EQ5D_Health);

                        if (user != null)
                            if (item.Equals("continue"))
                            {
                                ViewBag.Message = "Continue";
                                return RedirectToAction("Edit", new { id = model.PatientID, page = model.Page, search = model.FilterSearchName, returnUrl = model.ReturnUrl });
                            }
                            else
                                return RedirectToAction(model.ReturnUrl, new { page = model.Page, search = model.FilterSearchName, message = "Patient (" + model.Name + " " + model.Surname + ") updated." });
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

        [Authorize]
        public ActionResult Delete(int id = 0, int page = 1, string search = "")
        {
            string message = "Error in deleting!";
            if (_patientServices.Delete(id))
            {
                message = "Patient deleted";
            }

            return RedirectToAction("index", new { page = page, search = search, message = message });
        }

        [Authorize]
        public ActionResult ActivePatients(int? page, string search, string message)
        {
            if ( _userServices.GetUserRole(CurrentUserID()) == 0)
            {
                return RedirectToAction("../home/Restricted");
            }
            else
            {
                long hopitalId = CurrentHospitalId();
                if (search == "undefined") { search = ""; }
                var results = _patientServices.GetActive(hopitalId, search);

                ViewBag.MenuPatientsActive = "active";
                ViewBag.HospitalName = _hospitalServices.GetHospitalNameById(hopitalId);
                ViewBag.FilterSearch = search;
                ViewBag.Message = message;
                int pageNumber = (page ?? 1);
                return View(results.OrderBy(x => x.Name)); //.ToPagedList(pageNumber, PageSize));
            } 
        }

        [Authorize]
        public ActionResult Followup30Day(int? page, string search, string message)
        {
            long hopitalId = CurrentHospitalId();
            if (search == "undefined") { search = ""; }
            var results = _patientServices.Get30(hopitalId, search);

            ViewBag.MenuPatients30 = "active";
            ViewBag.HospitalName = _hospitalServices.GetHospitalNameById(hopitalId);
            ViewBag.FilterSearch = search;
            ViewBag.Message = message;
            int pageNumber = (page ?? 1);
            return View(results.OrderBy(x => x.Name).ToPagedList(pageNumber, PageSize));
        }

        [Authorize]
        public ActionResult Followup120Day(int? page, string search, string message)
        {
            long hopitalId = CurrentHospitalId();
            if (search == "undefined") { search = ""; }
            var results = _patientServices.Get120(hopitalId, search);

            ViewBag.MenuPatients120 = "active";
            ViewBag.HospitalName = _hospitalServices.GetHospitalNameById(hopitalId);
            ViewBag.FilterSearch = search;
            ViewBag.Message = message;
            int pageNumber = (page ?? 1);
            return View(results.OrderBy(x => x.Name).ToPagedList(pageNumber, PageSize));
        }


        [Authorize]
        public ActionResult Calendar()
        {
            long hospitalId = CurrentHospitalId();

            ViewBag.MenuCalendar = "active";
            ViewBag.HospitalName = _hospitalServices.GetHospitalNameById(hospitalId);
            return View("Calendar");
        }

        [Authorize]
        [HttpGet]
        public JsonResult CalendarEvents()
        {
            long hospitalId = CurrentHospitalId();
            var events = _patientServices.GetCalendarData(hospitalId);
            return Json(events, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult YearlyValidation(int? year)
        {
            long hospitalId = CurrentHospitalId();

            ViewBag.MenuValidation = "active";
            ViewBag.HospitalName = _hospitalServices.GetHospitalNameById(hospitalId);

            List<SelectListItem> yearList = new List<SelectListItem>();
            yearList.Add(new SelectListItem { Text = "2016", Value = "2016" });
            yearList.Add(new SelectListItem { Text = "2017", Value = "2017" });
            yearList.Add(new SelectListItem { Text = "2018", Value = "2018" });
            yearList.Add(new SelectListItem { Text = "2019", Value = "2019", Selected = true });
            yearList.Add(new SelectListItem { Text = "2020", Value = "2020" });
            ViewBag.yearList = yearList;

            var results = _patientServices.YearlyValidation(hospitalId, year ?? 2019);

            return View(results);
        }

        [NonAction]
        public PatientModel PreparePatientModel(Patient patient)
        {
            var model = new PatientModel();
            model.Name = patient.Name;
            model.Surname = patient.Surname;
            model.MRN = patient.MRN;
            model.Phone = patient.Phone;
            model.DOB = string.Format("{0:dd-MMM-yyyy}", patient.DOB);
            model.Sex = patient.Sex;
            model.Indig = patient.Indig;
            model.Ethnic = patient.Ethnic;
            model.PostCode = patient.PostCode;
            model.Medicare = patient.Medicare;
            model.PatientType = patient.PatientType;
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
            model.ExpectedFollowup30 = patient.ExpectedFollowup30;
            model.FollowupDate30 = string.Format("{0:dd-MMM-yyyy}", patient.FollowupDate30);
            model.HealthServiceDischarge30 = string.Format("{0:dd-MMM-yyyy}", patient.HealthServiceDischarge30);
            model.Survival30 = patient.Survival30;
            model.Residence30 = patient.Residence30;
            model.WeightBear30 = patient.WeightBear30;
            model.WalkingAbility30 = patient.WalkingAbility30;
            model.BoneMed30 = patient.BoneMed30;
            model.Reoperation30 = patient.Reoperation30;
            model.ExpectedFollowup120 = patient.ExpectedFollowup120;
            model.FollowupDate120 = string.Format("{0:dd-MMM-yyyy}", patient.FollowupDate120);
            model.HealthServiceDischarge120 = string.Format("{0:dd-MMM-yyyy}", patient.HealthServiceDischarge120);
            model.Survival120 = patient.Survival120;
            model.Residence120 = patient.Residence120;
            model.WeightBear120 = patient.WeightBear120;
            model.WalkingAbility120 = patient.WalkingAbility120;
            model.BoneMed120 = patient.BoneMed120;
            model.Reoperation120 = patient.Reoperation120;
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
            model.FirstDayWalking = patient.FirstDayWalking;
            model.EQ5D_Mobility = patient.EQ5D_Mobility;
            model.EQ5D_SelfCare = patient.EQ5D_SelfCare;
            model.EQ5D_UsualActivity = patient.EQ5D_UsualActivity;
            model.EQ5D_Pain = patient.EQ5D_Pain;
            model.EQ5D_Anxiety = patient.EQ5D_Anxiety;
            model.EQ5D_Health = patient.EQ5D_Health?? 0;

            return model;
        }

        [NonAction]
        private PatientModel PrepearViewPatientModel(Patient patient)
        {
            ANZHFREntities Entity = new ANZHFREntities();
            var model = new PatientModel();
            short a = 0;
            long b = 0;

            model.Name = patient.Name;
            model.Surname = patient.Surname;
            model.MRN = patient.MRN;
            model.Phone = patient.Phone;
            model.DOB = string.Format("{0:dd-MMM-yyyy}", patient.DOB);

            model.Sex = patient.Sex;
            if (short.TryParse(patient.Sex, out a))
            {
                var s = Entity.Sexes.FirstOrDefault(x => x.SexID == a);
                if (s != null)
                {
                    model.Sex = s.Name;
                }
            }

            model.Indig = patient.Indig;
            if (short.TryParse(patient.Indig, out a))
            {
                var s = Entity.Indigs.FirstOrDefault(x => x.IndigID == a);
                if (s != null)
                {
                    model.Indig = s.Name;
                }
            }

            model.Ethnic = patient.Ethnic;
            if (short.TryParse(patient.Ethnic, out a))
            {
                var s = Entity.Ethnics.FirstOrDefault(x => x.EthnicID == a);
                if (s != null)
                {
                    model.Ethnic = s.Name;
                }
            }

            model.PostCode = patient.PostCode;
            model.Medicare = patient.Medicare;

            model.PatientType = patient.PatientType;
            if (short.TryParse(patient.PatientType, out a))
            {
                var s = Entity.PatientTypes.FirstOrDefault(x => x.PatientTypeID == a);
                if (s != null)
                {
                    model.PatientType = s.Name;
                }
            }
            model.AdmissionViaED = patient.AdmissionViaED;
            if (short.TryParse(patient.AdmissionViaED, out a))
            {
                var s = Entity.AdmissionViaEDs.FirstOrDefault(x => x.AdmissionViaEDID == a);
                if (s != null)
                {
                    model.AdmissionViaED = s.Name;
                }
            }

            model.UResidence = patient.UResidence;
            if (short.TryParse(patient.UResidence, out a))
            {
                var s = Entity.Residences.FirstOrDefault(x => x.ResidenceID == a);
                if (s != null)
                {
                    model.UResidence = s.Address;
                }
            }

            model.TransferHospital = patient.TransferHospital;
            if (long.TryParse(patient.TransferHospital, out b))
            {
                var s = Entity.Hospitals.FirstOrDefault(x => x.HospitalID == a);
                if (s != null)
                {
                    model.TransferHospital = s.HName;
                }
            }

            model.TransferDateTime = string.Format("{0:dd-MMM-yyyy hh:mm tt}", patient.TransferDateTime);
            model.ArrivalDateTime = string.Format("{0:dd-MMM-yyyy hh:mm tt}", patient.ArrivalDateTime);
            model.DepartureDateTime = string.Format("{0:dd-MMM-yyyy hh:mm tt}", patient.DepartureDateTime);
            model.InHospFractureDateTime = string.Format("{0:dd-MMM-yyyy hh:mm tt}", patient.InHospFractureDateTime);

            model.WardType = patient.WardType;
            if (short.TryParse(patient.WardType, out a))
            {
                var s = Entity.WardTypes.FirstOrDefault(x => x.WardTypeID == a);
                if (s != null)
                {
                    model.WardType = s.Name;
                }
            }

            model.PreAdWalk = patient.PreAdWalk;
            if (short.TryParse(patient.PreAdWalk, out a))
            {
                var s = Entity.PreAdWalks.FirstOrDefault(x => x.PreAdWalkID == a);
                if (s != null)
                {
                    model.PreAdWalk = s.Name;
                }
            }

            model.AMTS = patient.AMTS;
            if (short.TryParse(patient.AMTS, out a))
            {
                var s = Entity.AMTS.FirstOrDefault(x => x.AMTSID == a);
                if (s != null)
                {
                    model.AMTS = s.Name;
                }
            }

            model.CognitiveAssessment = patient.CognitiveAssessment;
            if (short.TryParse(patient.CognitiveAssessment, out a))
            {
                var s = Entity.CognitiveAssessments.FirstOrDefault(x => x.CognitiveAssessmentID == a);
                if (s != null)
                {
                    model.CognitiveAssessment = s.Name;
                }
            }

            model.CognitiveState = patient.CognitiveState;
            if (short.TryParse(patient.CognitiveState, out a))
            {
                var s = Entity.CognitiveStates.FirstOrDefault(x => x.CognitiveStateID == a);
                if (s != null)
                {
                    model.CognitiveState = s.Name;
                }
            }

            model.PainAssessment = patient.PainAssessment;
            if (short.TryParse(patient.PainAssessment, out a))
            {
                var s = Entity.PainAssessments.FirstOrDefault(x => x.PainAssessmentID == a);
                if (s != null)
                {
                    model.PainAssessment = s.Name;
                }
            }

            model.PainManagement = patient.PainManagement;
            if (short.TryParse(patient.PainManagement, out a))
            {
                var s = Entity.PainManagements.FirstOrDefault(x => x.PainManagementID == a);
                if (s != null)
                {
                    model.PainManagement = s.Name;
                }
            }

            model.ASAGrade = patient.ASAGrade;
            if (short.TryParse(patient.ASAGrade, out a))
            {
                var s = Entity.ASAGrades.FirstOrDefault(x => x.ASAGradeID == a);
                if (s != null)
                {
                    model.ASAGrade = s.Name;
                }
            }

            model.BoneMed = patient.BoneMed;
            if (short.TryParse(patient.BoneMed, out a))
            {
                var s = Entity.BoneMeds.FirstOrDefault(x => x.BoneMedID == a);
                if (s != null)
                {
                    model.BoneMed = s.Name;
                }
            }

            model.PreOpMedAss = patient.PreOpMedAss;
            if (short.TryParse(patient.PreOpMedAss, out a))
            {
                var s = Entity.PreOpMedAsses.FirstOrDefault(x => x.PreOpMedAssID == a);
                if (s != null)
                {
                    model.PreOpMedAss = s.Name;
                }
            }

            model.FractureSide = patient.FractureSide;
            if (short.TryParse(patient.FractureSide, out a))
            {
                var s = Entity.FractureSides.FirstOrDefault(x => x.FractureSideID == a);
                if (s != null)
                {
                    model.FractureSide = s.Name;
                }
            }

            model.AtypicalFracture = patient.AtypicalFracture;
            if (short.TryParse(patient.AtypicalFracture, out a))
            {
                var s = Entity.AtypicalFractures.FirstOrDefault(x => x.AtypicalFractureID == a);
                if (s != null)
                {
                    model.AtypicalFracture = s.Name;
                }
            }

            model.FractureType = patient.FractureType;
            if (short.TryParse(patient.FractureType, out a))
            {
                var s = Entity.FractureTypes.FirstOrDefault(x => x.FractureTypeID == a);
                if (s != null)
                {
                    model.FractureType = s.Name;
                }
            }

            model.Surgery = patient.Surgery;
            if (short.TryParse(patient.Surgery, out a))
            {
                var s = Entity.Surgeries.FirstOrDefault(x => x.SurgeryID == a);
                if (s != null)
                {
                    model.Surgery = s.Name;
                }
            }

            model.SurgeryDateTime = string.Format("{0:dd-MMM-yyyy hh:mm tt}", patient.SurgeryDateTime);

            model.SurgeryDelay = patient.SurgeryDelay;
            if (short.TryParse(patient.SurgeryDelay, out a))
            {
                var s = Entity.SurgeryDelays.FirstOrDefault(x => x.SurgeryDelayID == a);
                if (s != null)
                {
                    model.SurgeryDelay = s.Name;
                }
            }

            model.SurgeryDelayOther = patient.SurgeryDelayOther;

            model.Anaesthesia = patient.Anaesthesia;
            if (short.TryParse(patient.Anaesthesia, out a))
            {
                var s = Entity.Anaesthesias.FirstOrDefault(x => x.AnaesthesiaID == a);
                if (s != null)
                {
                    model.Anaesthesia = s.Name;
                }
            }

            model.Analgesia = patient.Analgesia;
            if (short.TryParse(patient.Analgesia, out a))
            {
                var s = Entity.Analgesias.FirstOrDefault(x => x.AnalgesiaID == a);
                if (s != null)
                {
                    model.Analgesia = s.Name;
                }
            }

            model.ConsultantPresent = patient.ConsultantPresent;
            if (short.TryParse(patient.ConsultantPresent, out a))
            {
                var s = Entity.ConsultantPresents.FirstOrDefault(x => x.ConsultantPresentID == a);
                if (s != null)
                {
                    model.ConsultantPresent = s.Name;
                }
            }

            model.Operation = patient.Operation;
            if (short.TryParse(patient.Operation, out a))
            {
                var s = Entity.Operations.FirstOrDefault(x => x.OperationID == a);
                if (s != null)
                {
                    model.Operation = s.Name;
                }
            }

            model.InterOpFracture = patient.InterOpFracture;
            if (short.TryParse(patient.InterOpFracture, out a))
            {
                var s = Entity.InterOpFractures.FirstOrDefault(x => x.InterOpFractureID == a);
                if (s != null)
                {
                    model.InterOpFracture = s.Name;
                }
            }

            model.FullWeightBear = patient.FullWeightBear;
            if (short.TryParse(patient.FullWeightBear, out a))
            {
                var s = Entity.WeightBears.FirstOrDefault(x => x.WeightBearID == a);
                if (s != null)
                {
                    model.FullWeightBear = s.Name;
                }
            }

            model.PressureUlcers = patient.PressureUlcers;
            if (short.TryParse(patient.PressureUlcers, out a))
            {
                var s = Entity.PressureUlcers.FirstOrDefault(x => x.PressureUlcersID == a);
                if (s != null)
                {
                    model.PressureUlcers = s.Name;
                }
            }

            model.Malnutrition = patient.Malnutrition;
            if (short.TryParse(patient.Malnutrition, out a))
            {
                var s = Entity.Malnutritions.FirstOrDefault(x => x.MalnutritionID == a);
                if (s != null)
                {
                    model.Malnutrition = s.Name;
                }
            }

            model.Mobilisation = patient.Mobilisation;
            if (short.TryParse(patient.Mobilisation, out a))
            {
                var s = Entity.Mobilisations.FirstOrDefault(x => x.MobilisationID == a);
                if (s != null)
                {
                    model.Mobilisation = s.Name;
                }
            }

            model.GeriatricAssessment = patient.GeriatricAssessment;
            if (short.TryParse(patient.GeriatricAssessment, out a))
            {
                var s = Entity.GeriatricAssessments.FirstOrDefault(x => x.GeriatricAssessmentID == a);
                if (s != null)
                {
                    model.GeriatricAssessment = s.Name;
                }
            }

            model.GeriatricAssDateTime = string.Format("{0:dd-MMM-yyyy}", patient.GeriatricAssDateTime);

            model.FallsAssessment = patient.FallsAssessment;
            if (short.TryParse(patient.FallsAssessment, out a))
            {
                var s = Entity.FallsAssessments.FirstOrDefault(x => x.FallsAssessmentID == a);
                if (s != null)
                {
                    model.FallsAssessment = s.Name;
                }
            }

            model.BoneMedDischarge = patient.BoneMedDischarge;
            if (short.TryParse(patient.BoneMedDischarge, out a))
            {
                var s = Entity.BoneMeds.FirstOrDefault(x => x.BoneMedID == a);
                if (s != null)
                {
                    model.BoneMedDischarge = s.Name;
                }
            }

            model.WardDischargeDate = string.Format("{0:dd-MMM-yyyy}", patient.WardDischargeDate);

            model.DischargeDest = patient.DischargeDest;
            if (short.TryParse(patient.DischargeDest, out a))
            {
                var s = Entity.DischargeDests.FirstOrDefault(x => x.DischargeDestID == a);
                if (s != null)
                {
                    model.DischargeDest = s.Name;
                }
            }

            model.HospitalDischargeDate = string.Format("{0:dd-MMM-yyyy}", patient.HospitalDischargeDate);

            model.OLengthofStay = ExportHelper.getOHLength(patient.WardDischargeDate, patient.ArrivalDateTime);
            model.HLengthofStay = ExportHelper.getOHLength(patient.HospitalDischargeDate, patient.ArrivalDateTime);

            model.DischargeResidence = patient.DischargeResidence;
            if (short.TryParse(patient.DischargeResidence, out a))
            {
                var s = Entity.DischargeResidences.FirstOrDefault(x => x.DischargeResidenceID == a);
                if (s != null)
                {
                    model.DischargeResidence = s.Address;
                }
            }

            model.FollowupDate30 = string.Format("{0:dd-MMM-yyyy}", patient.FollowupDate30);
            model.HealthServiceDischarge30 = string.Format("{0:dd-MMM-yyyy}", patient.HealthServiceDischarge30);

            model.Survival30 = patient.Survival30;
            if (short.TryParse(patient.Survival30, out a))
            {
                var s = Entity.Survivals.FirstOrDefault(x => x.SurvivalID == a);
                if (s != null)
                {
                    model.Survival30 = s.Name;
                }
            }

            model.Residence30 = patient.Residence30;
            if (short.TryParse(patient.Residence30, out a))
            {
                var s = Entity.DischargeDests.FirstOrDefault(x => x.DischargeDestID == a);
                if (s != null)
                {
                    model.Residence30 = s.Name;
                }
            }

            model.WeightBear30 = patient.WeightBear30;
            if (short.TryParse(patient.WeightBear30, out a))
            {
                var s = Entity.WeightBears.FirstOrDefault(x => x.WeightBearID == a);
                if (s != null)
                {
                    model.WeightBear30 = s.Name;
                }
            }

            model.WalkingAbility30 = patient.WalkingAbility30;
            if (short.TryParse(patient.WalkingAbility30, out a))
            {
                var s = Entity.WalkingAbilities.FirstOrDefault(x => x.WalkingAbilityID == a);
                if (s != null)
                {
                    model.WalkingAbility30 = s.Name;
                }
            }

            model.BoneMed30 = patient.BoneMed30;
            if (short.TryParse(patient.BoneMed30, out a))
            {
                var s = Entity.BoneMeds.FirstOrDefault(x => x.BoneMedID == a);
                if (s != null)
                {
                    model.BoneMed30 = s.Name;
                }
            }

            model.Reoperation30 = patient.Reoperation30;
            if (short.TryParse(patient.Reoperation30, out a))
            {
                var s = Entity.Reoperations.FirstOrDefault(x => x.ReoperationID == a);
                if (s != null)
                {
                    model.Reoperation30 = s.Name;
                }
            }

            model.FollowupDate120 = string.Format("{0:dd-MMM-yyyy}", patient.FollowupDate120);
            model.HealthServiceDischarge120 = string.Format("{0:dd-MMM-yyyy}", patient.HealthServiceDischarge120);

            model.Survival120 = patient.Survival120;
            if (short.TryParse(patient.Survival120, out a))
            {
                var s = Entity.Survivals.FirstOrDefault(x => x.SurvivalID == a);
                if (s != null)
                {
                    model.Survival120 = s.Name;
                }
            }

            model.Residence120 = patient.Residence120;
            if (short.TryParse(patient.Residence120, out a))
            {
                var s = Entity.DischargeDests.FirstOrDefault(x => x.DischargeDestID == a);
                if (s != null)
                {
                    model.Residence120 = s.Name;
                }
            }

            model.WeightBear120 = patient.WeightBear120;
            if (short.TryParse(patient.WeightBear120, out a))
            {
                var s = Entity.WeightBears.FirstOrDefault(x => x.WeightBearID == a);
                if (s != null)
                {
                    model.WeightBear120 = s.Name;
                }
            }

            model.WalkingAbility120 = patient.WalkingAbility120;
            if (short.TryParse(patient.WalkingAbility120, out a))
            {
                var s = Entity.WalkingAbilities.FirstOrDefault(x => x.WalkingAbilityID == a);
                if (s != null)
                {
                    model.WalkingAbility120 = s.Name;
                }
            }

            model.BoneMed120 = patient.BoneMed120;
            if (short.TryParse(patient.BoneMed120, out a))
            {
                var s = Entity.BoneMeds.FirstOrDefault(x => x.BoneMedID == a);
                if (s != null)
                {
                    model.BoneMed120 = s.Name;
                }
            }

            model.Reoperation120 = patient.Reoperation120;
            if (short.TryParse(patient.Reoperation120, out a))
            {
                var s = Entity.Reoperations.FirstOrDefault(x => x.ReoperationID == a);
                if (s != null)
                {
                    model.Reoperation120 = s.Name;
                }
            }
            model.Completeness = patient.Completeness.GetValueOrDefault();
            model.CompleteExemption = patient.CompleteExemption;
            model.Created = string.Format("{0:dd-MMM-yyyy}", patient.Created);
            model.Author = patient.Author;
            model.LastModified = string.Format("{0:dd-MMM-yyyy}", patient.LastModified);
            model.DeleriumAssessment = patient.DeleriumAssessment;
            if (short.TryParse(patient.DeleriumAssessment, out a))
            {
                var s = Entity.DeleriumAssessments.FirstOrDefault(x => x.DeleriumAssessmentID == a);
                if (s != null)
                {
                    model.DeleriumAssessment = s.Name;
                }
            }
            model.StartDate = string.Format("{0:dd-MMM-yyyy hh:mm tt}", patient.StartDate);
            model.Informed = patient.Informed.Value;
            model.OptedOut = patient.OptedOut.Value;
            model.CannotFollowup = patient.CannotFollowup.Value;
            model.DeathDate = string.Format("{0:dd-MMM-yyyy}", patient.DeathDate);

            return model;
        }

        private PatientModel LoadLookUpList(PatientModel model)
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
            model.FirstDayWalkingList = _patientServices.GetFirstDayWalking();
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
            model.EQ5DMobilityList = _patientServices.GetEQ5DMobility();
            model.EQ5DSelfCareList = _patientServices.GetEQ5DSelfCare();
            model.EQ5DUsualActivityList = _patientServices.GetEQ5DUsualActivity();
            model.EQ5DPainList = _patientServices.GetEQ5DPain();
            model.EQ5DAnxietyList = _patientServices.GetEQ5DAnxiety();

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

        public string CalculateStartDate(PatientModel model)
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

        public bool? HasQualityDocument(PatientModel model)
        {
          return true;  
          //return QualityPatient.Find(model.PatientID);
        }
        public decimal? CalculateCompleteness(PatientModel model)
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
                // Include 30 Followup fields if started
                if (!string.IsNullOrEmpty(model.FollowupDate30) && !string.IsNullOrEmpty(model.Survival30) && !model.Survival30.Equals("1"))
                {
                    fieldCount = fieldCount + 6;
                    if (string.IsNullOrEmpty(model.HealthServiceDischarge30)) ++missingfields;
                    if (string.IsNullOrEmpty(model.Residence30)) ++missingfields;
                    if (string.IsNullOrEmpty(model.WeightBear30)) ++missingfields;
                    if (string.IsNullOrEmpty(model.WalkingAbility30)) ++missingfields;
                    if (string.IsNullOrEmpty(model.BoneMed30)) ++missingfields;
                    if (string.IsNullOrEmpty(model.Reoperation30)) ++missingfields;
                }

                // Include 120 Followup fields if started
                if (!string.IsNullOrEmpty(model.FollowupDate120) && !string.IsNullOrEmpty(model.Survival120) && !model.Survival120.Equals("1"))
                {
                    fieldCount = fieldCount + 6;
                    if (string.IsNullOrEmpty(model.HealthServiceDischarge120)) ++missingfields;
                    if (string.IsNullOrEmpty(model.Residence120)) ++missingfields;
                    if (string.IsNullOrEmpty(model.WeightBear120)) ++missingfields;
                    if (string.IsNullOrEmpty(model.WalkingAbility120)) ++missingfields;
                    if (string.IsNullOrEmpty(model.BoneMed120)) ++missingfields;
                    if (string.IsNullOrEmpty(model.Reoperation120)) ++missingfields;
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