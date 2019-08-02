using OurLibrary.Models;
using OurLibrary.Util.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OurLibrary.Service
{
    public class book_issueService : BaseService
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
            List<object> List = SearchAdvanced(new Dictionary<string, object>()
            {
                {"id",Id }
            });
            if(List!=null && List.Count > 0)
            {
                return List.ElementAt(0);
            }
            return null;
            //book_issue Book_issue = (from c in dbEntities.book_issue where c.id.Equals(Id) select c).SingleOrDefault();
            //return Book_issue;
        }

        public override void Delete(object Obj)
        {
            book_issue Book_issue = (book_issue)Obj;
            dbEntities.book_issue.Remove(Book_issue);
            dbEntities.SaveChanges();
        }

        private List<object> ListWithSql(string sql, int limit = 0, int offset = 0)
        {
            List<object> book_IssuesList = new List<object>();
            var book_issues = dbEntities.book_issue
                .SqlQuery(sql
                ).
                Select(book_issue => book_issue);
            if (limit > 0)
            {
                book_issues = book_issues.Skip(offset * limit).Take(limit).ToList();
            }
            else
            {
                book_issues = book_issues.ToList();
            }
            foreach (var bs in book_issues)
            {
                book_issue BookIssue = bs;
                book_IssuesList.Add(BookIssue);
            }
            /*  where b.author.name.Contains(val)
               || b.title.Contains(val) || b.review.Contains(val) || b.author.name.Contains(val)

            || b.category.category_name.Contains(val) || b.publisher.name.Contains(val)
               select b;*/

            return book_IssuesList;
        }

        public override List<object> SearchAdvanced(Dictionary<string, object> Params, int limit = 0, int offset = 0)
        {
            string id = Params.ContainsKey("id") ? (string)Params["id"] : "";
            string book_issue_id = Params.ContainsKey("book_issue_id") ? (string)Params["book_issue_id"] : "";
            string student_id = Params.ContainsKey("student_id") ? (string)Params["student_id"] : "";
            string book_return = Params.ContainsKey("book_return") ? (string)Params["book_return"] : "";
            string issue_type = Params.ContainsKey("issue_type") ? (string)Params["issue_type"] : "";
            string book_record_id = Params.ContainsKey("book_record_id") ? (string)Params["book_record_id"] : "";

            string orderby = Params.ContainsKey("orderby") ? (string)Params["orderby"] : "";
            string ordertype = Params.ContainsKey("ordertype") ? (string)Params["ordertype"] : "";

            string sql = "select * from book_issue " +
                    " left join issue on issue.id = book_issue.issue_id " +
                    " where book_issue.id like '%" + id + "%'" +
                    (book_issue_id != null && book_issue_id != ""?" and book_issue.book_issue_id like '%" + book_issue_id + "%' " : "")+
                    (book_record_id != null && book_record_id != "" ? " and book_issue.book_record_id ='" + book_record_id + "' " : "") +
                    " and issue.student_id like '%" + student_id + "%' " +
                    " and issue.type like '%" + issue_type + "%' " + (book_return != null && book_return != "" ? " and book_issue.book_return = " + book_return : " ");
            if (!orderby.Equals(""))
            {
                sql += " ORDER BY " + orderby;
                if (!ordertype.Equals(""))
                {
                    sql += " " + ordertype;
                }
            }
            count = countSQL(sql, dbEntities.book_record);
            return ListWithSql(sql, limit, offset);
        }

        public List<book_issue> GetByBookIssueIdReturned(string id)
        {
            Dictionary<string, object> Params = new Dictionary<string, object>();
            Params.Add("book_issue_id", id);
            List<object> ReturnedBookIssues = SearchAdvanced(Params);
            return (List<book_issue>)ObjectUtil.ConvertList(ReturnedBookIssues, typeof(List<book_issue>));
        }

        public override int ObjectCount()
        {
            return dbEntities.book_issue.Count();
        }

        public override object Add(object Obj)
        {
            book_issue Book_issue = (book_issue)Obj;
            if (Book_issue.id == null)
                Book_issue.id = StringUtil.GenerateRandomChar(10);
            if (Book_issue.qty == null || Book_issue.qty == 0)
            {
                Book_issue.qty = 1;

            }
            Book_issue.ref_issue = "not used";
            try
            {
                Book_issue.book_issue2 = null;
                Book_issue.issue = null;
                Book_issue.book_issue1 = null;
                Book_issue.book_record = null;
                if (Book_issue.book_return == null)
                {
                    Book_issue.book_return = 0;
                }
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
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException dbEx)
            {

                Exception raise = new Exception(dbEx.StackTrace);

                throw raise;
                //  return null;
            }
            catch (System.Exception dbEx)
            {

                Exception raise = new Exception(dbEx.StackTrace);

                throw raise;
                //  return null;
            }

        }

        internal book_issue FindByIdFull(string book_issue_id)
        {
            throw new NotImplementedException();
        }
    }
}