using ANZHFR.Data.Models;
using ANZHFR.Services.Patients;
using ANZHFR.Web.Controllers;
using ANZHFR.Web.ExtensionMethods;
using ANZHFR.Web.Models;
using ANZHFR.Web.Properties;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace ANZHFR.Web.Helpers
{
    public class ReportHelper
    {
        #region Age Report

        public static object AgeReport(string reportType, Hospital hospital, int month, int year) //long hospitalID, string state, string country,
        {
            #region DB Query

            ANZHFREntities Entity = new ANZHFREntities();

            var _data = (from p in Entity.Patients
                         join h in Entity.Hospitals on p.HospitalID equals h.HospitalID
                         where h.HCountry == hospital.HCountry
                         select new ReportQueryModel
                         {
                             DOB = p.DOB,
                             HospitalID = h.HospitalID,
                             Hospital = h.HName,
                             AdmissionViaED = p.AdmissionViaED,
                             ArrivalDateTime = p.ArrivalDateTime,
                             InHospFractureDateTime = p.InHospFractureDateTime,
                             TransferDateTime = p.TransferDateTime,
                             DepartureDateTime = p.DepartureDateTime,
                             State = h.HState,
                             Country = h.HCountry
                         }).ToList();

            var data = (from s in _data
                        where s.StartDate.IsEarlierThan(new DateTime(year, month, 1)) &&
                           (s.DOB.GetAge() >= 60 && s.DOB.GetAge() <= 107)
                        select new ReportQueryModel
                        {
                            DOB = s.DOB,
                            Age = s.DOB.GetAge(),
                            HospitalID = s.HospitalID,
                            Hospital = s.Hospital,
                            AdmissionViaED = s.AdmissionViaED,
                            ArrivalDateTime = s.ArrivalDateTime,
                            InHospFractureDateTime = s.InHospFractureDateTime,
                            TransferDateTime = s.TransferDateTime,
                            DepartureDateTime = s.DepartureDateTime,
                            State = s.State,
                            Country = s.Country
                        }).ToList();

            #endregion DB Query

            if (data.Count > 0)
            {
                #region Hospital

                var filteredList = data.Where(x => x.HospitalID == hospital.HospitalID).ToList();

                List<object> rd = GetAgeGroupData(filteredList);

                ReportModel hospitalModel = new ReportModel { Label = hospital.HName, Data = rd };

                #endregion Hospital

                #region State

                ReportModel stateModel = null;
                if (ConfigurationManager.AppSettings["Location"] == "Australian")
                {
                    filteredList = data.Where(x => x.State == hospital.HState).ToList();
                    rd = GetAgeGroupData(filteredList);

                    stateModel = new ReportModel { Label = hospital.HState, Data = rd };
                }

                #endregion State

                #region National average

                var nationalAvgList = GetAgeNatioAvgData(data);
                ReportModel nationModel = new ReportModel { Label = hospital.HCountry, Data = nationalAvgList };

                #endregion Nation

                List<List<object>> reportData = new List<List<object>>();
                reportData.Add(hospitalModel.Data);

                if (stateModel != null)
                {
                    reportData.Add(stateModel.Data);
                }

                reportData.Add(nationModel.Data);

                List<string> reportLegends = new List<string>();
                reportLegends.Add(hospitalModel.Label);

                if (stateModel != null)
                {
                    reportLegends.Add(stateModel.Label);
                }

                reportLegends.Add(nationModel.Label);

                var report = Entity.Reports.Where(r => r.Code == reportType).FirstOrDefault();

                return new { Data = reportData, Labels = new string[] { "60 - 69", "70 - 79", "80 - 89", "90 - 99", "100 - 107" }, Legends = reportLegends, IsAverage = true, Rotatelabel = false, ChartTitle = report.Title, xTitle = report.XLabel, yTitle = report.YLabel, Description = report.Description };
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Sex Report

        public static object SexReport(string reportType, Hospital hospital, int month, int year)
        {
            #region DB Query

            ANZHFREntities Entity = new ANZHFREntities();

            var _sexes = (from p in Entity.Patients
                          join h in Entity.Hospitals on p.HospitalID equals h.HospitalID
                          where h.HCountry == hospital.HCountry
                          select new ReportQueryModel
                          {
                              DOB = p.DOB,
                              HospitalID = h.HospitalID,
                              Hospital = h.HName,
                              AdmissionViaED = p.AdmissionViaED,
                              ArrivalDateTime = p.ArrivalDateTime,
                              InHospFractureDateTime = p.InHospFractureDateTime,
                              TransferDateTime = p.TransferDateTime,
                              DepartureDateTime = p.DepartureDateTime,
                              Sex = p.Sex,
                              State = h.HState,
                              Country = h.HCountry
                          }).ToList();

            var sexes = (from s in _sexes
                         where s.StartDate.IsEarlierThan(new DateTime(year, month, 1)) &&
                            (s.DOB.GetAge() == 0 || (s.DOB.GetAge() >= 60 && s.DOB.GetAge() <= 107))
                         select new ReportQueryModel
                         {
                             DOB = s.DOB,
                             HospitalID = s.HospitalID,
                             Hospital = s.Hospital,
                             AdmissionViaED = s.AdmissionViaED,
                             ArrivalDateTime = s.ArrivalDateTime,
                             InHospFractureDateTime = s.InHospFractureDateTime,
                             TransferDateTime = s.TransferDateTime,
                             DepartureDateTime = s.DepartureDateTime,
                             Sex = s.Sex.SexValue(),
                             State = s.State,
                             Country = s.Country
                         }).ToList();

            #endregion DB Query

            if (sexes.Count > 0)
            {
                #region Hospital

                var filteredList = sexes.Where(x => x.HospitalID == hospital.HospitalID).ToList();

                List<object> rd = GetSexData(filteredList);
                ReportModel hospitals = new ReportModel { Label = hospital.HName, Data = rd };

                #endregion Hospital

                #region State

                ReportModel stateModel = null;
                if (ConfigurationManager.AppSettings["Location"] == "Australian")
                {
                    filteredList = sexes.Where(x => x.State == hospital.HState).ToList();
                    rd = GetSexData(filteredList);

                    stateModel = new ReportModel { Label = hospital.HState, Data = rd };
                }

                #endregion State

                #region Nation

                rd = GetSexData(sexes);

                ReportModel nations = new ReportModel { Label = hospital.HCountry, Data = rd };

                #endregion Nation

                List<List<object>> reportData = new List<List<object>>();
                reportData.Add(hospitals.Data);

                if (stateModel != null)
                {
                    reportData.Add(stateModel.Data);
                }

                reportData.Add(nations.Data);

                List<string> reportLegends = new List<string>();
                reportLegends.Add(hospitals.Label);

                if (stateModel != null)
                {
                    reportLegends.Add(stateModel.Label);
                }

                reportLegends.Add(nations.Label);

                var report = Entity.Reports.Where(r => r.Code == reportType).FirstOrDefault();

                return new { Data = reportData, Labels = new string[] { "Male", "Female" }, Legends = reportLegends, IsAverage = true, Rotatelabel = false, ChartTitle = report.Title, xTitle = report.XLabel, yTitle = report.YLabel, Description = report.Description };
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Fracture Type Report

        public static object FractureTypeReport(string reportType, Hospital hospital, int month, int year)
        {
            #region DB Query

            ANZHFREntities Entity = new ANZHFREntities();

            var _data = (from p in Entity.Patients
                         join h in Entity.Hospitals on p.HospitalID equals h.HospitalID
                         where h.HCountry == hospital.HCountry &&
                         p.FractureType != "" &&
                         p.FractureType != null
                         select new ReportQueryModel
                         {
                             DOB = p.DOB,
                             HospitalID = h.HospitalID,
                             Hospital = h.HName,
                             AdmissionViaED = p.AdmissionViaED,
                             ArrivalDateTime = p.ArrivalDateTime,
                             InHospFractureDateTime = p.InHospFractureDateTime,
                             TransferDateTime = p.TransferDateTime,
                             DepartureDateTime = p.DepartureDateTime,
                             State = h.HState,
                             Country = h.HCountry,
                             FractureTypeID = p.FractureType
                         }).ToList();

            var data = (from d in _data
                        join f in Entity.FractureTypes on Convert.ToInt16(d.FractureTypeID) equals f.FractureTypeID
                        where d.StartDate.IsEarlierThan(new DateTime(year, month, 1)) &&
                           (d.DOB.GetAge() == 0 || (d.DOB.GetAge() >= 60 && d.DOB.GetAge() <= 107))
                        select new ReportQueryModel
                        {
                            DOB = d.DOB,
                            HospitalID = d.HospitalID,
                            Hospital = d.Hospital,
                            AdmissionViaED = d.AdmissionViaED,
                            ArrivalDateTime = d.ArrivalDateTime,
                            InHospFractureDateTime = d.InHospFractureDateTime,
                            TransferDateTime = d.TransferDateTime,
                            DepartureDateTime = d.DepartureDateTime,
                            State = d.State,
                            Country = d.Country,
                            FractureTypeID = d.FractureTypeID,
                            FractureType = f.Name
                        }).ToList();

            #endregion DB Query

            if (data.Count > 0)
            {
                string[] keys = Entity.FractureTypes.Select(x => x.Name).Distinct().ToArray<string>();

                #region Hospital

                List<object> dataArray = new List<object>();

                for (int i = 0; i < keys.Count(); i++)
                {
                    dataArray.Add(data.Where(x => x.HospitalID == hospital.HospitalID && x.FractureType == keys[i]).Count());
                }

                dataArray = GetAverageFractureType(dataArray);

                ReportModel hospitalModel = new ReportModel { Label = hospital.HName, Data = dataArray };

                #endregion Hospital Data

                #region State Data

                ReportModel stateModel = null;
                if (ConfigurationManager.AppSettings["Location"] == "Australian")
                {
                    dataArray = new List<object>();

                    for (int i = 0; i < keys.Count(); i++)
                    {
                        dataArray.Add(data.Where(x => x.State == hospital.HState && x.FractureType == keys[i]).Count());
                    }

                    dataArray = GetAverageFractureType(dataArray);

                    stateModel = new ReportModel { Label = hospital.HState, Data = dataArray };
                }

                #endregion State

                #region Nation

                dataArray = new List<object>();

                for (int i = 0; i < keys.Count(); i++)
                {
                    dataArray.Add(data.Where(x => x.Country == hospital.HCountry && x.FractureType == keys[i]).Count());
                }

                dataArray = GetAverageFractureType(dataArray);

                ReportModel nationData = new ReportModel { Label = hospital.HCountry, Data = dataArray };

                #endregion Nation

                List<List<object>> reportData = new List<List<object>>();
                reportData.Add(hospitalModel.Data);

                if (stateModel != null)
                {
                    reportData.Add(stateModel.Data);
                }

                reportData.Add(nationData.Data);

                List<string> reportLegends = new List<string>();
                reportLegends.Add(hospitalModel.Label);

                if (stateModel != null)
                {
                    reportLegends.Add(stateModel.Label);
                }

                reportLegends.Add(nationData.Label);

                var report = Entity.Reports.Where(r => r.Code == reportType).FirstOrDefault();

                return new { Data = reportData, Labels = keys, Legends = reportLegends, IsAverage = true, Rotatelabel = false, ChartTitle = report.Title, xTitle = report.XLabel, yTitle = report.YLabel, Description = report.Description };
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Length of Stay Report

        public static object LengthOfStayReport(string reportType, Hospital hospital, int month, int year, bool acute)
        {
            #region DB Query

            ANZHFREntities Entity = new ANZHFREntities();

            var quarters = GetQuarters(new DateTime(year, month, 1));

            DateTime startDate = quarters.First().StartDate;
            DateTime endDate = quarters.Last().EndDate;

            var query = from p in Entity.Patients
                        join h in Entity.Hospitals on p.HospitalID equals h.HospitalID
                        where h.HCountry == hospital.HCountry &&
                           p.StartDate.HasValue && p.HospitalDischargeDate.HasValue
                        select new ReportQueryModel
                        {
                            DOB = p.DOB,
                            HospitalID = h.HospitalID,
                            Hospital = h.HName,
                            MyStart = p.StartDate,
                            AdmissionViaED = p.AdmissionViaED,
                            ArrivalDateTime = p.ArrivalDateTime,
                            InHospFractureDateTime = p.InHospFractureDateTime,
                            TransferDateTime = p.TransferDateTime,
                            DepartureDateTime = p.DepartureDateTime,
                            DischargeDate = p.HospitalDischargeDate,
                            State = h.HState,
                            Country = h.HCountry
                        };

            List<ReportQueryModel> _data = query.Where(p => p.MyStart >= startDate && p.MyStart < endDate).ToList();

            var data = (from d in _data
                        where (d.DOB.GetAge() >= 60 && d.DOB.GetAge() <= 107) &&
                            d.MyStart < d.DischargeDate
                        select new ReportQueryModel
                        {
                            DOB = d.DOB,
                            AdmissionViaED = d.AdmissionViaED,
                            ArrivalDateTime = d.ArrivalDateTime,
                            InHospFractureDateTime = d.InHospFractureDateTime,
                            TransferDateTime = d.TransferDateTime,
                            DepartureDateTime = d.DepartureDateTime,
                            DischargeDate = d.DischargeDate,
                            HospitalID = d.HospitalID,
                            Hospital = d.Hospital,
                            LengthOfStay = CalculateLengthOfStay(d.MyStart, d.DischargeDate),
                            State = d.State,
                            Country = d.Country
                        }).ToList();

            #endregion DB Query

            if (data.Count > 0)
            {
                #region Hospital

                var filteredList = data.Where(x => x.HospitalID == hospital.HospitalID).ToList();

                List<object> rd = GetLengthOfStayData(filteredList, quarters);
                ReportModel hospitalModel = new ReportModel { Label = hospital.HName, Data = rd };

                #endregion Hospital

                #region State

                ReportModel stateModel = null;
                if (ConfigurationManager.AppSettings["Location"] == "Australian")
                {
                    filteredList = data.Where(x => x.State == hospital.HState).ToList();
                    rd = GetLengthOfStayData(filteredList, quarters);
                    stateModel = new ReportModel { Label = hospital.HState, Data = rd };
                }

                #endregion State

                #region Nation

                rd = GetLengthOfStayData(data, quarters);
                ReportModel nationData = new ReportModel { Label = hospital.HCountry, Data = rd };

                #endregion Nation

                List<object> reportData = new List<object>();
                reportData.Add(hospitalModel.Data);

                if (stateModel != null)
                {
                    reportData.Add(stateModel.Data);
                }

                reportData.Add(nationData.Data);

                List<object> reportLegends = new List<object>();
                reportLegends.Add(hospitalModel.Label);

                if (stateModel != null)
                {
                    reportLegends.Add(stateModel.Label);
                }

                reportLegends.Add(nationData.Label);

                var report = Entity.Reports.Where(r => r.Code == reportType).FirstOrDefault();

                return new { Data = reportData, Labels = GetQuarterLabels(quarters).ToArray(), Legends = reportLegends, IsAverage = false, RotateLabel = false, ChartTitle = report.Title, xTitle = report.XLabel, yTitle = report.YLabel, Description = report.Description };
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Acute Length of Stay Report

        public static object AcuteLengthOfStayReport(string reportType, Hospital hospital, int month, int year, bool acute)
        {
            #region DB Query

            ANZHFREntities Entity = new ANZHFREntities();

            var quarters = GetQuarters(new DateTime(year, month, 1));

            DateTime startDate = quarters.First().StartDate;
            DateTime endDate = quarters.Last().EndDate;

            var query = from p in Entity.Patients
                        join h in Entity.Hospitals on p.HospitalID equals h.HospitalID
                        where h.HCountry == hospital.HCountry &&
                           p.StartDate.HasValue && p.WardDischargeDate.HasValue
                        select new ReportQueryModel
                        {
                            DOB = p.DOB,
                            HospitalID = h.HospitalID,
                            Hospital = h.HName,
                            MyStart = p.StartDate,
                            AdmissionViaED = p.AdmissionViaED,
                            ArrivalDateTime = p.ArrivalDateTime,
                            InHospFractureDateTime = p.InHospFractureDateTime,
                            TransferDateTime = p.TransferDateTime,
                            DepartureDateTime = p.DepartureDateTime,
                            DischargeDate = p.WardDischargeDate,
                            State = h.HState,
                            Country = h.HCountry
                        };

            List<ReportQueryModel> _data = query.Where(p => p.MyStart >= startDate && p.MyStart < endDate).ToList();

            var data = (from d in _data
                        where (d.DOB.GetAge() >= 60 && d.DOB.GetAge() <= 107) &&
                            d.MyStart < d.DischargeDate
                        select new ReportQueryModel
                        {
                            DOB = d.DOB,
                            AdmissionViaED = d.AdmissionViaED,
                            ArrivalDateTime = d.ArrivalDateTime,
                            InHospFractureDateTime = d.InHospFractureDateTime,
                            TransferDateTime = d.TransferDateTime,
                            DepartureDateTime = d.DepartureDateTime,
                            DischargeDate = d.DischargeDate,
                            HospitalID = d.HospitalID,
                            Hospital = d.Hospital,
                            LengthOfStay = CalculateLengthOfStay(d.MyStart, d.DischargeDate),
                            State = d.State,
                            Country = d.Country
                        }).ToList();

            #endregion DB Query

            if (data.Count > 0)
            {
                #region Hospital

                var filteredList = data.Where(x => x.HospitalID == hospital.HospitalID).ToList();

                List<object> rd = GetLengthOfStayData(filteredList, quarters);
                ReportModel hospitalModel = new ReportModel { Label = hospital.HName, Data = rd };

                #endregion Hospital

                #region State

                ReportModel stateModel = null;
                if (ConfigurationManager.AppSettings["Location"] == "Australian")
                {
                    filteredList = data.Where(x => x.State == hospital.HState).ToList();
                    rd = GetLengthOfStayData(filteredList, quarters);
                    stateModel = new ReportModel { Label = hospital.HState, Data = rd };
                }

                #endregion State

                #region Nation

                rd = GetLengthOfStayData(data, quarters);
                ReportModel nationData = new ReportModel { Label = hospital.HCountry, Data = rd };

                #endregion Nation

                List<object> reportData = new List<object>();
                reportData.Add(hospitalModel.Data);

                if (stateModel != null)
                {
                    reportData.Add(stateModel.Data);
                }

                reportData.Add(nationData.Data);

                List<object> reportLegends = new List<object>();
                reportLegends.Add(hospitalModel.Label);

                if (stateModel != null)
                {
                    reportLegends.Add(stateModel.Label);
                }

                reportLegends.Add(nationData.Label);

                var report = Entity.Reports.Where(r => r.Code == reportType).FirstOrDefault();

                return new { Data = reportData, Labels = GetQuarterLabels(quarters).ToArray(), Legends = reportLegends, IsAverage = false, RotateLabel = false, ChartTitle = report.Title, xTitle = report.XLabel, yTitle = report.YLabel, Description = report.Description };
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Completeness Report

        public static object CompletenessReport(string reportType, Hospital hospital, int month, int year)
        {
            #region DB Query

            ANZHFREntities Entity = new ANZHFREntities();

            var quarters = GetQuarters(new DateTime(year, month, 1));

            DateTime startDate = quarters.First().StartDate;
            DateTime endDate = quarters.Last().EndDate;

            var data = (from p in Entity.Patients
                        join h in Entity.Hospitals on p.HospitalID equals h.HospitalID
                        where h.HCountry == hospital.HCountry &&
                        p.ArrivalDateTime >= startDate &&
                        p.ArrivalDateTime < endDate
                        select new ReportQueryModel
                        {
                            HospitalID = p.HospitalID,
                            Hospital = h.HName,
                            AdmissionViaED = p.AdmissionViaED,
                            ArrivalDateTime = p.ArrivalDateTime,
                            Completeness = p.Completeness,
                            State = h.HState,
                            Country = h.HCountry
                        }).ToList();

            #endregion DB Query

            if (data.Count > 0)
            {
                #region Hospital

                var filteredList = data.Where(x => x.HospitalID == hospital.HospitalID).ToList();

                List<object> rd = GetCompletenessData(filteredList, quarters);
                ReportModel hospitalModel = new ReportModel { Label = hospital.HName, Data = rd };

                #endregion Hospital

                #region State

                ReportModel stateModel = null;
                if (ConfigurationManager.AppSettings["Location"] == "Australian")
                {
                    filteredList = data.Where(x => x.State == hospital.HState).ToList();

                    rd = GetCompletenessData(filteredList, quarters);
                    stateModel = new ReportModel { Label = hospital.HState, Data = rd };
                }

                #endregion State

                #region Nation

                rd = GetCompletenessData(data, quarters);
                ReportModel nationData = new ReportModel { Label = hospital.HCountry, Data = rd };

                #endregion Nation

                List<object> reportData = new List<object>();
                reportData.Add(hospitalModel.Data);

                if (stateModel != null)
                {
                    reportData.Add(stateModel.Data);
                }

                reportData.Add(nationData.Data);

                List<object> reportLegends = new List<object>();
                reportLegends.Add(hospitalModel.Label);

                if (stateModel != null)
                {
                    reportLegends.Add(stateModel.Label);
                }

                reportLegends.Add(nationData.Label);

                var report = Entity.Reports.Where(r => r.Code == reportType).FirstOrDefault();

                return new { Data = reportData, Labels = GetQuarterLabels(quarters).ToArray(), Legends = reportLegends, IsAverage = false, RotateLabel = false, ChartTitle = report.Title, xTitle = report.XLabel, yTitle = report.YLabel, Description = report.Description };
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Discharge Destination

        public static object DischargeDestinationReport(string reportType, Hospital hospital, int month, int year)
        {
            #region DB Query

            ANZHFREntities Entity = new ANZHFREntities();

            var _data = (from p in Entity.Patients
                         join h in Entity.Hospitals on p.HospitalID equals h.HospitalID
                         where h.HCountry == hospital.HCountry &&
                         p.DischargeResidence != "" &&
                         p.DischargeResidence != null
                         select new
                         {
                             DOB = p.DOB,
                             HospitalID = h.HospitalID,
                             Hospital = h.HName,
                             StartDate = p.StartDate,
                             AdmissionViaED = p.AdmissionViaED,
                             ArrivalDateTime = p.ArrivalDateTime,
                             InHospFractureDateTime = p.InHospFractureDateTime,
                             TransferDateTime = p.TransferDateTime,
                             DepartureDateTime = p.DepartureDateTime,
                             DischargeDate = p.DepartureDateTime,
                             State = h.HState,
                             Country = h.HCountry,
                             DischargeResID = p.DischargeResidence
                         }).ToList();

            var data = (from d in _data
                        join f in Entity.Residences on Convert.ToInt16(d.DischargeResID) equals f.ResidenceID
                        where d.StartDate.IsEarlierThan(new DateTime(year, month, 1))
                        select new ReportQueryModel
                        {
                            DOB = d.DOB,
                            HospitalID = d.HospitalID,
                            Hospital = d.Hospital,
                            AdmissionViaED = d.AdmissionViaED,
                            ArrivalDateTime = d.ArrivalDateTime,
                            InHospFractureDateTime = d.InHospFractureDateTime,
                            TransferDateTime = d.TransferDateTime,
                            DepartureDateTime = d.DepartureDateTime,
                            State = d.State,
                            Country = d.Country,
                            DischargeResID = d.DischargeResID,
                            DischargeResName = f.Address
                        }).ToList();

            #endregion DB Query

            if (data.Count > 0)
            {
                string[] keys = Entity.Residences.OrderBy(p => p.ResidenceID).Select(x => x.Address).ToArray<string>();

                #region Hospital

                List<object> dataArray = new List<object>();

                for (int i = 0; i < keys.Count(); i++)
                {
                    dataArray.Add(data.Where(x => x.HospitalID == hospital.HospitalID && x.DischargeResName == keys[i]).Count());
                }

                dataArray = GetAverageDischargeDestination(dataArray);

                ReportModel hospitalModel = new ReportModel { Label = hospital.HName, Data = dataArray };

                #endregion Hospital Data

                #region State Data

                ReportModel stateModel = null;
                if (ConfigurationManager.AppSettings["Location"] == "Australian")
                {
                    dataArray = new List<object>();

                    for (int i = 0; i < keys.Count(); i++)
                    {
                        dataArray.Add(data.Where(x => x.State == hospital.HState && x.DischargeResName == keys[i]).Count());
                    }

                    dataArray = GetAverageDischargeDestination(dataArray);

                    stateModel = new ReportModel { Label = hospital.HState, Data = dataArray };
                }

                #endregion State

                #region Nation

                dataArray = new List<object>();

                for (int i = 0; i < keys.Count(); i++)
                {
                    dataArray.Add(data.Where(x => x.Country == hospital.HCountry && x.DischargeResName == keys[i]).Count());
                }

                dataArray = GetAverageDischargeDestination(dataArray);

                ReportModel nationData = new ReportModel { Label = hospital.HCountry, Data = dataArray };

                #endregion Nation

                List<List<object>> reportData = new List<List<object>>();
                reportData.Add(hospitalModel.Data);

                if (stateModel != null)
                {
                    reportData.Add(stateModel.Data);
                }

                reportData.Add(nationData.Data);

                List<string> reportLegends = new List<string>();
                reportLegends.Add(hospitalModel.Label);

                if (stateModel != null)
                {
                    reportLegends.Add(stateModel.Label);
                }

                reportLegends.Add(nationData.Label);

                var report = Entity.Reports.Where(r => r.Code == reportType).FirstOrDefault();

                return new { Data = reportData, Labels = keys, Legends = reportLegends, IsAverage = true, Rotatelabel = true, ChartTitle = report.Title, xTitle = report.XLabel, yTitle = report.YLabel, Description = report.Description };
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Time to Surgery Report

        public static object TimeToSurgeryReport(string reportType, Hospital hospital, int month, int year)
        {
            #region DB Query

            ANZHFREntities Entity = new ANZHFREntities();

            var quarters = GetQuarters(new DateTime(year, month, 1));

            DateTime startDate = quarters.First().StartDate;
            DateTime endDate = quarters.Last().EndDate;

            var data = (from p in Entity.Patients
                        join h in Entity.Hospitals on p.HospitalID equals h.HospitalID
                        where h.HCountry == hospital.HCountry &&
                        p.ArrivalDateTime >= startDate &&
                        p.ArrivalDateTime < endDate &&
                        p.SurgeryDateTime >= p.ArrivalDateTime
                        select new ReportQueryModel
                        {
                            HospitalID = p.HospitalID,
                            Hospital = h.HName,
                            AdmissionViaED = p.AdmissionViaED,
                            ArrivalDateTime = p.ArrivalDateTime,
                            InHospFractureDateTime = p.InHospFractureDateTime,
                            TransferDateTime = p.TransferDateTime,
                            DepartureDateTime = p.DepartureDateTime,
                            SurgeryDateTime = p.SurgeryDateTime,
                            State = h.HState,
                            Country = h.HCountry
                        }).ToList();

            #endregion DB Query

            if (data.Count > 0)
            {
                #region Hospital

                var filteredList = data.Where(x => x.HospitalID == hospital.HospitalID).ToList();

                List<object> rd = GetTimeToSurgery(filteredList, quarters);

                ReportModel hospitalModel = new ReportModel { Label = hospital.HName, Data = rd };

                #endregion Hospital

                #region State

                ReportModel stateModel = null;
                if (ConfigurationManager.AppSettings["Location"] == "Australian")
                {
                    filteredList = data.Where(x => x.State == hospital.HState).ToList();
                    rd = GetTimeToSurgery(filteredList, quarters);

                    stateModel = new ReportModel { Label = hospital.HState, Data = rd };
                }

                #endregion State

                #region National average

                var nationalAvgList = GetTimeToSurgery(data, quarters);
                ReportModel nationModel = new ReportModel { Label = hospital.HCountry, Data = nationalAvgList };

                #endregion Nation

                List<List<object>> reportData = new List<List<object>>();
                reportData.Add(hospitalModel.Data);

                if (stateModel != null)
                {
                    reportData.Add(stateModel.Data);
                }

                reportData.Add(nationModel.Data);

                List<string> reportLegends = new List<string>();
                reportLegends.Add(hospitalModel.Label);

                if (stateModel != null)
                {
                    reportLegends.Add(stateModel.Label);
                }

                reportLegends.Add(nationModel.Label);

                var report = Entity.Reports.Where(r => r.Code == reportType).FirstOrDefault();

                return new { Data = reportData, Labels = GetQuarterLabels(quarters).ToArray(), Legends = reportLegends, IsAverage = true, Rotatelabel = false, ChartTitle = report.Title, xTitle = report.XLabel, yTitle = report.YLabel, Description = report.Description };
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Cognitive State

        public static object CognitiveStateReport(string reportType, Hospital hospital, int month, int year)
        {
            #region DB Query

            ANZHFREntities Entity = new ANZHFREntities();

            var quarters = GetQuarters(new DateTime(year, month, 1));

            DateTime startDate = quarters.First().StartDate;
            DateTime endDate = quarters.Last().EndDate;

            var data = (from p in Entity.Patients
                        join h in Entity.Hospitals on p.HospitalID equals h.HospitalID
                        where h.HCountry == hospital.HCountry &&
                        p.ArrivalDateTime >= startDate &&
                        p.ArrivalDateTime < endDate &&
                        p.CognitiveState != null &&
                        p.CognitiveState != ""
                        select new ReportQueryModel
                        {
                            HospitalID = p.HospitalID,
                            Hospital = h.HName,
                            AdmissionViaED = p.AdmissionViaED,
                            ArrivalDateTime = p.ArrivalDateTime,
                            InHospFractureDateTime = p.InHospFractureDateTime,
                            TransferDateTime = p.TransferDateTime,
                            DepartureDateTime = p.DepartureDateTime,
                            SurgeryDateTime = p.SurgeryDateTime,
                            CognitiveState = p.CognitiveState,
                            State = h.HState,
                            Country = h.HCountry
                        }).ToList();

            #endregion DB Query

            if (data.Count > 0)
            {
                #region Hospital

                var filteredList = data.Where(x => x.HospitalID == hospital.HospitalID).ToList();

                List<object> rd = GetCognitiveState(filteredList, quarters);

                ReportModel hospitalModel = new ReportModel { Label = hospital.HName, Data = rd };

                #endregion Hospital

                #region State

                ReportModel stateModel = null;
                if (ConfigurationManager.AppSettings["Location"] == "Australian")
                {
                    filteredList = data.Where(x => x.State == hospital.HState).ToList();
                    rd = GetCognitiveState(filteredList, quarters);

                    stateModel = new ReportModel { Label = hospital.HState, Data = rd };
                }

                #endregion State

                #region National average

                var nationalAvgList = GetCognitiveState(data, quarters);
                ReportModel nationModel = new ReportModel { Label = hospital.HCountry, Data = nationalAvgList };

                #endregion Nation

                List<List<object>> reportData = new List<List<object>>();
                reportData.Add(hospitalModel.Data);

                if (stateModel != null)
                {
                    reportData.Add(stateModel.Data);
                }

                reportData.Add(nationModel.Data);

                List<string> reportLegends = new List<string>();
                reportLegends.Add(hospitalModel.Label);

                if (stateModel != null)
                {
                    reportLegends.Add(stateModel.Label);
                }

                reportLegends.Add(nationModel.Label);

                var report = Entity.Reports.Where(r => r.Code == reportType).FirstOrDefault();

                return new { Data = reportData, Labels = GetQuarterLabels(quarters).ToArray(), Legends = reportLegends, IsAverage = true, Rotatelabel = false, ChartTitle = report.Title, xTitle = report.XLabel, yTitle = report.YLabel, Description = report.Description };
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Analgesia

        public static object AnalgesiaReport(string reportType, Hospital hospital, int month, int year)
        {
            #region DB Query

            ANZHFREntities Entity = new ANZHFREntities();

            var quarters = GetQuarters(new DateTime(year, month, 1));

            DateTime startDate = quarters.First().StartDate;
            DateTime endDate = quarters.Last().EndDate;

            var data = (from p in Entity.Patients
                        join h in Entity.Hospitals on p.HospitalID equals h.HospitalID
                        where h.HCountry == hospital.HCountry &&
                        p.ArrivalDateTime >= startDate &&
                        p.ArrivalDateTime < endDate &&
                        p.Analgesia != null &&
                        p.Analgesia != ""
                        select new ReportQueryModel
                        {
                            HospitalID = p.HospitalID,
                            Hospital = h.HName,
                            AdmissionViaED = p.AdmissionViaED,
                            ArrivalDateTime = p.ArrivalDateTime,
                            InHospFractureDateTime = p.InHospFractureDateTime,
                            TransferDateTime = p.TransferDateTime,
                            DepartureDateTime = p.DepartureDateTime,
                            SurgeryDateTime = p.SurgeryDateTime,
                            Analgesia = p.Analgesia,
                            State = h.HState,
                            Country = h.HCountry
                        }).ToList();

            #endregion DB Query

            if (data.Count > 0)
            {
                #region Hospital

                var filteredList = data.Where(x => x.HospitalID == hospital.HospitalID).ToList();

                List<object> rd = GetAnalgesia(filteredList, quarters);

                ReportModel hospitalModel = new ReportModel { Label = hospital.HName, Data = rd };

                #endregion Hospital

                #region State

                ReportModel stateModel = null;
                if (ConfigurationManager.AppSettings["Location"] == "Australian")
                {
                    filteredList = data.Where(x => x.State == hospital.HState).ToList();
                    rd = GetAnalgesia(filteredList, quarters);

                    stateModel = new ReportModel { Label = hospital.HState, Data = rd };
                }

                #endregion State

                #region National average

                var nationalAvgList = GetAnalgesia(data, quarters);
                ReportModel nationModel = new ReportModel { Label = hospital.HCountry, Data = nationalAvgList };

                #endregion Nation

                List<List<object>> reportData = new List<List<object>>();
                reportData.Add(hospitalModel.Data);

                if (stateModel != null)
                {
                    reportData.Add(stateModel.Data);
                }

                reportData.Add(nationModel.Data);

                List<string> reportLegends = new List<string>();
                reportLegends.Add(hospitalModel.Label);

                if (stateModel != null)
                {
                    reportLegends.Add(stateModel.Label);
                }

                reportLegends.Add(nationModel.Label);

                var report = Entity.Reports.Where(r => r.Code == reportType).FirstOrDefault();

                return new { Data = reportData, Labels = GetQuarterLabels(quarters).ToArray(), Legends = reportLegends, IsAverage = true, Rotatelabel = false, ChartTitle = report.Title, xTitle = report.XLabel, yTitle = report.YLabel, Description = report.Description };
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Medical Assessment

        public static object MedicalAssessmentReport(string reportType, Hospital hospital, int month, int year)
        {
            #region DB Query

            ANZHFREntities Entity = new ANZHFREntities();

            var quarters = GetQuarters(new DateTime(year, month, 1));

            DateTime startDate = quarters.First().StartDate;
            DateTime endDate = quarters.Last().EndDate;

            var data = (from p in Entity.Patients
                        join h in Entity.Hospitals on p.HospitalID equals h.HospitalID
                        where h.HCountry == hospital.HCountry &&
                        p.ArrivalDateTime >= startDate &&
                        p.ArrivalDateTime < endDate &&
                        p.PreOpMedAss != null
                        select new ReportQueryModel
                        {
                            HospitalID = p.HospitalID,
                            Hospital = h.HName,
                            AdmissionViaED = p.AdmissionViaED,
                            ArrivalDateTime = p.ArrivalDateTime,
                            InHospFractureDateTime = p.InHospFractureDateTime,
                            TransferDateTime = p.TransferDateTime,
                            DepartureDateTime = p.DepartureDateTime,
                            SurgeryDateTime = p.SurgeryDateTime,
                            PreOpMedAss = p.PreOpMedAss,
                            State = h.HState,
                            Country = h.HCountry
                        }).ToList();

            #endregion DB Query

            if (data.Count > 0)
            {
                #region Hospital

                var filteredList = data.Where(x => x.HospitalID == hospital.HospitalID).ToList();

                List<object> rd = GetPreOpMedAss(filteredList, quarters);

                ReportModel hospitalModel = new ReportModel { Label = hospital.HName, Data = rd };

                #endregion Hospital

                #region State

                ReportModel stateModel = null;
                if (ConfigurationManager.AppSettings["Location"] == "Australian")
                {
                    filteredList = data.Where(x => x.State == hospital.HState).ToList();
                    rd = GetPreOpMedAss(filteredList, quarters);

                    stateModel = new ReportModel { Label = hospital.HState, Data = rd };
                }

                #endregion State

                #region National average

                var nationalAvgList = GetPreOpMedAss(data, quarters);
                ReportModel nationModel = new ReportModel { Label = hospital.HCountry, Data = nationalAvgList };

                #endregion Nation

                List<List<object>> reportData = new List<List<object>>();
                reportData.Add(hospitalModel.Data);

                if (stateModel != null)
                {
                    reportData.Add(stateModel.Data);
                }

                reportData.Add(nationModel.Data);

                List<string> reportLegends = new List<string>();
                reportLegends.Add(hospitalModel.Label);

                if (stateModel != null)
                {
                    reportLegends.Add(stateModel.Label);
                }

                reportLegends.Add(nationModel.Label);

                var report = Entity.Reports.Where(r => r.Code == reportType).FirstOrDefault();

                return new { Data = reportData, Labels = GetQuarterLabels(quarters).ToArray(), Legends = reportLegends, IsAverage = true, Rotatelabel = false, ChartTitle = report.Title, xTitle = report.XLabel, yTitle = report.YLabel, Description = report.Description };
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Mobilisation

        public static object MobilisationReport(string reportType, Hospital hospital, int month, int year)
        {
            #region DB Query

            ANZHFREntities Entity = new ANZHFREntities();

            var quarters = GetQuarters(new DateTime(year, month, 1));

            DateTime startDate = quarters.First().StartDate;
            DateTime endDate = quarters.Last().EndDate;

            var data = (from p in Entity.Patients
                        join h in Entity.Hospitals on p.HospitalID equals h.HospitalID
                        where h.HCountry == hospital.HCountry &&
                        p.ArrivalDateTime >= startDate &&
                        p.ArrivalDateTime < endDate &&
                        p.Mobilisation != null
                        select new ReportQueryModel
                        {
                            HospitalID = p.HospitalID,
                            Hospital = h.HName,
                            AdmissionViaED = p.AdmissionViaED,
                            ArrivalDateTime = p.ArrivalDateTime,
                            InHospFractureDateTime = p.InHospFractureDateTime,
                            TransferDateTime = p.TransferDateTime,
                            DepartureDateTime = p.DepartureDateTime,
                            SurgeryDateTime = p.SurgeryDateTime,
                            Mobilisation = p.Mobilisation,
                            State = h.HState,
                            Country = h.HCountry
                        }).ToList();

            #endregion DB Query

            if (data.Count > 0)
            {
                #region Hospital

                var filteredList = data.Where(x => x.HospitalID == hospital.HospitalID).ToList();

                List<object> rd = GetMobilisation(filteredList, quarters);

                ReportModel hospitalModel = new ReportModel { Label = hospital.HName, Data = rd };

                #endregion Hospital

                #region State

                ReportModel stateModel = null;
                if (ConfigurationManager.AppSettings["Location"] == "Australian")
                {
                    filteredList = data.Where(x => x.State == hospital.HState).ToList();
                    rd = GetMobilisation(filteredList, quarters);

                    stateModel = new ReportModel { Label = hospital.HState, Data = rd };
                }

                #endregion State

                #region National average

                var nationalAvgList = GetMobilisation(data, quarters);
                ReportModel nationModel = new ReportModel { Label = hospital.HCountry, Data = nationalAvgList };

                #endregion Nation

                List<List<object>> reportData = new List<List<object>>();
                reportData.Add(hospitalModel.Data);

                if (stateModel != null)
                {
                    reportData.Add(stateModel.Data);
                }

                reportData.Add(nationModel.Data);

                List<string> reportLegends = new List<string>();
                reportLegends.Add(hospitalModel.Label);

                if (stateModel != null)
                {
                    reportLegends.Add(stateModel.Label);
                }

                reportLegends.Add(nationModel.Label);

                var report = Entity.Reports.Where(r => r.Code == reportType).FirstOrDefault();

                return new { Data = reportData, Labels = GetQuarterLabels(quarters).ToArray(), Legends = reportLegends, IsAverage = true, Rotatelabel = false, ChartTitle = report.Title, xTitle = report.XLabel, yTitle = report.YLabel, Description = report.Description };
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Weight Bearing

        public static object WeightBearingReport(string reportType, Hospital hospital, int month, int year)
        {
            #region DB Query

            ANZHFREntities Entity = new ANZHFREntities();

            var quarters = GetQuarters(new DateTime(year, month, 1));

            DateTime startDate = quarters.First().StartDate;
            DateTime endDate = quarters.Last().EndDate;

            var data = (from p in Entity.Patients
                        join h in Entity.Hospitals on p.HospitalID equals h.HospitalID
                        where h.HCountry == hospital.HCountry &&
                        p.ArrivalDateTime >= startDate &&
                        p.ArrivalDateTime < endDate &&
                        p.FullWeightBear != null
                        select new ReportQueryModel
                        {
                            HospitalID = p.HospitalID,
                            Hospital = h.HName,
                            AdmissionViaED = p.AdmissionViaED,
                            ArrivalDateTime = p.ArrivalDateTime,
                            InHospFractureDateTime = p.InHospFractureDateTime,
                            TransferDateTime = p.TransferDateTime,
                            DepartureDateTime = p.DepartureDateTime,
                            SurgeryDateTime = p.SurgeryDateTime,
                            FullWeightBear = p.FullWeightBear,
                            State = h.HState,
                            Country = h.HCountry
                        }).ToList();

            #endregion DB Query

            if (data.Count > 0)
            {
                #region Hospital

                var filteredList = data.Where(x => x.HospitalID == hospital.HospitalID).ToList();

                List<object> rd = GetWeightBearing(filteredList, quarters);

                ReportModel hospitalModel = new ReportModel { Label = hospital.HName, Data = rd };

                #endregion Hospital

                #region State

                ReportModel stateModel = null;
                if (ConfigurationManager.AppSettings["Location"] == "Australian")
                {
                    filteredList = data.Where(x => x.State == hospital.HState).ToList();
                    rd = GetWeightBearing(filteredList, quarters);

                    stateModel = new ReportModel { Label = hospital.HState, Data = rd };
                }

                #endregion State

                #region National average

                var nationalAvgList = GetWeightBearing(data, quarters);
                ReportModel nationModel = new ReportModel { Label = hospital.HCountry, Data = nationalAvgList };

                #endregion Nation

                List<List<object>> reportData = new List<List<object>>();
                reportData.Add(hospitalModel.Data);

                if (stateModel != null)
                {
                    reportData.Add(stateModel.Data);
                }

                reportData.Add(nationModel.Data);

                List<string> reportLegends = new List<string>();
                reportLegends.Add(hospitalModel.Label);

                if (stateModel != null)
                {
                    reportLegends.Add(stateModel.Label);
                }

                reportLegends.Add(nationModel.Label);

                var report = Entity.Reports.Where(r => r.Code == reportType).FirstOrDefault();

                return new { Data = reportData, Labels = GetQuarterLabels(quarters).ToArray(), Legends = reportLegends, IsAverage = true, Rotatelabel = false, ChartTitle = report.Title, xTitle = report.XLabel, yTitle = report.YLabel, Description = report.Description };
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Pressure Ulcers

        public static object PressureUlcersReport(string reportType, Hospital hospital, int month, int year)
        {
            #region DB Query

            ANZHFREntities Entity = new ANZHFREntities();

            var quarters = GetQuarters(new DateTime(year, month, 1));

            DateTime startDate = quarters.First().StartDate;
            DateTime endDate = quarters.Last().EndDate;

            var data = (from p in Entity.Patients
                        join h in Entity.Hospitals on p.HospitalID equals h.HospitalID
                        where h.HCountry == hospital.HCountry &&
                        p.ArrivalDateTime >= startDate &&
                        p.ArrivalDateTime < endDate &&
                        p.PressureUlcers != null
                        select new ReportQueryModel
                        {
                            HospitalID = p.HospitalID,
                            Hospital = h.HName,
                            AdmissionViaED = p.AdmissionViaED,
                            ArrivalDateTime = p.ArrivalDateTime,
                            InHospFractureDateTime = p.InHospFractureDateTime,
                            TransferDateTime = p.TransferDateTime,
                            DepartureDateTime = p.DepartureDateTime,
                            SurgeryDateTime = p.SurgeryDateTime,
                            PressureUlcers = p.PressureUlcers,
                            State = h.HState,
                            Country = h.HCountry
                        }).ToList();

            #endregion DB Query

            if (data.Count > 0)
            {
                #region Hospital

                var filteredList = data.Where(x => x.HospitalID == hospital.HospitalID).ToList();

                List<object> rd = GetPressureUlcers(filteredList, quarters);

                ReportModel hospitalModel = new ReportModel { Label = hospital.HName, Data = rd };

                #endregion Hospital

                #region State

                ReportModel stateModel = null;
                if (ConfigurationManager.AppSettings["Location"] == "Australian")
                {
                    filteredList = data.Where(x => x.State == hospital.HState).ToList();
                    rd = GetPressureUlcers(filteredList, quarters);

                    stateModel = new ReportModel { Label = hospital.HState, Data = rd };
                }

                #endregion State

                #region National average

                var nationalAvgList = GetPressureUlcers(data, quarters);
                ReportModel nationModel = new ReportModel { Label = hospital.HCountry, Data = nationalAvgList };

                #endregion Nation

                List<List<object>> reportData = new List<List<object>>();
                reportData.Add(hospitalModel.Data);

                if (stateModel != null)
                {
                    reportData.Add(stateModel.Data);
                }

                reportData.Add(nationModel.Data);

                List<string> reportLegends = new List<string>();
                reportLegends.Add(hospitalModel.Label);

                if (stateModel != null)
                {
                    reportLegends.Add(stateModel.Label);
                }

                reportLegends.Add(nationModel.Label);

                var report = Entity.Reports.Where(r => r.Code == reportType).FirstOrDefault();

                return new { Data = reportData, Labels = GetQuarterLabels(quarters).ToArray(), Legends = reportLegends, IsAverage = true, Rotatelabel = false, ChartTitle = report.Title, xTitle = report.XLabel, yTitle = report.YLabel, Description = report.Description };
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Bone Medication on Discharge

        public static object BoneMedicationReport(string reportType, Hospital hospital, int month, int year)
        {
            #region DB Query

            ANZHFREntities Entity = new ANZHFREntities();

            var quarters = GetQuarters(new DateTime(year, month, 1));

            DateTime startDate = quarters.First().StartDate;
            DateTime endDate = quarters.Last().EndDate;

            var data = (from p in Entity.Patients
                        join h in Entity.Hospitals on p.HospitalID equals h.HospitalID
                        where h.HCountry == hospital.HCountry &&
                        p.ArrivalDateTime >= startDate &&
                        p.ArrivalDateTime < endDate &&
                        p.BoneMedDischarge != null
                        select new ReportQueryModel
                        {
                            HospitalID = p.HospitalID,
                            Hospital = h.HName,
                            AdmissionViaED = p.AdmissionViaED,
                            ArrivalDateTime = p.ArrivalDateTime,
                            InHospFractureDateTime = p.InHospFractureDateTime,
                            TransferDateTime = p.TransferDateTime,
                            DepartureDateTime = p.DepartureDateTime,
                            SurgeryDateTime = p.SurgeryDateTime,
                            BoneMedDischarge = p.BoneMedDischarge,
                            State = h.HState,
                            Country = h.HCountry
                        }).ToList();

            #endregion DB Query

            if (data.Count > 0)
            {
                #region Hospital

                var filteredList = data.Where(x => x.HospitalID == hospital.HospitalID).ToList();

                List<object> rd = GetBoneMedDischarge(filteredList, quarters);

                ReportModel hospitalModel = new ReportModel { Label = hospital.HName, Data = rd };

                #endregion Hospital

                #region State

                ReportModel stateModel = null;
                if (ConfigurationManager.AppSettings["Location"] == "Australian")
                {
                    filteredList = data.Where(x => x.State == hospital.HState).ToList();
                    rd = GetBoneMedDischarge(filteredList, quarters);

                    stateModel = new ReportModel { Label = hospital.HState, Data = rd };
                }

                #endregion State

                #region National average

                var nationalAvgList = GetBoneMedDischarge(data, quarters);
                ReportModel nationModel = new ReportModel { Label = hospital.HCountry, Data = nationalAvgList };

                #endregion Nation

                List<List<object>> reportData = new List<List<object>>();
                reportData.Add(hospitalModel.Data);

                if (stateModel != null)
                {
                    reportData.Add(stateModel.Data);
                }

                reportData.Add(nationModel.Data);

                List<string> reportLegends = new List<string>();
                reportLegends.Add(hospitalModel.Label);

                if (stateModel != null)
                {
                    reportLegends.Add(stateModel.Label);
                }

                reportLegends.Add(nationModel.Label);

                var report = Entity.Reports.Where(r => r.Code == reportType).FirstOrDefault();

                return new { Data = reportData, Labels = GetQuarterLabels(quarters).ToArray(), Legends = reportLegends, IsAverage = true, Rotatelabel = false, ChartTitle = report.Title, xTitle = report.XLabel, yTitle = report.YLabel, Description = report.Description };
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Reoperation 30

        public static object ReoperationReport(string reportType, Hospital hospital, int month, int year)
        {
            #region DB Query

            ANZHFREntities Entity = new ANZHFREntities();

            var quarters = GetQuarters(new DateTime(year, month, 1));

            DateTime startDate = quarters.First().StartDate;
            DateTime endDate = quarters.Last().EndDate;

            var data = (from p in Entity.Patients
                        join h in Entity.Hospitals on p.HospitalID equals h.HospitalID
                        where h.HCountry == hospital.HCountry &&
                        p.ArrivalDateTime >= startDate &&
                        p.ArrivalDateTime < endDate &&
                        p.Reoperation30 != null
                        select new ReportQueryModel
                        {
                            HospitalID = p.HospitalID,
                            Hospital = h.HName,
                            AdmissionViaED = p.AdmissionViaED,
                            ArrivalDateTime = p.ArrivalDateTime,
                            InHospFractureDateTime = p.InHospFractureDateTime,
                            TransferDateTime = p.TransferDateTime,
                            DepartureDateTime = p.DepartureDateTime,
                            SurgeryDateTime = p.SurgeryDateTime,
                            Reoperation30 = p.Reoperation30,
                            State = h.HState,
                            Country = h.HCountry
                        }).ToList();

            #endregion DB Query

            if (data.Count > 0)
            {
                #region Hospital

                var filteredList = data.Where(x => x.HospitalID == hospital.HospitalID).ToList();

                List<object> rd = GetReoperation30(filteredList, quarters);

                ReportModel hospitalModel = new ReportModel { Label = hospital.HName, Data = rd };

                #endregion Hospital

                #region State

                ReportModel stateModel = null;
                if (ConfigurationManager.AppSettings["Location"] == "Australian")
                {
                    filteredList = data.Where(x => x.State == hospital.HState).ToList();
                    rd = GetReoperation30(filteredList, quarters);

                    stateModel = new ReportModel { Label = hospital.HState, Data = rd };
                }

                #endregion State

                #region National average

                var nationalAvgList = GetReoperation30(data, quarters);
                ReportModel nationModel = new ReportModel { Label = hospital.HCountry, Data = nationalAvgList };

                #endregion Nation

                List<List<object>> reportData = new List<List<object>>();
                reportData.Add(hospitalModel.Data);

                if (stateModel != null)
                {
                    reportData.Add(stateModel.Data);
                }

                reportData.Add(nationModel.Data);

                List<string> reportLegends = new List<string>();
                reportLegends.Add(hospitalModel.Label);

                if (stateModel != null)
                {
                    reportLegends.Add(stateModel.Label);
                }

                reportLegends.Add(nationModel.Label);

                var report = Entity.Reports.Where(r => r.Code == reportType).FirstOrDefault();

                return new { Data = reportData, Labels = GetQuarterLabels(quarters).ToArray(), Legends = reportLegends, IsAverage = true, Rotatelabel = false, ChartTitle = report.Title, xTitle = report.XLabel, yTitle = report.YLabel, Description = report.Description };
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Best Practice

        public static object SurgeryBestPracticeReport(string reportType, Hospital hospital, int month, int year)
        {
            #region DB Query

            ANZHFREntities Entity = new ANZHFREntities();

            var quarters = GetQuarters(new DateTime(year, month, 1));

            DateTime startDate = quarters.First().StartDate;
            DateTime endDate = quarters.Last().EndDate;

            var data = (from p in Entity.Patients
                        join h in Entity.Hospitals on p.HospitalID equals h.HospitalID
                        where h.HCountry == hospital.HCountry &&
                        p.ArrivalDateTime >= startDate &&
                        p.ArrivalDateTime < endDate &&
                        p.SurgeryDateTime >= p.ArrivalDateTime
                        select new ReportQueryModel
                        {
                            HospitalID = p.HospitalID,
                            Hospital = h.HName,
                            AdmissionViaED = p.AdmissionViaED,
                            ArrivalDateTime = p.ArrivalDateTime,
                            InHospFractureDateTime = p.InHospFractureDateTime,
                            TransferDateTime = p.TransferDateTime,
                            DepartureDateTime = p.DepartureDateTime,
                            SurgeryDateTime = p.SurgeryDateTime,
                            State = h.HState,
                            Country = h.HCountry
                        }).ToList();

            #endregion DB Query

            if (data.Count > 0)
            {
                #region Hospital

                var filteredList = data.Where(x => x.HospitalID == hospital.HospitalID).ToList();

                List<object> rd = GetSurgeryBestPractice(filteredList, quarters);

                ReportModel hospitalModel = new ReportModel { Label = hospital.HName, Data = rd };

                #endregion Hospital

                #region State

                ReportModel stateModel = null;
                if (ConfigurationManager.AppSettings["Location"] == "Australian")
                {
                    filteredList = data.Where(x => x.State == hospital.HState).ToList();
                    rd = GetSurgeryBestPractice(filteredList, quarters);

                    stateModel = new ReportModel { Label = hospital.HState, Data = rd };
                }

                #endregion State

                #region National average

                var nationalAvgList = GetSurgeryBestPractice(data, quarters);
                ReportModel nationModel = new ReportModel { Label = hospital.HCountry, Data = nationalAvgList };

                #endregion Nation

                List<List<object>> reportData = new List<List<object>>();
                reportData.Add(hospitalModel.Data);

                if (stateModel != null)
                {
                    reportData.Add(stateModel.Data);
                }

                reportData.Add(nationModel.Data);

                List<string> reportLegends = new List<string>();
                reportLegends.Add(hospitalModel.Label);

                if (stateModel != null)
                {
                    reportLegends.Add(stateModel.Label);
                }

                reportLegends.Add(nationModel.Label);

                var report = Entity.Reports.Where(r => r.Code == reportType).FirstOrDefault();

                return new { Data = reportData, Labels = GetQuarterLabels(quarters).ToArray(), Legends = reportLegends, IsAverage = true, Rotatelabel = false, ChartTitle = report.Title, xTitle = report.XLabel, yTitle = report.YLabel, Description = report.Description };
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Survival 30 Days

        public static object Survival30Days(string reportType, Hospital hospital, int month, int year)
        {
            #region DB Query

            ANZHFREntities Entity = new ANZHFREntities();

            var quarters = GetQuarters(new DateTime(year, month, 1));

            DateTime startDate = quarters.First().StartDate;
            DateTime endDate = quarters.Last().EndDate;

            var data = (from p in Entity.Patients
                        join h in Entity.Hospitals on p.HospitalID equals h.HospitalID
                        where p.Survival30 != "" && p.Survival30 != null &&
                        p.FollowupDate30 >= startDate &&
                        p.FollowupDate30 < endDate
                        select new ReportQueryModel
                        {
                            HospitalID = p.HospitalID,
                            Hospital = h.HName,
                            FollowupDate30 = p.FollowupDate30,
                            Survival30 = p.Survival30,
                            State = h.HState,
                            Country = h.HCountry
                        }).ToList();

            #endregion DB Query

            if (data.Count > 0)
            {
                #region Hospital

                var filteredList = data.Where(x => x.HospitalID == hospital.HospitalID).ToList();

                List<object> rd = GetSurvival30DaysData(filteredList, quarters);

                ReportModel hospitalModel = new ReportModel { Label = hospital.HName, Data = rd };

                #endregion Hospital

                #region State

                ReportModel stateModel = null;
                if (ConfigurationManager.AppSettings["Location"] == "Australian")
                {
                    filteredList = data.Where(x => x.State == hospital.HState).ToList();
                    rd = GetSurvival30DaysData(filteredList, quarters);

                    stateModel = new ReportModel { Label = hospital.HState, Data = rd };
                }

                #endregion State

                #region National average

                var nationalAvgList = GetSurvival30DaysData(data, quarters);
                ReportModel nationModel = new ReportModel { Label = hospital.HCountry, Data = nationalAvgList };

                #endregion Nation

                List<List<object>> reportData = new List<List<object>>();
                reportData.Add(hospitalModel.Data);

                if (stateModel != null)
                {
                    reportData.Add(stateModel.Data);
                }

                reportData.Add(nationModel.Data);

                List<string> reportLegends = new List<string>();
                reportLegends.Add(hospitalModel.Label);

                if (stateModel != null)
                {
                    reportLegends.Add(stateModel.Label);
                }

                reportLegends.Add(nationModel.Label);

                var report = Entity.Reports.Where(r => r.Code == reportType).FirstOrDefault();

                return new { Data = reportData, Labels = GetQuarterLabels(quarters).ToArray(), Legends = reportLegends, IsAverage = true, Rotatelabel = false, ChartTitle = report.Title, xTitle = report.XLabel, yTitle = report.YLabel, Description = report.Description };
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Total Entered Report

        public static object TotalEnteredReport(string reportType, Hospital hospital, int month, int year)
        {
            #region DB Query

            ANZHFREntities Entity = new ANZHFREntities();

            var quarters = GetQuarters(new DateTime(year, month, 1));

            DateTime startDate = quarters.First().StartDate;
            DateTime endDate = quarters.Last().EndDate;

            var data = (from p in Entity.Patients
                        join h in Entity.Hospitals on p.HospitalID equals h.HospitalID
                        where h.HCountry == hospital.HCountry &&
                        p.ArrivalDateTime.Value >= startDate &&
                        p.ArrivalDateTime.Value < endDate
                        select new ReportQueryModel
                        {
                            HospitalID = p.HospitalID,
                            State = h.HState,
                            Country = h.HCountry,
                            AdmissionViaED = p.AdmissionViaED,
                            ArrivalDateTime = p.ArrivalDateTime,
                            InHospFractureDateTime = p.InHospFractureDateTime,
                            TransferDateTime = p.TransferDateTime,
                            DepartureDateTime = p.DepartureDateTime
                        }).ToList();

            #endregion DB Query

            if (data.Count > 0)
            {
                #region Hospital

                var filteredList = data.Where(x => x.HospitalID == hospital.HospitalID).ToList();

                List<object> rd = GetTotalEnteredGroupData(filteredList, quarters);

                ReportModel hospitalModel = new ReportModel { Label = hospital.HName, Data = rd };

                #endregion Hospital

                #region State

                ReportModel stateModel = null;
                if (ConfigurationManager.AppSettings["Location"] == "Australian")
                {
                    filteredList = data.Where(x => x.State == hospital.HState).ToList();
                    rd = GetTotalEnteredGroupData(filteredList, quarters);

                    stateModel = new ReportModel { Label = hospital.HState, Data = rd };
                }

                #endregion State

                #region National average

                var nationalAvgList = GetTotalEnteredGroupData(data, quarters);
                ReportModel nationModel = new ReportModel { Label = hospital.HCountry, Data = nationalAvgList };

                #endregion Nation

                List<List<object>> reportData = new List<List<object>>();
                reportData.Add(hospitalModel.Data);
                if (stateModel != null)
                {
                    reportData.Add(stateModel.Data);
                }

                reportData.Add(nationModel.Data);

                List<string> reportLegends = new List<string>();
                reportLegends.Add(hospitalModel.Label);

                if (stateModel != null)
                {
                    reportLegends.Add(stateModel.Label);
                }

                reportLegends.Add(nationModel.Label);

                var report = Entity.Reports.Where(r => r.Code == reportType).FirstOrDefault();

                return new { Data = reportData, Labels = GetQuarterLabels(quarters).ToArray(), Legends = reportLegends, IsAverage = false, Rotatelabel = false, ChartTitle = report.Title, xTitle = report.XLabel, yTitle = report.YLabel, Description = report.Description };
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Estimated v Actual

        public static object EstimatedActualReport(string reportType, Hospital hospital, int month, int year)
        {
            #region DB Query

            ANZHFREntities Entity = new ANZHFREntities();

            DateTime startDate = new DateTime(year, month, 1);
            DateTime finYearStart = new DateTime(year, 7, 1);

            if (startDate < finYearStart)
            {
                startDate = startDate.AddYears(-1);
            }

            DateTime endDate = startDate.AddYears(1).AddDays(-1);

            var data = (from p in Entity.Patients
                        join h in Entity.Hospitals on p.HospitalID equals h.HospitalID
                        where h.HCountry == hospital.HCountry &&
                        p.ArrivalDateTime.Value >= startDate &&
                        p.ArrivalDateTime.Value <= endDate
                        select new ReportQueryModel
                        {
                            HospitalID = p.HospitalID,
                            State = h.HState,
                            Country = h.HCountry
                        }).ToList();

            var surveys = (from s in Entity.Surveys
                           join h in Entity.Hospitals on s.HospitalID equals h.HospitalID
                           where s.Year == startDate.Year
                           select new
                           {
                               Survey = s,
                               Hospital = h
                           }).ToList();

            var estimated = (from s in surveys
                             select new ReportQueryModel
                             {
                                 HospitalID = s.Survey.HospitalID,
                                 State = s.Hospital.HState,
                                 Country = s.Hospital.HCountry,
                                 EstimatedRecords = s.Survey.EstimatedNumberOfFractures
                             }).ToList();

            #endregion DB Query

            if (data.Count > 0)
            {
                #region Hospital

                var filteredList = data.Where(x => x.HospitalID == hospital.HospitalID).ToList();
                var filteredEst = estimated.Where(x => x.HospitalID == hospital.HospitalID).ToList();

                List<object> rd = GetEstimatedActualGroupData(filteredList, filteredEst);

                ReportModel hospitalModel = new ReportModel { Label = hospital.HName, Data = rd };

                #endregion Hospital

                #region State

                ReportModel stateModel = null;
                if (ConfigurationManager.AppSettings["Location"] == "Australian")
                {
                    filteredList = data.Where(x => x.State == hospital.HState).ToList();
                    filteredEst = estimated.Where(x => x.State == hospital.HState).ToList();
                    rd = GetEstimatedActualGroupData(filteredList, filteredEst);

                    stateModel = new ReportModel { Label = hospital.HState, Data = rd };
                }

                #endregion State

                #region National average

                var nationalAvgList = GetEstimatedActualNatioAvgData(data, estimated);
                ReportModel nationModel = new ReportModel { Label = hospital.HCountry, Data = nationalAvgList };

                #endregion Nation

                List<List<object>> reportData = new List<List<object>>();
                List<string> reportLegends = new List<string>();

                if (stateModel != null)
                {
                    reportData.Add(new List<object> { hospitalModel.Data[0], stateModel.Data[0], nationModel.Data[0] });
                    reportData.Add(new List<object> { hospitalModel.Data[1], stateModel.Data[1], nationModel.Data[1] });
                }
                else
                {
                    reportData.Add(new List<object> { hospitalModel.Data[0], nationModel.Data[0] });
                    reportData.Add(new List<object> { hospitalModel.Data[1], nationModel.Data[1] });
                }

                reportLegends.Add("Estimated");
                reportLegends.Add("Actual");

                List<string> labels = new List<string>();
                labels.Add(hospital.HName);

                if (stateModel != null)
                {
                    labels.Add(hospital.HState);
                }

                labels.Add(hospital.HCountry);

                //var report = Entity.Reports.Where(r => r.Code == reportType).FirstOrDefault();

                return new { Data = reportData, Labels = labels, Legends = reportLegends, IsAverage = false, Rotatelabel = false, ChartTitle = "Estimated v Actual Entered Report", xTitle = "Months", yTitle = "Estimated & Actual Count" };
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Private Methods

        #region For Time to Surgery

        private static List<object> GetTimeToSurgery(IEnumerable<ReportQueryModel> filteredList, List<QuarterData> quarters)
        {
            List<object> rd = new List<object>();
            foreach (var quarter in quarters)
            {
                var med = (from x in filteredList
                           where x.StartDate.Value >= quarter.StartDate &&
                           x.StartDate.Value < quarter.EndDate
                           select (double?)(x.SurgeryDateTime.Value - x.StartDate.Value).TotalHours).Median() ?? 0;

                rd.Add(string.Format("{0:0.00}", med));
            }

            return rd;
        }

        #endregion

        #region Cognitive State

        private static List<object> GetCognitiveState(IEnumerable<ReportQueryModel> filteredList, List<QuarterData> quarters)
        {
            List<object> rd = new List<object>();
            int avg = 0, total = 0;
            double cs = 0;
            foreach (var quarter in quarters)
            {
                avg = (from x in filteredList
                       where x.StartDate.Value >= quarter.StartDate &&
                             x.StartDate.Value < quarter.EndDate &&
                            (x.CognitiveState == "1" || x.CognitiveState == "2")
                       select x).Count();

                total = (from x in filteredList
                         where x.StartDate.Value >= quarter.StartDate &&
                             x.StartDate.Value < quarter.EndDate &&
                             x.CognitiveState != null
                         select x).Count();

                cs = total != 0 ? (double)((100 * avg) / total) : 0;

                rd.Add(string.Format("{0:0.00}", cs));
            }

            return rd;
        }

        #endregion

        #region Analgesia

        private static List<object> GetAnalgesia(IEnumerable<ReportQueryModel> filteredList, List<QuarterData> quarters)
        {
            List<object> rd = new List<object>();

            foreach (var quarter in quarters)
            {
                var count = (from x in filteredList
                             where x.StartDate.Value >= quarter.StartDate &&
                                   x.StartDate.Value < quarter.EndDate &&
                                  (x.Analgesia == "1" || x.Analgesia == "2" || x.Analgesia == "3")
                             select x).Count();

                var total = (from x in filteredList
                             where x.StartDate.Value >= quarter.StartDate &&
                                 x.StartDate.Value < quarter.EndDate
                             select x).Count();

                var avg = total != 0 ? (double)((100 * count) / total) : 0;

                rd.Add(string.Format("{0:0.00}", avg));
            }

            return rd;
        }

        #endregion

        #region Pain Assessment

        private static List<object> GetPainAssessment(IEnumerable<ReportQueryModel> filteredList, List<QuarterData> quarters)
        {
            List<object> rd = new List<object>();

            foreach (var quarter in quarters)
            {
                var count = (from x in filteredList
                             where x.StartDate.Value >= quarter.StartDate &&
                                   x.StartDate.Value < quarter.EndDate &&
                                  (x.PainAssessment == "1" || x.PainAssessment == "2")
                             select x).Count();

                var total = (from x in filteredList
                             where x.StartDate.Value >= quarter.StartDate &&
                                 x.StartDate.Value < quarter.EndDate &&
                                 x.PainAssessment != null
                             select x).Count();

                var avg = total != 0 ? (double)((100 * count) / total) : 0;

                rd.Add(string.Format("{0:0.00}", avg));
            }

            return rd;
        }

        #endregion

        #region Pain Management

        private static List<object> GetPainManagement(IEnumerable<ReportQueryModel> filteredList, List<QuarterData> quarters)
        {
            List<object> rd = new List<object>();

            foreach (var quarter in quarters)
            {
                var count = (from x in filteredList
                             where x.StartDate.Value >= quarter.StartDate &&
                                   x.StartDate.Value < quarter.EndDate &&
                                  (x.PainManagement == "1")
                             select x).Count();

                var total = (from x in filteredList
                             where x.StartDate.Value >= quarter.StartDate &&
                                 x.StartDate.Value < quarter.EndDate &&
                                 x.PainManagement != null
                             select x).Count();

                var avg = total != 0 ? (double)((100 * count) / total) : 0;

                rd.Add(string.Format("{0:0.00}", avg));
            }

            return rd;
        }

        #endregion

        #region Pre-Operative Medical Assessment

        private static List<object> GetPreOpMedAss(IEnumerable<ReportQueryModel> filteredList, List<QuarterData> quarters)
        {
            List<object> rd = new List<object>();
            int avg = 0, total = 0;
            double cs = 0;
            foreach (var quarter in quarters)
            {
                avg = (from x in filteredList
                       where x.StartDate.Value >= quarter.StartDate &&
                             x.StartDate.Value < quarter.EndDate &&
                            (x.PreOpMedAss == "1" || x.PreOpMedAss == "2" || x.PreOpMedAss == "3" || x.PreOpMedAss == "4")
                       select x).Count();

                total = (from x in filteredList
                         where x.StartDate.Value >= quarter.StartDate &&
                             x.StartDate.Value < quarter.EndDate &&
                             x.PreOpMedAss != null
                         select x).Count();

                cs = total != 0 ? (double)((100 * avg) / total) : 0;

                rd.Add(string.Format("{0:0.00}", cs));
            }

            return rd;
        }

        #endregion

        #region Mobilisation

        private static List<object> GetMobilisation(IEnumerable<ReportQueryModel> filteredList, List<QuarterData> quarters)
        {
            List<object> rd = new List<object>();
            int avg = 0, total = 0;
            double cs = 0;
            foreach (var quarter in quarters)
            {
                avg = (from x in filteredList
                       where x.StartDate.Value >= quarter.StartDate &&
                             x.StartDate.Value < quarter.EndDate &&
                            (x.Mobilisation == "0")
                       select x).Count();

                total = (from x in filteredList
                         where x.StartDate.Value >= quarter.StartDate &&
                             x.StartDate.Value < quarter.EndDate &&
                             x.Mobilisation != null
                         select x).Count();

                cs = total != 0 ? (double)((100 * avg) / total) : 0;

                rd.Add(string.Format("{0:0.00}", cs));
            }

            return rd;
        }

        #endregion

        #region Weight Bearing

        private static List<object> GetWeightBearing(IEnumerable<ReportQueryModel> filteredList, List<QuarterData> quarters)
        {
            List<object> rd = new List<object>();
            int avg = 0, total = 0;
            double cs = 0;
            foreach (var quarter in quarters)
            {
                avg = (from x in filteredList
                       where x.StartDate.Value >= quarter.StartDate &&
                             x.StartDate.Value < quarter.EndDate &&
                            (x.FullWeightBear == "0")
                       select x).Count();

                total = (from x in filteredList
                         where x.StartDate.Value >= quarter.StartDate &&
                             x.StartDate.Value < quarter.EndDate &&
                             x.FullWeightBear != null
                         select x).Count();

                cs = total != 0 ? (double)((100 * avg) / total) : 0;

                rd.Add(string.Format("{0:0.00}", cs));
            }

            return rd;
        }

        #endregion

        #region Pressure Ulcers

        private static List<object> GetPressureUlcers(IEnumerable<ReportQueryModel> filteredList, List<QuarterData> quarters)
        {
            List<object> rd = new List<object>();
            int avg = 0, total = 0;
            double cs = 0;
            foreach (var quarter in quarters)
            {
                avg = (from x in filteredList
                       where x.StartDate.Value >= quarter.StartDate &&
                             x.StartDate.Value < quarter.EndDate &&
                            (x.PressureUlcers == "1")
                       select x).Count();

                total = (from x in filteredList
                         where x.StartDate.Value >= quarter.StartDate &&
                             x.StartDate.Value < quarter.EndDate &&
                             x.PressureUlcers != null
                         select x).Count();

                cs = total != 0 ? (double)((100 * avg) / total) : 0;

                rd.Add(string.Format("{0:0.00}", cs));
            }

            return rd;
        }

        #endregion

        #region Bone Medication on Discharge

        private static List<object> GetBoneMedDischarge(IEnumerable<ReportQueryModel> filteredList, List<QuarterData> quarters)
        {
            List<object> rd = new List<object>();
            int avg = 0, total = 0;
            double cs = 0;
            foreach (var quarter in quarters)
            {
                avg = (from x in filteredList
                       where x.StartDate.Value >= quarter.StartDate &&
                             x.StartDate.Value < quarter.EndDate &&
                            (x.BoneMedDischarge == "1" || x.BoneMedDischarge == "2")
                       select x).Count();

                total = (from x in filteredList
                         where x.StartDate.Value >= quarter.StartDate &&
                             x.StartDate.Value < quarter.EndDate &&
                             x.BoneMedDischarge != null
                         select x).Count();

                cs = total != 0 ? (double)((100 * avg) / total) : 0;

                rd.Add(string.Format("{0:0.00}", cs));
            }

            return rd;
        }

        #endregion

        #region Reoperation 30

        private static List<object> GetReoperation30(IEnumerable<ReportQueryModel> filteredList, List<QuarterData> quarters)
        {
            List<object> rd = new List<object>();
            int avg = 0, total = 0;
            double cs = 0;
            foreach (var quarter in quarters)
            {
                avg = (from x in filteredList
                       where x.StartDate.Value >= quarter.StartDate &&
                             x.StartDate.Value < quarter.EndDate &&
                            (x.Reoperation30 == "2" || x.Reoperation30 == "3" || x.Reoperation30 == "4" || x.Reoperation30 == "5" || x.Reoperation30 == "6" || x.Reoperation30 == "7" || x.Reoperation30 == "8" || x.Reoperation30 == "11")
                       select x).Count();

                total = (from x in filteredList
                         where x.StartDate.Value >= quarter.StartDate &&
                             x.StartDate.Value < quarter.EndDate &&
                             x.Reoperation30 != null
                         select x).Count();

                cs = total != 0 ? (double)((100 * avg) / total) : 0;

                rd.Add(string.Format("{0:0.00}", cs));
            }

            return rd;
        }

        #endregion

        #region Best Practice Data

        private static List<object> GetSurgeryBestPractice(IEnumerable<ReportQueryModel> filteredList, List<QuarterData> quarters)
        {
            List<object> rd = new List<object>();

            foreach (var quarter in quarters)
            {
                var query = (from x in filteredList
                             where x.StartDate.Value >= quarter.StartDate &&
                                 x.StartDate.Value < quarter.EndDate
                             select x).ToList();

                decimal totalCount = query.Count;

                decimal count = (from q in query
                                 where (q.SurgeryDateTime.Value - q.StartDate.Value).TotalHours < Settings.Default.BestPracticeReportHours
                                 select q).Count();

                rd.Add(string.Format("{0:0.00}", totalCount > 0 ? count / query.Count * 100 : 0));
            }

            return rd;
        }

        #endregion

        #region Survival 30 Days Data

        private static List<object> GetSurvival30DaysData(IEnumerable<ReportQueryModel> filteredList, List<QuarterData> quarters)
        {
            List<object> rd = new List<object>();

            foreach (var quarter in quarters)
            {
                var query = (from x in filteredList
                             where x.FollowupDate30.Value >= quarter.StartDate &&
                                 x.FollowupDate30.Value < quarter.EndDate
                             select x).ToList();

                decimal totalCount = query.Count;

                decimal count = (from q in query
                                 where q.Survival30 == "2" // Yes
                                 select q).Count();

                rd.Add(string.Format("{0:0.00}", totalCount > 0 ? count / query.Count * 100 : 0));
            }

            return rd;
        }

        #endregion

        #region For Time to Surgery

        private static List<object> GetTotalEnteredGroupData(IEnumerable<ReportQueryModel> filteredList, List<QuarterData> quarters)
        {
            List<object> rd = new List<object>();

            foreach (var quarter in quarters)
            {
                var count = (from x in filteredList
                             where x.StartDate.Value >= quarter.StartDate &&
                             x.StartDate.Value < quarter.EndDate
                             select x).Count();

                rd.Add(string.Format("{0:0.00}", count));
            }

            return rd;
        }

        private static List<object> GetTotalEnteredNatioAvgData_DEPRICATED(List<ReportQueryModel> data, DateTime reportDate)
        {
            List<object> rd = new List<object>();
            for (int i = 5; i >= 0; i--)
            {
                DateTime workingDate = reportDate.AddMonths(-i);

                var count = (from x in data
                             where x.StartDate.Value.Year == workingDate.Year &&
                             x.StartDate.Value.Month == workingDate.Month
                             select x).Count();

                rd.Add(string.Format("{0:0.00}", count));
            }

            return rd;
        }

        #endregion

        #region Estimated v Actual

        private static List<object> GetEstimatedActualGroupData(IEnumerable<ReportQueryModel> filteredList, IEnumerable<ReportQueryModel> filteredEst)
        {
            double time1 = 0, time2 = 0;
            double natTime1 = 0, natTime2 = 0;

            List<object> rd = new List<object>();

            time1 = filteredEst.Sum(x => x.EstimatedRecords);
            time2 = filteredList.Count();

            natTime1 = time1;
            natTime2 = time2;

            rd.Add(string.Format("{0:n0}", natTime1));
            rd.Add(string.Format("{0:n0}", natTime2));

            return rd;
        }

        private static List<object> GetEstimatedActualNatioAvgData(List<ReportQueryModel> data, List<ReportQueryModel> est)
        {
            double natTime1 = 0, natTime2 = 0;
            double natAvg1 = 0, natAvg2 = 0;

            natTime1 = est.Sum(x => x.EstimatedRecords);
            natTime2 = data.Count();

            natAvg1 = natTime1;
            natAvg2 = natTime2;

            List<object> rd = new List<object>();
            rd.Add(string.Format("{0:n2}", natAvg1));
            rd.Add(string.Format("{0:n2}", natAvg2));

            return rd;
        }

        #endregion

        #region For Age

        private static List<object> GetAgeGroupData(IEnumerable<ReportQueryModel> filteredList)
        {
            int ag1 = 0, ag2 = 0, ag3 = 0, ag4 = 0, ag5 = 0;
            double natAvg1 = 0, natAvg2 = 0, natAvg3 = 0, natAvg4 = 0, natAvg5 = 0, total = 0;

            ag1 = (from x in filteredList
                   where x.Age >= 60 && x.Age <= 69
                   select x).Count();
            ag2 = (from x in filteredList
                   where x.Age >= 70 && x.Age <= 79
                   select x).Count();
            ag3 = (from x in filteredList
                   where x.Age >= 80 && x.Age <= 89
                   select x).Count();
            ag4 = (from x in filteredList
                   where x.Age >= 90 && x.Age <= 99
                   select x).Count();
            ag5 = (from x in filteredList
                   where x.Age >= 100 && x.Age <= 107
                   select x).Count();

            total += ag1 + ag2 + ag3 + ag4 + ag5;

            natAvg1 = total != 0 ? (double)((100 * ag1) / total) : 0;
            natAvg2 = total != 0 ? (double)((100 * ag2) / total) : 0;
            natAvg3 = total != 0 ? (double)((100 * ag3) / total) : 0;
            natAvg4 = total != 0 ? (double)((100 * ag4) / total) : 0;
            natAvg5 = total != 0 ? (double)((100 * ag5) / total) : 0;

            List<object> rd = new List<object>();
            rd.Add(string.Format("{0:0.00}", natAvg1));
            rd.Add(string.Format("{0:0.00}", natAvg2));
            rd.Add(string.Format("{0:0.00}", natAvg3));
            rd.Add(string.Format("{0:0.00}", natAvg4));
            rd.Add(string.Format("{0:0.00}", natAvg5));

            return rd;
        }

        private static List<object> GetAgeNatioAvgData(List<ReportQueryModel> data)
        {
            double natAg1 = 0, natAg2 = 0, natAg3 = 0, natAg4 = 0, natAg5 = 0, total = 0;
            double natAvg1 = 0, natAvg2 = 0, natAvg3 = 0, natAvg4 = 0, natAvg5 = 0;

            natAg1 = data.Where(p => p.DOB.GetAge() >= 60 && p.DOB.GetAge() <= 69).Count();
            natAg2 = data.Where(p => p.DOB.GetAge() >= 70 && p.DOB.GetAge() <= 79).Count();
            natAg3 = data.Where(p => p.DOB.GetAge() >= 80 && p.DOB.GetAge() <= 89).Count();
            natAg4 = data.Where(p => p.DOB.GetAge() >= 90 && p.DOB.GetAge() <= 99).Count();
            natAg5 = data.Where(p => p.DOB.GetAge() >= 100 && p.DOB.GetAge() <= 107).Count();

            total += natAg1 + natAg2 + natAg3 + natAg4 + natAg5;

            natAvg1 = natAg1 != 0 ? ((100 * natAg1) / total) : 0;
            natAvg2 = natAg2 != 0 ? ((100 * natAg2) / total) : 0;
            natAvg3 = natAg3 != 0 ? ((100 * natAg3) / total) : 0;
            natAvg4 = natAg4 != 0 ? ((100 * natAg4) / total) : 0;
            natAvg5 = natAg5 != 0 ? ((100 * natAg5) / total) : 0;

            List<object> rd = new List<object>();
            rd.Add(string.Format("{0:0.00}", natAvg1));
            rd.Add(string.Format("{0:0.00}", natAvg2));
            rd.Add(string.Format("{0:0.00}", natAvg3));
            rd.Add(string.Format("{0:0.00}", natAvg4));
            rd.Add(string.Format("{0:0.00}", natAvg5));

            return rd;
        }

        #endregion

        #region For Sex

        private static List<object> GetSexData(IEnumerable<ReportQueryModel> filteredList)
        {
            int m = 0, f = 0;
            double mAvg = 0, fAvg = 0, total = 0;

            foreach (var sex in filteredList)
            {
                m = (sex.Sex == "Male" ? (m + 1) : m);
                f = (sex.Sex == "Female" ? (f + 1) : f);
            }

            total += m + f;

            mAvg = total != 0 ? ((100 * m) / total) : 0;
            fAvg = total != 0 ? ((100 * f) / total) : 0;

            List<object> rd = new List<object>();
            rd.Add(string.Format("{0:0.00}", mAvg));
            rd.Add(string.Format("{0:0.00}", fAvg));

            return rd;
        }

        #endregion

        #region For Fracture Type

        private static List<object> GetAverageFractureType(List<object> filteredList)
        {
            double total = 0;
            var retDataArray = new List<object>();

            filteredList.ForEach(p => total += (int)p);

            foreach (var param in filteredList)
            {
                double ftAvg = total != 0 ? ((100 * (int)param) / total) : 0;
                retDataArray.Add(string.Format("{0:0.00}", ftAvg));
            }

            return retDataArray;
        }

        #endregion

        #region For Surgery Delay

        private static List<object> GetAverageSurgeryDelay(List<object> filteredList)
        {
            double total = 0;
            var retDataArray = new List<object>();

            filteredList.ForEach(p => total += (int)p);

            foreach (var param in filteredList)
            {
                double ftAvg = total != 0 ? ((100 * (int)param) / total) : 0;
                retDataArray.Add(string.Format("{0:0.00}", ftAvg));
            }

            return retDataArray;
        }

        #endregion

        #region For Length of Stay

        private static double CalculateLengthOfStay(DateTime? arrivalDate, DateTime? departureDate)
        {
            try
            {
                TimeSpan tSpan = (DateTime)departureDate - (DateTime)arrivalDate;

                if (tSpan != null)
                {
                    return tSpan.TotalDays;
                }

                return 0;
            }
            catch { return 0; }
        }

        private static List<object> GetLengthOfStayData(IEnumerable<ReportQueryModel> filteredList, List<QuarterData> quarters)
        {
            List<object> rd = new List<object>();
            foreach (var quarter in quarters)
            {
                var query = (from x in filteredList
                             where x.StartDate >= quarter.StartDate &&
                             x.StartDate < quarter.EndDate
                             select new
                             {
                                 ArrivalDateTime = x.MyStart,
                                 DischargeDate = x.DischargeDate,
                                 LengthOfStay = (double?)x.LengthOfStay
                             }).ToList();

                var med = query.Select(q => q.LengthOfStay).Median() ?? 0;

                rd.Add(string.Format("{0:0.00}", med));
            }

            return rd;
        }

        private static List<object> GetCompletenessData(IEnumerable<ReportQueryModel> filteredList, List<QuarterData> quarters)
        {
            List<object> rd = new List<object>();
            foreach (var quarter in quarters)
            {
                var query = (from x in filteredList
                             where x.ArrivalDateTime.Value >= quarter.StartDate &&
                             x.ArrivalDateTime.Value < quarter.EndDate
                             select new
                             {
                                 ArrivalDateTime = x.ArrivalDateTime,
                                 DischargeDate = x.DischargeDate,
                                 Completeness = (double?)x.Completeness
                             }).ToList();

                var avg = query.Select(q => q.Completeness).Average() ?? 0;

                rd.Add(string.Format("{0:0.00}", avg));
            }

            return rd;
        }

        #endregion

        #region For Fracture Type

        private static List<object> GetAverageDischargeDestination(List<object> filteredList)
        {
            double total = 0;
            var retDataArray = new List<object>();

            filteredList.ForEach(p => total += (int)p);

            foreach (var param in filteredList)
            {
                double ftAvg = total != 0 ? ((100 * (int)param) / total) : 0;
                retDataArray.Add(string.Format("{0:0.00}", ftAvg));
            }

            return retDataArray;
        }

        #endregion

        #endregion

        #region Quarter Handling

        private static List<QuarterData> GetQuarters(DateTime current)
        {
            var quarters = new List<QuarterData>();

            for (int i = 4; i > 0; i--)
            {
                DateTime startDate = current.AddMonths((-3 * i) + 1);
                DateTime endDate = startDate.AddMonths(3);

                quarters.Add(new QuarterData { StartDate = startDate, EndDate = endDate, Text = string.Format("{0:MMM}-{0:yy} - {1:MMM}-{1:yy}", startDate, endDate.AddDays(-1)) });
            }

            return quarters;
        }

        private static List<string> GetQuarterLabels(List<QuarterData> quarters)
        {
            List<string> labels = new List<string>();

            foreach (var quarter in quarters)
            {
                labels.Add(quarter.Text);
            }

            return labels;
        }

        private class QuarterData
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }

            public string Text { get; set; }
        }

        #endregion

        #region Pain Assessment

        public static object PainAssessmentReport(string reportType, Hospital hospital, int month, int year)
        {
            #region DB Query

            ANZHFREntities Entity = new ANZHFREntities();

            var quarters = GetQuarters(new DateTime(year, month, 1));

            DateTime startDate = quarters.First().StartDate;
            DateTime endDate = quarters.Last().EndDate;

            var data = (from p in Entity.Patients
                        join h in Entity.Hospitals on p.HospitalID equals h.HospitalID
                        where h.HCountry == hospital.HCountry &&
                        p.ArrivalDateTime >= startDate &&
                        p.ArrivalDateTime < endDate &&
                        p.PainAssessment != null &&
                        p.PainAssessment != ""
                        select new ReportQueryModel
                        {
                            HospitalID = p.HospitalID,
                            Hospital = h.HName,
                            AdmissionViaED = p.AdmissionViaED,
                            ArrivalDateTime = p.ArrivalDateTime,
                            InHospFractureDateTime = p.InHospFractureDateTime,
                            TransferDateTime = p.TransferDateTime,
                            DepartureDateTime = p.DepartureDateTime,
                            SurgeryDateTime = p.SurgeryDateTime,
                            PainAssessment = p.PainAssessment,
                            State = h.HState,
                            Country = h.HCountry
                        }).ToList();

            #endregion DB Query

            if (data.Count > 0)
            {
                #region Hospital

                var filteredList = data.Where(x => x.HospitalID == hospital.HospitalID).ToList();

                List<object> rd = GetPainAssessment(filteredList, quarters);

                ReportModel hospitalModel = new ReportModel { Label = hospital.HName, Data = rd };

                #endregion Hospital

                #region State

                ReportModel stateModel = null;
                if (ConfigurationManager.AppSettings["Location"] == "Australian")
                {
                    filteredList = data.Where(x => x.State == hospital.HState).ToList();
                    rd = GetPainAssessment(filteredList, quarters);

                    stateModel = new ReportModel { Label = hospital.HState, Data = rd };
                }

                #endregion State

                #region National average

                var nationalAvgList = GetPainAssessment(data, quarters);
                ReportModel nationModel = new ReportModel { Label = hospital.HCountry, Data = nationalAvgList };

                #endregion Nation

                List<List<object>> reportData = new List<List<object>>();
                reportData.Add(hospitalModel.Data);

                if (stateModel != null)
                {
                    reportData.Add(stateModel.Data);
                }

                reportData.Add(nationModel.Data);

                List<string> reportLegends = new List<string>();
                reportLegends.Add(hospitalModel.Label);

                if (stateModel != null)
                {
                    reportLegends.Add(stateModel.Label);
                }

                reportLegends.Add(nationModel.Label);

                var report = Entity.Reports.Where(r => r.Code == reportType).FirstOrDefault();

                return new { Data = reportData, Labels = GetQuarterLabels(quarters).ToArray(), Legends = reportLegends, IsAverage = true, Rotatelabel = false, ChartTitle = report.Title, xTitle = report.XLabel, yTitle = report.YLabel, Description = report.Description };
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Pain Management

        public static object PainManagementReport(string reportType, Hospital hospital, int month, int year)
        {
            #region DB Query

            ANZHFREntities Entity = new ANZHFREntities();

            var quarters = GetQuarters(new DateTime(year, month, 1));

            DateTime startDate = quarters.First().StartDate;
            DateTime endDate = quarters.Last().EndDate;

            var data = (from p in Entity.Patients
                        join h in Entity.Hospitals on p.HospitalID equals h.HospitalID
                        where h.HCountry == hospital.HCountry &&
                        p.ArrivalDateTime >= startDate &&
                        p.ArrivalDateTime < endDate &&
                        p.PainManagement != null &&
                        p.PainManagement != ""
                        select new ReportQueryModel
                        {
                            HospitalID = p.HospitalID,
                            Hospital = h.HName,
                            AdmissionViaED = p.AdmissionViaED,
                            ArrivalDateTime = p.ArrivalDateTime,
                            InHospFractureDateTime = p.InHospFractureDateTime,
                            TransferDateTime = p.TransferDateTime,
                            DepartureDateTime = p.DepartureDateTime,
                            SurgeryDateTime = p.SurgeryDateTime,
                            PainManagement = p.PainManagement,
                            State = h.HState,
                            Country = h.HCountry
                        }).ToList();

            #endregion DB Query

            if (data.Count > 0)
            {
                #region Hospital

                var filteredList = data.Where(x => x.HospitalID == hospital.HospitalID).ToList();

                List<object> rd = GetPainManagement(filteredList, quarters);

                ReportModel hospitalModel = new ReportModel { Label = hospital.HName, Data = rd };

                #endregion Hospital

                #region State

                ReportModel stateModel = null;
                if (ConfigurationManager.AppSettings["Location"] == "Australian")
                {
                    filteredList = data.Where(x => x.State == hospital.HState).ToList();
                    rd = GetPainManagement(filteredList, quarters);

                    stateModel = new ReportModel { Label = hospital.HState, Data = rd };
                }

                #endregion State

                #region National average

                var nationalAvgList = GetPainManagement(data, quarters);
                ReportModel nationModel = new ReportModel { Label = hospital.HCountry, Data = nationalAvgList };

                #endregion Nation

                List<List<object>> reportData = new List<List<object>>();
                reportData.Add(hospitalModel.Data);

                if (stateModel != null)
                {
                    reportData.Add(stateModel.Data);
                }

                reportData.Add(nationModel.Data);

                List<string> reportLegends = new List<string>();
                reportLegends.Add(hospitalModel.Label);

                if (stateModel != null)
                {
                    reportLegends.Add(stateModel.Label);
                }

                reportLegends.Add(nationModel.Label);

                var report = Entity.Reports.Where(r => r.Code == reportType).FirstOrDefault();

                return new { Data = reportData, Labels = GetQuarterLabels(quarters).ToArray(), Legends = reportLegends, IsAverage = true, Rotatelabel = false, ChartTitle = report.Title, xTitle = report.XLabel, yTitle = report.YLabel, Description = report.Description };
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Delay Reason Report

        public static object DelayReasonReport(string reportType, Hospital hospital, int month, int year)
        {
            #region DB Query

            ANZHFREntities Entity = new ANZHFREntities();

            DateTime endDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            DateTime startDate = endDate.AddYears(-1).AddDays(1);

            var _data = (from p in Entity.Patients
                         join h in Entity.Hospitals on p.HospitalID equals h.HospitalID
                         where h.HCountry == hospital.HCountry &&
                         p.SurgeryDelay != "" &&
                         p.SurgeryDelay != null &&
                         p.SurgeryDelay != "1"
                         select new ReportQueryModel
                         {
                             DOB = p.DOB,
                             HospitalID = h.HospitalID,
                             Hospital = h.HName,
                             StartDateTime = p.StartDate,
                             AdmissionViaED = p.AdmissionViaED,
                             ArrivalDateTime = p.ArrivalDateTime,
                             InHospFractureDateTime = p.InHospFractureDateTime,
                             TransferDateTime = p.TransferDateTime,
                             DepartureDateTime = p.DepartureDateTime,
                             State = h.HState,
                             Country = h.HCountry,
                             SurgeryDelayID = p.SurgeryDelay
                         }).ToList();

            var data = (from d in _data
                        join f in Entity.SurgeryDelays on Convert.ToInt16(d.SurgeryDelayID) equals f.SurgeryDelayID
                        where d.StartDateTime >= startDate && d.StartDateTime <= endDate
                        select new ReportQueryModel
                        {
                            DOB = d.DOB,
                            HospitalID = d.HospitalID,
                            Hospital = d.Hospital,
                            AdmissionViaED = d.AdmissionViaED,
                            ArrivalDateTime = d.ArrivalDateTime,
                            InHospFractureDateTime = d.InHospFractureDateTime,
                            TransferDateTime = d.TransferDateTime,
                            DepartureDateTime = d.DepartureDateTime,
                            State = d.State,
                            Country = d.Country,
                            SurgeryDelayID = d.SurgeryDelayID,
                            SurgeryDelay = f.Name
                        }).ToList();

            #endregion DB Query

            if (data.Count > 0)
            {
                string[] keys = Entity.SurgeryDelays.Where(x => !x.Name.Contains("No delay, surgery completed <48 hours")).OrderBy(x => x.SurgeryDelayID).Select(x => x.Name).ToArray();

                #region Hospital

                List<object> dataArray = new List<object>();

                for (int i = 0; i < keys.Count(); i++)
                {
                    dataArray.Add(data.Where(x => x.HospitalID == hospital.HospitalID && x.SurgeryDelay == keys[i]).Count());
                }

                dataArray = GetAverageSurgeryDelay(dataArray);

                ReportModel hospitalModel = new ReportModel { Label = hospital.HName, Data = dataArray };

                #endregion Hospital Data

                #region State Data

                ReportModel stateModel = null;
                if (ConfigurationManager.AppSettings["Location"] == "Australian")
                {
                    dataArray = new List<object>();

                    for (int i = 0; i < keys.Count(); i++)
                    {
                        dataArray.Add(data.Where(x => x.State == hospital.HState && x.SurgeryDelay == keys[i]).Count());
                    }

                    dataArray = GetAverageSurgeryDelay(dataArray);

                    stateModel = new ReportModel { Label = hospital.HState, Data = dataArray };
                }

                #endregion State

                #region Nation

                dataArray = new List<object>();

                for (int i = 0; i < keys.Count(); i++)
                {
                    dataArray.Add(data.Where(x => x.Country == hospital.HCountry && x.SurgeryDelay == keys[i]).Count());
                }

                dataArray = GetAverageSurgeryDelay(dataArray);

                ReportModel nationData = new ReportModel { Label = hospital.HCountry, Data = dataArray };

                #endregion Nation

                List<List<object>> reportData = new List<List<object>>();
                reportData.Add(hospitalModel.Data);

                if (stateModel != null)
                {
                    reportData.Add(stateModel.Data);
                }

                reportData.Add(nationData.Data);

                List<string> reportLegends = new List<string>();
                reportLegends.Add(hospitalModel.Label);

                if (stateModel != null)
                {
                    reportLegends.Add(stateModel.Label);
                }

                reportLegends.Add(nationData.Label);

                var report = Entity.Reports.Where(r => r.Code == reportType).FirstOrDefault();

                return new { Data = reportData, Labels = keys, Legends = reportLegends, IsAverage = true, Rotatelabel = false, ChartTitle = report.Title, xTitle = report.XLabel, yTitle = report.YLabel, Description = report.Description };
            }
            else
            {
                return null;
            }
        }

        #endregion

    }
}