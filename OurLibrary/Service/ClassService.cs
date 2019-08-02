using OurLibrary.Models;
using OurLibrary.Util.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OurLibrary.Service
{
    public class ClassService : BaseService
    {
        public ClassService()
        {

        }

        public override List<object> ObjectList(int offset, int limit)
        {
            List<object> ObjList = new List<object>();
            var Sql = (from p in dbEntities.classes orderby p.class_name select p);
            List<@class> List = Sql.Skip(offset * limit).Take(limit).ToList();
            foreach (@class c in List)
            {
                ObjList.Add(c);
            }
            count = dbEntities.classes.Count();
            return ObjList;
        }

        public override object Update(object Obj)
        {
            @class Class = (@class)Obj;
            Refresh();
            @class DBClass = (@class)GetById(Class.id);
            if (DBClass == null)
            {
                return null;
            }
            dbEntities.Entry(DBClass).CurrentValues.SetValues(Class);
            dbEntities.SaveChanges();
            return Class;
        }

        public override object GetById(string Id)
        {
            @class Class = (from c in dbEntities.classes where c.id.Equals(Id) select c).SingleOrDefault();
            return Class;
        }

        public override void Delete(object Obj)
        {
            @class Class = (@class)Obj;
            dbEntities.classes.Remove(Class);
            dbEntities.SaveChanges();
        }

        public override int ObjectCount()
        {
            return count;// dbEntities.classes.Count();
        }

        public override object Add(object Obj)
        {
            @class Class = (@class)Obj;
            if(Class.id==null)
                Class.id = StringUtil.GenerateRandomChar(7);
            @class newclass = dbEntities.classes.Add(Class);
            dbEntities.SaveChanges();
            return newclass;

        }

        private List<object> ListWithSql(string sql, int limit = 0, int offset = 0)
        {
            List<object> classList = new List<object>();
            var classes = dbEntities.classes
                .SqlQuery(sql
                ).
                Select(c => new
                {
                    c
                });
            if (limit > 0)
            {
                classes = classes.Skip(offset * limit).Take(limit).ToList();
            }
            else
            {
                classes = classes.ToList();
            }
            foreach (var c in classes)
            {
                @class Class = c.c;
                classList.Add(Class);
            }

            return classList;
        }

        public override List<object> SearchAdvanced(Dictionary<string, object> Params, int limit = 0, int offset = 0)
        {
            string id = Params.ContainsKey("id") ? (string)Params["id"] : "";
            string cls_name = Params.ContainsKey("class_name") ? (string)Params["class_name"] : "";

            string orderby = Params.ContainsKey("orderby") ? (string)Params["orderby"] : "";
            string ordertype = Params.ContainsKey("ordertype") ? (string)Params["ordertype"] : "";

            string sql = "select * from class where id like '%" + id + "%'" +
                " and class_name like '%" + cls_name + "%'";
            if (!orderby.Equals(""))
            {
                sql += " ORDER BY " + orderby;
                if (!ordertype.Equals(""))
                {
                    sql += " " + ordertype;
                }
            }
            count = countSQL(sql, dbEntities.classes);
            return ListWithSql(sql, limit, offset);
        }

        public override int countSQL(string sql, object dbSet)
        {
            return ((DbSet<@class>)dbSet)
                .SqlQuery(sql).Count();
        }

    }
}