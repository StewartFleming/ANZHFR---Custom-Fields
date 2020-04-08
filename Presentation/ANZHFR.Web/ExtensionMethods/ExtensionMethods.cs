using ANZHFR.Data.Models;
using ANZHFR.Services.Synonyms;
using ANZHFR.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace ANZHFR.Web.ExtensionMethods
{
    public enum AccessLevel
    {
        User = 1,
        HospitalAdministrator = 2,
        StateAdministrator = 3,
        SystemAdministrator = 4,
        FullAccess = 5
    }

    public static class ExtensionMethods
    {
        public static double? Median<TColl, TValue>(this IEnumerable<TColl> source, Func<TColl, TValue> selector)
        {
            return source.Select<TColl, TValue>(selector).Median();
        }

        public static double? Median<T>(this IEnumerable<T> source)
        {
            if (Nullable.GetUnderlyingType(typeof(T)) != null)
                source = source.Where(x => x != null);

            int count = source.Count();
            if (count == 0)
                return null;

            source = source.OrderBy(n => n);

            int midpoint = count / 2;
            if (count % 2 == 0)
                return (Convert.ToDouble(source.ElementAt(midpoint - 1)) + Convert.ToDouble(source.ElementAt(midpoint))) / 2.0;
            else
                return Convert.ToDouble(source.ElementAt(midpoint));
        }

        public static HospitalModel GetHospitalModel(this Hospital hospital)
        {
            HospitalModel model = new HospitalModel();

            model.HospitalID = hospital.HospitalID;
            model.Name = hospital.HName;
            model.StreetAddress1 = hospital.HStreetAddress1;
            model.StreetAddress2 = hospital.HStreetAddress2;
            model.Suburb = hospital.HSuburb;
            model.City = hospital.HCity;
            model.State = hospital.HState;
            model.PostCode = hospital.HPostCode;
            model.Country = hospital.HCountry;
            model.Phone = hospital.HPhone;
            model.AdminEmail = hospital.HAdminEmail;

            return model;
        }

        public static ContentModel GetContentModel(this Content content)
        {
            ContentModel model = new ContentModel();

            model.Content_ID = content.Content_ID;
            model.Content_Key = content.Content_Key;
            model.Content_HTML = content.Content_HTML;
            model.Content_Text = content.Content_Text;

            return model;
        }

        public static TransferHospitalModel GetTransferHospitalModel(this TransferHospital hospital)
        {
            TransferHospitalModel model = new TransferHospitalModel();

            model.TransferHospitalID = hospital.TransferHospitalID;
            model.HospitalID = hospital.HospitalID;
            model.Name = hospital.Name;
            model.StreetAddress = hospital.StreetAddress;
            model.StreetAddress2 = hospital.StreetAddress2;
            model.Suburb = hospital.Suburb;
            model.City = hospital.City;
            model.State = hospital.State;
            model.PostCode = hospital.PostCode;
            model.Country = hospital.Country;
            model.Phone = hospital.Phone;
            model.AdminEmail = hospital.AdminEmail;

            return model;
        }

        public static SynonymModel GetSynonymModel(this Synonym synonym)
        {
            SynonymModel model = new SynonymModel();

            model.Id = synonym.Id;
            model.Word = synonym.Word;

            return model;
        }

        public static SynonymChildModel GetSynonymChildModel(this SynonymChild synonymChild)
        {
            SynonymChildModel model = new SynonymChildModel();

            model.Id = synonymChild.Id;
            model.SynonymId = synonymChild.SynonymID;
            model.Word = synonymChild.Word;

            return model;
        }

        public static List<ImportPatientModel> GetPatientModel(this List<List<Dictionary<string, string>>> table, string fileFormat)
        {
            List<ImportPatientModel> convertedList = new List<ImportPatientModel>();
            try
            {
                if (ConfigurationManager.AppSettings["Location"] == "Australian")
                {
                    if (fileFormat == "2017")
                    {
                        convertedList = table.Select(x => new ImportPatientModel
                        {
                            Name = x.ElementAt(0).Values.ElementAt<string>(0),
                            Surname = x.ElementAt(1).Values.ElementAt<string>(0),
                            MRN = x.ElementAt(2).Values.ElementAt<string>(0),
                            PostCode = x.ElementAt(3).Values.ElementAt<string>(0),
                            Phone = x.ElementAt(4).Values.ElementAt<string>(0),
                            DOB = x.ElementAt(5).Values.ElementAt<string>(0),
                            Sex = x.ElementAt(6).Values.ElementAt<string>(0),
                            Indig = x.ElementAt(7).Values.ElementAt<string>(0),
                            Medicare = x.ElementAt(9).Values.ElementAt<string>(0),
                            PatientType = x.ElementAt(10).Values.ElementAt<string>(0),
                            UResidence = x.ElementAt(11).Values.ElementAt<string>(0),
                            AdmissionViaED = x.ElementAt(12).Values.ElementAt<string>(0),
                            TransferHospital = x.ElementAt(13).Values.ElementAt<string>(0),
                            TransferDate = x.ElementAt(14).Values.ElementAt<string>(0),
                            TransferTime = x.ElementAt(15).Values.ElementAt<string>(0),
                            ArrivalDate = x.ElementAt(16).Values.ElementAt<string>(0),
                            ArrivalTime = x.ElementAt(17).Values.ElementAt<string>(0),
                            DepartureDate = x.ElementAt(18).Values.ElementAt<string>(0),
                            DepartureTime = x.ElementAt(19).Values.ElementAt<string>(0),
                            InHospFractureDate = x.ElementAt(20).Values.ElementAt<string>(0),
                            InHospFractureTime = x.ElementAt(21).Values.ElementAt<string>(0),
                            WardType = x.ElementAt(22).Values.ElementAt<string>(0),
                            PreAdWalk = x.ElementAt(23).Values.ElementAt<string>(0),
                            PainAssessment = x.ElementAt(24).Values.ElementAt<string>(0),
                            PainManagement = x.ElementAt(25).Values.ElementAt<string>(0),
                            CognitiveAssessment = x.ElementAt(26).Values.ElementAt<string>(0),
                            CognitiveState = x.ElementAt(27).Values.ElementAt<string>(0),
                            BoneMed = x.ElementAt(28).Values.ElementAt<string>(0),
                            PreOpMedAss = x.ElementAt(29).Values.ElementAt<string>(0),
                            FractureSide = x.ElementAt(30).Values.ElementAt<string>(0),
                            AtypicalFracture = x.ElementAt(31).Values.ElementAt<string>(0),
                            FractureType = x.ElementAt(32).Values.ElementAt<string>(0),
                            ASAgrade = x.ElementAt(33).Values.ElementAt<string>(0),
                            Surgery = x.ElementAt(34).Values.ElementAt<string>(0),
                            SurgeryDate = x.ElementAt(35).Values.ElementAt<string>(0),
                            SurgeryTime = x.ElementAt(36).Values.ElementAt<string>(0),
                            SurgeryDelay = x.ElementAt(37).Values.ElementAt<string>(0),
                            SurgeryDelayOther = x.ElementAt(38).Values.ElementAt<string>(0),
                            Anaesthesia = x.ElementAt(39).Values.ElementAt<string>(0),
                            Analgesia = x.ElementAt(40).Values.ElementAt<string>(0),
                            ConsultantPresent = x.ElementAt(41).Values.ElementAt<string>(0),
                            Operation = x.ElementAt(42).Values.ElementAt<string>(0),
                            InterOpFracture = x.ElementAt(43).Values.ElementAt<string>(0),
                            FullWeightBear = x.ElementAt(44).Values.ElementAt<string>(0),
                            Mobilisation = x.ElementAt(45).Values.ElementAt<string>(0),
                            PressureUlcers = x.ElementAt(46).Values.ElementAt<string>(0),
                            GeriatricAssessment = x.ElementAt(47).Values.ElementAt<string>(0),
                            GeriatricAssDate = x.ElementAt(48).Values.ElementAt<string>(0),
                            FallsAssessment = x.ElementAt(49).Values.ElementAt<string>(0),
                            BoneMedDischarge = x.ElementAt(50).Values.ElementAt<string>(0),
                            WardDischargeDate = x.ElementAt(51).Values.ElementAt<string>(0),
                            DischargeDest = x.ElementAt(52).Values.ElementAt<string>(0),
                            HospitalDischargeDate = x.ElementAt(53).Values.ElementAt<string>(0),
                            DischargeResidence = x.ElementAt(54).Values.ElementAt<string>(0),
                            FollowupDate30 = x.ElementAt(57).Values.ElementAt<string>(0),
                            HealthServiceDischarge30 = x.ElementAt(58).Values.ElementAt<string>(0),
                            Survival30 = x.ElementAt(59).Values.ElementAt<string>(0),
                            Residence30 = x.ElementAt(60).Values.ElementAt<string>(0),
                            WeightBear30 = x.ElementAt(61).Values.ElementAt<string>(0),
                            WalkingAbility30 = x.ElementAt(62).Values.ElementAt<string>(0),
                            BoneMed30 = x.ElementAt(63).Values.ElementAt<string>(0),
                            Reoperation30 = x.ElementAt(64).Values.ElementAt<string>(0),
                            FollowupDate120 = x.ElementAt(65).Values.ElementAt<string>(0),
                            HealthServiceDischarge120 = x.ElementAt(66).Values.ElementAt<string>(0),
                            Survival120 = x.ElementAt(67).Values.ElementAt<string>(0),
                            Residence120 = x.ElementAt(68).Values.ElementAt<string>(0),
                            WeightBear120 = x.ElementAt(69).Values.ElementAt<string>(0),
                            WalkingAbility120 = x.ElementAt(70).Values.ElementAt<string>(0),
                            BoneMed120 = x.ElementAt(71).Values.ElementAt<string>(0),
                            Reoperation120 = x.ElementAt(72).Values.ElementAt<string>(0)
                        }).ToList();
                    }
                    else if (fileFormat == "2018")
                    {
                        convertedList = table.Select(x => new ImportPatientModel
                        {
                            Name = x.ElementAt(0).Values.ElementAt<string>(0),
                            Surname = x.ElementAt(1).Values.ElementAt<string>(0),
                            MRN = x.ElementAt(2).Values.ElementAt<string>(0),
                            PostCode = x.ElementAt(3).Values.ElementAt<string>(0),
                            Phone = x.ElementAt(4).Values.ElementAt<string>(0),
                            DOB = x.ElementAt(5).Values.ElementAt<string>(0),
                            Sex = x.ElementAt(6).Values.ElementAt<string>(0),
                            Indig = x.ElementAt(7).Values.ElementAt<string>(0),
                            Medicare = x.ElementAt(9).Values.ElementAt<string>(0),
                            PatientType = x.ElementAt(10).Values.ElementAt<string>(0),
                            UResidence = x.ElementAt(11).Values.ElementAt<string>(0),
                            AdmissionViaED = x.ElementAt(12).Values.ElementAt<string>(0),
                            TransferHospital = x.ElementAt(13).Values.ElementAt<string>(0),
                            TransferDate = x.ElementAt(14).Values.ElementAt<string>(0),
                            TransferTime = x.ElementAt(15).Values.ElementAt<string>(0),
                            ArrivalDate = x.ElementAt(16).Values.ElementAt<string>(0),
                            ArrivalTime = x.ElementAt(17).Values.ElementAt<string>(0),
                            DepartureDate = x.ElementAt(18).Values.ElementAt<string>(0),
                            DepartureTime = x.ElementAt(19).Values.ElementAt<string>(0),
                            InHospFractureDate = x.ElementAt(20).Values.ElementAt<string>(0),
                            InHospFractureTime = x.ElementAt(21).Values.ElementAt<string>(0),
                            WardType = x.ElementAt(22).Values.ElementAt<string>(0),
                            PreAdWalk = x.ElementAt(23).Values.ElementAt<string>(0),
                            PainAssessment = x.ElementAt(24).Values.ElementAt<string>(0),
                            PainManagement = x.ElementAt(25).Values.ElementAt<string>(0),
                            CognitiveAssessment = x.ElementAt(26).Values.ElementAt<string>(0),
                            CognitiveState = x.ElementAt(27).Values.ElementAt<string>(0),
                            BoneMed = x.ElementAt(28).Values.ElementAt<string>(0),
                            PreOpMedAss = x.ElementAt(29).Values.ElementAt<string>(0),
                            FractureSide = x.ElementAt(30).Values.ElementAt<string>(0),
                            AtypicalFracture = x.ElementAt(31).Values.ElementAt<string>(0),
                            FractureType = x.ElementAt(32).Values.ElementAt<string>(0),
                            ASAgrade = x.ElementAt(33).Values.ElementAt<string>(0),
                            Surgery = x.ElementAt(34).Values.ElementAt<string>(0),
                            SurgeryDate = x.ElementAt(35).Values.ElementAt<string>(0),
                            SurgeryTime = x.ElementAt(36).Values.ElementAt<string>(0),
                            SurgeryDelay = x.ElementAt(37).Values.ElementAt<string>(0),
                            SurgeryDelayOther = x.ElementAt(38).Values.ElementAt<string>(0),
                            Anaesthesia = x.ElementAt(39).Values.ElementAt<string>(0),
                            Analgesia = x.ElementAt(40).Values.ElementAt<string>(0),
                            ConsultantPresent = x.ElementAt(41).Values.ElementAt<string>(0),
                            Operation = x.ElementAt(42).Values.ElementAt<string>(0),
                            FullWeightBear = x.ElementAt(43).Values.ElementAt<string>(0),
                            Mobilisation = x.ElementAt(44).Values.ElementAt<string>(0),
                            DeleriumAssessment = x.ElementAt(45).Values.ElementAt<string>(0),
                            PressureUlcers = x.ElementAt(46).Values.ElementAt<string>(0),
                            GeriatricAssessment = x.ElementAt(47).Values.ElementAt<string>(0),
                            GeriatricAssDate = x.ElementAt(48).Values.ElementAt<string>(0),
                            FallsAssessment = x.ElementAt(49).Values.ElementAt<string>(0),
                            BoneMedDischarge = x.ElementAt(50).Values.ElementAt<string>(0),
                            WardDischargeDate = x.ElementAt(51).Values.ElementAt<string>(0),
                            DischargeDest = x.ElementAt(52).Values.ElementAt<string>(0),
                            HospitalDischargeDate = x.ElementAt(53).Values.ElementAt<string>(0),
                            DischargeResidence = x.ElementAt(54).Values.ElementAt<string>(0),
                            FollowupDate30 = x.ElementAt(57).Values.ElementAt<string>(0),
                            HealthServiceDischarge30 = x.ElementAt(58).Values.ElementAt<string>(0),
                            Survival30 = x.ElementAt(59).Values.ElementAt<string>(0),
                            Residence30 = x.ElementAt(60).Values.ElementAt<string>(0),
                            WeightBear30 = x.ElementAt(61).Values.ElementAt<string>(0),
                            WalkingAbility30 = x.ElementAt(62).Values.ElementAt<string>(0),
                            BoneMed30 = x.ElementAt(63).Values.ElementAt<string>(0),
                            Reoperation30 = x.ElementAt(64).Values.ElementAt<string>(0),
                            FollowupDate120 = x.ElementAt(65).Values.ElementAt<string>(0),
                            HealthServiceDischarge120 = x.ElementAt(66).Values.ElementAt<string>(0),
                            Survival120 = x.ElementAt(67).Values.ElementAt<string>(0),
                            Residence120 = x.ElementAt(68).Values.ElementAt<string>(0),
                            WeightBear120 = x.ElementAt(69).Values.ElementAt<string>(0),
                            WalkingAbility120 = x.ElementAt(70).Values.ElementAt<string>(0),
                            BoneMed120 = x.ElementAt(71).Values.ElementAt<string>(0),
                            Reoperation120 = x.ElementAt(72).Values.ElementAt<string>(0),
                            Informed = x.ElementAt(73).Values.ElementAt<string>(0).ToLower() == "true" ? true : false,
                            CannotFollowup = x.ElementAt(74).Values.ElementAt<string>(0).ToLower() == "true" ? true : false
                        }).ToList();
                    }
                    else if (fileFormat == "2019")// Assume 2019
                    {
                        convertedList = table.Select(x => new ImportPatientModel
                        {
                            Name = x.ElementAt(0).Values.ElementAt<string>(0),
                            Surname = x.ElementAt(1).Values.ElementAt<string>(0),
                            MRN = x.ElementAt(2).Values.ElementAt<string>(0),
                            PostCode = x.ElementAt(3).Values.ElementAt<string>(0),
                            Phone = x.ElementAt(4).Values.ElementAt<string>(0),
                            DOB = x.ElementAt(5).Values.ElementAt<string>(0),
                            Sex = x.ElementAt(6).Values.ElementAt<string>(0),
                            Indig = x.ElementAt(7).Values.ElementAt<string>(0),
                            Medicare = x.ElementAt(9).Values.ElementAt<string>(0),
                            PatientType = x.ElementAt(10).Values.ElementAt<string>(0),
                            UResidence = x.ElementAt(11).Values.ElementAt<string>(0),
                            AdmissionViaED = x.ElementAt(12).Values.ElementAt<string>(0),
                            TransferHospital = x.ElementAt(13).Values.ElementAt<string>(0),
                            TransferDate = x.ElementAt(14).Values.ElementAt<string>(0),
                            TransferTime = x.ElementAt(15).Values.ElementAt<string>(0),
                            ArrivalDate = x.ElementAt(16).Values.ElementAt<string>(0),
                            ArrivalTime = x.ElementAt(17).Values.ElementAt<string>(0),
                            DepartureDate = x.ElementAt(18).Values.ElementAt<string>(0),
                            DepartureTime = x.ElementAt(19).Values.ElementAt<string>(0),
                            InHospFractureDate = x.ElementAt(20).Values.ElementAt<string>(0),
                            InHospFractureTime = x.ElementAt(21).Values.ElementAt<string>(0),
                            WardType = x.ElementAt(22).Values.ElementAt<string>(0),
                            PreAdWalk = x.ElementAt(23).Values.ElementAt<string>(0),
                            PainAssessment = x.ElementAt(24).Values.ElementAt<string>(0),
                            PainManagement = x.ElementAt(25).Values.ElementAt<string>(0),
                            CognitiveAssessment = x.ElementAt(26).Values.ElementAt<string>(0),
                            CognitiveState = x.ElementAt(27).Values.ElementAt<string>(0),
                            BoneMed = x.ElementAt(28).Values.ElementAt<string>(0),
                            PreOpMedAss = x.ElementAt(29).Values.ElementAt<string>(0),
                            FractureSide = x.ElementAt(30).Values.ElementAt<string>(0),
                            AtypicalFracture = x.ElementAt(31).Values.ElementAt<string>(0),
                            FractureType = x.ElementAt(32).Values.ElementAt<string>(0),
                            ASAgrade = x.ElementAt(33).Values.ElementAt<string>(0),
                            Surgery = x.ElementAt(34).Values.ElementAt<string>(0),
                            SurgeryDate = x.ElementAt(35).Values.ElementAt<string>(0),
                            SurgeryTime = x.ElementAt(36).Values.ElementAt<string>(0),
                            SurgeryDelay = x.ElementAt(37).Values.ElementAt<string>(0),
                            SurgeryDelayOther = x.ElementAt(38).Values.ElementAt<string>(0),
                            Anaesthesia = x.ElementAt(39).Values.ElementAt<string>(0),
                            Analgesia = x.ElementAt(40).Values.ElementAt<string>(0),
                            ConsultantPresent = x.ElementAt(41).Values.ElementAt<string>(0),
                            Operation = x.ElementAt(42).Values.ElementAt<string>(0),
                            FullWeightBear = x.ElementAt(43).Values.ElementAt<string>(0),
                            Mobilisation = x.ElementAt(44).Values.ElementAt<string>(0),
                            DeleriumAssessment = x.ElementAt(45).Values.ElementAt<string>(0),
                            PressureUlcers = x.ElementAt(46).Values.ElementAt<string>(0),
                            Malnutrition = x.ElementAt(47).Values.ElementAt<string>(0),
                            GeriatricAssessment = x.ElementAt(48).Values.ElementAt<string>(0),
                            GeriatricAssDate = x.ElementAt(49).Values.ElementAt<string>(0),
                            FallsAssessment = x.ElementAt(50).Values.ElementAt<string>(0),
                            BoneMedDischarge = x.ElementAt(51).Values.ElementAt<string>(0),
                            WardDischargeDate = x.ElementAt(52).Values.ElementAt<string>(0),
                            DischargeDest = x.ElementAt(53).Values.ElementAt<string>(0),
                            HospitalDischargeDate = x.ElementAt(54).Values.ElementAt<string>(0),
                            DischargeResidence = x.ElementAt(55).Values.ElementAt<string>(0),
                            FollowupDate120 = x.ElementAt(58).Values.ElementAt<string>(0),
                            HealthServiceDischarge120 = x.ElementAt(59).Values.ElementAt<string>(0),
                            Survival120 = x.ElementAt(60).Values.ElementAt<string>(0),
                            Residence120 = x.ElementAt(61).Values.ElementAt<string>(0),
                            WeightBear120 = x.ElementAt(62).Values.ElementAt<string>(0),
                            WalkingAbility120 = x.ElementAt(63).Values.ElementAt<string>(0),
                            BoneMed120 = x.ElementAt(64).Values.ElementAt<string>(0),
                            Reoperation120 = x.ElementAt(65).Values.ElementAt<string>(0),
                            Informed = x.ElementAt(66).Values.ElementAt<string>(0).ToLower() == "true" ? true : false,
                            CannotFollowup = x.ElementAt(67).Values.ElementAt<string>(0).ToLower() == "true" ? true : false
                        }).ToList();
                    }
                    else // Assume 2020
                    {
                        convertedList = table.Select(x => new ImportPatientModel
                        {
                            Name = x.ElementAt(0).Values.ElementAt<string>(0),
                            Surname = x.ElementAt(1).Values.ElementAt<string>(0),
                            MRN = x.ElementAt(2).Values.ElementAt<string>(0),
                            PostCode = x.ElementAt(3).Values.ElementAt<string>(0),
                            Phone = x.ElementAt(4).Values.ElementAt<string>(0),
                            DOB = x.ElementAt(5).Values.ElementAt<string>(0),
                            Sex = x.ElementAt(6).Values.ElementAt<string>(0),
                            Indig = x.ElementAt(7).Values.ElementAt<string>(0),
                            Medicare = x.ElementAt(9).Values.ElementAt<string>(0),
                            PatientType = x.ElementAt(10).Values.ElementAt<string>(0),
                            UResidence = x.ElementAt(11).Values.ElementAt<string>(0),
                            AdmissionViaED = x.ElementAt(12).Values.ElementAt<string>(0),
                            TransferHospital = x.ElementAt(13).Values.ElementAt<string>(0),
                            TransferDate = x.ElementAt(14).Values.ElementAt<string>(0),
                            TransferTime = x.ElementAt(15).Values.ElementAt<string>(0),
                            ArrivalDate = x.ElementAt(16).Values.ElementAt<string>(0),
                            ArrivalTime = x.ElementAt(17).Values.ElementAt<string>(0),
                            DepartureDate = x.ElementAt(18).Values.ElementAt<string>(0),
                            DepartureTime = x.ElementAt(19).Values.ElementAt<string>(0),
                            InHospFractureDate = x.ElementAt(20).Values.ElementAt<string>(0),
                            InHospFractureTime = x.ElementAt(21).Values.ElementAt<string>(0),
                            WardType = x.ElementAt(22).Values.ElementAt<string>(0),
                            PreAdWalk = x.ElementAt(23).Values.ElementAt<string>(0),
                            PainAssessment = x.ElementAt(24).Values.ElementAt<string>(0),
                            PainManagement = x.ElementAt(25).Values.ElementAt<string>(0),
                            CognitiveAssessment = x.ElementAt(26).Values.ElementAt<string>(0),
                            CognitiveState = x.ElementAt(27).Values.ElementAt<string>(0),
                            BoneMed = x.ElementAt(28).Values.ElementAt<string>(0),
                            PreOpMedAss = x.ElementAt(29).Values.ElementAt<string>(0),
                            FractureSide = x.ElementAt(30).Values.ElementAt<string>(0),
                            AtypicalFracture = x.ElementAt(31).Values.ElementAt<string>(0),
                            FractureType = x.ElementAt(32).Values.ElementAt<string>(0),
                            ASAgrade = x.ElementAt(33).Values.ElementAt<string>(0),
                            Surgery = x.ElementAt(34).Values.ElementAt<string>(0),
                            SurgeryDate = x.ElementAt(35).Values.ElementAt<string>(0),
                            SurgeryTime = x.ElementAt(36).Values.ElementAt<string>(0),
                            SurgeryDelay = x.ElementAt(37).Values.ElementAt<string>(0),
                            SurgeryDelayOther = x.ElementAt(38).Values.ElementAt<string>(0),
                            Anaesthesia = x.ElementAt(39).Values.ElementAt<string>(0),
                            Analgesia = x.ElementAt(40).Values.ElementAt<string>(0),
                            ConsultantPresent = x.ElementAt(41).Values.ElementAt<string>(0),
                            Operation = x.ElementAt(42).Values.ElementAt<string>(0),
                            FullWeightBear = x.ElementAt(43).Values.ElementAt<string>(0),
                            Mobilisation = x.ElementAt(44).Values.ElementAt<string>(0),
                            FirstDayWalking = x.ElementAt(45).Values.ElementAt<string>(0),
                            DeleriumAssessment = x.ElementAt(46).Values.ElementAt<string>(0),
                            PressureUlcers = x.ElementAt(47).Values.ElementAt<string>(0),
                            Malnutrition = x.ElementAt(48).Values.ElementAt<string>(0),
                            GeriatricAssessment = x.ElementAt(49).Values.ElementAt<string>(0),
                            GeriatricAssDate = x.ElementAt(50).Values.ElementAt<string>(0),
                            FallsAssessment = x.ElementAt(51).Values.ElementAt<string>(0),
                            BoneMedDischarge = x.ElementAt(52).Values.ElementAt<string>(0),
                            WardDischargeDate = x.ElementAt(53).Values.ElementAt<string>(0),
                            DischargeDest = x.ElementAt(54).Values.ElementAt<string>(0),
                            HospitalDischargeDate = x.ElementAt(55).Values.ElementAt<string>(0),
                            DischargeResidence = x.ElementAt(56).Values.ElementAt<string>(0),
                            FollowupDate120 = x.ElementAt(59).Values.ElementAt<string>(0),
                            HealthServiceDischarge120 = x.ElementAt(60).Values.ElementAt<string>(0),
                            Survival120 = x.ElementAt(61).Values.ElementAt<string>(0),
                            Residence120 = x.ElementAt(62).Values.ElementAt<string>(0),
                            WalkingAbility120 = x.ElementAt(63).Values.ElementAt<string>(0),
                            BoneMed120 = x.ElementAt(64).Values.ElementAt<string>(0),
                            Reoperation120 = x.ElementAt(65).Values.ElementAt<string>(0),
                            DeathDate = x.ElementAt(66).Values.ElementAt<string>(0),
                            Informed = x.ElementAt(67).Values.ElementAt<string>(0).ToLower() == "true" ? true : false,
                            CannotFollowup = x.ElementAt(68).Values.ElementAt<string>(0).ToLower() == "true" ? true : false
                        }).ToList();
                    }
                }
                else  // Assume New Zealand
                {
                    if (fileFormat == "2017")
                    {
                        convertedList = table.Select(x => new ImportPatientModel
                        {
                            Name = x.ElementAt(0).Values.ElementAt<string>(0),
                            Surname = x.ElementAt(1).Values.ElementAt<string>(0),
                            MRN = x.ElementAt(2).Values.ElementAt<string>(0),
                            PostCode = x.ElementAt(3).Values.ElementAt<string>(0),
                            Phone = x.ElementAt(4).Values.ElementAt<string>(0),
                            DOB = x.ElementAt(5).Values.ElementAt<string>(0),
                            Sex = x.ElementAt(6).Values.ElementAt<string>(0),
                            Ethnic = x.ElementAt(7).Values.ElementAt<string>(0),
                            Medicare = x.ElementAt(9).Values.ElementAt<string>(0),
                            UResidence = x.ElementAt(10).Values.ElementAt<string>(0),
                            AdmissionViaED = x.ElementAt(11).Values.ElementAt<string>(0),
                            TransferHospital = x.ElementAt(12).Values.ElementAt<string>(0),
                            TransferDate = x.ElementAt(13).Values.ElementAt<string>(0),
                            TransferTime = x.ElementAt(14).Values.ElementAt<string>(0),
                            ArrivalDate = x.ElementAt(15).Values.ElementAt<string>(0),
                            ArrivalTime = x.ElementAt(16).Values.ElementAt<string>(0),
                            DepartureDate = x.ElementAt(17).Values.ElementAt<string>(0),
                            DepartureTime = x.ElementAt(18).Values.ElementAt<string>(0),
                            InHospFractureDate = x.ElementAt(19).Values.ElementAt<string>(0),
                            InHospFractureTime = x.ElementAt(20).Values.ElementAt<string>(0),
                            WardType = x.ElementAt(21).Values.ElementAt<string>(0),
                            PreAdWalk = x.ElementAt(22).Values.ElementAt<string>(0),
                            PainAssessment = x.ElementAt(23).Values.ElementAt<string>(0),
                            PainManagement = x.ElementAt(24).Values.ElementAt<string>(0),
                            CognitiveAssessment = x.ElementAt(25).Values.ElementAt<string>(0),
                            CognitiveState = x.ElementAt(26).Values.ElementAt<string>(0),
                            BoneMed = x.ElementAt(27).Values.ElementAt<string>(0),
                            PreOpMedAss = x.ElementAt(28).Values.ElementAt<string>(0),
                            FractureSide = x.ElementAt(29).Values.ElementAt<string>(0),
                            AtypicalFracture = x.ElementAt(30).Values.ElementAt<string>(0),
                            FractureType = x.ElementAt(31).Values.ElementAt<string>(0),
                            ASAgrade = x.ElementAt(32).Values.ElementAt<string>(0),
                            Surgery = x.ElementAt(33).Values.ElementAt<string>(0),
                            SurgeryDate = x.ElementAt(34).Values.ElementAt<string>(0),
                            SurgeryTime = x.ElementAt(35).Values.ElementAt<string>(0),
                            SurgeryDelay = x.ElementAt(36).Values.ElementAt<string>(0),
                            SurgeryDelayOther = x.ElementAt(37).Values.ElementAt<string>(0),
                            Anaesthesia = x.ElementAt(38).Values.ElementAt<string>(0),
                            Analgesia = x.ElementAt(39).Values.ElementAt<string>(0),
                            ConsultantPresent = x.ElementAt(40).Values.ElementAt<string>(0),
                            Operation = x.ElementAt(41).Values.ElementAt<string>(0),
                            InterOpFracture = x.ElementAt(42).Values.ElementAt<string>(0),
                            FullWeightBear = x.ElementAt(43).Values.ElementAt<string>(0),
                            Mobilisation = x.ElementAt(44).Values.ElementAt<string>(0),
                            PressureUlcers = x.ElementAt(45).Values.ElementAt<string>(0),
                            GeriatricAssessment = x.ElementAt(46).Values.ElementAt<string>(0),
                            GeriatricAssDate = x.ElementAt(47).Values.ElementAt<string>(0),
                            FallsAssessment = x.ElementAt(48).Values.ElementAt<string>(0),
                            BoneMedDischarge = x.ElementAt(49).Values.ElementAt<string>(0),
                            WardDischargeDate = x.ElementAt(50).Values.ElementAt<string>(0),
                            DischargeDest = x.ElementAt(51).Values.ElementAt<string>(0),
                            HospitalDischargeDate = x.ElementAt(52).Values.ElementAt<string>(0),
                            DischargeResidence = x.ElementAt(53).Values.ElementAt<string>(0),
                            FollowupDate30 = x.ElementAt(56).Values.ElementAt<string>(0),
                            HealthServiceDischarge30 = x.ElementAt(57).Values.ElementAt<string>(0),
                            Survival30 = x.ElementAt(58).Values.ElementAt<string>(0),
                            Residence30 = x.ElementAt(59).Values.ElementAt<string>(0),
                            WeightBear30 = x.ElementAt(60).Values.ElementAt<string>(0),
                            WalkingAbility30 = x.ElementAt(61).Values.ElementAt<string>(0),
                            BoneMed30 = x.ElementAt(62).Values.ElementAt<string>(0),
                            Reoperation30 = x.ElementAt(63).Values.ElementAt<string>(0),
                            FollowupDate120 = x.ElementAt(64).Values.ElementAt<string>(0),
                            HealthServiceDischarge120 = x.ElementAt(65).Values.ElementAt<string>(0),
                            Survival120 = x.ElementAt(66).Values.ElementAt<string>(0),
                            Residence120 = x.ElementAt(67).Values.ElementAt<string>(0),
                            WeightBear120 = x.ElementAt(68).Values.ElementAt<string>(0),
                            WalkingAbility120 = x.ElementAt(69).Values.ElementAt<string>(0),
                            BoneMed120 = x.ElementAt(70).Values.ElementAt<string>(0),
                            Reoperation120 = x.ElementAt(71).Values.ElementAt<string>(0)
                        }).ToList();
                    }
                    else if (fileFormat == "2018") // New Zealand 2018
                    {
                        convertedList = table.Select(x => new ImportPatientModel
                        {
                            Name = x.ElementAt(0).Values.ElementAt<string>(0),
                            Surname = x.ElementAt(1).Values.ElementAt<string>(0),
                            MRN = x.ElementAt(2).Values.ElementAt<string>(0),
                            PostCode = x.ElementAt(3).Values.ElementAt<string>(0),
                            Phone = x.ElementAt(4).Values.ElementAt<string>(0),
                            DOB = x.ElementAt(5).Values.ElementAt<string>(0),
                            Sex = x.ElementAt(6).Values.ElementAt<string>(0),
                            Ethnic = x.ElementAt(7).Values.ElementAt<string>(0),
                            Medicare = x.ElementAt(9).Values.ElementAt<string>(0),
                            UResidence = x.ElementAt(10).Values.ElementAt<string>(0),
                            AdmissionViaED = x.ElementAt(11).Values.ElementAt<string>(0),
                            TransferHospital = x.ElementAt(12).Values.ElementAt<string>(0),
                            TransferDate = x.ElementAt(13).Values.ElementAt<string>(0),
                            TransferTime = x.ElementAt(14).Values.ElementAt<string>(0),
                            ArrivalDate = x.ElementAt(15).Values.ElementAt<string>(0),
                            ArrivalTime = x.ElementAt(16).Values.ElementAt<string>(0),
                            DepartureDate = x.ElementAt(17).Values.ElementAt<string>(0),
                            DepartureTime = x.ElementAt(18).Values.ElementAt<string>(0),
                            InHospFractureDate = x.ElementAt(19).Values.ElementAt<string>(0),
                            InHospFractureTime = x.ElementAt(20).Values.ElementAt<string>(0),
                            WardType = x.ElementAt(21).Values.ElementAt<string>(0),
                            PreAdWalk = x.ElementAt(22).Values.ElementAt<string>(0),
                            PainAssessment = x.ElementAt(23).Values.ElementAt<string>(0),
                            PainManagement = x.ElementAt(24).Values.ElementAt<string>(0),
                            CognitiveAssessment = x.ElementAt(25).Values.ElementAt<string>(0),
                            CognitiveState = x.ElementAt(26).Values.ElementAt<string>(0),
                            BoneMed = x.ElementAt(27).Values.ElementAt<string>(0),
                            PreOpMedAss = x.ElementAt(28).Values.ElementAt<string>(0),
                            FractureSide = x.ElementAt(29).Values.ElementAt<string>(0),
                            AtypicalFracture = x.ElementAt(30).Values.ElementAt<string>(0),
                            FractureType = x.ElementAt(31).Values.ElementAt<string>(0),
                            ASAgrade = x.ElementAt(32).Values.ElementAt<string>(0),
                            Surgery = x.ElementAt(33).Values.ElementAt<string>(0),
                            SurgeryDate = x.ElementAt(34).Values.ElementAt<string>(0),
                            SurgeryTime = x.ElementAt(35).Values.ElementAt<string>(0),
                            SurgeryDelay = x.ElementAt(36).Values.ElementAt<string>(0),
                            SurgeryDelayOther = x.ElementAt(37).Values.ElementAt<string>(0),
                            Anaesthesia = x.ElementAt(38).Values.ElementAt<string>(0),
                            Analgesia = x.ElementAt(39).Values.ElementAt<string>(0),
                            ConsultantPresent = x.ElementAt(40).Values.ElementAt<string>(0),
                            Operation = x.ElementAt(41).Values.ElementAt<string>(0),
                            FullWeightBear = x.ElementAt(42).Values.ElementAt<string>(0),
                            Mobilisation = x.ElementAt(43).Values.ElementAt<string>(0),
                            DeleriumAssessment = x.ElementAt(44).Values.ElementAt<string>(0),
                            PressureUlcers = x.ElementAt(45).Values.ElementAt<string>(0),
                            GeriatricAssessment = x.ElementAt(46).Values.ElementAt<string>(0),
                            GeriatricAssDate = x.ElementAt(47).Values.ElementAt<string>(0),
                            FallsAssessment = x.ElementAt(48).Values.ElementAt<string>(0),
                            BoneMedDischarge = x.ElementAt(49).Values.ElementAt<string>(0),
                            WardDischargeDate = x.ElementAt(50).Values.ElementAt<string>(0),
                            DischargeDest = x.ElementAt(51).Values.ElementAt<string>(0),
                            HospitalDischargeDate = x.ElementAt(52).Values.ElementAt<string>(0),
                            DischargeResidence = x.ElementAt(53).Values.ElementAt<string>(0),
                            FollowupDate30 = x.ElementAt(56).Values.ElementAt<string>(0),
                            HealthServiceDischarge30 = x.ElementAt(57).Values.ElementAt<string>(0),
                            Survival30 = x.ElementAt(58).Values.ElementAt<string>(0),
                            Residence30 = x.ElementAt(59).Values.ElementAt<string>(0),
                            WeightBear30 = x.ElementAt(60).Values.ElementAt<string>(0),
                            WalkingAbility30 = x.ElementAt(61).Values.ElementAt<string>(0),
                            BoneMed30 = x.ElementAt(62).Values.ElementAt<string>(0),
                            Reoperation30 = x.ElementAt(63).Values.ElementAt<string>(0),
                            FollowupDate120 = x.ElementAt(64).Values.ElementAt<string>(0),
                            HealthServiceDischarge120 = x.ElementAt(65).Values.ElementAt<string>(0),
                            Survival120 = x.ElementAt(66).Values.ElementAt<string>(0),
                            Residence120 = x.ElementAt(67).Values.ElementAt<string>(0),
                            WeightBear120 = x.ElementAt(68).Values.ElementAt<string>(0),
                            WalkingAbility120 = x.ElementAt(69).Values.ElementAt<string>(0),
                            BoneMed120 = x.ElementAt(70).Values.ElementAt<string>(0),
                            Reoperation120 = x.ElementAt(71).Values.ElementAt<string>(0),
                            Informed = x.ElementAt(72).Values.ElementAt<string>(0).ToLower() == "true" ? true : false,
                            CannotFollowup = x.ElementAt(73).Values.ElementAt<string>(0).ToLower() == "true" ? true : false
                        }).ToList();
                    }
                    else if (fileFormat == "2019")
                    {
                        convertedList = table.Select(x => new ImportPatientModel
                        {
                            Name = x.ElementAt(0).Values.ElementAt<string>(0),
                            Surname = x.ElementAt(1).Values.ElementAt<string>(0),
                            MRN = x.ElementAt(2).Values.ElementAt<string>(0),
                            PostCode = x.ElementAt(3).Values.ElementAt<string>(0),
                            Phone = x.ElementAt(4).Values.ElementAt<string>(0),
                            DOB = x.ElementAt(5).Values.ElementAt<string>(0),
                            Sex = x.ElementAt(6).Values.ElementAt<string>(0),
                            Ethnic = x.ElementAt(7).Values.ElementAt<string>(0),
                            Medicare = x.ElementAt(9).Values.ElementAt<string>(0),
                            UResidence = x.ElementAt(10).Values.ElementAt<string>(0),
                            AdmissionViaED = x.ElementAt(11).Values.ElementAt<string>(0),
                            TransferHospital = x.ElementAt(12).Values.ElementAt<string>(0),
                            TransferDate = x.ElementAt(13).Values.ElementAt<string>(0),
                            TransferTime = x.ElementAt(14).Values.ElementAt<string>(0),
                            ArrivalDate = x.ElementAt(15).Values.ElementAt<string>(0),
                            ArrivalTime = x.ElementAt(16).Values.ElementAt<string>(0),
                            DepartureDate = x.ElementAt(17).Values.ElementAt<string>(0),
                            DepartureTime = x.ElementAt(18).Values.ElementAt<string>(0),
                            InHospFractureDate = x.ElementAt(19).Values.ElementAt<string>(0),
                            InHospFractureTime = x.ElementAt(20).Values.ElementAt<string>(0),
                            WardType = x.ElementAt(21).Values.ElementAt<string>(0),
                            PreAdWalk = x.ElementAt(22).Values.ElementAt<string>(0),
                            PainAssessment = x.ElementAt(23).Values.ElementAt<string>(0),
                            PainManagement = x.ElementAt(24).Values.ElementAt<string>(0),
                            CognitiveAssessment = x.ElementAt(25).Values.ElementAt<string>(0),
                            CognitiveState = x.ElementAt(26).Values.ElementAt<string>(0),
                            BoneMed = x.ElementAt(27).Values.ElementAt<string>(0),
                            PreOpMedAss = x.ElementAt(28).Values.ElementAt<string>(0),
                            FractureSide = x.ElementAt(29).Values.ElementAt<string>(0),
                            AtypicalFracture = x.ElementAt(30).Values.ElementAt<string>(0),
                            FractureType = x.ElementAt(31).Values.ElementAt<string>(0),
                            ASAgrade = x.ElementAt(32).Values.ElementAt<string>(0),
                            Surgery = x.ElementAt(33).Values.ElementAt<string>(0),
                            SurgeryDate = x.ElementAt(34).Values.ElementAt<string>(0),
                            SurgeryTime = x.ElementAt(35).Values.ElementAt<string>(0),
                            SurgeryDelay = x.ElementAt(36).Values.ElementAt<string>(0),
                            SurgeryDelayOther = x.ElementAt(37).Values.ElementAt<string>(0),
                            Anaesthesia = x.ElementAt(38).Values.ElementAt<string>(0),
                            Analgesia = x.ElementAt(39).Values.ElementAt<string>(0),
                            ConsultantPresent = x.ElementAt(40).Values.ElementAt<string>(0),
                            Operation = x.ElementAt(41).Values.ElementAt<string>(0),
                            FullWeightBear = x.ElementAt(42).Values.ElementAt<string>(0),
                            Mobilisation = x.ElementAt(43).Values.ElementAt<string>(0),
                            DeleriumAssessment = x.ElementAt(44).Values.ElementAt<string>(0),
                            PressureUlcers = x.ElementAt(45).Values.ElementAt<string>(0),
                            Malnutrition = x.ElementAt(46).Values.ElementAt<string>(0),
                            GeriatricAssessment = x.ElementAt(47).Values.ElementAt<string>(0),
                            GeriatricAssDate = x.ElementAt(48).Values.ElementAt<string>(0),
                            FallsAssessment = x.ElementAt(49).Values.ElementAt<string>(0),
                            BoneMedDischarge = x.ElementAt(50).Values.ElementAt<string>(0),
                            WardDischargeDate = x.ElementAt(51).Values.ElementAt<string>(0),
                            DischargeDest = x.ElementAt(52).Values.ElementAt<string>(0),
                            HospitalDischargeDate = x.ElementAt(53).Values.ElementAt<string>(0),
                            DischargeResidence = x.ElementAt(54).Values.ElementAt<string>(0),
                            FollowupDate120 = x.ElementAt(57).Values.ElementAt<string>(0),
                            HealthServiceDischarge120 = x.ElementAt(58).Values.ElementAt<string>(0),
                            Survival120 = x.ElementAt(59).Values.ElementAt<string>(0),
                            Residence120 = x.ElementAt(60).Values.ElementAt<string>(0),
                            WeightBear120 = x.ElementAt(61).Values.ElementAt<string>(0),
                            WalkingAbility120 = x.ElementAt(62).Values.ElementAt<string>(0),
                            BoneMed120 = x.ElementAt(63).Values.ElementAt<string>(0),
                            Reoperation120 = x.ElementAt(64).Values.ElementAt<string>(0),
                            Informed = x.ElementAt(65).Values.ElementAt<string>(0).ToLower() == "true" ? true : false,
                            CannotFollowup = x.ElementAt(66).Values.ElementAt<string>(0).ToLower() == "true" ? true : false
                        }).ToList();
                    }
                    else  // Assuming 2020 & New Zealand
                    {
                        convertedList = table.Select(x => new ImportPatientModel
                        {
                            Name = x.ElementAt(0).Values.ElementAt<string>(0),
                            Surname = x.ElementAt(1).Values.ElementAt<string>(0),
                            MRN = x.ElementAt(2).Values.ElementAt<string>(0),
                            PostCode = x.ElementAt(3).Values.ElementAt<string>(0),
                            Phone = x.ElementAt(4).Values.ElementAt<string>(0),
                            DOB = x.ElementAt(5).Values.ElementAt<string>(0),
                            Sex = x.ElementAt(6).Values.ElementAt<string>(0),
                            Ethnic = x.ElementAt(7).Values.ElementAt<string>(0),
                            Medicare = x.ElementAt(9).Values.ElementAt<string>(0),
                            UResidence = x.ElementAt(10).Values.ElementAt<string>(0),
                            AdmissionViaED = x.ElementAt(11).Values.ElementAt<string>(0),
                            TransferHospital = x.ElementAt(12).Values.ElementAt<string>(0),
                            TransferDate = x.ElementAt(13).Values.ElementAt<string>(0),
                            TransferTime = x.ElementAt(14).Values.ElementAt<string>(0),
                            ArrivalDate = x.ElementAt(15).Values.ElementAt<string>(0),
                            ArrivalTime = x.ElementAt(16).Values.ElementAt<string>(0),
                            DepartureDate = x.ElementAt(17).Values.ElementAt<string>(0),
                            DepartureTime = x.ElementAt(18).Values.ElementAt<string>(0),
                            InHospFractureDate = x.ElementAt(19).Values.ElementAt<string>(0),
                            InHospFractureTime = x.ElementAt(20).Values.ElementAt<string>(0),
                            WardType = x.ElementAt(21).Values.ElementAt<string>(0),
                            PreAdWalk = x.ElementAt(22).Values.ElementAt<string>(0),
                            PainAssessment = x.ElementAt(23).Values.ElementAt<string>(0),
                            PainManagement = x.ElementAt(24).Values.ElementAt<string>(0),
                            CognitiveAssessment = x.ElementAt(25).Values.ElementAt<string>(0),
                            CognitiveState = x.ElementAt(26).Values.ElementAt<string>(0),
                            BoneMed = x.ElementAt(27).Values.ElementAt<string>(0),
                            PreOpMedAss = x.ElementAt(28).Values.ElementAt<string>(0),
                            FractureSide = x.ElementAt(29).Values.ElementAt<string>(0),
                            AtypicalFracture = x.ElementAt(30).Values.ElementAt<string>(0),
                            FractureType = x.ElementAt(31).Values.ElementAt<string>(0),
                            ASAgrade = x.ElementAt(32).Values.ElementAt<string>(0),
                            Surgery = x.ElementAt(33).Values.ElementAt<string>(0),
                            SurgeryDate = x.ElementAt(34).Values.ElementAt<string>(0),
                            SurgeryTime = x.ElementAt(35).Values.ElementAt<string>(0),
                            SurgeryDelay = x.ElementAt(36).Values.ElementAt<string>(0),
                            SurgeryDelayOther = x.ElementAt(37).Values.ElementAt<string>(0),
                            Anaesthesia = x.ElementAt(38).Values.ElementAt<string>(0),
                            Analgesia = x.ElementAt(39).Values.ElementAt<string>(0),
                            ConsultantPresent = x.ElementAt(40).Values.ElementAt<string>(0),
                            Operation = x.ElementAt(41).Values.ElementAt<string>(0),
                            FullWeightBear = x.ElementAt(42).Values.ElementAt<string>(0),
                            Mobilisation = x.ElementAt(43).Values.ElementAt<string>(0),
                            FirstDayWalking = x.ElementAt(44).Values.ElementAt<string>(0),
                            DeleriumAssessment = x.ElementAt(45).Values.ElementAt<string>(0),
                            PressureUlcers = x.ElementAt(46).Values.ElementAt<string>(0),
                            Malnutrition = x.ElementAt(47).Values.ElementAt<string>(0),
                            GeriatricAssessment = x.ElementAt(48).Values.ElementAt<string>(0),
                            GeriatricAssDate = x.ElementAt(49).Values.ElementAt<string>(0),
                            FallsAssessment = x.ElementAt(50).Values.ElementAt<string>(0),
                            BoneMedDischarge = x.ElementAt(51).Values.ElementAt<string>(0),
                            WardDischargeDate = x.ElementAt(52).Values.ElementAt<string>(0),
                            DischargeDest = x.ElementAt(53).Values.ElementAt<string>(0),
                            HospitalDischargeDate = x.ElementAt(54).Values.ElementAt<string>(0),
                            DischargeResidence = x.ElementAt(55).Values.ElementAt<string>(0),
                            FollowupDate120 = x.ElementAt(58).Values.ElementAt<string>(0),
                            HealthServiceDischarge120 = x.ElementAt(59).Values.ElementAt<string>(0),
                            Survival120 = x.ElementAt(60).Values.ElementAt<string>(0),
                            Residence120 = x.ElementAt(61).Values.ElementAt<string>(0),
                            WalkingAbility120 = x.ElementAt(62).Values.ElementAt<string>(0),
                            BoneMed120 = x.ElementAt(63).Values.ElementAt<string>(0),
                            Reoperation120 = x.ElementAt(64).Values.ElementAt<string>(0),
                            DeathDate = x.ElementAt(65).Values.ElementAt<string>(0),
                            Informed = x.ElementAt(66).Values.ElementAt<string>(0).ToLower() == "true" ? true : false,
                            CannotFollowup = x.ElementAt(67).Values.ElementAt<string>(0).ToLower() == "true" ? true : false
                        }).ToList();
                    }
                }
            }
            catch (Exception e)
            {
                string msg = e.Message;
                //Redirect to an error page
                return null;
            }

            return convertedList;
        }

        public static DataTable ConvertToDatatable<T>(this IList<T> data, string fileFormat)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));

            DataTable table = new DataTable();

            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                if ((ConfigurationManager.AppSettings["Location"] != "Australian" && props[i].Name == "PatientType") ||
                        (fileFormat == "2017" && (props[i].Name == "DeleriumAssessment" || props[i].Name == "Informed" || props[i].Name == "CannotFollowup" ||
                        props[i].Name == "Malnutrition" ||
                        props[i].Name == "FirstDayWalking" || props[i].Name == "DeathDate")) ||

                        (fileFormat == "2018" && (props[i].Name == "InterOpFracture" || 
                        props[i].Name == "Malnutrition" ||
                        props[i].Name == "FirstDayWalking" || props[i].Name == "DeathDate")) ||

                        (fileFormat == "2019" && (props[i].Name == "InterOpFracture" || props[i].Name == "FollowupDate30" || props[i].Name == "HealthServiceDischarge30" || props[i].Name == "Survival30" ||
                        props[i].Name == "Residence30" || props[i].Name == "WeightBear30" || props[i].Name == "WalkingAbility30" || props[i].Name == "BoneMed30" ||
                        props[i].Name == "Reoperation30" ||
                        props[i].Name == "FirstDayWalking" || props[i].Name == "DeathDate")) ||

                        (fileFormat == "2020" && (props[i].Name == "InterOpFracture" || props[i].Name == "FollowupDate30" || props[i].Name == "HealthServiceDischarge30" || props[i].Name == "Survival30" ||
                        props[i].Name == "Residence30" || props[i].Name == "WeightBear30" || props[i].Name == "WalkingAbility30" || props[i].Name == "BoneMed30" ||
                        props[i].Name == "Reoperation30" ||
                        props[i].Name =="WeightBear120")))
                {
                    //Skip
                }
                else
                {
                    if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        table.Columns.Add(prop.Name, prop.PropertyType.GetGenericArguments()[0]);
                    else
                        table.Columns.Add(prop.Name, prop.PropertyType);
                }
            }

            foreach (T item in data)
            {
                List<object> values = new List<object>();

                //int j = -1;
                for (int i = 0; i < props.Count; i++)
                {
                    if ((ConfigurationManager.AppSettings["Location"] != "Australian" && props[i].Name == "PatientType") ||
                        (fileFormat == "2017" && (props[i].Name == "DeleriumAssessment" || props[i].Name == "Informed" || props[i].Name == "CannotFollowup" || 
                        props[i].Name == "Malnutrition" || props[i].Name == "StartDate" ||
                        props[i].Name == "FirstDayWalking" || props[i].Name == "DeathDate")) ||

                        (fileFormat == "2018" && (props[i].Name == "InterOpFracture" || props[i].Name == "StartDate" || 
                        props[i].Name == "Malnutrition" ||
                        props[i].Name == "FirstDayWalking" || props[i].Name == "DeathDate")) ||

                        (fileFormat == "2019" && (props[i].Name == "InterOpFracture" || props[i].Name == "StartDate" || props[i].Name == "FollowupDate30" || 
                        props[i].Name == "HealthServiceDischarge30" || props[i].Name == "Survival30" || 
                        props[i].Name == "Residence30" || props[i].Name == "WeightBear30" || props[i].Name == "WalkingAbility30" || 
                        props[i].Name == "BoneMed30" || props[i].Name == "Reoperation30" || props[i].Name == "FirstDayWalking" || props[i].Name == "DeathDate" )) ||
                        
                        (fileFormat == "2020" && (props[i].Name == "InterOpFracture" || props[i].Name == "FollowupDate30" ||
                        props[i].Name == "HealthServiceDischarge30" || props[i].Name == "Survival30" ||
                        props[i].Name == "Residence30" || props[i].Name == "WeightBear30" || props[i].Name == "WalkingAbility30" ||
                        props[i].Name == "BoneMed30" || props[i].Name == "Reoperation30" ||
                        props[i].Name == "WeightBear120" || props[i].Name == "StartDate")))
                    {
                        // skip
                    }
                    else
                    {
                        values.Add(props[i].GetValue(item));
                    }
                }

                table.Rows.Add(values.ToArray());
            }

            return table;
        }

        public static byte[] ToByteArray(this string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string Synonym(this string word)
        {
            return (new SynonymChildServices()).GetSynonym(word);
        }

        public static UserInfoModel GetUserInfo(this System.Security.Principal.IPrincipal CurrentUser)
        {
            ANZHFREntities Entity = new ANZHFREntities();
            UserInfoModel userInfo = new UserInfoModel();
            User user = Entity.Users.SingleOrDefault(x => x.UEmail == CurrentUser.Identity.Name);

            if (user != null)
            {
                userInfo.ID = user.UserID;
                userInfo.Email = user.UEmail;
                userInfo.AccessLevel = user.UAccessLevel;
                userInfo.HospitalID = user.UHospitalID;
            }

            return userInfo;
        }

        public static string GetFractureSide(this Patient patient)
        {
            using (ANZHFREntities Entity = new ANZHFREntities())
            {
                short b;

                if (patient.FractureSide != null && short.TryParse(patient.FractureSide, out b))
                {
                    var a = Entity.FractureSides.SingleOrDefault(x => x.FractureSideID == b);
                    if (a != null)
                    {
                        return a.Name.Synonym();
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static int GetAge(this DateTime? bday)
        {
            if (bday == null)
            {
                return 0;
            }

            DateTime birthday = DateTime.Parse(bday.ToString());
            DateTime now = DateTime.Today;
            int age = now.Year - birthday.Year;
            if (now < birthday.AddYears(age)) age--;

            return age;
        }

        public static bool IsEarlierThan(this DateTime? _date, DateTime date)
        {
            if (_date == null)
            {
                return false;
            }
            else
            {
                //return (new DateTime(_date.Value.Year, _date.Value.Month, 1) <= date);
                return (new DateTime(_date.Value.Year, _date.Value.Month, 1) == date);
            }
        }

        public static string SexValue(this string _Sex)
        {
            short a;
            if (short.TryParse(_Sex, out a))
            {
                using (ANZHFREntities Entity = new ANZHFREntities())
                {
                    var s = Entity.Sexes.SingleOrDefault(x => x.SexID == a);
                    if (s == null)
                    {
                        return string.Empty;
                    }
                    else
                    {
                        return s.Name;
                    }
                }
            }

            return string.Empty;
        }

        public static int? ToInt(this object value)
        {
            int? returnValue = null;

            if (value != null)
            {
                int result = 0;
                if (int.TryParse(value.ToString(), out result))
                {
                    returnValue = result;
                }
            }

            return returnValue;
        }

        #region Generate Random String

        public const string RandomStringCharacters = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789";
        public const string RandomStringCharactersWithSymbols = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789!@#$%^&*()_+";

        public static string GenerateRandomString(int minLength, int maxLength, bool useSymbols)
        {
            Random rnd = new Random();
            char[] str = new char[rnd.Next(minLength, maxLength + 1)];
            string randomCharacters = useSymbols ? RandomStringCharactersWithSymbols : RandomStringCharacters;

            for (int x = 0; x < str.Length; x++)
            {
                str[x] = randomCharacters[rnd.Next(0, randomCharacters.Length)];
            }

            return new string(str);
        }

        #endregion
    }
}