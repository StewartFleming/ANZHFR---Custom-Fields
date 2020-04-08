using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ANZHFR.Web.Models
{
	public class SynonymModel : BaseModel
	{
		public long Id
		{
			get;
			set;
		}
		public string Word
		{
			get;
			set;
		}
	}

	public class SynonymChildModel : BaseModel
	{
		public long Id
		{
			get;
			set;
		}
		public long SynonymId
		{
			get;
			set;
		}
		public string Word
		{
			get;
			set;
		}
	}
}