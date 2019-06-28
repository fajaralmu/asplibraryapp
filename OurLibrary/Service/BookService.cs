using OurLibrary.Models;
using OurLibrary.Util.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace OurLibrary.Service
{
    public class BookService : BaseService
    {
        

        public BookService()
        {

        }

        public override List<object> ObjectList(int offset, int limit)
        {
            List<object> ObjList = new List<object>();
            var Sql = (from p in dbEntities.books orderby p.title select p);
            List<book> List = Sql.Skip(offset * limit).Take(limit).ToList();
            foreach (book c in List)
            {
                ObjList.Add(c);
            }
            return ObjList;
        }
        public override object Update(object Obj)
        {
            book book = (book)Obj;
            dbEntities.Entry(book).CurrentValues.SetValues(book);
            dbEntities.SaveChanges();
            return book;
        }

        public book GetCompleteBook(string ID)
        {

            try
            {
                var DBBook = dbEntities.books
                      .Where(b => b.id.Equals(ID)).
                      Select(book => new
                      {
                          book,
                          publisher = dbEntities.publishers.Where(p => p.id.Equals(book.publisher_id)).Select(p => p).FirstOrDefault(),
                          author = dbEntities.authors.Where(a => a.id.Equals(book.author_id)).Select(p => p).FirstOrDefault(),
                          category = dbEntities.categories.Where(c => c.id.Equals(book.category_id)).Select(p => p).FirstOrDefault()
                      }).Single();

                book Book = null;
                if (DBBook != null)
                {
                    Book = new book();
                    Book = DBBook.book;
                    Book.category = DBBook.category;
                    Book.publisher = DBBook.publisher;
                    Book.author = DBBook.author;
                }

                return Book;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        private List<object> ListWithSql(string sql, int limit =0, int offset = 0)
        {
            List<object> bookList = new List<object>();
            var books = dbEntities.books
                .SqlQuery(sql
                ).
                Select(book => new
                {
                    book,
                    publisher = dbEntities.publishers.Where(p => p.id.Equals(book.publisher_id)).Select(p => p).FirstOrDefault(),
                    author = dbEntities.authors.Where(a => a.id.Equals(book.author_id)).Select(p => p).FirstOrDefault(),
                    category = dbEntities.categories.Where(c => c.id.Equals(book.category_id)).Select(p => p).FirstOrDefault()
                });
            if (limit > 0) {
               books = books.Skip(offset*limit).Take(limit) .ToList();
            }else
            {
                books = books.ToList();
            }
            /*  where b.author.name.Contains(val)
               || b.title.Contains(val) || b.review.Contains(val) || b.author.name.Contains(val)

            || b.category.category_name.Contains(val) || b.publisher.name.Contains(val)
               select b;*/
            foreach (var b in books)
            {
                book Book = new book();
                Book = b.book;
                Book.category = b.category;
                Book.publisher = b.publisher;
                Book.author = b.author;
                //   Debug.WriteLine("title:"+Book.title+", cat:"+Book.category.category_name+", auth:"+Book.author.name);
                bookList.Add(Book);
            }

            return  bookList;
        }

        public override List<object> SearchAdvanced(Dictionary<string, object> Params, int limit=0, int offset =0)
        {

            string id = Params.ContainsKey("id") ? (string)Params["id"] : "";
            string title = Params.ContainsKey("title")  ? (string) Params["title"] : "";
            string publisher = Params.ContainsKey("publisher")  ? (string)Params["publisher"] : "";
            string category = Params.ContainsKey("category")  ? (string)Params["category"] : "";
            string author = Params.ContainsKey("author")  ? (string)Params["author"] : "";
            string page = Params.ContainsKey("page") ? (string)Params["page"] : "";
            string review = Params.ContainsKey("review") ? (string)Params["review"] : "";
            string orderby = Params.ContainsKey("orderby") ? (string)Params["orderby"] : "";
            string ordertype = Params.ContainsKey("ordertype") ? (string)Params["ordertype"] : "";
            int pageInt = 0;
            int.TryParse(page, out pageInt);
            string sql = "select * from book " +
               "left join author on author.id = book.author_id " +
               "left join publisher on publisher.id = book.publisher_id " +
               "left join category on category.id = book.category_id " +
               "where book.title like '%" + title + "%'" +
            //    " and book.page =" + pageInt +
                " and book.id like '%" + id + "%'" +
                " and book.review like '%" + review + "%'" +
               " and publisher.name like '%" + publisher + "%'" +
               " and category.category_name like '%" +category + "%'" +
               " and author.name like  '%" + author + "%'";
            if (!orderby.Equals(""))
            {
                sql += " ORDER BY " + orderby;
                if (!ordertype.Equals(""))
                {
                    sql += " " + ordertype;
                }
            }
            count = countSQL(sql, dbEntities.books);
            return ListWithSql(sql, limit, offset);
        }

        public List<object> SearchBookUniversal(string val, int limit=0, int offset=0)
        {
            string sql = "select * from book " +
                "left join author on author.id = book.author_id " +
                "left join publisher on publisher.id = book.publisher_id " +
                "left join category on category.id = book.category_id " +
                "where book.title like '%" + val + "%'" +
                " or book.review like '%" + val + "%'" +
                " or publisher.name like '%" + val + "%'" +
                " or category.category_name like '%" + val + "%'" +
                " or author.name like  '%" + val + "%'";
            count = countSQL(sql, dbEntities.books);
            return ListWithSql(sql, limit, offset);
        }

        public override object GetById(string Id)
        {
            book book = (from c in dbEntities.books where c.id.Equals(Id) select c).SingleOrDefault();
            return book;
        }

        public override void Delete(object Obj)
        {
            book book = (book)Obj;
            dbEntities.books.Remove(book);
            dbEntities.SaveChanges();
        }

        public override int ObjectCount()
        {
            return dbEntities.books.Count();
        }

        public override object Add(object Obj)
        {
            book book = (book)Obj;
            if (book.id == null)
                book.id = StringUtil.GenerateRandomChar(7);
            book newbook = dbEntities.books.Add(book);
            try
            {
                dbEntities.SaveChanges();
                return newbook;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {

                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
                //  return null;
            }

        }

        public override int countSQL(string sql, object dbSet)
        {
            return ((DbSet<book>)dbSet)
                .SqlQuery(sql).Count();
        }
    }
}