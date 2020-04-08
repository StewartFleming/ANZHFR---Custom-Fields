using ANZHFR.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Collections;

namespace ANZHFR.Services.Patients
{
    public class PatientServices : BaseServices
    {
        public Patient GetUniquePatient(long hospitalID, string MRN, string FractureSide)
        {
            if (Entity.Patients.Count(x => x.HospitalID == hospitalID && x.MRN == MRN && x.FractureSide == FractureSide) > 0)
            {
                return Entity.Patients.SingleOrDefault(x => x.HospitalID == hospitalID && x.MRN == MRN && x.FractureSide == FractureSide);
            }
            return null;
        }
        public QualityPatient GetUniqueQualityPatient(long hospitalID, string MRN, string FractureSide)
        {
            if (Entity.QualityPatients.Count(x => x.HospitalID == hospitalID && x.MRN == MRN && x.FractureSide == FractureSide) > 0)
            {
                return Entity.QualityPatients.SingleOrDefault(x => x.HospitalID == hospitalID && x.MRN == MRN && x.FractureSide == FractureSide);
            }
            return null;
        }

        public int QualityCount(long patientID)
        {
            return Entity.QualityPatients.Count(x => x.ANZHFRID == patientID);
        }
        public bool IsQualityHospital(long hospitalID)
        {
            return Entity.QualityPatients.Count(x => x.HospitalID == hospitalID) > 0;
        }

