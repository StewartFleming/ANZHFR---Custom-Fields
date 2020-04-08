using ANZHFR.Data.Models;
using ANZHFR.Services.Synonyms;
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
    public class SynonymController : BaseController
    {
        private readonly SynonymServices _synonymServices;

        public SynonymController()
        {
            _synonymServices = new SynonymServices();
        }

        [Authorize]
        public ActionResult Index(long? Id, int? page, string search, string message)
        {
            var results = _synonymServices.GetAll(search);

            ViewBag.MenuSynonyms = "active";
            ViewBag.FilterSearch = search;
            ViewBag.Message = message;
            int pageNumber = (page ?? 1);

            return View(results.OrderBy(x => x.Word).ToPagedList(pageNumber, PageSize));
        }

        [Authorize]
        [HttpPost]
        public ActionResult Add(SynonymModel model)
        {
            string msg = "Word could not be added.";
            if (ModelState.IsValid)
            {
                Synonym synonym = _synonymServices.Insert(model.Word);
                if (synonym != null)
                {
                    msg = "Word added.";
                }
            }

            return RedirectToAction("Index", new { page = (model.Page == 0 ? 1 : model.Page), name = model.FilterSearchName, message = msg });
        }

        //public ActionResult Edit(long id = 0, int page = 1, string search = "")
        //{
        //	Synonym synonym = _synonymServices.GetSynonymById(id);

        //	if (synonym == null)
        //	{
        //		return null;
        //	}

        //	SynonymModel model = synonym.GetSynonymModel();

        //	model.Page = page;
        //	model.FilterSearch = search;
        //	return View(model);
        //}

        [Authorize]
        public JsonResult Edit(long id = 0)
        {
            Synonym synonym = _synonymServices.GetSynonymById(id);

            if (synonym == null)
            {
                return null;
            }

            SynonymModel model = synonym.GetSynonymModel();

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Edit(SynonymModel model)
        {
            if (ModelState.IsValid)
            {
                Synonym synonym = _synonymServices.Update(model.Id, model.Word);

                return Json(new SynonymModel { Id = synonym.Id, Word = synonym.Word });
            }

            return null;
        }

        [Authorize]
        public ActionResult Delete(long id = 0, int page = 1, string search = "")
        {
            string message = "Error in deleting!";
            if (_synonymServices.Delete(id))
            {
                message = "Word deleted.";
            }

            return RedirectToAction("index", new { page = page, search = search, message = message });
        }
    }
}