using ANZHFR.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANZHFR.Services.Synonyms
{
    public class SynonymChildServices : BaseServices
	{
		public List<SynonymChild> GetAll(string word)
		{
			List<SynonymChild> results = Entity.SynonymChilds.ToList();

			if (!string.IsNullOrEmpty(word))
			{
				results = results.Where(x => x.Word.Trim().ToUpper().Contains(word.Trim().ToUpper())).ToList();
			}

			return results;
		}

		public SynonymChild GetSynonymById(long id)
        {
			return Entity.SynonymChilds.Find(id);
        }

		public SynonymChild Insert(long synonymId, string word)
        {
            try
            {
				SynonymChild synonymChilds = new SynonymChild();
				synonymChilds.SynonymID = synonymId;
				synonymChilds.Word = word;

				Entity.SynonymChilds.Add(synonymChilds);
                Entity.SaveChanges();

				return synonymChilds;
            }
            catch
            {
                return null;
            }
        }

		public SynonymChild Update(long Id, string word)
        {
            try
            {
				SynonymChild synonymChilds = GetSynonymById(Id);
				synonymChilds.Word = word;

                Entity.SaveChanges();

				return synonymChilds;
            }
            catch
            {
                return null;
            }
        }

        public bool Delete(long id)
        {
            try
            {
				SynonymChild synonymChilds = GetSynonymById(id);
				Entity.SynonymChilds.Remove(synonymChilds);
                Entity.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

		public string GetSynonym(string word)
		{
			if (Entity.SynonymChilds.Count(x => x.Word == word) > 0)
			{
				return Entity.SynonymChilds.SingleOrDefault(x => x.Word == word).Synonym.Word;
			}

			return word;
		}
    }
}
