using Newtonsoft.Json;
using OurLibrary.Models;
using OurLibrary.Service;
using OurLibrary.Util.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OurLibrary.Web.API
{
    public partial class Info : System.Web.UI.Page
    {
        private book_issueService BookIssueService = new book_issueService();
        private StudentService studentService = new StudentService();
        private VisitService VisitService = new VisitService();
        private BookService bookService = new BookService();
        private IssueService issueService = new IssueService();
        private Book_recordService bookRecordService = new Book_recordService();
        private int BookCount = 0;
        private int StudentCount = 0;
        private int IssueCount = 0;
        private int VisitCount = 0;
        private int BookIssueCount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.RequestType == "POST")
            {
                string Out = "\"" + GetTime() + "\"";
                int Result = 1;
                switch (Request.Form["Action"])
                {
                    case "studentVisit":
                        Out = GetStudentById();
                        if (Out != null && Out != "0")
                        {
                            string Visit = StudentVisit(Request.Form["Id"].ToString());
                            if (Visit != null && Visit != "0")
                            {
                                Out = "{" +
                                    "\"student\":" + Out + ", \"visit\":" + Visit
                                   + "}";
                                Result = 0;
                            }
                        }
                        break;
                    case "studentById":
                        Out = GetStudentById();
                        if (Out != null && Out != "0")
                        {
                            Out = "{\"student\":" + Out + "}";
                            Result = 0;
                        }
                        break;
                    case "bookList":
                        Out = GetBookList();
                        if (Out != null && Out != "0")
                        {
                            Out += (
                                ",\"offset\":" + Request.Form["offset"]
                                 + ",\"count\":" + BookCount
                                + ",\"limit\":" + Request.Form["limit"]
                                );
                            Result = 0;
                        }
                        break;
                    case "studentList":
                        Out = GetStudentList();
                        if (Out != null && Out != "0")
                        {
                            Out += (
                                ",\"offset\":" + Request.Form["offset"]
                                 + ",\"count\":" + StudentCount
                                + ",\"limit\":" + Request.Form["limit"]
                                );
                            Result = 0;
                        }
                        break;
                    case "issuesList":
                        Out = GetIssuesList();
                        if (Out != null && Out != "0")
                        {
                            Out += (
                                ",\"offset\":" + Request.Form["offset"]
                                 + ",\"count\":" + IssueCount
                                + ",\"limit\":" + Request.Form["limit"]
                                );
                            Result = 0;
                        }
                        break;
                    case "visitList":
                        Out = GetVisitsList();
                        if (Out != null && Out != "0")
                        {
                            Out += (
                                ",\"offset\":" + Request.Form["offset"]
                                 + ",\"count\":" + VisitCount
                                + ",\"limit\":" + Request.Form["limit"]
                                );
                            Result = 0;
                        }
                        break;
                    case "bookIssueList":
                        Out = GetBookIssueList();
                        if (Out != null && Out != "0")
                        {
                            Out += (
                                ",\"offset\":" + Request.Form["offset"]
                                 + ",\"count\":" + VisitCount
                                + ",\"limit\":" + Request.Form["limit"]
                                );
                            Result = 0;
                        }
                        break;
                    case "checkReturnedBook":
                        book_issue ReturnBookIssue = GetBookIssue();
                        if (ReturnBookIssue != null)
                        {
                            Out = (
                                "{\"book_issue_id\":\"" + ReturnBookIssue.id + "\"}"
                                );
                            Result = 0;
                        }else
                        {
                            Result = 1;
                        }
                        break;


                }

                Response.Clear();
                Response.ContentType = "application/json; charset=utf-8";
                Response.Write("{\"result\":" + Result + ",\"data\":" + Out + "}");
                Response.End();
            }
            else
            {
                return;
            }
        }

        private string GetBookIssueList()
        {
            List<object> book_issuesObj = GetObjectList(BookIssueService);
            BookIssueCount = BookIssueService.ObjectCount();
            if (book_issuesObj != null)
            {
                List<book_issue> book_issues = (List<book_issue>)ObjectUtil.ConvertList(book_issuesObj, typeof(List<book_issue>));
                List<book_issue> book_issuesToSerialize = new List<book_issue>();
                if (book_issues.Count > 0)
                {
                    foreach (book_issue BookIssue in book_issues)
                    {
                        book_issue book_issue = (book_issue)ObjectUtil.GetObjectValues(new string[]{
                            "id","book_record_id","book_issue_id","issue_id","qty","book_return"
                        }, BookIssue);

                        book_issuesToSerialize.Add(book_issue);
                    }

                    return JsonConvert.SerializeObject(book_issuesToSerialize);
                }
                return "0";
            }
            return "0";
        }

        private string GetVisitsList()
        {
            List<object> VisitsObj = GetObjectList(VisitService);
            VisitCount = VisitService.ObjectCount();
            if (VisitsObj != null)
            {
                List<visit> Visits = (List<visit>)ObjectUtil.ConvertList(VisitsObj, typeof(List<visit>));
                List<visit> VisitsToSerialize = new List<visit>();
                if (Visits.Count > 0)
                {
                    foreach (visit Vst in Visits)
                    {
                      

                        visit Visit = (visit)ObjectUtil.GetObjectValues(new string[]{
                            "id","student_id","date","info"
                        }, Vst);
                       
                        VisitsToSerialize.Add(Visit);
                    }

                    return JsonConvert.SerializeObject(VisitsToSerialize);
                }
                return "0";
            }
            return "0";
        }

        private book_issue GetBookIssue()
        {
            List<object> BookIssues = GetObjectList(BookIssueService);
           if (BookIssues != null && BookIssues.Count > 0)
            {
                List<book_issue> BookIssuesList = (List<book_issue>)ObjectUtil.ConvertList(BookIssues, typeof(List<book_issue>));
                return BookIssuesList.ElementAt(0);
            }
            return null;
        }

        private string GetStudentList()
        {
            List<object> StudentObj = GetObjectList(studentService);
            StudentCount = studentService.ObjectCount();
            if (StudentObj != null)
            {
                List<student> Students = (List<student>)ObjectUtil.ConvertList(StudentObj, typeof(List<student>));
                List<student> StudentsToSerialize = new List<student>();
                if (Students.Count > 0)
                {
                    foreach (student Std in Students)
                    {
                        @class StdClass = new @class()
                        {
                            id = Std.class_id,
                            class_name = Std.@class.class_name
                        };

                        student StdNew = (student)ObjectUtil.GetObjectValues(new string[]{
                            "id","name","bod","class_id","address","email"
                        }, Std);
                        StdNew.@class = StdClass;
                        StudentsToSerialize.Add(StdNew);
                    }

                    return JsonConvert.SerializeObject(StudentsToSerialize);
                }
                return "0";
            }
            return "0";
        }

        private List<object> GetObjectList(BaseService Service)
        {
            int Offset = 0;
            int Limit = 0;
            Dictionary<string, object> Params = new Dictionary<string, object>();
            if (StringUtil.NotNullAndNotBlank(Request.Form["limit"]) && StringUtil.NotNullAndNotBlank(Request.Form["offset"]))
            {
                try
                {
                    Offset = int.Parse(Request.Form["offset"].ToString());
                    Limit = int.Parse(Request.Form["limit"].ToString());

                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            if (StringUtil.NotNullAndNotBlank(Request.Form["search_param"]))
            {
                string Param = Request.Form["search_param"].ToString();
                Param = Param.Replace("${", "");
                Param = Param.Replace("}$", "");
                Param = Param.Replace(";", "&");
                Params = StringUtil.QUeryStringToDict(Param);
            }
            return Service.SearchAdvanced(Params, Limit, Offset);

        }

        private string GetBookList()
        {

            List<object> BooksObj = GetObjectList(bookService);
            BookCount = bookService.ObjectCount();
            if (BooksObj != null)
            {
                List<book> Books = (List<book>)ObjectUtil.ConvertList(BooksObj, typeof(List<book>));
                List<book> BooksToSerialize = new List<book>();
                if (Books.Count > 0)
                {
                    foreach (book B in Books)
                    {
                        book Book = (book)ObjectUtil.GetObjectValues(
                            new string[] { "id", "title", "page", "isbn", "img", "publisher_id", "author_id", "category_id", "review" }, B
                            );
                        category newCat = (category)ObjectUtil.GetObjectValues(new string[]
                        {
                            "id","category_name"
                        }, B.category);
                        B.publisher.books = null;
                        B.author.books = null;
                        B.category.books = null;
                        ICollection<book_record> BRs = new List<book_record>();
                        foreach (book_record BR in B.book_record)
                        {
                            book_record BRNew = (book_record)ObjectUtil.GetObjectValues(
                                new string[] { "id", "book_code", "available", "book_id" }, BR);
                            BRs.Add(BRNew);
                        }
                        Book.category = newCat;
                        Book.author = B.author;
                        Book.publisher = B.publisher;
                        Book.book_record = BRs;
                        BooksToSerialize.Add(Book);


                    }

                    return JsonConvert.SerializeObject(BooksToSerialize);
                }
                return "0";
            }
            return "0";
        }

        private string GetIssuesList()
        {
            List<object> Issues = GetObjectList(issueService);
            IssueCount = issueService.ObjectCount();
            if (Issues != null && Issues.Count > 0)
            {
                List<issue> IssuesToSerialize = new List<issue>();
                foreach (issue Issue in Issues)
                {
                    List<book_issue> BookIssues = new List<book_issue>();
                    foreach (book_issue BookIssue in Issue.book_issue)
                    {

                        book Book = (book)ObjectUtil.GetObjectValues(
                           new string[] { "id", "title", "page", "isbn", "img", "publisher_id", "author_id", "category_id", "review" }, BookIssue.book_record.book
                           );
                        book_record BRNew = (book_record)ObjectUtil.GetObjectValues(
                                new string[] { "id", "book_code", "available", "book_id" }, BookIssue.book_record);

                        book_issue BS = (book_issue)ObjectUtil.GetObjectValues(new string[]
                        {
                            "id","book_record_id","qty","book_return","book_issue_id",
                        }, BookIssue);
                        BRNew.book = Book;
                        BS.book_record = BRNew;
                        BookIssues.Add(BS);
                    }

                    issue IssueSerialize = (issue)ObjectUtil.GetObjectValues(new string[] {
                        "id","date","type","additional_info","student_id"
                    }, Issue);
                    IssueSerialize.book_issue = BookIssues;
                    IssuesToSerialize.Add(IssueSerialize);
                }
                return JsonConvert.SerializeObject(IssuesToSerialize);
            }
            else
            {
                return "0";
            }


        }


        private string StudentVisit(string StdId)
        {
            visit Visit = new visit()
            {
                student_id = StdId,
                date = DateTime.Now,
                info = "visit"
            };
            visit VisitDB = (visit)VisitService.Add(Visit);
            if (VisitDB != null)
            {
                return JsonConvert.SerializeObject(VisitDB);
            }
            return "0";
        }

        private string GetStudentById()
        {
            if (StringUtil.NotNullAndNotBlank(Request.Form["Id"]))
            {
                student Std = studentService.GetByIdFull(Request.Form["Id"].ToString());
                if (Std == null)
                {
                    return "0";
                }
                student toSend = (student)ObjectUtil.GetObjectValues(new string[]{
                            "id","name","bod","class_id","address","email","class"
                        }, Std);
                return JsonConvert.SerializeObject(toSend);
            }
            return "0";
        }

        private string GetTime()
        {
            return DateTime.Now.ToString();
        }
    }
}