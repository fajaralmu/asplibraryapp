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
            try
            {
                book_record book_record = (book_record)Obj;
                dbEntities.Entry(book_record).CurrentValues.SetValues(book_record);
                dbEntities.SaveChanges();
                return book_record;
            }
            catch (Exception e)
            {
                throw e;
            }
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
                book_record.id = StringUtil.GenerateRandomChar(7);
            book_record newbook_record = dbEntities.book_record.Add(book_record);
            dbEntities.SaveChanges();
            return newbook_record;

        }

        public bool IsAvailable(string book_rec_id)
        {
            book_record BookRecord = (book_record)GetById(book_rec_id);
            return BookRecord.available == 1;
        }

        public book_record FindByIdFull(string Id)
        {
            string sql = "select * from book_record where id = '" + Id + "'";
            List<book_record> book_record = (List<book_record>)ObjectUtil.ConvertList(ListWithSql(sql), typeof(List<book_record>));
            if (book_record.Count > 0)
                return book_record[0];
            return null;
        }

        public List<book_record> FindByBookId(string Id)
        {
            string sql = "select * from book_record where book_id = '" + Id + "'";
            List<book_record> book_record = (List<book_record>)ObjectUtil.ConvertList(ListWithSql(sql), typeof(List<book_record>));
            return book_record;
        }

        private List<object> ListWithSql(string sql, int limit = 0, int offset = 0)
        {
            List<object> book_recordsList = new List<object>();
            var book_records = dbEntities.book_record
                .SqlQuery(sql
                ).
                Select(book_record => new
                {
                    book_record,
                    book = dbEntities.books.Where(b => b.id.Equals(book_record.book_id)).Select(p => p).FirstOrDefault()
                });
            if (limit > 0)
            {
                book_records = book_records.Skip(offset * limit).Take(limit).ToList();
            }
            else
            {
                book_records = book_records.ToList();
            }
            /*  where b.author.name.Contains(val)
               || b.title.Contains(val) || b.review.Contains(val) || b.author.name.Contains(val)

            || b.category.category_name.Contains(val) || b.publisher.name.Contains(val)
               select b;*/
            foreach (var b in book_records)
            {
                book_record Book = new book_record();
                Book = b.book_record;
                Book.book = b.book;
                //   Debug.WriteLine("title:"+Book.title+", cat:"+Book.category.category_name+", auth:"+Book.author.name);
                book_recordsList.Add(Book);
            }

            return book_recordsList;
        }

        public override List<object> SearchAdvanced(Dictionary<string, object> Params, int limit = 0, int offset = 0)
        {
            string id = Params.ContainsKey("id") ? (string)Params["id"] : "";
            string book_code = Params.ContainsKey("book_code") ? (string)Params["book_code"] : "";
            string book = Params.ContainsKey("book") ? (string)Params["book"] : "";
            string additional_info = Params.ContainsKey("additional_info") ? (string)Params["additional_info"] : "";
            string orderby = Params.ContainsKey("orderby") ? (string)Params["orderby"] : "";
            string ordertype = Params.ContainsKey("ordertype") ? (string)Params["ordertype"] : "";

            string sql = "select * from book_record " +
               "left join book on book.id = book_record.book_id " +
             " where book_record.id like '%" + id + "%'" +
                " and book.title like '%" + book + "%'" +
                 " and book_record.book_code like '%" + book_code + "%'" +
                  " and book_record.additional_info like '%" + additional_info + "%'";
            if (!orderby.Equals(""))
            {
                sql += " ORDER BY " + orderby;
                if (!ordertype.Equals(""))
                {
                    sql += " " + ordertype;
                }
            }
            count = countSQL(sql, dbEntities.book_record);
            return ListWithSql(sql, limit, offset);
        }


    }
}