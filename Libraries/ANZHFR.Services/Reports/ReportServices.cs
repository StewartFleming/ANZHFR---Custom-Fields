using ANZHFR.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANZHFR.Services.Reports
{
    public class ReportServices : BaseServices
    {
        public List<Data.Models.Report> GetAll()
        {
            return (from r in Entity.Reports
                    where r.ActiveFlag == true
                    orderby r.DisplayOrder, r.Name
                    select r).ToList();
        }

        public Data.Models.Report Get(string code)
        {
            var report = Entity.Reports.Where(x => x.Code == code).FirstOrDefault();

            return report;
        }
    }
}