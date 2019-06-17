using OurLibrary.Models;
using OurLibrary.Util.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OurLibrary.Service
{
    public class StudentService : BaseService
    {
        public StudentService()
        {

        }

        public override List<object> ObjectList(int offset, int limit)
        {
            List<object> ObjList = new List<object>();
            var Sql = (from p in dbEntities.students.Include("class") orderby p.name select p);
            List<student> List = Sql.Skip(offset * limit).Take(limit).ToList();
            foreach (student c in List)
            {
                ObjList.Add(c);
            }
            return ObjList;
        }
        public override object Update(object Obj)
        {
            student student = (student)Obj;
            dbEntities.Entry(student).CurrentValues.SetValues(student);
            dbEntities.SaveChanges();
            return student;
        }

        public override object GetById(string Id)
        {
            student student = (from c in dbEntities.students where c.id.Equals(Id) select c).SingleOrDefault();
            return student;
        }

        public override void Delete(object Obj)
        {
            student student = (student)Obj;
            dbEntities.students.Remove(student);
            dbEntities.SaveChanges();
        }

        public override int ObjectCount()
        {
            return dbEntities.students.Count();
        }

        public override object Add(object Obj)
        {
            student student = (student)Obj;
            if (student.id == null)
                student.id = StringUtil.GenerateRandomNumber(7);
            student newstudent = dbEntities.students.Add(student);
            try
            {
                dbEntities.SaveChanges();
                return newstudent;
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
    }
}