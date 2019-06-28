using OurLibrary.Models;
using OurLibrary.Util.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            visit visit = (visit)Obj;
            /*if (visit.id == null)
                visit.id = StringUtil.GenerateRandomNumber(7);*/
            visit newvisit = dbEntities.visits.Add(visit);
            try
            {
                dbEntities.SaveChanges();
                return newvisit;
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
            catch (System.Data.Entity.Infrastructure.DbUpdateException dbEx)
            {
                Exception raise = new Exception(dbEx.StackTrace);
                throw raise;
                //  return null;
            }
        }

        private List<object> ListWithSql(string sql, int limit = 0, int offset = 0)
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
            return ((DbSet<visit>)dbSet)
                .SqlQuery(sql).Count();
        }
    }
}