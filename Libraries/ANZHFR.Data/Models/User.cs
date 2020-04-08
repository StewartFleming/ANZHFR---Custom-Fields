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
    
    public partial class User
    {
        public User()
        {
            this.Surveys = new HashSet<Survey>();
        }
    
        public long UserID { get; set; }
        public long UHospitalID { get; set; }
        public int UAccessLevel { get; set; }
        public string UEmail { get; set; }
        public string UPassword { get; set; }
        public string UPosition { get; set; }
        public string UTitle { get; set; }
        public string UFirstName { get; set; }
        public string USurname { get; set; }
        public string UMobile { get; set; }
        public string UWorkPhone { get; set; }
        public Nullable<System.DateTime> UDateCreated { get; set; }
        public Nullable<System.DateTime> ULastAccessed { get; set; }
        public byte UActive { get; set; }
        public string AdminNotes { get; set; }
        public Nullable<short> URole { get; set; }
    
        public virtual Hospital Hospital { get; set; }
        public virtual AccessLevel AccessLevel { get; set; }
        public virtual ICollection<Survey> Surveys { get; set; }
    }
}