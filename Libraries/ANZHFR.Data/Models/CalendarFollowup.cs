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
    
    public partial class CalendarFollowup
    {
        public int ANZHFRID { get; set; }
        public long HospitalID { get; set; }
        public string Name { get; set; }
        public int Followup { get; set; }
        public Nullable<System.DateTime> ExpectedDate { get; set; }
        public string Survival30 { get; set; }
    }
}
