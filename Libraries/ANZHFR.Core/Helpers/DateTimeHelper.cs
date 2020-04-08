using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ANZHFR.Core.Helpers
{
    public class DateTimeHelper
    {
		//public static DateTime? ConvertToDate(string date)
		//{
		//	return null;
		//}

		public DateTime? ConvertToDate(string date, bool dateOnly = true)
		{
			DateTime temp;

			if (!string.IsNullOrEmpty(date))
			{
				try
				{
					if (DateTime.TryParse(date, out temp))
					{
						if (dateOnly)
							return DateTime.ParseExact(temp.ToString("yyyy/MM/dd"), "yyyy/MM/dd", CultureInfo.InvariantCulture);
						else
							return DateTime.ParseExact(temp.ToString("yyyy/MM/dd hh:mm tt"), "yyyy/MM/dd hh:mm tt", CultureInfo.InvariantCulture);
					}
				}
				catch { }
			}

			return null;
		}
    }
}
