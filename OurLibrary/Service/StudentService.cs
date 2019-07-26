using OurLibrary.Models;
using OurLibrary.Util.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            count = dbEntities.students.Count();
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
            student student = (from s in dbEntities.students where s.id.Equals(Id) select s).SingleOrDefault();
            return student;
        }

        public student GetByIdFull(string Id)
        {
            dbEntities = LibraryEntities.Instance();

            try
            {
                student student = (from s in dbEntities.students where s.id.Equals(Id) select s).SingleOrDefault();
                if (student != null)
                    student.@class = (from c in dbEntities.classes where c.id.Equals(student.class_id) select c).SingleOrDefault();
                return student;
            }catch(InvalidOperationException ex)
            {
                return null;
            }catch (Exception ex)
            {
                return null;
            }
        }

        public override void Delete(object Obj)
        {
            student student = (student)Obj;
            dbEntities.students.Remove(student);
            dbEntities.SaveChanges();
        }

        public override int ObjectCount()
        {
            return count;// dbEntities.students.Count();
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

        private List<object> ListWithSql(string sql, int limit = 0, int offset = 0)
        {
            List<object> studentList = new List<object>();
            var students = dbEntities.students
                .SqlQuery(sql
                ).
                Select(student => new
                {
                    student,
                    @class = dbEntities.classes.Where(c => c.id.Equals(student.class_id)).Select(c => c).FirstOrDefault()
                });
            if (limit > 0)
            {
                students = students.Skip(offset * limit).Take(limit).ToList();
            }
            else
            {
                students = students.ToList();
            }
            /*  where b.author.name.Contains(val)
               || b.name.Contains(val) || b.review.Contains(val) || b.author.name.Contains(val)

            || b.category.category_name.Contains(val) || b.publisher.name.Contains(val)
               select b;*/
            foreach (var s in students)
            {
                student student = new student();
                student = s.student;
                student.@class = s.@class;
                //   Debug.WriteLine("name:"+student.name+", cat:"+student.category.category_name+", auth:"+student.author.name);
                studentList.Add(student);
            }

            return studentList;
        }

        public override List<object> SearchAdvanced(Dictionary<string, object> Params, int limit = 0, int offset = 0)
        {
            string id = Params.ContainsKey("id") ? (string)Params["id"] : "";
            string name = Params.ContainsKey("name") ? (string)Params["name"] : "";
            string email = Params.ContainsKey("email") ? (string)Params["email"] : "";
            string address = Params.ContainsKey("address") ? (string)Params["address"] : "";
            string @class = Params.ContainsKey("class") ? (string)Params["class"] : "";
            string orderby = Params.ContainsKey("orderby") ? (string)Params["orderby"] : "";
            string ordertype = Params.ContainsKey("ordertype") ? (string)Params["ordertype"] : "";

            string sql = "select * from student " +
               "left join class on class.id = student.class_id " +
                "where student.name like '%" + name + "%'" +
                 " and student.id like '%" + id + "%'" +
               " and student.email like '%" + email + "%'" +
               " and student.address like '%" + address + "%'" +
               " and class.class_name like  '%" + @class + "%'";
            if (!orderby.Equals(""))
            {
                sql += " ORDER BY " + orderby;
                if (!ordertype.Equals(""))
                {
                    sql += " " + ordertype;
                }
            }
            count = countSQL(sql, dbEntities.students);
            return ListWithSql(sql, limit, offset);
        }

        public override int countSQL(string sql, object dbSet)
        {
            try
            {
                return ((DbSet<student>)dbSet)
                    .SqlQuery(sql).Count();
            }catch(Exception e)
            {
                return 0;
            }
        }

    }
}