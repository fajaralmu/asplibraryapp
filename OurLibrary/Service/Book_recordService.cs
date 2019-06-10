using OurLibrary.Models;
using OurLibrary.Util.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OurLibrary.Service
{
    public class Book_recordService : BaseService
    {
        public Book_recordService()
        {

        }

        public override List<object> ObjectList(int offset, int limit)
        {
            List<object> ObjList = new List<object>();
            var Sql = (from p in dbEntities.book_record orderby p.book_id select p);
            List<book_record> List = Sql.Skip(offset * limit).Take(limit).ToList();
            foreach (book_record c in List)
            {
                ObjList.Add(c);
            }
            return ObjList;
        }
        public override object Update(object Obj)
        {
            book_record book_record = (book_record)Obj;
            dbEntities.Entry(book_record).CurrentValues.SetValues(book_record);
            dbEntities.SaveChanges();
            return book_record;
        }

        public override object GetById(string Id)
        {
            book_record book_record = (from c in dbEntities.book_record where c.id.Equals(Id) select c).SingleOrDefault();
            return book_record;
        }

        public override void Delete(object Obj)
        {
            book_record book_record = (book_record)Obj;
            dbEntities.book_record.Remove(book_record);
            dbEntities.SaveChanges();
        }

        public override int ObjectCount()
        {
            return dbEntities.book_record.Count();
        }

        public override object Add(object Obj)
        {
            book_record book_record = (book_record)Obj;
            if (book_record.id == null)
                book_record.id = StringUtil.GenerateRandom(7);
            book_record newbook_record = dbEntities.book_record.Add(book_record);
            dbEntities.SaveChanges();
            return newbook_record;

        }

        public List<book_record> FindByBookId(string Id)
        {
            List<book_record> book_record = (from b_r in dbEntities.book_record where b_r.book_id.Equals(Id) orderby b_r.book_code select b_r).ToList();
            return book_record;
        }
    }
}