      public string CreateQualityRecords(long? hospitalID, long? qualityCount, int year)
        {
            //Find all the eligable patients for the selected year
            DateTime startDate, endDate;
            CultureInfo provider = CultureInfo.InvariantCulture;

            endDate = DateTime.ParseExact("31/12/" + year.ToString(), "dd/MM/yyyy", provider);
            startDate = DateTime.ParseExact("01/01/" + year.ToString(), "dd/MM/yyyy", provider);

            var patients = Entity.Patients.Where(x => x.HospitalID == hospitalID && (x.OptedOut.HasValue && x.OptedOut != true) &&
            x.StartDate >= startDate && x.StartDate <= endDate).ToList();

            // Select random records from the list
            if (patients.Count <= qualityCount)
            {
                return "Not enough records found.";
            }
            else
            {
                var random = new Random();
                for (var i = 0; i < qualityCount; i++)
                {  // Get the next item at random. 
                    var randomIndex = random.Next(0, patients.Count);
                    var qi = patients[randomIndex];
                    // Remove the item from the patient list and copy it to the Quality Table. 
                    patients.RemoveAt(randomIndex);
                    QInsert(qi.ANZHFRID, qi.HospitalID, qi.Name, qi.Surname, qi.MRN, qi.Medicare, qi.FractureSide);
                }
            }
            return "Created!";
        }
        public int PatientCountByYear(long? hospitalID, int year)
        {
            DateTime startDate, endDate;
            CultureInfo provider = CultureInfo.InvariantCulture;
            endDate = DateTime.ParseExact("31/12/"+year.ToString(), "dd/MM/yyyy", provider);
            startDate = DateTime.ParseExact("01/01/" + year.ToString(), "dd/MM/yyyy", provider);
            var patientCount = Entity.Patients.Where(x => x.HospitalID == hospitalID && (x.OptedOut.HasValue && x.OptedOut != true) &&
            x.StartDate >= startDate && x.StartDate <= endDate).Count();
            return patientCount;
        }
        public List<Exporting> GetPatientsForExport(long hospitalID, string from_date, string to_date)
        {
            DateTime startDate, endDate;
            CultureInfo provider = CultureInfo.InvariantCulture;
            var exports = Entity.Exportings.Where(x => x.HospitalID == hospitalID && (x.OptedOut.HasValue && x.OptedOut != true)).ToList();
            if (from_date == "" && to_date == "")
            {
                return exports;
            }
            else if (from_date == "")
            {
                endDate = DateTime.ParseExact(to_date, "dd/MM/yyyy", provider);
                exports = exports.Where(x => x.StartDate <= endDate).ToList();
                return exports;
            }
            else
            {
                endDate = DateTime.ParseExact(to_date, "dd/MM/yyyy", provider);
                startDate = DateTime.ParseExact(from_date, "dd/MM/yyyy", provider);
                exports = exports.Where(x => x.StartDate >= startDate && x.StartDate <= endDate).ToList();
                return exports;
            }

            //return exports;
            // public List<Patients> GetPatientsForExport(long hospitalID)
            // var patients = Entity.Patients.Where(x => x.HospitalID == hospitalID && x.OptedOut != true).ToList<Patient>();
            // return patients;
        }
        public List<Patient> Get(long hospitalID, string name = "")
        {
            var results = Entity.Patients.Where(x => x.HospitalID == hospitalID).ToList();

            if (!string.IsNullOrEmpty(name))
            {
                results = results.Where(x => (x.Name != null && x.Name.Trim().ToUpper().Contains(name.Trim().ToUpper()))
                       || (x.Surname != null && x.Surname.Trim().ToUpper().Contains(name.Trim().ToUpper()))
                       || (x.Name != null && x.Surname != null && (x.Name.Trim() + " " + x.Surname.Trim()).ToUpper().Contains(name.Trim().ToUpper()))
                       || (x.Medicare != null && x.Medicare.Contains(name.Trim()))
                       || (x.MRN.Contains(name.Trim()))).ToList();
            }

            return results;
        }
        public List<Patient> GetAll()
        {
            var results = Entity.Patients.ToList();
            return results;
        }
        public List<Patient> GetAllHospital(long hospitalID)
        {
            var results = Entity.Patients.Where(x => x.HospitalID == hospitalID).ToList();
            return results;
        }
        public List<Patient> GetCurrentYear(long hospitalID, long year)
        {
            var results = Entity.Patients.Where(x => x.HospitalID == hospitalID && x.StartDate.Value.Year == year).ToList();
            return results;
        }
        public List<Patient> GetActive(long hospitalID, string name = "")
        {
            var results = Entity.Patients.Where(x => x.HospitalID == hospitalID && !x.HospitalDischargeDate.HasValue && x.OptedOut != true && x.CannotFollowup != true).ToList();

            if (!string.IsNullOrEmpty(name))
            {
                results = results.Where(x => (x.Name != null && x.Name.Trim().ToUpper().Contains(name.Trim().ToUpper()))
                       || (x.Surname != null && x.Surname.Trim().ToUpper().Contains(name.Trim().ToUpper()))
                       || (x.Name != null && x.Surname != null && (x.Name.Trim() + " " + x.Surname.Trim()).ToUpper().Contains(name.Trim().ToUpper()))
                       || (x.Medicare != null && x.Medicare.Contains(name.Trim()))
                       || (x.MRN.Contains(name.Trim()))).ToList();
            }

            return results;
        }
        public List<Patient> GetDashboard(long hospitalID, string patientType, string period, string from_date, string to_date)
        {
            DateTime startDate, endDate;            
            CultureInfo provider = CultureInfo.InvariantCulture;
            if (from_date != "")
            {
                startDate = DateTime.ParseExact(from_date, "dd/MM/yyyy", provider);
            }
            else
            {
                startDate = new DateTime(System.DateTime.Now.AddYears(-100).Year, 1, 1);
            }
            if (to_date != "")
            {
                endDate = DateTime.ParseExact(to_date, "dd/MM/yyyy", provider);
            }
            else
            {
                endDate = System.DateTime.Now;
            }
            endDate = endDate.AddDays(1);

            var results = Entity.Patients.Where(x => x.HospitalID == hospitalID && x.StartDate >= startDate && x.StartDate <= endDate && x.OptedOut != true ).ToList();

            switch (patientType)
            {
                case "ED":
                    results = results.Where(x => x.AdmissionViaED == "1").ToList();
                    break;
                case "EDTransferred":
                    results = results.Where(x => x.AdmissionViaED == "1" || x.AdmissionViaED == "2").ToList();
                    break;
                case "EDTransferredFall":
                    results = results.Where(x => x.AdmissionViaED == "1" || x.AdmissionViaED == "2" || x.AdmissionViaED == "3").ToList();
                    break;
                case "EDFallOther":
                    results = results.Where(x => x.AdmissionViaED == "1" || x.AdmissionViaED == "3" || x.AdmissionViaED == "4").ToList();
                    break;
                case "EDTransferredFallOther":
                    //No Change
                    break;
                case "EDFall":
                    results = results.Where(x => x.AdmissionViaED == "1" || x.AdmissionViaED == "3").ToList();
                    break;
                case "TransferredFall":
                    results = results.Where(x => x.AdmissionViaED == "2" || x.AdmissionViaED == "3").ToList();
                    break;
                case "EDOther":
                    results = results.Where(x => x.AdmissionViaED == "1" || x.AdmissionViaED == "4").ToList();
                    break;
                case "FallOther":
                    results = results.Where(x => x.AdmissionViaED == "3" || x.AdmissionViaED == "4").ToList();
                    break;
                case "TransferredOther":
                    results = results.Where(x => x.AdmissionViaED == "2" || x.AdmissionViaED == "4").ToList();
                    break;
                case "Transferred":
                    results = results.Where(x => x.AdmissionViaED == "2").ToList();
                    break;
                case "Fall":
                    results = results.Where(x => x.AdmissionViaED == "3").ToList();
                    break;
                case "Other":
                    results = results.Where(x => x.AdmissionViaED == "4").ToList();
                    break;
                case "TransferredFallOther":
                    results = results.Where(x => x.AdmissionViaED == "2" || x.AdmissionViaED == "3" || x.AdmissionViaED == "4").ToList();
                    break;
            }

            return results;
        }

