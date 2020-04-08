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

namespace ANZHFR.Web.Controllers
{
    public class TransferHospitalController : BaseController
    {
        private readonly TransferHospitalServices _transferHospitalServices;

        public TransferHospitalController()
        {
            _transferHospitalServices = new TransferHospitalServices();
        }

        //
        // GET: /Hospital/
        [Authorize]
        public ActionResult Index(int? page, string search, string message)
        {
            long hopitalId = CurrentHospitalId();

            var results = _transferHospitalServices.Get(hopitalId, search);

            ViewBag.MenuTransferHospitals = "active";
            ViewBag.FilterSearch = search;
            ViewBag.Message = message;
            int pageNumber = (page ?? 1);
            return View(results.OrderBy(x => x.Name).ToPagedList(pageNumber, PageSize));
        }

        [Authorize]
        public ActionResult Add(int page = 1, string search = "")
        {
            TransferHospitalModel model = new TransferHospitalModel();

            model.Page = page;
            model.FilterSearchName = search;
            model.HospitalID = CurrentHospitalId();

            return View(model);
        }

        [HttpPost]
        public ActionResult Add(TransferHospitalModel model)
        {
            if (ModelState.IsValid)
            {
                TransferHospital hospital = _transferHospitalServices.Insert(model.HospitalID, model.Name, model.StreetAddress, model.StreetAddress2, model.Suburb, model.City, model.PostCode, model.State, model.Country, model.AdminEmail, model.Phone);

                return RedirectToAction("Index", new { page = model.Page, search = model.FilterSearchName, message = "Transfer Hospital added." });
            }
            return View();
        }

        [Authorize]
        public ActionResult Edit(long id = 0, int page = 1, string search = "")
        {
            TransferHospital hospital = _transferHospitalServices.GetTransferHospitalById(id);

            if (hospital == null)
            {
                return HttpNotFound();
            }

            TransferHospitalModel model = hospital.GetTransferHospitalModel();

            model.Page = page;
            model.FilterSearchName = search;
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(TransferHospitalModel model)
        {
            if (ModelState.IsValid)
            {
                TransferHospital hospital = _transferHospitalServices.Update(model.TransferHospitalID, model.HospitalID, model.Name, model.StreetAddress, model.StreetAddress2, model.Suburb, model.City, model.PostCode, model.State, model.Country, model.AdminEmail, model.Phone);

                return RedirectToAction("Index", new { page = model.Page, search = model.FilterSearchName, message = "Transfer Hospital saved." });
            }

            return View(model);
        }

        [Authorize]
        public ActionResult Delete(long id = 0, int page = 1, string search = "")
        {
            string message = "Error in deleting!";
            if (_transferHospitalServices.Delete(id))
            {
                message = "Transfer Hospital deleted.";
            }

            return RedirectToAction("index", new { page = page, search = search, message = message });
        }
    }
}