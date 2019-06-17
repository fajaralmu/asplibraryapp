using OurLibrary.Models;
using OurLibrary.Util.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OurLibrary.Service
{
    public class PublisherServiceOLD:BaseService
    {
       

        public List<publisher> PublisherList(int offset, int limit)
        {
            var Sql=(from p in dbEntities.publishers orderby p.name select p);
            List<publisher> List = Sql.Skip(offset*limit).Take(limit).ToList();
            return List;
        }

        public publisher UpdatePublisher(publisher Publisher)
        {
          //  publisher SavedPublisher =dbEntities.publishers.Add(Publisher);
          //  dbEntities.Entry(Publisher).State = System.Data.Entity.EntityState.Modified;
            dbEntities.Entry(Publisher).CurrentValues.SetValues(Publisher);
            dbEntities.SaveChanges();
            return Publisher;
        }

        public publisher GetPublisherById(string Id)
        {
            publisher Publisher = (from p in dbEntities.publishers where p.id.Equals(Id) select p).SingleOrDefault();
            return Publisher;
        }

        public void DeletePublisher(publisher Publisher)
        {
            dbEntities.publishers.Remove(Publisher);
            dbEntities.SaveChanges();
        }

        public int PublisherCount()
        {
           return dbEntities.publishers.Count();
        }

        public publisher AddPublisher(publisher Publisher)
        {
            Publisher.id = StringUtil.GenerateRandomChar(7);
            publisher newPublisher = dbEntities.publishers.Add(Publisher);
            try
            {
                dbEntities.SaveChanges();
                return newPublisher;
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