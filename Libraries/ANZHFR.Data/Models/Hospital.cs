//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ANZHFR.Data.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Hospital
    {
        public Hospital()
        {
            this.Users = new HashSet<User>();
            this.Patients = new HashSet<Patient>();
            this.Surveys = new HashSet<Survey>();
        }
    
        public long HospitalID { get; set; }
        public string HName { get; set; }
        public string HStreetAddress1 { get; set; }
        public string HStreetAddress2 { get; set; }
        public string HSuburb { get; set; }
        public string HCity { get; set; }
        public string HPostCode { get; set; }
        public string HCountry { get; set; }
        public string HPhone { get; set; }
        public string HAdminEmail { get; set; }
        public string HState { get; set; }
        public Nullable<short> HStatus { get; set; }
        public Nullable<short> HType { get; set; }
    
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Patient> Patients { get; set; }
        public virtual HospitalStatus HospitalStatus { get; set; }
        public virtual HospitalType HospitalType { get; set; }
        public virtual ICollection<Survey> Surveys { get; set; }
    }
}
