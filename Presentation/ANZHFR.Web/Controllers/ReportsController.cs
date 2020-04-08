using ANZHFR.Data.Models;
using ANZHFR.Services.Patients;
using ANZHFR.Services.Reports;
using ANZHFR.Web.ExtensionMethods;
using ANZHFR.Web.Helpers;
using ANZHFR.Web.Models;
using ANZHFR.Web.Properties;
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
    public class ReportsController : BaseController
    {
        private readonly PatientServices _patientService = null;
        private readonly ReportServices _reportService = null;

        private readonly ANZHFREntities Entity = null;

        public ReportsController()
        {
            _patientService = new PatientServices();
            _reportService = new ReportServices();
            Entity = new ANZHFREntities();
        }

        [Authorize]
        public ActionResult Index()
        {
            ViewBag.MenuReports = "active";
            ViewBag.Month = DateTime.Now.Month;
            ViewBag.Year = DateTime.Now.Year;
            ViewBag.BestPracticeReportHours = Settings.Default.BestPracticeReportHours;

            List<SelectListItem> reportList = new List<SelectListItem>();
            reportList.AddRange(from r in _reportService.GetAll()
                                select new SelectListItem
                                {
                                    Text = r.Name,
                                    Value = r.Code
                                });

            ViewBag.ReportList = reportList;

            return View();
        }

        [Authorize]
        [HttpPost]
        public JsonResult GetReportData(string reportType, int month = 0, int year = 0)
        {
            if (string.IsNullOrEmpty(reportType) || (month == 0 && year == 0))
            {
                return null;
            }
            else
            {
                long hospitalID = CurrentHospitalId();
                Hospital hospital = Entity.Hospitals.SingleOrDefault(x => x.HospitalID == hospitalID);

                if (hospital != null)
                {
                    if (reportType == "age")
                    {
                        return Json(ReportHelper.AgeReport(reportType, hospital, month, year));
                    }
                    else if (reportType == "sex")
                    {
                        return Json(ReportHelper.SexReport(reportType, hospital, month, year));
                    }
                    else if (reportType == "fracture-type")
                    {
                        return Json(ReportHelper.FractureTypeReport(reportType, hospital, month, year));
                    }
                    else if (reportType == "length-of-stay")
                    {
                        return Json(ReportHelper.LengthOfStayReport(reportType, hospital, month, year, false));
                    }
                    else if (reportType == "acute-length-of-stay")
                    {
                        return Json(ReportHelper.AcuteLengthOfStayReport(reportType, hospital, month, year, true));
                    }
                    else if (reportType == "discharge-destination")
                    {
                        return Json(ReportHelper.DischargeDestinationReport(reportType, hospital, month, year));
                    }
                    else if (reportType == "time-to-surgery")
                    {
                        return Json(ReportHelper.TimeToSurgeryReport(reportType, hospital, month, year));
                    }
                    else if (reportType == "total-entered")
                    {
                        return Json(ReportHelper.TotalEnteredReport(reportType, hospital, month, year));
                    }
                    else if (reportType == "estimated-v-actual")
                    {
                        return Json(ReportHelper.EstimatedActualReport(reportType, hospital, month, year));
                    }
                    else if (reportType == "surgery-best-practice")
                    {
                        return Json(ReportHelper.SurgeryBestPracticeReport(reportType, hospital, month, year));
                    }
                    else if (reportType == "survival-30-days")
                    {
                        return Json(ReportHelper.Survival30Days(reportType, hospital, month, year));
                    }
                    else if (reportType == "cognitive-state")
                    {
                        return Json(ReportHelper.CognitiveStateReport(reportType, hospital, month, year));
                    }
                    else if (reportType == "medical-assessment")
                    {
                        return Json(ReportHelper.MedicalAssessmentReport(reportType, hospital, month, year));
                    }
                    else if (reportType == "mobilisation")
                    {
                        return Json(ReportHelper.MobilisationReport(reportType, hospital, month, year));
                    }
                    else if (reportType == "weight-bearing")
                    {
                        return Json(ReportHelper.WeightBearingReport(reportType, hospital, month, year));
                    }
                    else if (reportType == "pressure-ulcers")
                    {
                        return Json(ReportHelper.PressureUlcersReport(reportType, hospital, month, year));
                    }
                    else if (reportType == "bone-medication")
                    {
                        return Json(ReportHelper.BoneMedicationReport(reportType, hospital, month, year));
                    }
                    else if (reportType == "reoperation")
                    {
                        return Json(ReportHelper.ReoperationReport(reportType, hospital, month, year));
                    }
                    else if (reportType == "completeness")
                    {
                        return Json(ReportHelper.CompletenessReport(reportType, hospital, month, year));
                    }
                    else if (reportType == "analgesia")
                    {
                        return Json(ReportHelper.AnalgesiaReport(reportType, hospital, month, year));
                    }
                    else if (reportType == "pain-assessment")
                    {
                        return Json(ReportHelper.PainAssessmentReport(reportType, hospital, month, year));
                    }
                    else if (reportType == "pain-management")
                    {
                        return Json(ReportHelper.PainManagementReport(reportType, hospital, month, year));
                    }
                    else if (reportType == "reason-for-delay")
                    {
                        return Json(ReportHelper.DelayReasonReport(reportType, hospital, month, year));
                    }
                }

                return null;
            }
        }
    }
}