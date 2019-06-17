using OurLibrary.Models;
using OurLibrary.Util.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OurLibrary.Service
{
    public class AuthorService : BaseService
    {
        public AuthorService()
        {

        }

        public override List<object> ObjectList(int offset, int limit)
        {
            List<object> ObjList = new List<object>();
            var Sql = (from p in dbEntities.authors orderby p.name select p);
            List<author> List = Sql.Skip(offset * limit).Take(limit).ToList();
            foreach (author c in List)
            {
                ObjList.Add(c);
            }
            return ObjList;
        }
        public override object Update(object Obj)
        {
            author author = (author)Obj;
            dbEntities.Entry(author).CurrentValues.SetValues(author);
            dbEntities.SaveChanges();
            return author;
        }

        public override object GetById(string Id)
        {
            author author = (from c in dbEntities.authors where c.id.Equals(Id) select c).SingleOrDefault();
            return author;
        }

        public override void Delete(object Obj)
        {
            author author = (author)Obj;
            dbEntities.authors.Remove(author);
            dbEntities.SaveChanges();
        }

        public override int ObjectCount()
        {
            return dbEntities.authors.Count();
        }

        public override object Add(object Obj)
        {
            author author = (author)Obj;
            if (author.id == null)
                author.id = StringUtil.GenerateRandomChar(7);
            author newauthor = dbEntities.authors.Add(author);
            dbEntities.SaveChanges();
            return newauthor;

        }
    }
}