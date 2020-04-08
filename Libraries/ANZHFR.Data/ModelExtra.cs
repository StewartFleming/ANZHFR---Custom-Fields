using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ANZHFR.Data.Models
{
    public partial class Hospital
    {
        public string Location
        {
            get
            {
                string location = this.HCountry;

                if (!string.IsNullOrWhiteSpace(this.HSuburb) && !string.IsNullOrWhiteSpace(this.HState) && !string.IsNullOrWhiteSpace(this.HCountry))
                {
                    location = string.Format("{0}, {1}, {2}", this.HSuburb, this.HState, this.HCountry);
                }
                else if (!string.IsNullOrWhiteSpace(this.HState) && !string.IsNullOrWhiteSpace(this.HCountry))
                {
                    location = string.Format("{0}, {1}", this.HState, this.HCountry);
                }

                return location;
            }
        }
    }

    public partial class Survey
    {
        public static int GetYear()
        {
            return GetYear(DateTime.Now);
        }

        public static int GetYear(DateTime baseDate)
        {
            DateTime calcDate = Convert.ToDateTime(ConfigurationManager.AppSettings["SurveyStartDate"]);
            DateTime surveyStartDate = new DateTime(baseDate.Year, calcDate.Month, calcDate.Day);

            int year = DateTime.Now.Year;

            if (baseDate < surveyStartDate)
            {
                year--;
            }

            return year;
        }

        public static string GetYearText(int year)
        {
            return GetYearText(new DateTime(year, 1, 1));
        }

        public static string GetYearText()
        {
            return GetYearText(DateTime.Now);
        }

        public static string GetYearText(DateTime baseDate)
        {
            int year = GetYear(baseDate);
            DateTime thisYear = new DateTime(year, 1, 1);
            DateTime nextYear = new DateTime(year + 1, 1, 1);

            return string.Format("{0:yyyy}/{1:yy}", thisYear, nextYear);
        }

        public int EstimatedNumberOfFractures
        {
            get
            {
                string estimateText = "";

                if (this.EstimatedRecords.Contains("-"))
                {
                    string[] values = this.EstimatedRecords.Split('-');
                    estimateText = values[1];
                }
                else if (this.EstimatedRecords.Contains("+"))
                {
                    estimateText = this.EstimatedRecords.Replace("+", "");
                }

                int estimate = 0;
                int.TryParse(estimateText, out estimate);

                return estimate;
            }
        }
    }

    public partial class Patient
    {
        public DateTime? ExpectedFollowup30
        {
            get
            {
                if (!this.StartDate.HasValue)
                {
                    return null;
                }

                return this.StartDate.Value.AddDays(30);
            }
        }

        public DateTime? ExpectedFollowup120
        {
            get
            {
                if (!this.StartDate.HasValue)
                {
                    return null;
                }

                return this.StartDate.Value.AddDays(120);
            }
        }
    }

    public class CalendarData
    {
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string url { get; set; }
        public string color { get; set; }
    }

    [MetadataType(typeof(YearlyValidationMetadata))]
    public partial class YearlyValidation
    {
    }

    public class YearlyValidationMetadata
    {
        [Display(Name = "Fracture Side")]
        public string FractureSide { get; set; }

        [Display(Name = "Created")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? Created { get; set; }

        [Display(Name = "Updated")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? LastModified { get; set; }

        [Display(Name = "Age at Admission")]
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public int? AgeAtAdmission { get; set; }

        [Display(Name = "Time in ED")]
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public int? TimeInED { get; set; }

        [Display(Name = "Time to Surgery")]
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public int? TimeToSurgery { get; set; }

        [Display(Name = "Length of Acute Stay")]
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public int? LengthOfAcuteStay { get; set; }

        [Display(Name = "Length of Stay")]
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public int? LengthOfStay { get; set; }
    }
}