        public List<Patient> GetExportCount(long hospitalID, string from_date, string to_date)
        {
            DateTime startDate, endDate;
            CultureInfo provider = CultureInfo.InvariantCulture;
            var results = Entity.Patients.Where(x => x.HospitalID == hospitalID && x.OptedOut != true).ToList();
            if (from_date == "" && to_date == "")
            {
                return results;
            }else if(from_date == "")
            {
                endDate = DateTime.ParseExact(to_date, "dd/MM/yyyy", provider);
                results = results.Where(x => x.StartDate <= endDate).ToList();
                return results;
            }
            else //Asujme both dates are provided.
            {
                endDate = DateTime.ParseExact(to_date, "dd/MM/yyyy", provider);
                startDate = DateTime.ParseExact(from_date, "dd/MM/yyyy", provider);
                results = results.Where(x => x.StartDate >= startDate && x.StartDate <= endDate).ToList();
                return results;
            }
        }
        public List<Patient> Get30(long hospitalID, string name = "")
        {
            System.DateTime today = System.DateTime.Now;
            System.TimeSpan duration = new System.TimeSpan(-30, 0, 0, 0);
            System.DateTime answer = today.Add(duration);

            var results = Entity.Patients.Where(x => x.HospitalID == hospitalID &&
                //(x.ArrivalDateTime <= answer || x.InHospFractureDateTime <= answer || x.TransferDateTime <= answer) &&
                (x.StartDate <= answer) &&
                !x.FollowupDate30.HasValue &&
                x.DischargeResidence != "5" &&
                x.DischargeDest != "6" &&
                x.CannotFollowup != true);

            if (!string.IsNullOrEmpty(name))
            {
                results = results.Where(x => (x.Name != null && x.Name.Trim().ToUpper().Contains(name.Trim().ToUpper()))
                       || (x.Surname != null && x.Surname.Trim().ToUpper().Contains(name.Trim().ToUpper()))
                       || (x.Name != null && x.Surname != null && (x.Name.Trim() + " " + x.Surname.Trim()).ToUpper().Contains(name.Trim().ToUpper()))
                       || (x.Medicare != null && x.Medicare.Contains(name.Trim()))
                       || (x.MRN.Contains(name.Trim())));
            }

            return results.ToList();
        }

        public List<Patient> Get120(long hospitalID, string name = "")
        {
            System.DateTime today = System.DateTime.Now;
            System.TimeSpan duration = new System.TimeSpan(-120, 0, 0, 0);
            System.DateTime answer = today.Add(duration);

            var results = Entity.Patients.Where(x => x.HospitalID == hospitalID &&
                //(x.ArrivalDateTime <= answer || x.InHospFractureDateTime <= answer || x.TransferDateTime <= answer) &&
                (x.StartDate <= answer) &&
                !x.FollowupDate120.HasValue &&
                x.DischargeResidence != "5" &&
                x.DischargeDest != "6" &&
                x.CannotFollowup != true &&
                x.Survival30 != "1").ToList();

            if (!string.IsNullOrEmpty(name))
            {
                results = results.Where(x => (x.Name != null && x.Name.Trim().ToUpper().Contains(name.Trim().ToUpper()))
                       || (x.Surname != null && x.Surname.Trim().ToUpper().Contains(name.Trim().ToUpper()))
                       || (x.Name != null && x.Surname != null && (x.Name.Trim() + " " + x.Surname.Trim()).ToUpper().Contains(name.Trim().ToUpper()))
                       || (x.Medicare != null && x.Medicare.Contains(name.Trim()))
                       || (x.MRN.Contains(name.Trim()))).ToList();
            }

            return results;
        }
        public List<QualityPatient> GetQuality(long hospitalID, string name = "")
        {
            System.DateTime today = System.DateTime.Now;
            System.TimeSpan duration = new System.TimeSpan(-120, 0, 0, 0);
            System.DateTime answer = today.Add(duration);

            var results = Entity.QualityPatients.Where(x => x.HospitalID == hospitalID && x.QualityScore == null).ToList();

            if (!string.IsNullOrEmpty(name))
            {
                results = results.Where(x => (x.Name != null && x.Name.Trim().ToUpper().Contains(name.Trim().ToUpper()))
                       || (x.Surname != null && x.Surname.Trim().ToUpper().Contains(name.Trim().ToUpper()))
                       || (x.Name != null && x.Surname != null && (x.Name.Trim() + " " + x.Surname.Trim()).ToUpper().Contains(name.Trim().ToUpper()))
                       || (x.Medicare != null && x.Medicare.Contains(name.Trim()))
                       || (x.MRN.Contains(name.Trim()))).ToList();
            }

            return results;
        }
        public List<CalendarData> GetCalendarData(long hospitalID)
        {
            System.DateTime today = System.DateTime.Now;
            System.TimeSpan duration = new System.TimeSpan(-30, 0, 0, 0);
            System.DateTime answer = today.Add(duration);

            var followups = (from c in Entity.CalendarFollowups
                             where c.HospitalID == hospitalID
                             select c).ToList();

            var results = (from c in followups
                           select new CalendarData
                           {
                               title = string.Format("{0} followup: {1}", c.Followup, c.Name),
                               start = c.ExpectedDate.Value.ToString("yyyy-MM-dd HH:mm"),
                               end = c.ExpectedDate.Value.ToString("yyyy-MM-dd HH:mm"),
                               url = string.Format("/patient/Edit/{0}", c.ANZHFRID),
                               color = c.Followup == 30 ? "#2980B9" : "#27AE60"
                           }).ToList();

            return results;
        }
        public List<YearlyValidation> YearlyValidation(long hospitalID, int? year)
        {
            var results = (from x in Entity.YearlyValidations
                           where x.HospitalID == hospitalID &&
                           x.StartDate.HasValue &&
                           x.StartDate.Value.Year == year
                           select x).ToList();

            return results;
        }
        public Patient GetPatientById(int id)
        {
            return Entity.Patients.Find(id);
        }
        public QualityPatient GetQualityPatientById(int id)
        {
            return Entity.QualityPatients.Find(id);
        }

