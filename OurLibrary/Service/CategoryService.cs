using OurLibrary.Models;
using OurLibrary.Util.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OurLibrary.Service
{
    public class CategoryService : BaseService
    {


        public override List<object> ObjectList(int offset, int limit)
        {
            List<object> ObjList = new List<object>();
            var Sql = (from p in dbEntities.categories orderby p.category_name select p);
            List<category> List = Sql.Skip(offset * limit).Take(limit).ToList();
            foreach (category c in List)
            {
                ObjList.Add(c);
            }
            count = dbEntities.categories.Count();
            return ObjList;
        }
        public override object Update(object Obj)
        {
            category Category = (category)Obj;
            dbEntities.Entry(Category).CurrentValues.SetValues(Category);
            dbEntities.SaveChanges();
            return Category;
        }

        public override object GetById(string Id)
        {
            category Category = (from c in dbEntities.categories where c.id.Equals(Id) select c).SingleOrDefault();
            return Category;
        }

        public override void Delete(object Obj)
        {
            category Category = (category)Obj;
            dbEntities.categories.Remove(Category);
            dbEntities.SaveChanges();
        }

        public override int ObjectCount()
        {
            return count;// dbEntities.categories.Count();
        }

        public override object Add(object Obj)
        {
            category Category = (category)Obj;
            if (Category.id == null)
                Category.id = StringUtil.GenerateRandomChar(7);
            category newCategory = dbEntities.categories.Add(Category);
            try
            {
                dbEntities.SaveChanges();
                return newCategory;
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

        private List<object> ListWithSql(string sql, int limit = 0, int offset = 0)
        {
            List<object> categoryList = new List<object>();
            var categories = dbEntities.categories
                .SqlQuery(sql
                ).
                Select(category => new
                {
                    category
                });
            if (limit > 0)
            {
                categories = categories.Skip(offset * limit).Take(limit).ToList();
            }
            else
            {
                categories = categories.ToList();
            }
            foreach (var c in categories)
            {
                category Cat = c.category;
                categoryList.Add(Cat);
            }

            return categoryList;
        }

        public override List<object> SearchAdvanced(Dictionary<string, object> Params, int limit = 0, int offset = 0)
        {

            string id = Params.ContainsKey("id") ? (string)Params["id"] : "";
            string cat_name = Params.ContainsKey("category_name") ? (string)Params["category_name"] : "";
            string orderby = Params.ContainsKey("orderby") ? (string)Params["orderby"] : "";
            string ordertype = Params.ContainsKey("ordertype") ? (string)Params["ordertype"] : "";

            string sql = "select * from category where id like '%"+id+"%'"+
                " and category_name like '%"+ cat_name + "%'";
            if (!orderby.Equals(""))
            {
                sql += " ORDER BY " + orderby;
                if (!ordertype.Equals(""))
                {
                    sql += " " + ordertype;
                }
            }
            count = countSQL(sql, dbEntities.categories);
            return ListWithSql(sql, limit, offset);
        }


        public override int countSQL(string sql, object dbSet)
        {
            return ((DbSet<category>)dbSet)
                .SqlQuery(sql).Count();
        }

    }
}