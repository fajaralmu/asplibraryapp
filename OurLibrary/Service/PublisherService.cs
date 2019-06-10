using OurLibrary.Models;
using OurLibrary.Util.Common;
using System;
using System.Collections.Generic;
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
                publisher.id = StringUtil.GenerateRandom(7);
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
    }
}