using ANZHFR.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANZHFR.Services.Patients
{
    public class ContentServices : BaseServices
    {
        public List<Content> GetAll(string name = "")
        {
            var results = Entity.Contents.ToList();

            if (!string.IsNullOrEmpty(name))
            {
                results = results.Where(x => x.Content_Key.Trim().ToUpper().Contains(name.Trim().ToUpper())).ToList();
            }

            return results;
        }

        public Content GetContentById(long id)
        {
            return Entity.Contents.Find(id);
        }

        public Content Insert(string Content_Key, string Content_HTML, string Content_Text)
        {
            try
            {
                Content content = new ANZHFR.Data.Models.Content();

                content.Content_Key = Content_Key;
                content.Content_HTML = Content_HTML;
                content.Content_Text = Content_Text;
                Entity.Contents.Add(content);
                Entity.SaveChanges();

                return content;
            }
            catch
            {
                return null;
            }
        }

        public Content Update(long id, string Content_Key, string Content_HTML, string Content_Text)
        {
            try
            {
                Content content = GetContentById(id);

                content.Content_Key = Content_Key;
                content.Content_HTML = Content_HTML;
                content.Content_Text = Content_Text;
            

                Entity.SaveChanges();

                return content;
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
                Content content = GetContentById(id);
                Entity.Contents.Remove(content);
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
