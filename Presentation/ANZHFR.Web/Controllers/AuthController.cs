using ANZHFR.Data.Models;
using ANZHFR.Services.Auth;
using ANZHFR.Services.Patients;
using ANZHFR.Web.ExtensionMethods;
using ANZHFR.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace ANZHFR.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserServices _userServices;
        private readonly HospitalServices _hospitalServices;
        private readonly PatientServices _patientServices;

        public AuthController()
        {
            _userServices = new UserServices();
            _hospitalServices = new HospitalServices();
            _patientServices = new PatientServices();
        }

        //
        // GET: /Auth/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Login/
        public ActionResult Login()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("dashboard2", "home");
            }

            ViewBag.Message = string.Format("{0}", TempData["ForgotPasswordMessage"]);

            return View();
        }

        //
        // POST: /Login/
        [HttpPost]
        public JsonResult Login(string email, string password, string returnUrl, bool rememberMe = false)
        {
            string hp = ANZHFR.Core.Helpers.StringHelper.GenerateSaltedSHA1(password.Trim());
            if (_userServices.IsValidUser(email, hp))
            //if (true)
            {
                FormsAuthentication.SetAuthCookie(email, rememberMe);

                var user = _userServices.GetUserByEmail(email);
                // Set the auth cookie
                HttpCookie cookie = new HttpCookie("ANZHFRInfo");
                cookie["Name"] = user.UFirstName;
                cookie["FullName"] = user.UFirstName + " " + user.USurname;
                cookie["Email"] = email;
                cookie["UserID"] = user.UserID.ToString();
                cookie["HospitalID"] = user.UHospitalID.ToString();

                var cookieExpiryMinutes = 1440; // ConfigurationManager.AppSettings["CookieExpiryMinutes"].ToInt() ?? 1440;
                cookie.Expires = DateTime.Now.AddMinutes(cookieExpiryMinutes);

                Response.Cookies.Add(cookie);

                // redirect url
                string url = "/home/dashboard2";
                if (!string.IsNullOrWhiteSpace(returnUrl))
                {
                    url = returnUrl;
                }

                return Json(new { Result = true, Url = url });
            }
            else
            {
                return Json(new { Result = false, Message = "Invalid email or password." + _userServices.myglobalerror });
            }
        }

        //
        // GET: /Logout/
        public ActionResult Logout()
        {
            HttpCookie userCookie = Request.Cookies["ANZHFRInfo"];
            if (userCookie != null)
            {
                userCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(userCookie);
            }

            FormsAuthentication.SignOut();
            return RedirectToAction("index", "home");
        }

        //
        // GET: /Login/
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            var user = _userServices.GetUserByEmail(email);
            if (user != null)
            {
                string password = ExtensionMethods.ExtensionMethods.GenerateRandomString(10, 16, true);
                string hp = ANZHFR.Core.Helpers.StringHelper.GenerateSaltedSHA1(password.Trim());

                _userServices.ChangePassword(user.UserID, hp);

                MailMessage mm = new MailMessage();
                mm.To.Add(new MailAddress(email));
                mm.From = new MailAddress(ConfigurationManager.AppSettings["EmailFromAddress"]);
                mm.Subject = string.Format("Requested password reset for {0} Hip Fracture Registry.", ConfigurationManager.AppSettings["Location"]);

                string loginUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, ConfigurationManager.AppSettings["LoginPath"]);
                mm.Body = string.Format("Hi {0}, <br /><br />Your new password is: {1}<br /><br />Please login <a href='{2}'>here</a>.<br /><br />Regards<br />Hip Fracture Registry Team", user.UFirstName, password, loginUrl);
                mm.IsBodyHtml = true;

                new SmtpClient().Send(mm);
            }

            TempData["ForgotPasswordMessage"] = "Forgot password email has been sent to the specified email address.";
            return RedirectToAction("Login");
        }

        [Authorize]
        public ActionResult ChangePassword(long id = 0)
        {
            var user = _userServices.GetUserById(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            var model = PrepareUserModel(user);
            return View(model);
        }

        [NonAction]
        private UserModel PrepareUserModel(User user)
        {
            var model = new UserModel();
            model.UserID = user.UserID;
            model.Email = user.UEmail;
            model.FirstName = user.UFirstName;
            model.Surname = user.USurname;
            model.AccessLevel = user.UAccessLevel;
            model.Password = "123456";//dummy
            model.ConfirmPassword = "123456";//dummy
            model.HospitalID = user.UHospitalID;
            model.AdminNotes = user.AdminNotes;
            model.Active = user.UActive;
            model.UPosition = user.UPosition;

            return model;
        }

        [HttpPost]
        public ActionResult ChangePassword(UserModel model)
        {
            ViewBag.MenuUsers = "active";

            if (ModelState.IsValid)
            {
                string hp = "";
                if (!string.IsNullOrWhiteSpace(model.Password1))
                {
                    hp = ANZHFR.Core.Helpers.StringHelper.GenerateSaltedSHA1(model.Password1.Trim());
                }
                var user = _userServices.ChangePassword(model.UserID, hp);

                return RedirectToAction("Index", new { page = model.Page, search = model.FilterSearchName, message = "User saved" });
            }

            return View(model);
        }
        [Authorize]
        public ActionResult SwitchHospital(long? hospitalID)
        {
            ViewBag.MenuSwitch = "active";
            // Get Hospitals
            List<SelectListItem> hospitalList = new List<SelectListItem>();
            hospitalList.AddRange(from h in _hospitalServices.GetAll()
                                  select new SelectListItem
                                  {
                                      Text = h.HName,
                                      Value = h.HospitalID.ToString()
                                  });
            
            ViewBag.HospitalList = hospitalList;
            HttpCookie userCookie = Request.Cookies["ANZHFRInfo"];
            if (hospitalID.ToString() != "" && hospitalID.ToString() != userCookie["HospitalID"])
            { 
                HttpCookie cookie = HttpContext.Response.Cookies["ANZHFRInfo"];
                cookie["Name"] = _hospitalServices.GetHospitalById((long)hospitalID).HName;
                cookie["FullName"] = userCookie["FullName"];
                cookie["Email"] = userCookie["Email"];
                cookie["UserID"] = userCookie["UserID"];
                cookie["HospitalID"] = hospitalID.ToString();
                var cookieExpiryMinutes = 1440; // ConfigurationManager.AppSettings["CookieExpiryMinutes"].ToInt() ?? 1440;
                cookie.Expires = DateTime.Now.AddMinutes(cookieExpiryMinutes);

                Response.Cookies.Add(cookie);
                ViewBag.HospitalID = hospitalID.ToString();
                ViewBag.Year = "2019";
                return View((long)hospitalID);
            }
            else
            {
                ViewBag.HospitalID = userCookie["HospitalID"];
                return View();
            }
        }
        public ActionResult QualityRecords(string submit, long? hospitalID, long? qualityCount, int? year)
        {
            ViewBag.MenuQualityRecords = "active";

            if (hospitalID.ToString() == "")
            {
                HttpCookie quserCookie = Request.Cookies["ANZHFRInfo"];
                hospitalID = long.Parse(quserCookie["HospitalID"]);
            }
            ViewBag.HospitalID = hospitalID.ToString();

            // Get Hospitals
            List<SelectListItem> hospitalList = new List<SelectListItem>();
            hospitalList.AddRange(from h in _hospitalServices.GetAll()
                                  select new SelectListItem
                                  {
                                      Text = h.HName,
                                      Value = h.HospitalID.ToString()
                                  });

            ViewBag.HospitalList = hospitalList;

            //Get Year
            int cYear = 2019;
            if (year != null)
            {
                cYear = year.Value;
            }
            

            //Get Patient Count
            var recordCount = _patientServices.PatientCountByYear(hospitalID, cYear);
            ViewBag.RecordCount = recordCount.ToString();
            ViewBag.Year = cYear.ToString();
            //

            //Determine Quality Record Count
            var estqualityCount = 0;
            if (recordCount > 0)
            {
                estqualityCount = recordCount / 4;
            }
            ViewBag.QualityCount = estqualityCount.ToString();

            switch (submit)
            {
                case "Update":
                    return View();
                    //break;
                case "Create":
                    //do updates
                    ViewBag.CreateResult = _patientServices.CreateQualityRecords(hospitalID, qualityCount, cYear);

                    break;
                default:
                    return View();
                    //break;
            }
            return View();
        }
    }
}