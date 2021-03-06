﻿using ANZHFR.Data.Models;
using ANZHFR.Services.Auth;
using ANZHFR.Services.Patients;
using ANZHFR.Services.Survey;
using ANZHFR.Web.ExtensionMethods;
using ANZHFR.Web.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ANZHFR.Web.Controllers
{
    [Authorize]
    public class SurveyController : BaseController
    {
        private readonly UserServices _userServices;
        private readonly HospitalServices _hospitalServices;
        private readonly LookupServices _lookupServices;
        private readonly SurveyServices _surveyServices;

        public SurveyController()
        {
            _userServices = new UserServices();
            _hospitalServices = new HospitalServices();
            _lookupServices = new LookupServices();
            _surveyServices = new SurveyServices();
        }

        [Authorize]
        public ActionResult Index(int? page, string year, string hospitalID, string message)
        {
            UserInfoModel user = HttpContext.User.GetUserInfo();

            if (user.AccessLevel < (int)ExtensionMethods.AccessLevel.FullAccess)
            {
                return RedirectToAction("Edit");
            }

            var results = _surveyServices.GetAll(year, hospitalID);

            ViewBag.MenuSurvey = "active";
            ViewBag.FilterSearchYear = year;
            ViewBag.FilterSearchHospitalID = hospitalID;
            ViewBag.Message = message;

            List<SelectListItem> hospitalList = new List<SelectListItem>();
            hospitalList.AddRange(from h in _hospitalServices.GetAll()
                                  select new SelectListItem
                                  {
                                      Text = h.HName,
                                      Value = h.HospitalID.ToString()
                                  });

            ViewBag.HospitalList = hospitalList;

            var yearList = (from s in _surveyServices.GetYears()
                            select new SelectListItem
                            {
                                Text = s.Value.ToString()
                            }).ToList();

            ViewBag.YearList = yearList;

            int pageNumber = (page ?? 1);
            return View(results.ToPagedList(pageNumber, PageSize));
        }

        [AllowAnonymous]
        public ActionResult Entry(string hospitalName)
        {
            int year = Survey.GetYear();
            string yearText = Survey.GetYearText(year);
            var hospital = _hospitalServices.GetHospitalByName(hospitalName);

            if (hospital == null)
            {
                ViewBag.Message = string.Format("The hospital name {0} does not exist.", hospitalName);
                ViewBag.Title = string.Format(ConfigurationManager.AppSettings["SurveyTitle"], "", yearText);
                return View("Success");
            }

            ViewBag.Title = string.Format(ConfigurationManager.AppSettings["SurveyTitle"], hospital.HName, yearText);

            var survey = _surveyServices.GetByHospitalID(hospital.HospitalID, year);

            if (survey == null)
            {
                survey = new Survey();
            }

            if (survey.SubmittedFlag == true)
            {
                ViewBag.Message = string.Format("The survey for the {0} period has already been submitted.", yearText);
                return View("Success");
            }

            PopulateLists();

            return View("Edit", GetModel(survey, hospital.HospitalID, hospital.HName, yearText));
        }

        [Authorize]
        public ActionResult Edit(int? id)
        {
            UserInfoModel user = HttpContext.User.GetUserInfo();

            int year = Survey.GetYear();
            long hospitalID = CurrentHospitalId();
            string hospitalName = "";
            string yearText = Survey.GetYearText();
            Survey survey = null;

            if (id > 0)
            {
                survey = _surveyServices.Get(id.Value);

                if (survey != null && survey.SurveyID == id && user.AccessLevel == (int)ExtensionMethods.AccessLevel.FullAccess)
                {
                    hospitalName = survey.Hospital.HName;
                    yearText = string.Format("{0:yyyy}/{1:yy}", new DateTime(survey.Year.Value, 1, 1), new DateTime(survey.Year.Value + 1, 1, 1));
                }
                else
                {
                    ViewBag.Message = string.Format("The survey with ID {0} does not exist.", id);
                    return RedirectToAction("Index");
                }
            }
            else
            {
                survey = _surveyServices.GetByHospitalID(hospitalID, year);

                hospitalName = _hospitalServices.GetHospitalNameById(hospitalID);
            }

            ViewBag.MenuSurvey = "active";
            ViewBag.Title = string.Format(ConfigurationManager.AppSettings["SurveyTitle"], hospitalName, yearText);

            if (survey != null && ((user.AccessLevel != (int)ExtensionMethods.AccessLevel.FullAccess && survey.SubmittedFlag == true) || (user.AccessLevel != (int)ExtensionMethods.AccessLevel.FullAccess && survey.HospitalID > 0 && survey.HospitalID != hospitalID)))
            {
                ViewBag.Message = string.Format("The survey for the {0} period has already been submitted.", yearText);
                return View("Success");
            }
            else
            {
                PopulateLists();

                if (survey == null)
                {
                    survey = new Survey();
                }

                return View(GetModel(survey, hospitalID, hospitalName, yearText));
            }
        }

        private SurveyModel GetModel(Survey survey, long hospitalID, string hospitalName, string yearText)
        {
            SurveyModel model = new SurveyModel();
            model.SurveyID = survey.SurveyID;
            model.HospitalID = survey.HospitalID != 0 ? survey.HospitalID : hospitalID;
            model.HospitalName = hospitalName;
            model.Year = survey.Year.HasValue ? survey.Year.Value : Survey.GetYear();
            model.YearText = yearText;
            model.EstimatedRecordsPerMonth = survey.EstimatedRecordsPerMonth;
            model.RoleName = survey.RoleName;
            model.EstimatedRecords = survey.EstimatedRecords;
            model.DesignatedMajorTraumaCentre = survey.DesignatedMajorTraumaCentre;
            model.FormalOrthogeriatricService = survey.FormalOrthogeriatricService;
            model.ModelOfCare = survey.ModelOfCare;
            model.ModelCareOther = survey.ModelCareOther;
            model.ProtocolAccessCTMRI = survey.ProtocolAccessCTMRI;
            model.HipFracturePathway = survey.HipFracturePathway;
            model.VTEProtocol = survey.VTEProtocol;
            model.ProtocolPain = survey.ProtocolPain;
            model.PlannedList = survey.PlannedList;
            model.ChoiceOfAnaesthesia = survey.ChoiceOfAnaesthesia;
            model.LocalNerveBlocks = survey.LocalNerveBlocks;
            model.LocalNerveBlocksPostoperative = survey.LocalNerveBlocksPostoperative;
            model.WeekendTherapyServices = survey.WeekendTherapyServices;
            model.WrittenCareInformation = survey.WrittenCareInformation;
            model.InPatientRehabilitation = survey.InPatientRehabilitation;
            model.SupportedHomeBasedRehabilitation = survey.SupportedHomeBasedRehabilitation;
            model.IndividualisedWrittenInformation = survey.IndividualisedWrittenInformation;
            model.FallsClinicPublic = survey.FallsClinicPublic;
            model.OsteoporosisClinicPublic = survey.OsteoporosisClinicPublic;
            model.CombinedFallsBoneHealthClinicPublic = survey.CombinedFallsBoneHealthClinicPublic;
            model.OrthopaedicClinicPublic = survey.OrthopaedicClinicPublic;
            model.FractureLiaisonService = survey.FractureLiaisonService;
            model.CollectLocalHipFractureData = survey.CollectLocalHipFractureData;
            model.WhoCollectsData = survey.WhoCollectsData;
            model.ServiceChanges = survey.ServiceChanges;
            model.ServiceChangesPlanned = survey.ServiceChangesPlanned;
            model.IdentifiedBarriers = survey.IdentifiedBarriers;
            model.IdentifiedBarrierDetails = survey.IdentifiedBarrierDetails;

            return model;
        }

        [HttpPost]
        public ActionResult Edit(SurveyModel model)
        {
            return SaveSurvey(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Entry(SurveyModel model)
        {
            return SaveSurvey(model);
        }

        public ActionResult SaveSurvey(SurveyModel model)
        {
            string yearText = Survey.GetYearText(model.Year);

            ViewBag.Title = string.Format(ConfigurationManager.AppSettings["SurveyTitle"], model.HospitalName, yearText);

            if (ModelState.IsValid)
            {
                _surveyServices.Save(model.SurveyID, model.HospitalID, model.Year, model.RoleName, model.EstimatedRecords, model.DesignatedMajorTraumaCentre,
                    model.FormalOrthogeriatricService, model.ModelOfCare, model.ModelCareOther, model.ProtocolAccessCTMRI, model.HipFracturePathway,
                    model.VTEProtocol, model.ProtocolPain, model.PlannedList, model.ChoiceOfAnaesthesia, model.LocalNerveBlocks, model.LocalNerveBlocksPostoperative,
                    model.WeekendTherapyServices, model.WrittenCareInformation, model.InPatientRehabilitation, model.SupportedHomeBasedRehabilitation,
                    model.IndividualisedWrittenInformation, model.FallsClinicPublic, model.OsteoporosisClinicPublic, model.CombinedFallsBoneHealthClinicPublic,
                    model.OrthopaedicClinicPublic, model.FractureLiaisonService, model.CollectLocalHipFractureData, model.WhoCollectsData, model.ServiceChanges,
                    model.ServiceChangesPlanned, model.IdentifiedBarriers, model.IdentifiedBarrierDetails);

                ViewBag.Message = string.Format("Thank-you. The survery for the {0} period has been successfully submitted.", yearText);
                return View("Success");
            }

            // Just for testing in order to know when it is the serverside validation that has failed
            ModelState.AddModelError("", "Server-side validation failed.");

            PopulateLists();

            // If we got this far something failed, redisplay form
            return View("Edit", model);
        }

        private void PopulateLists()
        {
            List<SelectListItem> roleName = new List<SelectListItem>();
            roleName.Add(new SelectListItem { Text = "Orthopaedic Surgeon" });
            roleName.Add(new SelectListItem { Text = "Geriatrician" });
            roleName.Add(new SelectListItem { Text = "Nurse" });
            roleName.Add(new SelectListItem { Text = "Allied Health" });
            roleName.Add(new SelectListItem { Text = "Other" });

            List<SelectListItem> estimatedRecords = new List<SelectListItem>();
            estimatedRecords.Add(new SelectListItem { Text = "0-50", Value = "0-50" });
            estimatedRecords.Add(new SelectListItem { Text = "51-100", Value = "51-100" });
            estimatedRecords.Add(new SelectListItem { Text = "101-200", Value = "101-200" });
            estimatedRecords.Add(new SelectListItem { Text = "201-300", Value = "201-300" });
            estimatedRecords.Add(new SelectListItem { Text = "301-400", Value = "301-400" });
            estimatedRecords.Add(new SelectListItem { Text = "400+", Value = "400+" });

            List<SelectListItem> yesNo = new List<SelectListItem>();
            yesNo.Add(new SelectListItem { Text = "Yes" });
            yesNo.Add(new SelectListItem { Text = "No" });

            List<SelectListItem> modelOfCare = new List<SelectListItem>();
            modelOfCare.Add(new SelectListItem { Text = "A shared care arrangement where there is joint responsibility for the patient from admission between orthopaedics and geriatric medicine for all older hip fracture patients" });
            modelOfCare.Add(new SelectListItem { Text = "An orthogeriatric liaison service where geriatric medicine provides regular review of all older hip fracture patients (daily during working week)" });
            modelOfCare.Add(new SelectListItem { Text = "An orthogeriatric liaison service where geriatric medicine provides intermittent review of all older hip fracture patients (2-3 times weekly)" });
            modelOfCare.Add(new SelectListItem { Text = "A medical liaison service where a general physician or GP provides intermittent review of hip fracture patients (2-3 times weekly)" });
            modelOfCare.Add(new SelectListItem { Text = "A geriatric service where a consult system determines which patients are reviewed i.e. referral on a needs basis" });
            modelOfCare.Add(new SelectListItem { Text = "A medical service where a consult system determines which patients are reviewed i.e. referral on a needs basis" });
            modelOfCare.Add(new SelectListItem { Text = "No  formal service exists" });
            modelOfCare.Add(new SelectListItem { Text = "Other – describe" });

            List<SelectListItem> afrn = new List<SelectListItem>();
            afrn.Add(new SelectListItem { Text = "Always" });
            afrn.Add(new SelectListItem { Text = "Frequently" });
            afrn.Add(new SelectListItem { Text = "Rarely" });
            afrn.Add(new SelectListItem { Text = "Never" });

            List<SelectListItem> oob = new List<SelectListItem>();
            oob.Add(new SelectListItem { Text = "Onsite" });
            oob.Add(new SelectListItem { Text = "Offsite" });
            oob.Add(new SelectListItem { Text = "Both" });

            List<SelectListItem> yesNoHip = new List<SelectListItem>();
            yesNoHip.Add(new SelectListItem { Text = "Yes - hip fracture patients only" });
            yesNoHip.Add(new SelectListItem { Text = "No" });
            yesNoHip.Add(new SelectListItem { Text = "Yes - all fracture patients (including hip)" });

            List<SelectListItem> collectLocalData = new List<SelectListItem>();
            collectLocalData.Add(new SelectListItem { Text = "Yes - ANZ Hip Fracture Registry" });
            collectLocalData.Add(new SelectListItem { Text = "Yes - local system" });
            collectLocalData.Add(new SelectListItem { Text = "No" });

            List<SelectListItem> therapyServiceWeekends = new List<SelectListItem>();
            therapyServiceWeekends.Add(new SelectListItem { Text = "Yes - Physiotherapy only" });
            therapyServiceWeekends.Add(new SelectListItem { Text = "Yes - Other" });
            therapyServiceWeekends.Add(new SelectListItem { Text = "No" });

            ViewBag.RoleNameList = roleName;
            ViewBag.EstimatedRecordsList = estimatedRecords;
            ViewBag.YesNo = yesNo;
            ViewBag.ModelOfCareList = modelOfCare;
            ViewBag.AFRN = afrn;
            ViewBag.OOB = oob;
            ViewBag.YesNoHip = yesNoHip;
            ViewBag.TSW = therapyServiceWeekends;
            ViewBag.CollectLocalDataList = collectLocalData;
        }
    }
}