using ANZHFR.Services.Patients;
using ANZHFR.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace ANZHFR.Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly HospitalServices _hospitalServices;
        private readonly PatientServices _patientServices;

        public HomeController()
        {
            _hospitalServices = new HospitalServices();
            _patientServices = new PatientServices();
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ProcessContact(string RName, string RHospital, string RPhone, string REmail, string RComments)
        {
            MailMessage mm = new MailMessage();
            mm.From = new MailAddress(ConfigurationManager.AppSettings["EmailFromAddress"]);
            mm.To.Add(new MailAddress(ConfigurationManager.AppSettings["EmailToAddress"]));
            mm.Subject = string.Format("Account request from the {0} registry.", ConfigurationManager.AppSettings["Location"]);
            mm.Body = string.Format("{0} from {1} has applied for an account.<br>{2}<br>{3}<br>{4}", RName, RHospital, REmail, RPhone, RComments);
            mm.IsBodyHtml = true;

            new SmtpClient().Send(mm);

            return View();
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            return View();
        }

        [Authorize]
        public ActionResult Dashboard()
        {
            long hospitalId = CurrentHospitalId();

            ViewBag.MenuDashboard = "active";

            if (hospitalId > 0)
            {
                ViewBag.HospitalName = _hospitalServices.GetHospitalNameById(hospitalId);
            }

            var hospitals = _hospitalServices.GetAll();

            var filteredHospitals = (from h in hospitals
                                     where !Regex.IsMatch(h.HName, ConfigurationManager.AppSettings["ExcludeFromHospitalsRegex"], RegexOptions.IgnoreCase)
                                     select h).ToList();

            ViewBag.NewHospitals = filteredHospitals.OrderByDescending(h => h.HospitalID).Take(10).OrderBy(h => h.HName);
            return View();
        }

        [Authorize]
        public ActionResult Dashboard2()
        {
            long hospitalId = CurrentHospitalId();

            ViewBag.MenuDashboard = "active";

            ViewBag.ActivePatients = _patientServices.GetActive(hospitalId).Count();
            ViewBag.AllPatients = _patientServices.GetAllHospital(hospitalId).Count();
            ViewBag.Year = System.DateTime.Now.Year;
            ViewBag.CurrentYear = _patientServices.GetCurrentYear(hospitalId, System.DateTime.Now.Year).Count();
            if (ViewBag.AllPatients > 0)
            {
                DateTime? LastModified = _patientServices.GetAllHospital(hospitalId)
                .OrderByDescending(x => x.LastModified)
                .Select(x => x.LastModified)
                .First();
                DateTime lastmodified = (DateTime)LastModified;

                ViewBag.LastModified = lastmodified.Day + "<br />" + lastmodified.ToString("MMM") + " " + lastmodified.Year;
            }
            else
            {
                ViewBag.LastModified = "No records<br />found";
            }

            



            if (hospitalId > 0)
            {
                ViewBag.HospitalName = _hospitalServices.GetHospitalNameById(hospitalId);
            }

            var hospitals = _hospitalServices.GetAll();

            var filteredHospitals = (from h in hospitals
                                     where !Regex.IsMatch(h.HName, ConfigurationManager.AppSettings["ExcludeFromHospitalsRegex"], RegexOptions.IgnoreCase)
                                     select h).ToList();

            ViewBag.NewHospitals = filteredHospitals.OrderByDescending(h => h.HospitalID).Take(10).OrderBy(h => h.HName);
            var V = RouteData.Values.First().Value.ToString();
            return View();
        }

        [Authorize]
        public JsonResult Dashboard_Refresh()
        {
            long hospitalId = CurrentHospitalId();
            var patientTypes = Request["PatientTypes"];
            var period = Request["Period"];
            var from_date = Request["From"];
            var to_date = Request["To"];

            var patients = _patientServices.GetDashboard(hospitalId, patientTypes, period, from_date, to_date);
            ResultString = patients.Count.ToString() + ":";

            #region Time In ED Calculations
            //Time in ED Calculations
            string AvgED, MedianED, ShortestED, ShortestID, LongestED, LongestID;
            int midList;
            var EDPatients = patients.Where(item => item.AdmissionViaED == "1" && item.ArrivalDateTime != null && item.DepartureDateTime != null).ToList();
            if(EDPatients.Count > 0)
            {
                AvgED = EDPatients.Average(item => (item.DepartureDateTime - item.ArrivalDateTime).Value.TotalHours).ToString("0.00");
                var MedianEDList = EDPatients.OrderBy(item => (item.DepartureDateTime - item.ArrivalDateTime).Value.TotalHours).ToList();
                midList = (MedianEDList.Count() / 2);
                if (MedianEDList.Count() % 2 == 0)
                {
                    // Take average of two middle values
                    MedianED = (((MedianEDList.ElementAt(midList).DepartureDateTime - MedianEDList.ElementAt(midList).ArrivalDateTime).Value.TotalHours +
                       (MedianEDList.ElementAt(midList-1).DepartureDateTime - MedianEDList.ElementAt(midList-1).ArrivalDateTime).Value.TotalHours)/2).ToString("0.00");
                }
                else
                {
                    //Take middle value only
                    MedianED = (MedianEDList.ElementAt(midList).DepartureDateTime - MedianEDList.ElementAt(midList).ArrivalDateTime).Value.TotalHours.ToString("0.00");
                }
                ShortestED = EDPatients.Min(item => (item.DepartureDateTime - item.ArrivalDateTime).Value.TotalHours).ToString("0.00");
                ShortestID = EDPatients.OrderBy(item => (item.DepartureDateTime - item.ArrivalDateTime)).FirstOrDefault().ANZHFRID.ToString();
                LongestED = EDPatients.Max(item => (item.DepartureDateTime - item.ArrivalDateTime).Value.TotalHours).ToString("0.00");
                LongestID = EDPatients.OrderByDescending(item => (item.DepartureDateTime - item.ArrivalDateTime)).FirstOrDefault().ANZHFRID.ToString();
            }
            else
            {
                AvgED = "-";
                MedianED = "-";
                ShortestED = "-";
                ShortestID = "-";
                LongestED = "-";
                LongestID = "-";
            }
            ResultString += EDPatients.Count.ToString() + ":"; // 1
            ResultString += AvgED.ToString() + ":";            // 2
            ResultString += MedianED.ToString() + ":";         // 3
            ResultString += ShortestED.ToString() + ":";       // 4
            ResultString += ShortestID + ":";                  // 5
            ResultString += LongestED.ToString() + ":";        // 6
            ResultString += LongestID + ":";                   // 7

            #endregion

            #region Time To Surgery Calculations

            //Time to Surgery Calculations
            string AvgSurgery, MedianSurgery, ShortestSurgery, LongestSurgery;
            var SurgeryPatients = patients.Where(item => item.Surgery == "2" && item.SurgeryDateTime != null).ToList();
            if (SurgeryPatients.Count > 0)
            {
                AvgSurgery = SurgeryPatients.Average(item => (item.SurgeryDateTime - item.StartDate).Value.TotalHours).ToString("0.00");
                var MedianSurgeryList = SurgeryPatients.OrderBy(item => (item.SurgeryDateTime - item.StartDate).Value.TotalHours).ToList();
                midList = (MedianSurgeryList.Count() / 2);
                if (MedianSurgeryList.Count() % 2 == 0)
                {
                    // Take average of two middle values
                    MedianSurgery = (((MedianSurgeryList.ElementAt(midList).SurgeryDateTime - MedianSurgeryList.ElementAt(midList).StartDate).Value.TotalHours +
                       (MedianSurgeryList.ElementAt(midList - 1).SurgeryDateTime - MedianSurgeryList.ElementAt(midList - 1).StartDate).Value.TotalHours) / 2).ToString("0.00");
                }
                else
                {
                    //Take middle value only
                    MedianSurgery = (MedianSurgeryList.ElementAt(midList).SurgeryDateTime - MedianSurgeryList.ElementAt(midList).StartDate).Value.TotalHours.ToString("0.00");
                }
                ShortestSurgery = SurgeryPatients.Min(item => (item.SurgeryDateTime - item.StartDate).Value.TotalHours).ToString("0.00");
                ShortestID = SurgeryPatients.OrderBy(item => (item.SurgeryDateTime - item.StartDate)).FirstOrDefault().ANZHFRID.ToString();
                LongestSurgery = SurgeryPatients.Max(item => (item.SurgeryDateTime - item.StartDate).Value.TotalHours).ToString("0.00");
                LongestID = SurgeryPatients.OrderByDescending(item => (item.SurgeryDateTime - item.StartDate)).FirstOrDefault().ANZHFRID.ToString();
            }
            else
            {
                AvgSurgery = "-";
                MedianSurgery = "-";
                ShortestSurgery = "-";
                ShortestID = "-";
                LongestSurgery = "-";
                LongestID = "-";
            }
            //Add Surgery data to ResultString
            ResultString += SurgeryPatients.Count.ToString() + ":"; // 8
            ResultString += AvgSurgery.ToString() + ":";            // 9
            ResultString += MedianSurgery.ToString() + ":";         // 10
            ResultString += ShortestSurgery.ToString() + ":";       // 11
            ResultString += ShortestID + ":";                       // 12
            ResultString += LongestSurgery.ToString() + ":";        // 13
            ResultString += LongestID + ":";                        // 14

            #endregion

            #region Acute Length of Stay Calculations

            //Acute Length of Stay Calculations
            string AvgAcuteLOS, MedianAcuteLOS, ShortestAcuteLOS, LongestAcuteLOS;
            var AcuteLOSPatients = patients.Where(item => item.WardDischargeDate != null && item.DischargeDest != "6" ).ToList();
            if (AcuteLOSPatients.Count > 0)
            {
                AvgAcuteLOS = AcuteLOSPatients.Average(item => (item.WardDischargeDate - item.StartDate).Value.TotalDays).ToString("0.00");
                var MedianList = AcuteLOSPatients.OrderBy(item => (item.WardDischargeDate - item.StartDate).Value.TotalDays).ToList();
                midList = (MedianList.Count() / 2);
                if (MedianList.Count() % 2 == 0)
                {
                    // Take average of two middle values
                    MedianAcuteLOS = (((MedianList.ElementAt(midList).WardDischargeDate - MedianList.ElementAt(midList).StartDate).Value.TotalDays +
                       (MedianList.ElementAt(midList - 1).WardDischargeDate - MedianList.ElementAt(midList - 1).StartDate).Value.TotalDays) / 2).ToString("0.00");
                }
                else
                {
                    //Take middle value only
                    MedianAcuteLOS = (MedianList.ElementAt(midList).WardDischargeDate - MedianList.ElementAt(midList).StartDate).Value.TotalDays.ToString("0.00");
                }
                ShortestAcuteLOS = AcuteLOSPatients.Min(item => (item.WardDischargeDate - item.StartDate).Value.TotalDays).ToString("0.00");
                ShortestID = AcuteLOSPatients.OrderBy(item => (item.WardDischargeDate - item.StartDate)).FirstOrDefault().ANZHFRID.ToString();
                LongestAcuteLOS = AcuteLOSPatients.Max(item => (item.WardDischargeDate - item.StartDate).Value.TotalDays).ToString("0.00");
                LongestID = AcuteLOSPatients.OrderByDescending(item => (item.WardDischargeDate - item.StartDate)).FirstOrDefault().ANZHFRID.ToString();
            }
            else
            {
                AvgAcuteLOS = "-";
                MedianAcuteLOS = "-";
                ShortestAcuteLOS = "-";
                ShortestID = "-";
                LongestAcuteLOS = "-";
                LongestID = "-";
            }

            //Add AcuteLOS data to ResultString
            ResultString += AcuteLOSPatients.Count.ToString() + ":"; // 15
            ResultString += AvgAcuteLOS.ToString() + ":";            // 16
            ResultString += MedianAcuteLOS.ToString() + ":";         // 17
            ResultString += ShortestAcuteLOS.ToString() + ":";       // 18
            ResultString += ShortestID + ":";                        // 19
            ResultString += LongestAcuteLOS.ToString() + ":";        // 20
            ResultString += LongestID + ":";                         // 21

            #endregion

            #region Hospital Length of Stay Calculations

            //Acute Length of Stay Calculations
            string AvgHospitalLOS, MedianHospitalLOS, ShortestHospitalLOS, LongestHospitalLOS;
            var HospitalLOSPatients = patients.Where(item => item.HospitalDischargeDate != null && item.DischargeResidence != "5").ToList();
            if (HospitalLOSPatients.Count > 0)
            {
                AvgHospitalLOS = HospitalLOSPatients.Average(item => (item.HospitalDischargeDate - item.StartDate).Value.TotalDays).ToString("0.00");
                var MedianList = HospitalLOSPatients.OrderBy(item => (item.HospitalDischargeDate - item.StartDate).Value.TotalDays).ToList();
                midList = (MedianList.Count() / 2);
                if (MedianList.Count() % 2 == 0)
                {
                    // Take average of two middle values
                    MedianHospitalLOS = (((MedianList.ElementAt(midList).HospitalDischargeDate - MedianList.ElementAt(midList).StartDate).Value.TotalDays +
                       (MedianList.ElementAt(midList - 1).HospitalDischargeDate - MedianList.ElementAt(midList - 1).StartDate).Value.TotalDays) / 2).ToString("0.00");
                }
                else
                {
                    //Take middle value only
                    MedianHospitalLOS = (MedianList.ElementAt(midList).HospitalDischargeDate - MedianList.ElementAt(midList).StartDate).Value.TotalDays.ToString("0.00");
                }
                ShortestHospitalLOS = HospitalLOSPatients.Min(item => (item.HospitalDischargeDate - item.StartDate).Value.TotalDays).ToString("0.00");
                ShortestID = HospitalLOSPatients.OrderBy(item => (item.HospitalDischargeDate - item.StartDate)).FirstOrDefault().ANZHFRID.ToString();
                LongestHospitalLOS = HospitalLOSPatients.Max(item => (item.HospitalDischargeDate - item.StartDate).Value.TotalDays).ToString("0.00");
                LongestID = HospitalLOSPatients.OrderByDescending(item => (item.HospitalDischargeDate - item.StartDate)).FirstOrDefault().ANZHFRID.ToString();
            }
            else
            {
                AvgHospitalLOS = "-";
                MedianHospitalLOS = "-";
                ShortestHospitalLOS = "-";
                ShortestID = "-";
                LongestHospitalLOS = "-";
                LongestID = "-";
            }

            //Add AcuteLOS data to ResultString
            ResultString += HospitalLOSPatients.Count.ToString() + ":"; // 22
            ResultString += AvgHospitalLOS.ToString() + ":";            // 23
            ResultString += MedianHospitalLOS.ToString() + ":";         // 24
            ResultString += ShortestHospitalLOS.ToString() + ":";       // 25
            ResultString += ShortestID + ":";                           // 26
            ResultString += LongestHospitalLOS.ToString() + ":";        // 27
            ResultString += LongestID + ":";                            // 28

            #endregion

            #region Care At Presentation Calculation

            string CareAtPresentation;
            var CareAtPresentationPatients = patients.Where(item => item.CognitiveAssessment != null).ToList();
            if (CareAtPresentationPatients.Count() > 0 )
            {
                CareAtPresentation = (CareAtPresentationPatients.Where(item => item.CognitiveAssessment == "1" || item.CognitiveAssessment == "3").Count() * 100 / CareAtPresentationPatients.Count()).ToString("0") + "%";
            }
            else
            {
                CareAtPresentation = "-";
            }

            //Add CareAtPresentation data to ResultString
            ResultString += CareAtPresentationPatients.Count.ToString() + ":"; // 29
            ResultString += CareAtPresentation.ToString() + ":";               // 30

            #endregion

            #region Pain Assessment Calculation

            string PainAssessment;
            var PainAssessmentPatients = patients.Where(item => item.PainAssessment != null).ToList();
            if (PainAssessmentPatients.Count() > 0)
            {
                PainAssessment = (PainAssessmentPatients.Where(item => item.PainAssessment == "1").Count() * 100 / PainAssessmentPatients.Count()).ToString("0") + "%";
            }
            else
            {
                PainAssessment = "-";
            }

            //Add CareAtPresentation data to ResultString
            ResultString += PainAssessmentPatients.Count.ToString() + ":";
            ResultString += PainAssessment.ToString() + ":";

            #endregion

            #region Nerve Block (Analgesia) Calculation

            string Analgesia;
            var AnalgesiaPatients = patients.Where(item => item.Analgesia != null).ToList();
            if (AnalgesiaPatients.Count() > 0)
            {
                Analgesia = (AnalgesiaPatients.Where(item => item.Analgesia == "1" || item.Analgesia =="2" || item.Analgesia == "3").Count() * 100 / AnalgesiaPatients.Count()).ToString("0") + "%";
            }
            else
            {
                Analgesia = "-";
            }

            //Add CareAtPresentation data to ResultString
            ResultString += AnalgesiaPatients.Count.ToString() + ":";
            ResultString += Analgesia.ToString() + ":";

            #endregion

            #region Geriatric Assessment Calculation

            string GeriatricAssessment;
            var GeriatricAssessmentPatients = patients.Where(item => item.GeriatricAssessment != null).ToList();
            if (GeriatricAssessmentPatients.Count() > 0)
            {
                GeriatricAssessment = (GeriatricAssessmentPatients.Where(item => item.GeriatricAssessment == "1").Count() * 100 / GeriatricAssessmentPatients.Count()).ToString("0") + "%";
            }
            else
            {
                GeriatricAssessment = "-";
            }

            //Add CareAtPresentation data to ResultString
            ResultString += GeriatricAssessmentPatients.Count.ToString() + ":";
            ResultString += GeriatricAssessment.ToString() + ":";

            #endregion

            #region Surgery within 48 hours Calculation

            string Surgery48;
            var Surgery48Patients = patients.Where(item => item.SurgeryDateTime != null).ToList();
            if (Surgery48Patients.Count() > 0)
            {
                Surgery48 = (Surgery48Patients.Where(item => ((item.SurgeryDateTime - item.StartDate).Value.TotalHours <= 48)).Count() * 100 / Surgery48Patients.Count()).ToString("0") + "%";
            }
            else
            {
                Surgery48 = "-";
            }

            //Add CareAtPresentation data to ResultString
            ResultString += Surgery48Patients.Count.ToString() + ":";
            ResultString += Surgery48.ToString() + ":";

            #endregion

            #region Day 1 Mobilisation Calculation

            string Mobilisation;
            var MobilisationPatients = patients.Where(item => item.Mobilisation != null).ToList();
            if (MobilisationPatients.Count() > 0)
            {
                Mobilisation = (MobilisationPatients.Where(item => item.Mobilisation == "0").Count() * 100 / MobilisationPatients.Count()).ToString("0") + "%";
            }
            else
            {
                Mobilisation = "-";
            }

            //Add CareAtPresentation data to ResultString
            ResultString += MobilisationPatients.Count.ToString() + ":";
            ResultString += Mobilisation.ToString() + ":";

            #endregion

            #region Unrestricted Weight Bearing Calculation

            string WeightBearing;
            var WeightBearingPatients = patients.Where(item => item.FullWeightBear != null).ToList();
            if (WeightBearingPatients.Count() > 0)
            {
                WeightBearing = (WeightBearingPatients.Where(item => item.FullWeightBear == "0").Count() * 100 / WeightBearingPatients.Count()).ToString("0") + "%";
            }
            else
            {
                WeightBearing = "-";
            }

            //Add CareAtPresentation data to ResultString
            ResultString += WeightBearingPatients.Count.ToString() + ":";
            ResultString += WeightBearing.ToString() + ":";

            #endregion

            #region New Pressure Ulcers Calculation

            string PressureUlcers;
            var PressureUlcerPatients = patients.Where(item => item.PressureUlcers != null).ToList();
            if (PressureUlcerPatients.Count() > 0)
            {
                PressureUlcers = (PressureUlcerPatients.Where(item => item.PressureUlcers == "1").Count() * 100 / PressureUlcerPatients.Count()).ToString("0") + "%";
            }
            else
            {
                PressureUlcers = "-";
            }

            //Add CareAtPresentation data to ResultString
            ResultString += PressureUlcerPatients.Count.ToString() + ":";
            ResultString += PressureUlcers.ToString() + ":";

            #endregion

            #region Discharge Bone Medication Calculation

            string DischargeBoneMedication;
            var DischargeBoneMedicationPatients = patients.Where(item => item.BoneMedDischarge != null).ToList();
            if (DischargeBoneMedicationPatients.Count() > 0)
            {
                DischargeBoneMedication = (DischargeBoneMedicationPatients.Where(item => item.BoneMedDischarge == "1" || item.BoneMedDischarge == "2").Count() * 100 / DischargeBoneMedicationPatients.Count()).ToString("0") + "%";
            }
            else
            {
                DischargeBoneMedication = "-";
            }

            //Add CareAtPresentation data to ResultString
            ResultString += DischargeBoneMedicationPatients.Count.ToString() + ":";
            ResultString += DischargeBoneMedication.ToString() + ":";

            #endregion

            #region Falls Assessment Calculation

            string FallsAssessment;
            var FallsAssessmentPatients = patients.Where(item => item.FallsAssessment != null).ToList();
            if (FallsAssessmentPatients.Count() > 0)
            {
                FallsAssessment = (FallsAssessmentPatients.Where(item => item.FallsAssessment == "1").Count() * 100 / FallsAssessmentPatients.Count()).ToString("0") + "%";
            }
            else
            {
                FallsAssessment = "-";
            }

            //Add CareAtPresentation data to ResultString
            ResultString += FallsAssessmentPatients.Count.ToString() + ":";
            ResultString += FallsAssessment.ToString() + ":";

            #endregion

            #region Transition Calculation

            string Transition;
            var TransitionPatients = patients.Where(item => item.UResidence == "1" && item.Residence120 != null).ToList();
            if (TransitionPatients.Count() > 0)
            {
                Transition = (TransitionPatients.Where(item => item.Residence120 == "1").Count() * 100 / TransitionPatients.Count()).ToString("0") + "%";
            }
            else
            {
                Transition = "-";
            }

            //Add CareAtPresentation data to ResultString
            ResultString += TransitionPatients.Count.ToString() + ":";
            ResultString += Transition.ToString() + ":";

            #endregion

            //Return result
            return Json(ResultString, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult Restricted()
        {
            ViewBag.MenuDashboard = "active";
            return View();
        }

        [Authorize]
        public ActionResult QualityLock(int QID)
        {
            ViewBag.MenuDashboard = "active";

             ViewBag.QualityURL = "/quality/edit/" + QID.ToString();
            return View();
        }

        [Authorize]
        public ActionResult Training()
        {
            ViewBag.MenuTraining = "active";
            return View();
        }
    }
}