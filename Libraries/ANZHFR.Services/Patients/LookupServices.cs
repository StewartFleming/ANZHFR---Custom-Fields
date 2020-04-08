using ANZHFR.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANZHFR.Services.Patients
{
    public class LookupServices : BaseServices
    {
        public List<AccessLevel> GetAccessLevels()
        {
            return Entity.AccessLevels.OrderBy(al => al.Name).ToList();
        }

        public List<string> GetStates()
        {
            List<string> states = new List<string>();
            states.Add("ACT");
            states.Add("NSW");
            states.Add("NT");
            states.Add("QLD");
            states.Add("SA");
            states.Add("TAS");
            states.Add("VIC");
            states.Add("WA");

            return states;
        }
    }
}