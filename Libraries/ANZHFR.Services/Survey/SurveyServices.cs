using ANZHFR.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANZHFR.Services.Survey
{
    public class SurveyServices : BaseServices
    {
        public Data.Models.Survey Get(int id)
        {
            var survey = Entity.Surveys.Where(x => x.SurveyID == id).FirstOrDefault();

            if (survey == null)
            {
                survey = new Data.Models.Survey();
            }

            return survey;
        }

        public Data.Models.Survey GetByHospitalID(long hospitalID, int year)
        {
            var survey = Entity.Surveys.Where(x => x.HospitalID == hospitalID && x.Year == year).FirstOrDefault();

            //if (survey == null)
            //{
            //    survey = new Data.Models.Survey();
            //}

            return survey;
        }

        public List<int?> GetYears()
        {
            var years = Entity.Surveys.Where(x => x.Year.HasValue).Select(x => x.Year).Distinct().ToList();

            return years;
        }

        public List<Data.Models.Survey> GetAll(string year = "", string hospitalID = "")
        {
            var results = from s in Entity.Surveys
                          select s;

            if (!string.IsNullOrWhiteSpace(year))
            {
                int yearNum = 0;
                if (int.TryParse(year, out yearNum) && yearNum > 0)
                {
                    results = results.Where(s => s.Year == yearNum);
                }
            }

            if (!string.IsNullOrWhiteSpace(hospitalID))
            {
                int HID = 0;
                if (int.TryParse(hospitalID, out HID) && HID > 0)
                {
                    results = results.Where(s => s.HospitalID == HID);
                }
            }

            var surveys = results.OrderBy(s => s.Year).ThenBy(s => s.Hospital.HName).ToList();

            return surveys;
        }

        public Data.Models.Survey Save(long id, long hospitalID, int year, string roleName, string estimatedRecords, string designatedMajorTraumaCentre,
            string formalOrthogeriatricService, string modelOfCare, string modelCareOther, string protocolAccessCTMRI, string hipFracturePathway, string vTEProtocol,
            string protocolPain, string plannedList, string choiceOfAnaesthesia, string localNerveBlocks, string localNerveBlocksPostoperative, string weekendTherapyServices,
            string writtenCareInformation, string inPatientRehabilitation, string supportedHomeBasedRehabilitation, string individualisedWrittenInformation,
            string fallsClinicPublic, string osteoporosisClinicPublic, string combinedFallsBoneHealthClinicPublic, string orthopaedicClinicPublic, string fractureLiaisonService,
            string collectLocalHipFractureData, string whoCollectsData, string serviceChanges, string serviceChangesPlanned, string identifiedBarriers, string identifiedBarrierDetails)
        {
            try
            {
                Data.Models.Survey survey = GetByHospitalID(hospitalID, year);

                if (survey == null)
                {
                    survey = new Data.Models.Survey();
                    Entity.Surveys.Add(survey);

                    survey.CreatedDate = DateTime.Now;
                }

                survey.UpdatedDate = DateTime.Now;

                survey.Year = year;
                survey.SubmittedFlag = true;

                survey.HospitalID = hospitalID;
                survey.RoleName = roleName;
                survey.EstimatedRecords = estimatedRecords;
                survey.RoleName = roleName;
                survey.EstimatedRecords = estimatedRecords;
                survey.DesignatedMajorTraumaCentre = designatedMajorTraumaCentre;
                survey.FormalOrthogeriatricService = formalOrthogeriatricService;
                survey.ModelOfCare = modelOfCare;
                survey.ModelCareOther = modelCareOther;
                survey.ProtocolAccessCTMRI = protocolAccessCTMRI;
                survey.HipFracturePathway = hipFracturePathway;
                survey.VTEProtocol = vTEProtocol;
                survey.ProtocolPain = protocolPain;
                survey.PlannedList = plannedList;
                survey.ChoiceOfAnaesthesia = choiceOfAnaesthesia;
                survey.LocalNerveBlocks = localNerveBlocks;
                survey.LocalNerveBlocksPostoperative = localNerveBlocksPostoperative;
                survey.WeekendTherapyServices = weekendTherapyServices;
                survey.WrittenCareInformation = writtenCareInformation;
                survey.InPatientRehabilitation = inPatientRehabilitation;
                survey.SupportedHomeBasedRehabilitation = supportedHomeBasedRehabilitation;
                survey.IndividualisedWrittenInformation = individualisedWrittenInformation;
                survey.FallsClinicPublic = fallsClinicPublic;
                survey.OsteoporosisClinicPublic = osteoporosisClinicPublic;
                survey.CombinedFallsBoneHealthClinicPublic = combinedFallsBoneHealthClinicPublic;
                survey.OrthopaedicClinicPublic = orthopaedicClinicPublic;
                survey.FractureLiaisonService = fractureLiaisonService;
                survey.CollectLocalHipFractureData = collectLocalHipFractureData;
                survey.WhoCollectsData = whoCollectsData;
                survey.ServiceChanges = serviceChanges;
                survey.ServiceChangesPlanned = serviceChangesPlanned;
                survey.IdentifiedBarriers = identifiedBarriers;
                survey.IdentifiedBarrierDetails = identifiedBarrierDetails;

                Entity.SaveChanges();

                return survey;
            }
            catch
            {
                return null;
            }
        }
    }
}