        public QualityPatient GetQualityPatientByPatientId(int id)
        { 
            return Entity.QualityPatients.SingleOrDefault(x => x.ANZHFRID == id);    
        }
        public Patient Insert(long hospitalId, string name, string surname, string mrn, string phone,
            DateTime? dob, string sex, string indig, string ethnic, string postCode, string medicare,
            string patientType, string AdmissionViaED, string uResidence, string transferHospital, DateTime? transferDateTime,
            DateTime? arrivalDateTime, DateTime? departureDateTime, DateTime? inHospFractureDateTime,
            string wardType, string preAdWalk, string cognitiveState, string boneMed, string preOpMedAss,
            string fractureSide, string atypicalFracture, string fractureType, string surgery, DateTime? surgeryDateTime,
            string surgeryDelay, string surgeryDelayOther, string anaesthesia, string analgesia, string consultantPresent,
            string operation, string interOpFracture, string fullWeightBear, string mobilisation, string pressureUlcers, string geriatricAssessment,
            DateTime? geriatricAssDateTime, string fallsAssessment, string boneMedDischarge, DateTime? wardDischargeDate,
            string dischargeDest, DateTime? hospitalDischargeDate, string dischargeResidence, short? oLengthofStay, short? hLengthofStay,
            DateTime? followupDate30, DateTime? healthServiceDischarge30, string survival30, string residence30, string weightBear30,
            string walkingAbility30, string boneMed30, string reoperation30, DateTime? followupDate120, DateTime? healthServiceDischarge120,
            string survival120, string residence120, string weightBear120, string walkingAbility120, string boneMed120, string reoperation120, string asaGrade,
            decimal? completeness, string completeExemption, DateTime? created, string author, DateTime? lastModified, string modifiedBy,
            string cognitiveAssessment, string painAssessment, string painManagement,
            string DeleriumAssessment, DateTime? startDate, bool informed, bool optedOut, bool cannotFollowup, string Malnutrition
           )
        {
           String er = "";

            try
            {
                var model = new Patient();
                model.HospitalID = hospitalId;
                model.Name = name;
                model.Surname = surname;
                model.MRN = mrn;
                model.Phone = phone;
                model.DOB = dob;
                model.Sex = sex;
                model.Indig = indig;
                model.Ethnic = ethnic;
                model.PostCode = postCode;
                model.Medicare = medicare;
                model.PatientType = patientType;
                model.AdmissionViaED = AdmissionViaED;
                model.UResidence = uResidence;
                model.TransferHospital = transferHospital;
                model.TransferDateTime = transferDateTime;
                model.ArrivalDateTime = arrivalDateTime;
                model.DepartureDateTime = departureDateTime;
                model.InHospFractureDateTime = inHospFractureDateTime;
                model.WardType = wardType;
                model.PreAdWalk = preAdWalk;
                model.CognitiveAssessment = cognitiveAssessment;
                model.CognitiveState = cognitiveState;
                model.PainAssessment = painAssessment;
                model.PainManagement = painManagement;
                model.BoneMed = boneMed;
                model.PreOpMedAss = preOpMedAss;
                model.FractureSide = fractureSide;
                model.AtypicalFracture = atypicalFracture;
                model.FractureType = fractureType;
                model.Surgery = surgery;
                model.SurgeryDateTime = surgeryDateTime;
                model.SurgeryDelay = surgeryDelay;
                model.SurgeryDelayOther = surgeryDelayOther;
                model.Anaesthesia = anaesthesia;
                model.Analgesia = analgesia;
                model.ConsultantPresent = consultantPresent;
                model.Operation = operation;
                model.InterOpFracture = interOpFracture;
                model.FullWeightBear = fullWeightBear;
                model.Mobilisation = mobilisation;
                model.PressureUlcers = pressureUlcers;
                model.GeriatricAssessment = geriatricAssessment;
                model.GeriatricAssDateTime = geriatricAssDateTime;
                model.FallsAssessment = fallsAssessment;
                model.BoneMedDischarge = boneMedDischarge;
                model.WardDischargeDate = wardDischargeDate;
                model.DischargeDest = dischargeDest;
                model.HospitalDischargeDate = hospitalDischargeDate;
                model.DischargeResidence = dischargeResidence;
                model.OLengthofStay = oLengthofStay;
                model.HLengthofStay = hLengthofStay;
                model.FollowupDate30 = followupDate30;
                model.HealthServiceDischarge30 = healthServiceDischarge30;
                model.Survival30 = survival30;
                model.Residence30 = reoperation30;
                model.WeightBear30 = weightBear30;
                model.WalkingAbility30 = walkingAbility30;
                model.BoneMed30 = boneMed30;
                model.Reoperation30 = reoperation30;
                model.FollowupDate120 = followupDate120;
                model.HealthServiceDischarge120 = healthServiceDischarge120;
                model.Survival120 = survival120;
                model.Residence120 = reoperation120;
                model.WeightBear120 = weightBear120;
                model.WalkingAbility120 = walkingAbility120;
                model.BoneMed120 = boneMed120;
                model.Reoperation120 = reoperation120;
                model.ASAGrade = asaGrade;
                model.Completeness = completeness;
                model.CompleteExemption = completeExemption;
                model.Created = created;
                model.Author = author;
                model.LastModified = lastModified;
                model.ModifiedBy = modifiedBy;
                model.DeleriumAssessment = DeleriumAssessment;
                model.StartDate = startDate;
                model.Informed = informed;
                model.OptedOut = optedOut;
                model.CannotFollowup = cannotFollowup;
                model.Malnutrition = Malnutrition;

                Entity.Patients.Add(model);
                Entity.SaveChanges();

                return model;
            }
            catch (Exception e)
            {
                er = "Failed to Insert: " + e;
                return null;
            }
        }

