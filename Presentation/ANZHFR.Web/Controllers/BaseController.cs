using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ANZHFR.Web.Controllers
{
    public class BaseController : Controller
    {
        public int PageSize = 10;
        public string ResultString;

        public BaseController()
        {
            
        }

        public string GetBaseUrl()
        {
            var request = HttpContext.Request;
            var appUrl = HttpRuntime.AppDomainAppVirtualPath;
            var baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl);

            return baseUrl;
        }
        public string CurrentUser()
        {
            string currentUser = "";

            try
            {
                HttpCookie userCookie = Request.Cookies["ANZHFRInfo"];
                if (userCookie != null)
                {
                    currentUser = userCookie["FullName"];
                }
            }
            catch { }

            return currentUser;
        }
        public int CurrentUserID()
        {
            int currentUserID = 0;

            try
            {
                HttpCookie userCookie = Request.Cookies["ANZHFRInfo"];
                if (userCookie != null)
                {
                    currentUserID = Convert.ToInt32(userCookie["UserID"]);
                }
            }
            catch { }

            return currentUserID;
        }
        public long CurrentHospitalId()
        {
            long id = 0;
            try
            {
				HttpCookie userCookie = Request.Cookies["ANZHFRInfo"];
                if (userCookie != null)
                {
                    id = Convert.ToInt64(userCookie["HospitalID"]);
                }
            }
            catch { }

            return id;
        }
    }
}
