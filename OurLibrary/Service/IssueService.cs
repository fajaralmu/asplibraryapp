using OurLibrary.Models;
using OurLibrary.Util.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OurLibrary.Service
{
    public class IssueService : BaseService
    {
        public IssueService()
        {

        }
        public override List<object> ObjectList(int offset, int limit)
        {
            List<object> ObjList = new List<object>();
            var Sql = (from issue in dbEntities.issues orderby issue.id select issue);
            List<issue> List = Sql.Skip(offset * limit).Take(limit).ToList();
            foreach (issue issue in List)
            {
                ObjList.Add(issue);
            }
            count = dbEntities.issues.Count();
            return ObjList;
        }
        public override object Update(object Obj)
        {
            issue Issue = (issue)Obj;
            dbEntities.Entry(Issue).CurrentValues.SetValues(Issue);
            dbEntities.SaveChanges();
            return Issue;
        }

        public override object GetById(string Id)
        {
            issue Issue = (from c in dbEntities.issues where c.id.Equals(Id) select c).SingleOrDefault();
            return Issue;
        }

        public override void Delete(object Obj)
        {
            issue Issue = (issue)Obj;
            dbEntities.issues.Remove(Issue);
            dbEntities.SaveChanges();
        }

        public override int ObjectCount()
        {
            return count;
        }

        public override object Add(object Obj)
        {
            issue Issue = (issue)Obj;
            if (Issue.id == null)
                Issue.id = StringUtil.GenerateRandomNumber(9);
            if (Issue.type == null)
            {
                Issue.type = "default";
            }
            try
            {
                issue newclass = dbEntities.issues.Add(Issue);
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
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException dbEx)
            {
                throw dbEx;
            }
        }

        public override List<object> SearchAdvanced(Dictionary<string, object> Params, int limit = 0, int offset = 0)
        {
            bool exactSearch = false;
            if(Params.ContainsKey("exact")  && Params["exact"].GetType().Equals(typeof(bool))){
                if(((bool) Params["exact"])){
                    exactSearch = true;
                }
            }
            string id = Params.ContainsKey("id") ? (string)Params["id"] : "";
            string date = Params.ContainsKey("date") ? (string)Params["date"] : "";
            string user_id = Params.ContainsKey("user_id") ? (string)Params["user_id"] : "";
            string additional_info = Params.ContainsKey("additional_info") ? (string)Params["additional_info"] : "";
            string student_id = Params.ContainsKey("student_id") ? (string)Params["student_id"] : "";
            string type = Params.ContainsKey("type") ? (string)Params["type"] : "";
            string orderby = Params.ContainsKey("orderby") ? (string)Params["orderby"] : "";
            string ordertype = Params.ContainsKey("ordertype") ? (string)Params["ordertype"] : "";

            string filter_student_sql = exactSearch ? " student_id = '" + student_id + "'" : " student_id like '%" + student_id + "%'";


            string sql = "select * from issue " +
                "where "+
                    filter_student_sql+
                " and id like '%" + id + "%'" +
                " and user_id like '%" + user_id + "%'" +
                " and date like '%" + date + "%'" +
                " and addtional_info like '%" + additional_info + "%'" +
                " and type like '%" + type + "%' ";

            if (!orderby.Equals(""))
            {
                sql += " ORDER BY " + orderby;
                if (!ordertype.Equals(""))
                {
                    sql += " " + ordertype;
                }
            }
            count = countSQL(sql, dbEntities.issues);
            return ListWithSql(sql, limit, offset);
        }

        private List<object> ListWithSql(string sql, int limit = 0, int offset = 0)
        {
            List<object> issueList = new List<object>();
            var issues = dbEntities.issues
                .SqlQuery(sql
                ).
                Select(issue => new
                {
                    issue,
                    book_issue = dbEntities.book_issue.Where(b => b.issue_id.Equals(issue.id)).Select(bi => new
                    {
                        bi,
                        book_record = dbEntities.book_record.Where(br => br.id.Equals(bi.book_record_id)).FirstOrDefault()
                    }).ToList(),
                });
            if (limit > 0)
            {
                issues = issues.Skip(offset * limit).Take(limit).ToList();
            }
            else
            {
                issues = issues.ToList();
            }
            /*  where b.author.name.Contains(val)
               || b.title.Contains(val) || b.review.Contains(val) || b.author.name.Contains(val)

            || b.category.category_name.Contains(val) || b.publisher.name.Contains(val)
               select b;*/
            foreach (var i in issues)
            {
                issue Issue = new issue();
                Issue = i.issue;
                Issue.book_issue = new List<book_issue>();
                foreach (var bi in i.book_issue)
                {
                    book_issue BookIssue = new book_issue();
                    BookIssue = bi.bi;
                    BookIssue.book_record = bi.book_record;
                    Issue.book_issue.Add(BookIssue);
                }
                //   Debug.WriteLine("title:"+Book.title+", cat:"+Book.category.category_name+", auth:"+Book.author.name);
                issueList.Add(Issue);
            }

            return issueList;
        }

        public override int countSQL(string sql, object dbSet)
        {
            return ((DbSet<issue>)dbSet)
                .SqlQuery(sql).Count();
        }

    }
}