        public Patient Update(int id, string name, string surname, string mrn, string phone,
            DateTime? dob, string sex, string indig, string ethnic, string postCode, string medicare,
            string patientType, string AdmissionViaED, string uResidence, string transferHospital, DateTime? transferDateTime,
            DateTime? arrivalDateTime, DateTime? departureDateTime, DateTime? inHospFractureDateTime,
            string wardType, string preAdWalk, string cognitiveState, string boneMed, string preOpMedAss,
            string fractureSide, string atypicalFracture, string fractureType, string surgery, DateTime? surgeryDateTime,
            string surgeryDelay, string surgeryDelayOther, string anaesthesia, string analgesia, string consultantPresent,
            string operation, string interOpFracture, string fullWeightBear, string mobilisation, string pressureUlcers, string geriatricAssessment,
            DateTime? geriatricAssDateTime, string fallsAssessment, string boneMedDischarge, DateTime? wardDischargeDate,
            string dischargeDest, DateTime? hospitalDischargeDate, string dischargeResidence, short? oLengthofStay, short? hLengthofStay,
            DateTime? followupDate30, DateTime? healthServiceDischarge30, string survival30, string residence30, string weightBear30,
            string walkingAbility30, string boneMed30, string reoperation30, DateTime? followupDate120, DateTime? healthServiceDischarge120,
            string survival120, string residence120, string weightBear120, string walkingAbility120, string boneMed120, string reoperation120, string asaGrade,
            decimal completeness, string completeExemption, DateTime? lastModified, string modifiedBy,
            string cognitiveAssessment, string painAssessment, string painManagement,
            string DeleriumAssessment, DateTime? startDate, bool informed, bool optedOut, bool cannotFollowup, string Malnutrition,
            DateTime? DeathDate, string FirstDayWalking,
            string EQ5D_Mobility, string EQ5D_SelfCare, string EQ5D_UsualActivity, string EQ5D_Pain, string EQ5D_Anxiety, short? EQ5D_Health
            )
        {
            try
            {
                var model = GetPatientById(id);
                model.Name = name;
                model.Surname = surname;
                model.MRN = mrn;
                model.Phone = phone;
                model.DOB = dob;
                model.Sex = sex;
                model.Indig = indig;
                model.Ethnic = ethnic;
                model.PostCode = postCode;
                model.Medicare = medicare;
                model.PatientType = patientType;
                model.AdmissionViaED = AdmissionViaED;
                model.UResidence = uResidence;
                model.TransferHospital = transferHospital;
                model.TransferDateTime = transferDateTime;
                model.ArrivalDateTime = arrivalDateTime;
                model.DepartureDateTime = departureDateTime;
                model.InHospFractureDateTime = inHospFractureDateTime;
                model.WardType = wardType;
                model.PreAdWalk = preAdWalk;
                model.CognitiveState = cognitiveState;
                model.BoneMed = boneMed;
                model.PreOpMedAss = preOpMedAss;
                model.FractureSide = fractureSide;
                model.AtypicalFracture = atypicalFracture;
                model.FractureType = fractureType;
                model.Surgery = surgery;
                model.SurgeryDateTime = surgeryDateTime;
                model.SurgeryDelay = surgeryDelay;
                model.SurgeryDelayOther = surgeryDelayOther;
                model.Anaesthesia = anaesthesia;
                model.Analgesia = analgesia;
                model.ConsultantPresent = consultantPresent;
                model.Operation = operation;
                model.InterOpFracture = interOpFracture;
                model.FullWeightBear = fullWeightBear;
                model.Mobilisation = mobilisation;
                model.PressureUlcers = pressureUlcers;
                model.GeriatricAssessment = geriatricAssessment;
                model.GeriatricAssDateTime = geriatricAssDateTime;
                model.FallsAssessment = fallsAssessment;
                model.BoneMedDischarge = boneMedDischarge;
                model.WardDischargeDate = wardDischargeDate;
                model.DischargeDest = dischargeDest;
                model.HospitalDischargeDate = hospitalDischargeDate;
                model.OLengthofStay = oLengthofStay;
                model.HLengthofStay = hLengthofStay;
                model.DischargeResidence = dischargeResidence;
                model.FollowupDate30 = followupDate30;
                model.HealthServiceDischarge30 = healthServiceDischarge30;
                model.Survival30 = survival30;
                model.Residence30 = residence30;
                model.WeightBear30 = weightBear30;
                model.WalkingAbility30 = walkingAbility30;
                model.BoneMed30 = boneMed30;
                model.Reoperation30 = reoperation30;
                model.FollowupDate120 = followupDate120;
                model.HealthServiceDischarge120 = healthServiceDischarge120;
                model.Survival120 = survival120;
                model.Residence120 = residence120;
                model.WeightBear120 = weightBear120;
                model.WalkingAbility120 = walkingAbility120;
                model.BoneMed120 = boneMed120;
                model.Reoperation120 = reoperation120;
                model.ASAGrade = asaGrade;
                model.Completeness = completeness;
                model.CompleteExemption = completeExemption;
                model.LastModified = lastModified;
                model.ModifiedBy = modifiedBy;
                model.CognitiveAssessment = cognitiveAssessment;
                model.PainAssessment = painAssessment;
                model.PainManagement = painManagement;
                model.DeleriumAssessment = DeleriumAssessment;
                model.StartDate = startDate;
                model.Informed = informed;
                model.OptedOut = optedOut;
                model.CannotFollowup = cannotFollowup;
                model.Malnutrition = Malnutrition;
                model.DeathDate = DeathDate;
                model.FirstDayWalking = FirstDayWalking;
                model.EQ5D_Mobility = EQ5D_Mobility;
                model.EQ5D_SelfCare = EQ5D_SelfCare;
                model.EQ5D_UsualActivity = EQ5D_UsualActivity;
                model.EQ5D_Pain = EQ5D_Pain;
                model.EQ5D_Anxiety = EQ5D_Anxiety;
                model.EQ5D_Health = EQ5D_Health;

                Entity.SaveChanges();

                return model;
            }
            catch
            {
                return null;
            }
        }

