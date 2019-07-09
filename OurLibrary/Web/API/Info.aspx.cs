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
        private StudentService studentService = new StudentService();
        private VisitService VisitService = new VisitService();
        private BookService bookService = new BookService();
        private Book_recordService bookRecordService = new Book_recordService();
        private int BookCount = 0;
        private int StudentCount = 0;
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
            return Service.SearchAdvanced(new Dictionary<string, object>(), Limit, Offset);

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
                        B.book_record = BRs;
                        BooksToSerialize.Add(B);


                    }

                    return JsonConvert.SerializeObject(BooksToSerialize);
                }
                return "0";
            }
            return "0";
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