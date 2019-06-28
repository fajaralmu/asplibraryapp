using OurLibrary.Models;
using OurLibrary.Util.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public override List<object> SearchAdvanced(Dictionary<string, object> Params, int limit = 0, int offset = 0)
        {
            string id = Params.ContainsKey("id") ? (string)Params["id"] : "";
            string name = Params.ContainsKey("name") ? (string)Params["name"] : "";
            string email = Params.ContainsKey("email") ? (string)Params["email"] : "";
            string phone = Params.ContainsKey("phone") ? (string)Params["phone"] : "";
            string address = Params.ContainsKey("address") ? (string)Params["address"] : "";
            string orderby = Params.ContainsKey("orderby") ? (string)Params["orderby"] : "";
            string ordertype = Params.ContainsKey("ordertype") ? (string)Params["ordertype"] : "";

            string sql = "select * from author " +
               "where author.name like '%" + name + "%'" +
               " and author.id like '%" + id + "%'" +
               " and author.email like '%" + email + "%'" +
               " and author.phone like '%" + phone + "%'" +
               " and author.address like '%" + address + "%'";
            if (!orderby.Equals(""))
            {
                sql += " ORDER BY " + orderby;
                if (!ordertype.Equals(""))
                {
                    sql += " " + ordertype;
                }
            }
            count = countSQL(sql, dbEntities.authors);
            return ListWithSql(sql, limit, offset);
        }

        public override int countSQL(string sql, object dbSet)
        {
            return ((DbSet<author>)dbSet)
                .SqlQuery(sql).Count();
        }

        private List<object> ListWithSql(string sql, int limit = 0, int offset = 0)
        {
            List<object> authorList = new List<object>();
            var authors = dbEntities.authors
                .SqlQuery(sql
                ).
                Select(author => new
                {
                    author
                });
            if (limit > 0)
            {
                authors = authors.Skip(offset * limit).Take(limit).ToList();
            }
            else
            {
                authors = authors.ToList();
            }
            /*  where b.author.name.Contains(val)
               || b.name.Contains(val) || b.review.Contains(val) || b.author.name.Contains(val)

            || b.category.category_name.Contains(val) || b.publisher.name.Contains(val)
               select b;*/
            foreach (var a in authors)
            {
                author Auhtor = a.author;
                authorList.Add(Auhtor);
            }

            return authorList;
        }
    }
}