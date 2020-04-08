using ANZHFR.Data.Models;
using ANZHFR.Services.Auth;
using ANZHFR.Services.Patients;
using ANZHFR.Web.ExtensionMethods;
using ANZHFR.Web.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ANZHFR.Web.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        private readonly UserServices _userServices;
        private readonly HospitalServices _hospitalServices;
        private readonly LookupServices _lookupServices;

        public UserController()
        {
            _userServices = new UserServices();
            _hospitalServices = new HospitalServices();
            _lookupServices = new LookupServices();
        }

        [Authorize]
        public ActionResult index(int? page, string name, string email, string message, string hospitalID, string state)
        {
            UserInfoModel user = HttpContext.User.GetUserInfo();

            if (user.AccessLevel < (int)ExtensionMethods.AccessLevel.FullAccess)
            {
                return RedirectToAction("Edit", new { id = user.ID, page = page, name = name, email = email, hospitalID = hospitalID, state = state });
            }

            var results = _userServices.Get(hospitalID, name, email, state);

            ViewBag.MenuUsers = "active";
            ViewBag.FilterSearchName = name;
            ViewBag.FilterSearchEmail = email;
            ViewBag.Message = message;

            List<SelectListItem> hospitalList = new List<SelectListItem>();
            hospitalList.AddRange(from h in _hospitalServices.GetAll()
                                  select new SelectListItem
                                  {
                                      Text = h.HName,
                                      Value = h.HospitalID.ToString()
                                  });

            ViewBag.HospitalList = hospitalList;

            List<SelectListItem> stateList = new List<SelectListItem>();
            stateList.AddRange(from s in _lookupServices.GetStates()
                               select new SelectListItem
                               {
                                   Text = s
                               });

            ViewBag.State = stateList;

            int pageNumber = (page ?? 1);
            return View(results.OrderBy(x => x.UFirstName).ToPagedList(pageNumber, PageSize));
        }

        [Authorize]
        public ActionResult Create(int page = 1, string name = "", string email = "", string state = "")
        {
            ViewBag.MenuUsers = "active";

            var model = new UserModel();
            model.Page = page;
            model.FilterSearchName = name;
            model.FilterSearchEmail = email;
            model.FilterSearchState = state;
            model.HospitalID = CurrentHospitalId();

            model.HospitalList = _hospitalServices.GetAll();
            model.AccessLevelList = _lookupServices.GetAccessLevels();

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(UserModel model)
        {
            ViewBag.MenuUsers = "active";

            if (ModelState.IsValid)
            {
                //Email should be unique
                if (_userServices.IsUserExists(model.Email))
                {
                    model.Email = string.Empty;
                    model.ErrorMessage = "Email already taken. Try with different email.";
                }
                else
                {
                    string hp = ANZHFR.Core.Helpers.StringHelper.GenerateSaltedSHA1(model.Password.Trim());

                    var user = _userServices.Insert(model.Email, model.FirstName, model.Surname, hp, model.HospitalID, model.AccessLevel, model.AdminNotes, model.Active, model.UPosition);

                    if (user != null)
                        return RedirectToAction("Index", new { page = model.Page, name = model.FilterSearchName, email = model.FilterSearchEmail, state = model.FilterSearchState, message = "User created" });
                    else
                        model.ErrorMessage = "Error in creating user!";
                }
            }

            model.HospitalList = _hospitalServices.GetAll();
            model.AccessLevelList = _lookupServices.GetAccessLevels();

            return View(model);
        }

        [Authorize]
        public ActionResult Edit(long id = 0, int page = 1, string search = "")
        {
            ViewBag.MenuUsers = "active";

            var user = _userServices.GetUserById(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            var model = PrepearUserModel(user);

            model.Page = page;
            model.FilterSearchName = search;

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(UserModel model)
        {
            ViewBag.MenuUsers = "active";

            if (ModelState.IsValid)
            {
                string hp = "";
                if (!string.IsNullOrWhiteSpace(model.Password1))
                {
                    hp = ANZHFR.Core.Helpers.StringHelper.GenerateSaltedSHA1(model.Password1.Trim());
                }
                var user = _userServices.Update(model.UserID, model.FirstName, model.Surname, hp, model.HospitalID, model.AccessLevel, model.AdminNotes, model.Active, model.UPosition);

                return RedirectToAction("Index", new { page = model.Page, name = model.FilterSearchName, email = model.FilterSearchEmail, message = "User saved" });
            }

            model.HospitalList = _hospitalServices.GetAll();
            model.AccessLevelList = _lookupServices.GetAccessLevels();

            return View(model);
        }

        [NonAction]
        private UserModel PrepearUserModel(User user)
        {
            var model = new UserModel();
            model.UserID = user.UserID;
            model.Email = user.UEmail;
            model.FirstName = user.UFirstName;
            model.Surname = user.USurname;
            model.AccessLevel = user.UAccessLevel;
            model.AccessLevelList = _lookupServices.GetAccessLevels();
            model.Password = "123456";//dummy
            model.ConfirmPassword = "123456";//dummy
            model.HospitalList = _hospitalServices.GetAll();
            model.HospitalID = user.UHospitalID;
            model.AdminNotes = user.AdminNotes;
            model.Active = user.UActive;
            model.UPosition = user.UPosition;

            return model;
        }

        [HttpPost]
        public JsonResult CheckUserName(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                    return Json(new { Result = false, Message = "Username required" });

                if (_userServices.IsUserExists(email))
                    return Json(new { Result = false, Message = email + " - aleady taken. Please try with different email" });
            }
            catch { return Json(new { Result = false, Message = "Error! Try again later." }); }

            return Json(new { Result = true });
        }

        [Authorize]
        public ActionResult Delete(long id = 0, int page = 1, string search = "")
        {
            string message = "Error in deleting!";
            if (_userServices.Delete(id))
            {
                message = "User deleted";
            }

            return RedirectToAction("index", new { page = page, search = search, message = message });
        }

        [Authorize]
        public ActionResult ChangePassword(string email)
        {
            var user = _userServices.GetUserByEmail(email);

            if (user == null)
            {
                return HttpNotFound();
            }

            var model = PrepearUserModel(user);

            model.Page = 1;

            return View(model);
        }

        //[HttpPost]
        //
        //public ActionResult ChangePassword(UserModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        string hp = ANZHFR.Core.Helpers.StringHelper.GenerateSaltedSHA1(model.Password.Trim());

        //        if (_userServices.IsValidUser(model.Email, hp))
        //        {
        //            hp = KiKhai.Core.Helpers.StringHelper.CreateHash(userModel.Password.Trim());
        //            _authServices.ChangePassword(userModel.Id, hp);
        //            ViewBag.Message = "Password changed successfully";
        //            userModel.Password = "";
        //            userModel.CurrentPassword = "";
        //        }
        //        else
        //            ViewBag.Message = "Invalid current password";
        //    }

        //    return View(userModel);
        //}
    }
}