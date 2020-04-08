using ANZHFR.Data.Models;
using ANZHFR.Services.Patients;
using ANZHFR.Web.ExtensionMethods;
using ANZHFR.Web.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ANZHFR.Web.Controllers
{
    public class HospitalController : BaseController
    {
        private readonly HospitalServices _hospitalServices;
        private readonly LookupServices _lookupServices;

        public HospitalController()
        {
            _hospitalServices = new HospitalServices();
            _lookupServices = new LookupServices();
        }

        //
        // GET: /Hospital/
        [Authorize]
        public ActionResult Index(int? page, string search, string message)
        {
            var results = _hospitalServices.GetAll(search);

            ViewBag.MenuHospitals = "active";
            ViewBag.FilterSearch = search;
            ViewBag.Message = message;
            int pageNumber = (page ?? 1);
            return View(results.OrderBy(x => x.HName).ToPagedList(pageNumber, PageSize));
        }

        [Authorize]
        public ActionResult Add(int page = 1, string search = "")
        {
            HospitalModel model = new HospitalModel();

            model.StateList = _lookupServices.GetStates();

            model.Page = page;
            model.FilterSearchName = search;

            return View(model);
        }

        [HttpPost]
        public ActionResult Add(HospitalModel model)
        {
            if (ModelState.IsValid)
            {
                Hospital hospital = _hospitalServices.Insert(model.Name, model.StreetAddress1, model.StreetAddress2, model.Suburb, model.City, model.State, model.PostCode, model.Country, model.Phone, model.AdminEmail);

                return RedirectToAction("Index", new { page = model.Page, name = model.FilterSearchName, message = "Hospital added." });
            }
            return View();
        }

        [Authorize]
        public ActionResult Edit(long id = 0, int page = 1, string search = "")
        {
            Hospital hospital = _hospitalServices.GetHospitalById(id);

            if (hospital == null)
            {
                return HttpNotFound();
            }

            HospitalModel model = hospital.GetHospitalModel();

            model.StateList = _lookupServices.GetStates();

            model.Page = page;
            model.FilterSearchName = search;
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(HospitalModel model)
        {
            if (ModelState.IsValid)
            {
                Hospital hospital = _hospitalServices.Update(model.HospitalID, model.Name, model.StreetAddress1, model.StreetAddress2, model.Suburb, model.City, model.State, model.PostCode, model.Country, model.Phone, model.AdminEmail);

                return RedirectToAction("Index", new { page = model.Page, name = model.FilterSearchName, message = "Hospital saved." });
            }

            return View(model);
        }

        [Authorize]
        public ActionResult Delete(long id = 0, int page = 1, string search = "")
        {
            string message = "Error in deleting!";
            if (_hospitalServices.Delete(id))
            {
                message = "Hospital deleted.";
            }

            return RedirectToAction("index", new { page = page, search = search, message = message });
        }
    }
}