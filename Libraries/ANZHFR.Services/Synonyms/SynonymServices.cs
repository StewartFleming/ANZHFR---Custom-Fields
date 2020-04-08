using ANZHFR.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANZHFR.Services.Synonyms
{
    public class SynonymServices : BaseServices
	{
		public List<Synonym> GetAll(string word)
		{
			List<Synonym> results = Entity.Synonyms.ToList();

			if (!string.IsNullOrEmpty(word))
			{
				results = results.Where(x => x.Word.Trim().ToUpper().Contains(word.Trim().ToUpper())).ToList();
			}

			return results;
		}

		public Synonym GetSynonymById(long id)
        {
            return Entity.Synonyms.Find(id);
        }

		public Synonym Insert(string word)
        {
            try
            {
                Synonym synonym = new Synonym();
				synonym.Word = word;

				Entity.Synonyms.Add(synonym);
                Entity.SaveChanges();

				return synonym;
            }
            catch
            {
                return null;
            }
        }

		public Synonym Update(long Id, string word)
        {
            try
            {
				Synonym synonym = GetSynonymById(Id);
				synonym.Word = word;

                Entity.SaveChanges();

                return synonym;
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
				Synonym synonym = GetSynonymById(id);
				Entity.Synonyms.Remove(synonym);
                Entity.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
