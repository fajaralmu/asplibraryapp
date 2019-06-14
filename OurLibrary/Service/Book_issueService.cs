using OurLibrary.Models;
using OurLibrary.Util.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OurLibrary.Service
{
    public class book_issueService :BaseService
    {
        public book_issueService()
        {

        }
        public override List<object> ObjectList(int offset, int limit)
        {
            List<object> ObjList = new List<object>();
            var Sql = (from p in dbEntities.book_issue select p);
            List<book_issue> List = Sql.Skip(offset * limit).Take(limit).ToList();
            foreach (book_issue c in List)
            {
                ObjList.Add(c);
            }
            return ObjList;
        }
        public override object Update(object Obj)
        {
            book_issue Book_issue = (book_issue)Obj;
            dbEntities.Entry(Book_issue).CurrentValues.SetValues(Book_issue);
            dbEntities.SaveChanges();
            return Book_issue;
        }

        public override object GetById(string Id)
        {
            book_issue Book_issue = (from c in dbEntities.book_issue where c.id.Equals(Id) select c).SingleOrDefault();
            return Book_issue;
        }

        public override void Delete(object Obj)
        {
            book_issue Book_issue = (book_issue)Obj;
            dbEntities.book_issue.Remove(Book_issue);
            dbEntities.SaveChanges();
        }

        public override int ObjectCount()
        {
            return dbEntities.book_issue.Count();
        }

        public override object Add(object Obj)
        {
            book_issue Book_issue = (book_issue)Obj;
            if (Book_issue.id == null)
                Book_issue.id = StringUtil.GenerateRandom(10);
            if(Book_issue.qty == null || Book_issue.qty == 0)
            {
                Book_issue.qty = 1;

            }
            Book_issue.ref_issue = "not used";
            try
            {
                Book_issue.book_record = null;
                book_issue newclass = dbEntities.book_issue.Add(Book_issue);
               dbEntities.SaveChanges();
                return newclass;
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
            }catch(System.Data.Entity.Infrastructure.DbUpdateException dbEx)
            {

                Exception raise = new Exception(dbEx.StackTrace);
                
                throw raise;
                //  return null;
            }

        }
    }
}