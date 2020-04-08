using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using ANZHFR.Web.Models;
using ANZHFR.Data.Models;
using ANZHFR.Web.ExtensionMethods;

using ANZHFR.Services.Synonyms;

namespace ANZHFR.Web.Controllers
{
    public class SynonymChildController : BaseController
    {
        private readonly SynonymChildServices _synonymChildServices;

		public SynonymChildController()
        {
			_synonymChildServices = new SynonymChildServices();
        }

        [Authorize]
		public JsonResult Index(long SynonymId, string search)
        {
			List<SynonymChildModel> results = GetSynonyms(SynonymId);

			if (results.Count > 0)
			{
				return Json(results, JsonRequestBehavior.AllowGet);
			}

			return Json(null, JsonRequestBehavior.AllowGet);
        }

		[HttpPost]
		public JsonResult Add(long SynonymId, string Word)
		{
			//string msg = "Synonym could not be added.";
			SynonymChild synonym = _synonymChildServices.Insert(SynonymId, Word);
			if (synonym != null) {
				//msg = "Synonym added.";
                return Json(GetSynonyms(SynonymId));
			}

			return Json(null);
		}

        [Authorize]
		public JsonResult Edit(long id = 0)
		{
			SynonymChild synonym = _synonymChildServices.GetSynonymById(id);

			if (synonym == null)
			{
				return null;
			}

			SynonymChildModel model = synonym.GetSynonymChildModel();

			return Json(model, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult Edit(long Id, string word)
		{
			SynonymChild synonym = _synonymChildServices.Update(Id, word);

			return Json(new SynonymChildModel { Id = synonym.Id, SynonymId = synonym.SynonymID, Word = synonym.Word });
		}

        [Authorize]
		public JsonResult Delete(long id = 0, long synonymId = 0)
		{
			if (_synonymChildServices.Delete(id))
			{
				List<SynonymChildModel> results = GetSynonyms(synonymId);

				if (results.Count > 0)
					return Json(results, JsonRequestBehavior.AllowGet);
				else
					return Json(null, JsonRequestBehavior.AllowGet);
			}
			return Json(null, JsonRequestBehavior.AllowGet);
		}

		private List<SynonymChildModel> GetSynonyms(long SynonymId)
		{
			List<SynonymChild> results = _synonymChildServices.GetAll("").Where(s => s.SynonymID == SynonymId).OrderByDescending(s => s.Id).ToList();

			return results.Select(s => new SynonymChildModel
			{
				Id = s.Id,
				SynonymId = s.SynonymID,
				Word = s.Word
			}).ToList<SynonymChildModel>();
		}

		//private List<SynonymChild> GetSynonyms(long SynonymId, string search)
		//{
		//	List<SynonymChild> results = _synonymChildServices.GetAll(search).Where(s => s.SynonymID == SynonymId).OrderByDescending(s => s.Id).ToList();

		//	return results;
		//}
    }
}