        public QualityPatient QUpdate(int qid, int id, string name, string surname, string mrn, string phone,
           DateTime? dob, string sex, string indig, string ethnic, string postCode, string medicare,
           string patientType, string AdmissionViaED, string uResidence, string transferHospital, DateTime? transferDateTime,
           DateTime? arrivalDateTime, DateTime? departureDateTime, DateTime? inHospFractureDateTime,
           string wardType, string preAdWalk, string cognitiveState, string boneMed, string preOpMedAss,
           string fractureSide, string atypicalFracture, string fractureType, string surgery, DateTime? surgeryDateTime,
           string surgeryDelay, string surgeryDelayOther, string anaesthesia, string analgesia, string consultantPresent,
           string operation, string interOpFracture, string fullWeightBear, string mobilisation, string pressureUlcers, string geriatricAssessment,
           DateTime? geriatricAssDateTime, string fallsAssessment, string boneMedDischarge, DateTime? wardDischargeDate,
           string dischargeDest, DateTime? hospitalDischargeDate, string dischargeResidence, short? oLengthofStay, short? hLengthofStay,
           string asaGrade, decimal completeness, string completeExemption, DateTime? lastModified, string modifiedBy,
           string cognitiveAssessment, string painAssessment, string painManagement,
           string DeleriumAssessment, DateTime? startDate, bool informed, bool optedOut, bool cannotFollowup, string Malnutrition,
           DateTime? deathDate, string firstDayWalking,

           int qdob, int qsex, int qindig, int qethnic,
           int qpatientType, int qAdmissionViaED, int quResidence, int qtransferHospital, int qtransferDateTime,
           int qarrivalDateTime, int qdepartureDateTime, int qinHospFractureDateTime,
           int q15arrivalDateTime, int q15departureDateTime, int q15inHospFractureDateTime,
           int qwardType, int qpreAdWalk, int qcognitiveState, int qboneMed, int qpreOpMedAss,
           int qatypicalFracture, int qfractureType, int qsurgery, int qsurgeryDateTime, int q15surgeryDateTime,
           int qsurgeryDelay, int qanaesthesia, int qanalgesia, int qconsultantPresent,
           int qoperation, int qinterOpFracture, int qfullWeightBear, int qmobilisation, int qpressureUlcers, int qgeriatricAssessment,
           int qgeriatricAssDateTime, int qfallsAssessment, int qboneMedDischarge, int qwardDischargeDate,
           int qdischargeDest, int qhospitalDischargeDate, int qdischargeResidence, int qasaGrade,
           int qcognitiveAssessment, int qpainAssessment, int qpainManagement,
           int qDeleriumAssessment, int qMalnutrition,
           int qdeathDate, int qfirstDayWalking,

           int qScore, int qScore15, int qScoreMM, string qScoreComments
           )
        {
            try
            {
                var model = GetQualityPatientById(qid);
                model.ANZHFRID = id;
                model.Name = name;
                model.Surname = surname;
                model.MRN = mrn;
                model.Phone = phone;
                model.DOB = dob;
                model.Sex = sex;
                model.Indig = indig;
                model.Ethnic = ethnic;
                model.PostCode = postCode;
                model.Medicare = medicare;
                model.PatientType = patientType;
                model.AdmissionViaED = AdmissionViaED;
                model.UResidence = uResidence;
                model.TransferHospital = transferHospital;
                model.TransferDateTime = transferDateTime;
                model.ArrivalDateTime = arrivalDateTime;
                model.DepartureDateTime = departureDateTime;
                model.InHospFractureDateTime = inHospFractureDateTime;
                model.WardType = wardType;
                model.PreAdWalk = preAdWalk;
                model.CognitiveState = cognitiveState;
                model.BoneMed = boneMed;
                model.PreOpMedAss = preOpMedAss;
                model.FractureSide = fractureSide;
                model.AtypicalFracture = atypicalFracture;
                model.FractureType = fractureType;
                model.Surgery = surgery;
                model.SurgeryDateTime = surgeryDateTime;
                model.SurgeryDelay = surgeryDelay;
                model.SurgeryDelayOther = surgeryDelayOther;
                model.Anaesthesia = anaesthesia;
                model.Analgesia = analgesia;
                model.ConsultantPresent = consultantPresent;
                model.Operation = operation;
                model.InterOpFracture = interOpFracture;
                model.FullWeightBear = fullWeightBear;
                model.Mobilisation = mobilisation;
                model.PressureUlcers = pressureUlcers;
                model.GeriatricAssessment = geriatricAssessment;
                model.GeriatricAssDateTime = geriatricAssDateTime;
                model.FallsAssessment = fallsAssessment;
                model.BoneMedDischarge = boneMedDischarge;
                model.WardDischargeDate = wardDischargeDate;
                model.DischargeDest = dischargeDest;
                model.HospitalDischargeDate = hospitalDischargeDate;
                model.OLengthofStay = oLengthofStay;
                model.HLengthofStay = hLengthofStay;
                model.DischargeResidence = dischargeResidence;
                model.ASAGrade = asaGrade;
                model.Completeness = completeness;
                model.CompleteExemption = completeExemption;
                model.LastModified = lastModified;
                model.ModifiedBy = modifiedBy;
                model.CognitiveAssessment = cognitiveAssessment;
                model.PainAssessment = painAssessment;
                model.PainManagement = painManagement;
                model.DeleriumAssessment = DeleriumAssessment;
                model.StartDate = startDate;
                model.Informed = informed;
                model.OptedOut = optedOut;
                model.CannotFollowup = cannotFollowup;
                model.Malnutrition = Malnutrition;
                model.DeathDate = deathDate;
                model.FirstDayWalking = firstDayWalking;

                model.QSex = qsex;
                model.QIndig = qindig;
                model.QEthnic = qethnic;
                model.QPatientType = qpatientType;
                model.QAdmissionViaED = qAdmissionViaED;
                model.QUResidence = quResidence;
                model.QTransferHospital = qtransferHospital;
                model.QTransferDateTime = qtransferDateTime;
                model.QArrivalDateTime = qarrivalDateTime;
                model.QDepartureDateTime = qdepartureDateTime;
                model.QInHospFractureDateTime = qinHospFractureDateTime;
                model.QWardType = qwardType;
                model.QPreAdWalk = qpreAdWalk;
                model.QCognitiveState = qcognitiveState;
                model.QBoneMed = qboneMed;
                model.QPreOpMedAss = qpreOpMedAss;
                model.QAtypicalFracture = qatypicalFracture;
                model.QFractureType = qfractureType;
                model.QSurgery = qsurgery;
                model.QSurgeryDateTime = qsurgeryDateTime;
                model.QSurgeryDelay = qsurgeryDelay;
                model.QAnaesthesia = qanaesthesia;
                model.QAnalgesia = qanalgesia;
                model.QConsultantPresent = qconsultantPresent;
                model.QOperation = qoperation;
                model.QInterOpFracture = qinterOpFracture;
                model.QFullWeightBear = qfullWeightBear;
                model.QMobilisation = qmobilisation;
                model.QPressureUlcers = qpressureUlcers;
                model.QGeriatricAssessment = qgeriatricAssessment;
                model.QGeriatricAssDateTime = qgeriatricAssDateTime;
                model.QFallsAssessment = qfallsAssessment;
                model.QBoneMedDischarge = qboneMedDischarge;
                model.QWardDischargeDate = qwardDischargeDate;
                model.QDischargeDest = qdischargeDest;
                model.QHospitalDischargeDate = qhospitalDischargeDate;
                model.QDischargeResidence = qdischargeResidence;
                model.QASAGrade = qasaGrade;
                model.QCognitiveAssessment = qcognitiveAssessment;
                model.QPainAssessment = qpainAssessment;
                model.QPainManagement = qpainManagement;
                model.QDeleriumAssessment = qDeleriumAssessment;
                model.QMalnutrition = qMalnutrition;
                model.QDeathDate = qdeathDate;
                model.QFirstDayWalking = qfirstDayWalking;

                model.QualityScore = qScore;
                model.QualityScore30 = qScore15;
                model.QualityScoreMM = qScoreMM;
                model.QualityScoreComments = qScoreComments;

                Entity.SaveChanges();

                return model;
            }
            catch
            {
                return null;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var model = GetPatientById(id);
                Entity.Patients.Remove(model);
                Entity.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool QDelete(int id)
        {
            try
            {
                var model = GetQualityPatientById(id);
                Entity.QualityPatients.Remove(model);
                Entity.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public QualityPatient QInsert(int anzhfrid, long hospitalId, string name, string surname, string mrn, string medicare, string fractureside)
        {
            try
            {
                var model = new QualityPatient();
                model.ANZHFRID = anzhfrid;
                model.HospitalID = hospitalId;
                model.Name = name;
                model.Surname = surname;
                model.MRN = mrn;
                model.Medicare = medicare;
                model.FractureSide = fractureside;
                model.Created = DateTime.Now;
                model.Author = "Quality Process";
                model.LastModified = DateTime.Now;
                model.ModifiedBy = "Quality Process";
               
                Entity.QualityPatients.Add(model);
                Entity.SaveChanges();

                return model;
            }
            catch
            {
                return null;
            }
        }

        public List<Sex> GetSex()
        {
            return Entity.Sexes.ToList();
        }

        public List<Indig> GetIndig()
        {
            return Entity.Indigs.ToList();
        }

        public List<Ethnic> GetEthnic()
        {
            return Entity.Ethnics.ToList();
        }

        public List<PatientType> GetPatientType()
        {
            return Entity.PatientTypes.ToList();
        }

        public List<AdmissionViaED> GetAdmissionViaED()
        {
            return Entity.AdmissionViaEDs.ToList();
        }

        public List<Residence> GetResidence()
        {
            return Entity.Residences.ToList();
        }

        public List<Hospital> GetHospitals()
        {
            return Entity.Hospitals.ToList();
        }

        public List<WardType> GetWardType()
        {
            return Entity.WardTypes.ToList();
        }

        public List<PreAdWalk> GetPreAdWalk()
        {
            return Entity.PreAdWalks.ToList();
        }

        public List<CognitiveState> GetCognitiveState()
        {
            return Entity.CognitiveStates.ToList();
        }

        public List<BoneMed> GetBoneMed()
        {
            return Entity.BoneMeds.ToList();
        }

        public List<PreOpMedAss> GetPreOpMedAss()
        {
            return Entity.PreOpMedAsses.ToList();
        }

        public List<FractureSide> GetFractureSide()
        {
            return Entity.FractureSides.ToList();
        }

        public List<AtypicalFracture> GetAtypicalFracture()
        {
            return Entity.AtypicalFractures.ToList();
        }

        public List<FractureType> GetFractureType()
        {
            return Entity.FractureTypes.ToList();
        }

        public List<Surgery> GetSurgery()
        {
            return Entity.Surgeries.ToList();
        }

        public List<SurgeryDelay> GetSurgeryDelay()
        {
            return Entity.SurgeryDelays.ToList();
        }

        public List<Anaesthesia> GetAnaesthesia()
        {
            return Entity.Anaesthesias.ToList();
        }

        public List<ASAGrade> GetASAGrade()
        {
            return Entity.ASAGrades.ToList();
        }

        public List<Analgesia> GetAnalgesia()
        {
            return Entity.Analgesias.ToList();
        }

        public List<ConsultantPresent> GetConsultantPresent()
        {
            return Entity.ConsultantPresents.ToList();
        }

        public List<Operation> GetOperation()
        {
            return Entity.Operations.ToList();
        }

        public List<InterOpFracture> GetInterOpFracture()
        {
            return Entity.InterOpFractures.ToList();
        }

        public List<Mobilisation> GetMobilisation()
        {
            return Entity.Mobilisations.ToList();
        }
        public List<FirstDayWalking> GetFirstDayWalking()
        {
            return Entity.FirstDayWalkings.ToList();
        }
        public List<PressureUlcer> GetPressureUlcers()
        {
            return Entity.PressureUlcers.ToList();
        }

        public List<GeriatricAssessment> GetGeriatricAssessment()
        {
            return Entity.GeriatricAssessments.ToList();
        }

        public List<FallsAssessment> GetFallsAssessment()
        {
            return Entity.FallsAssessments.ToList();
        }

        public List<DischargeDest> GetDischargeDest()
        {
            return Entity.DischargeDests.ToList();
        }

        public List<DischargeResidence> GetDischargeResidence()
        {
            return Entity.DischargeResidences.ToList();
        }

        public List<LengthofStay> GetLengthofStay()
        {
            return Entity.LengthofStays.ToList();
        }

        public List<Survival> GetSurvival()
        {
            return Entity.Survivals.ToList();
        }

        public List<WeightBear> GetWeightBear()
        {
            return Entity.WeightBears.ToList();
        }

        public List<WalkingAbility> GetWalkingAbility()
        {
            return Entity.WalkingAbilities.ToList();
        }

        public List<Reoperation> GetReoperation()
        {
            return Entity.Reoperations.ToList();
        }

        public List<AMTS> GetAMTS()
        {
            return Entity.AMTS.ToList();
        }

        public List<CognitiveAssessment> GetCognitiveAssessment()
        {
            return Entity.CognitiveAssessments.ToList();
        }

        public List<PainAssessment> GetPainAssessment()
        {
            return Entity.PainAssessments.ToList();
        }

        public List<PainManagement> GetPainManagement()
        {
            return Entity.PainManagements.ToList();
        }

        public List<DeleriumAssessment> GetDeleriumAssessment()
        {
            return Entity.DeleriumAssessments.ToList();
        }

        public List<UserPosition> GetUserPosition()
        {
            return Entity.UserPositions.ToList();
        }
        public List<Malnutrition> GetMalnutrition()
        {
            return Entity.Malnutritions.ToList();
        }
        public List<EQ5DMobility> GetEQ5DMobility()
        {
            return Entity.EQ5DMobility.ToList();
        }
        public List<EQ5DSelfCare> GetEQ5DSelfCare()
        {
            return Entity.EQ5DSelfCare.ToList();
        }
        public List<EQ5DUsualActivity> GetEQ5DUsualActivity()
        {
            return Entity.EQ5DUsualActivity.ToList();
        }
        public List<EQ5DPain> GetEQ5DPain()
        {
            return Entity.EQ5DPain.ToList();
        }
        public List<EQ5DAnxiety> GetEQ5DAnxiety()
        {
            return Entity.EQ5DAnxiety.ToList();
        }
    }
}