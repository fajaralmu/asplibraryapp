using OurLibrary.Models;
using OurLibrary.Util.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace OurLibrary.Service
{
    public class VisitService : BaseService
    {
        public VisitService()
        {

        }

        public override List<object> ObjectList(int offset, int limit)
        {
            List<object> ObjList = new List<object>();
            var Sql = (from p in dbEntities.visits orderby p.date descending select p);
            List<visit> List = Sql.Skip(offset * limit).Take(limit).ToList();
            foreach (visit c in List)
            {
                ObjList.Add(c);
            }
            count = dbEntities.visits.Count();
            return ObjList;
        }

        public override object Update(object Obj)
        {
            visit visit = (visit)Obj;
            dbEntities.Entry(visit).CurrentValues.SetValues(visit);
            dbEntities.SaveChanges();
            return visit;
        }

        public override object GetById(string Id)
        {
            int IdInt = 0;
            int.TryParse(Id, out IdInt);
            visit visit = (from s in dbEntities.visits where s.id.Equals(IdInt) select s).SingleOrDefault();
            return visit;
        }



        public override void Delete(object Obj)
        {
            visit visit = (visit)Obj;
            dbEntities.visits.Remove(visit);
            dbEntities.SaveChanges();
        }

        public override int ObjectCount()
        {
            return count;// dbEntities.visits.Count();
        }

        public override object Add(object Obj)
        {
            using (LibraryEntities ctx = LibraryEntities.Instance())
            {
                visit visit = (visit)Obj;
                visit DBVisit = null;
                try
                {
                    DBVisit= ctx.visits.Add(visit);
                    int RES = ctx.SaveChanges();

                }
                
                catch (Exception e)
                {
                    DBVisit = null;
                }
                return DBVisit;
            }

        }

        private List<object> ListWithSql(string sql, int limit = 0, int offset = 0)
        {
            try
            {
                List<object> visitList = new List<object>();
                var visits = dbEntities.visits
                    .SqlQuery(sql
                    ).
                    Select(v => new
                    {
                        v
                    });
                if (limit > 0)
                {
                    visits = visits.Skip(offset * limit).Take(limit).ToList();
                }
                else
                {
                    visits = visits.ToList();
                }
                foreach (var v in visits)
                {
                    visit Visit = v.v;
                    visitList.Add(Visit);
                }

                return visitList;
            }
            catch (Exception e)
            {
                return new List<object>();
            }
        }

        public override List<object> SearchAdvanced(Dictionary<string, object> Params, int limit = 0, int offset = 0)
        {
            string id = Params.ContainsKey("id") ? (string)Params["id"] : "";
            string info = Params.ContainsKey("info") ? (string)Params["info"] : "";
            string date = Params.ContainsKey("date") ? (string)Params["date"] : "";
            string student_id = Params.ContainsKey("student_id") ? (string)Params["student_id"] : "";
            string orderby = Params.ContainsKey("orderby") ? (string)Params["orderby"] : "";
            string ordertype = Params.ContainsKey("ordertype") ? (string)Params["ordertype"] : "";

            string sql = "select * from visit where id like '%" + id + "%'" +
                " and student_id like '%" + student_id + "%' " +
                " and info like '%" + info + "'" +
                " and date like '%" + date + "'";
            if (!orderby.Equals(""))
            {
                sql += " ORDER BY " + orderby;
                if (!ordertype.Equals(""))
                {
                    sql += " " + ordertype;
                }
            }
            count = countSQL(sql, dbEntities.visits);
            return ListWithSql(sql, limit, offset);
        }

        public override int countSQL(string sql, object dbSet)
        {
            try
            {
                return ((DbSet<visit>)dbSet)
                    .SqlQuery(sql).Count();
            }
            catch (Exception e)
            {
                return 0;
            }
        }
    }
}