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
    public class ContentController : BaseController
    {
        private readonly ContentServices _contentServices;

        //private ANZHFREntities db = new ANZHFREntities();

        public ContentController()
        {
            _contentServices = new ContentServices();
        }

        //
        // GET: /Content/
        [Authorize]
        public ActionResult Index(int? page, string search, string message)
        {
            var results = _contentServices.GetAll(search);

            ViewBag.MenuContent = "active";
            ViewBag.FilterSearch = search;
            ViewBag.Message = message;
            int pageNumber = (page ?? 1);
            return View(results.OrderBy(x => x.Content_Key).ToPagedList(pageNumber, PageSize));
        }

        //
        // GET: /Content/Create
        [Authorize]
        public ActionResult Create(int page = 1, string search = "")
        {
            ContentModel model = new ContentModel();

            model.Page = page;
            model.FilterSearchName = search;

            return View(model);
        }

        //
        // POST: /Content/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ContentModel model)
        {
            if (ModelState.IsValid)
            {
                Content content = _contentServices.Insert(model.Content_Key, model.Content_HTML, model.Content_Text);

                return RedirectToAction("Index", new { page = model.Page, name = model.FilterSearchName, message = "Content added." });
            }
            return View();
        }

        //
        // GET: /Content/Edit/5
        [Authorize]
        public ActionResult Edit(long id = 0, int page = 1, string search = "")
        {
            Content content = _contentServices.GetContentById(id);

            if (content == null)
            {
                return HttpNotFound();
            }

            ContentModel model = content.GetContentModel();

            model.Page = page;
            model.FilterSearchName = search;
            return View(model);
        }

        //
        // POST: /Content/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ContentModel model)
        {
            if (ModelState.IsValid)
            {
                Content content = _contentServices.Update(model.Content_ID, model.Content_Key, model.Content_HTML, model.Content_Text);

                return RedirectToAction("Index", new { page = model.Page, name = model.FilterSearchName, message = "Content saved." });
            }

            return View(model);
        }

        //
        // GET: /Content/Delete/5

        [Authorize]
        public ActionResult Delete(long id = 0, int page = 1, string search = "")
        {
            string message = "Error in deleting!";
            if (_contentServices.Delete(id))
            {
                message = "Content deleted.";
            }

            return RedirectToAction("index", new { page = page, search = search, message = message });
        }
    }
}