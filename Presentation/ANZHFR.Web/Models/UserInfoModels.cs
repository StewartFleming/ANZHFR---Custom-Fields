using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ANZHFR.Web.Models
{
	public class UserInfoModel
	{
		public long ID { get; set; }
		public string Email { get; set; }
		public int AccessLevel { get; set; }
        public long HospitalID { get; set; }
	}
}