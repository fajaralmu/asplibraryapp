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
    public class PublisherService : BaseService
    {


        public override List<object> ObjectList(int offset, int limit)
        {
            List<object> ObjList = new List<object>();
            var Sql = (from p in dbEntities.publishers orderby p.name select p);
            List<publisher> List = Sql.Skip(offset * limit).Take(limit).ToList();
            foreach (publisher c in List)
            {
                ObjList.Add(c);
            }
            return ObjList;
        }
        public override object Update(object Obj)
        {
            publisher publisher = (publisher)Obj;
            dbEntities.Entry(publisher).CurrentValues.SetValues(publisher);
            dbEntities.SaveChanges();
            return publisher;
        }

        public override object GetById(string Id)
        {
            publisher publisher = (from c in dbEntities.publishers where c.id.Equals(Id) select c).SingleOrDefault();
            return publisher;
        }

        public override void Delete(object Obj)
        {
            publisher publisher = (publisher)Obj;
            dbEntities.publishers.Remove(publisher);
            dbEntities.SaveChanges();
        }

        public override int ObjectCount()
        {
            return dbEntities.publishers.Count();
        }

        public override object Add(object Obj)
        {
            publisher publisher = (publisher)Obj;
            if (publisher.id == null)
                publisher.id = StringUtil.GenerateRandomChar(7);
            publisher newpublisher = dbEntities.publishers.Add(publisher);
            try
            {
                dbEntities.SaveChanges();
                return newpublisher;
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
            List<object> publisherList = new List<object>();
            var publishers = dbEntities.publishers
                .SqlQuery(sql
                ).
                Select(p => new
                {
                    p
                });
            if (limit > 0)
            {
                publishers = publishers.Skip(offset * limit).Take(limit).ToList();
            }
            else
            {
                publishers = publishers.ToList();
            }
            foreach (var p in publishers)
            {
                publisher Pub = p.p;
                publisherList.Add(Pub);
            }

            return publisherList;
        }

        public override List<object> SearchAdvanced(Dictionary<string, object> Params, int limit = 0, int offset = 0)
        {
            string id = Params.ContainsKey("id") ? (string)Params["id"] : "";
            string name = Params.ContainsKey("name") ? (string)Params["name"] : "";
            string address = Params.ContainsKey("address") ? (string)Params["address"] : "";
            string contact = Params.ContainsKey("contact") ? (string)Params["contact"] : "";

            string orderby = Params.ContainsKey("orderby") ? (string)Params["orderby"] : "";
            string ordertype = Params.ContainsKey("ordertype") ? (string)Params["ordertype"] : "";

            string sql = "select * from publisher where id like '%" + id + "%'" +
                " and name like '%" + name + "%'" +
            " and address like '%" + address + "%'" +
            " and contact like '%" + contact + "%'";
            if (!orderby.Equals(""))
            {
                sql += " ORDER BY " + orderby;
                if (!ordertype.Equals(""))
                {
                    sql += " " + ordertype;
                }
            }
            count = countSQL(sql, dbEntities.publishers);
            return ListWithSql(sql, limit, offset);
        }

        public override int countSQL(string sql, object dbSet)
        {
            DbSqlQuery<publisher> DbPub = ((DbSet<publisher>)dbSet)
                .SqlQuery(sql);

            return DbPub.Count();
        }

    }
}