using Newtonsoft.Json;
using OurLibrary.Models;
using OurLibrary.Service;
using OurLibrary.Util.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OurLibrary.Web.API
{
    public partial class Info : System.Web.UI.Page
    {
        private UserService UserService = new UserService();
        private ClassService ClassService;
        private AuthorService AuthorService;
        private PublisherService PublisherService;
        private CategoryService CategoryService;
        private book_issueService BookIssueService;
        private StudentService studentService;
        private VisitService VisitService;
        private BookService bookService;
        private IssueService issueService;

        private user User;
        private Book_recordService bookRecordService;

        private int BookCount = 0;
        private int StudentCount = 0;
        private int IssueCount = 0;
        private int VisitCount = 0;
        private int BookIssueCount = 0;
        private int ClassCoount = 0;
        private int AuthorCount = 0;
        private int PublisherCount = 0;
        private int CategoryCount = 0;

        private bool UserValid = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.RequestType == "POST")
            {
                CheckUser();
                string Out = "\"" + GetTime() + "\"";
                int Result = 1;
                switch (Request.Form["Action"])
                {
                    case "studentVisit":
                        Out = true || UserValid ? GetStudentById() : "0";
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
                    case "addStudent":
                        Out = UserValid ? AddStudent() : "0";
                        if (Out != null && Out != "0")
                        {
                            Out = "{\"student\":" + Out + "}";
                            Result = 0;
                        }
                        break;
                    case "addBook":
                        Out = UserValid ? AddBook() : "0";
                        if (Out != null && Out != "0")
                        {
                            Out = "{\"book\":" + Out + "}";
                            Result = 0;
                        }
                        break;
                    case "studentById":
                        Out = UserValid ? GetStudentById() : "0";
                        if (Out != null && Out != "0")
                        {
                            Out = "{\"student\":" + Out + "}";
                            Result = 0;
                        }
                        break;

                    case "bookRecById":
                        Out = UserValid ? GetBookRecordById() : "0";
                        if (Out != null && Out != "0")
                        {
                            Out = "{\"book_record\":" + Out + "}";
                            Result = 0;
                        }
                        break;
                    case "bookList":
                        Out = UserValid ? GetBookList() : "0";
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
                        Out = UserValid ? GetStudentList() : "0";
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
                        Out = UserValid ? GetIssuesList() : "0";
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
                        Out = UserValid ? GetVisitsList() : "0";
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
                    case "classList":
                        Out = UserValid ? GetClassList() : "0";
                        if (Out != null && Out != "0")
                        {
                            Out += (
                                ",\"offset\":" + Request.Form["offset"]
                                 + ",\"count\":" + ClassCoount
                                + ",\"limit\":" + Request.Form["limit"]
                                );
                            Result = 0;
                        }
                        break;
                    case "authorList":
                        Out = UserValid ? GetAuthorList() : "0";
                        if (Out != null && Out != "0")
                        {
                            Out += (
                                ",\"offset\":" + Request.Form["offset"]
                                 + ",\"count\":" + AuthorCount
                                + ",\"limit\":" + Request.Form["limit"]
                                );
                            Result = 0;
                        }
                        break;
                    case "publisherList":
                        Out = UserValid ? GetPublisherList() : "0";
                        if (Out != null && Out != "0")
                        {
                            Out += (
                                ",\"offset\":" + Request.Form["offset"]
                                 + ",\"count\":" + PublisherCount
                                + ",\"limit\":" + Request.Form["limit"]
                                );
                            Result = 0;
                        }
                        break;
                    case "categoryList":
                        Out = UserValid ? GetCategoryList() : "0";
                        if (Out != null && Out != "0")
                        {
                            Out += (
                                ",\"offset\":" + Request.Form["offset"]
                                 + ",\"count\":" + CategoryCount
                                + ",\"limit\":" + Request.Form["limit"]
                                );
                            Result = 0;
                        }
                        break;
                    case "bookIssueList":
                        Out = UserValid ? GetBookIssueList() : "0";
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
                        book_issue ReturnBookIssue = UserValid ? GetBookIssue() : null;
                        if (ReturnBookIssue != null)
                        {
                            Out = (
                                "{\"book_issue_id\":\"" + ReturnBookIssue.id + "\"}"
                                );
                            Result = 0;
                        }
                        else
                        {
                            Result = 1;
                        }
                        break;
                    case "getBookIssueByStudentId":
                        string StudentBookIssue = UserValid ? GetBookStudentIssue() : null;
                        if (StudentBookIssue != null && StudentBookIssue != "0")
                        {
                            Out = (
                                "{\"book_issue\":" + StudentBookIssue + "}"
                                );
                            Result = 0;
                        }
                        else
                        {
                            Result = 1;
                        }
                        break;
                    case "issueBook":
                        issue Issue = UserValid ? IssueBook() : null;
                        if (Issue != null)
                        {
                            string RecIds = ObjectUtil.ListToDelimitedString(Issue.book_issue, ";", "~", "book_record_id", "id");
                            Out = (
                                "{\"issue_id\":\"" + Issue.id + "\",\"date\":\"" + Issue.date + "\", \"items\":\"" + RecIds + "\"}"
                                );
                            Result = 0;
                        }
                        else
                        {
                            Result = 1;
                        }
                        break;
                    case "returnBook":
                        issue IssueReturn = UserValid ? ReturnBook() : null;
                        if (IssueReturn != null)
                        {
                            string RecIds = ObjectUtil.ListToDelimitedString(IssueReturn.book_issue, ";", "~", "book_record_id", "id");
                            Out = (
                                "{\"issue_id\":\"" + IssueReturn.id + "\",\"date\":\"" + IssueReturn.date + "\", \"items\":\"" + RecIds + "\"}"
                                );
                            Result = 0;
                        }
                        else
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

        private string AddStudent()
        {
            student Student = (student)ObjectUtil.FillObjectWithMap(new student(), BaseService.ReqToDict(Request));
            if (Student != null)
            {
                studentService = new StudentService();
                student Std = null;
                if (StringUtil.NotNullAndNotBlank(Student.id))
                {
                    Std = (student)studentService.Update(Student);
                }
                else
                {
                    Std = (student)studentService.Add(Student);
                }
                if (Std == null)
                {
                    return "0";
                }
                student toSend = (student)ObjectUtil.GetObjectValues(new string[]{
                            "id","name","bod","class_id","address","email"
                        }, Std);
                return JsonConvert.SerializeObject(toSend);
            }
            return "0";
        }

        private string AddBook()
        {
            book Book = (book)ObjectUtil.FillObjectWithMap(new book(), BaseService.ReqToDict(Request));
            if (Book != null)
            {
                bookService = new BookService();
                book BookDB = null;
                if(Book.id != null && Book.id != ""){
                  BookDB = (book)bookService.Update(Book);
                   
                }
                else{
                    BookDB=(book)bookService.Add(Book);
                }
                if (BookDB == null)
                {
                    return "0";
                }
                book toSend = (book)ObjectUtil.GetObjectValues(new string[]{
                            "id","title","publisher_id","author_id","category_id","page","review"
                        }, BookDB);
                return JsonConvert.SerializeObject(toSend);
            }
            return "0";
        }


        private issue ReturnBook()
        {
            if (StringUtil.NotNullAndNotBlank(Request.Form["student_id"]) &&
                 StringUtil.NotNullAndNotBlank(Request.Form["book_recs"]))
            {
                issueService = new IssueService();
                studentService = new StudentService();
                bookRecordService = new Book_recordService();
                BookIssueService = new book_issueService();

                string StudentId = Request.Form["student_id"];
                student Student = (student)studentService.GetById(StudentId);
                if (Student == null)
                {
                    return null;
                }

                string IssueID = StringUtil.GenerateRandomNumber(9);
                issue Issue = new issue();
                Issue.user_id = User.id;
                Issue.id = IssueID;
                Issue.type = "return";
                Issue.date = DateTime.Now;
                Issue.student_id = Student.id;
                Issue.addtional_info = "test";

                string[] BookRecIds = Request.Form["book_recs"].Split(';');

                if (BookRecIds.Length < 1)
                {
                    return null;
                }

                if (null == issueService.Add(Issue))
                {
                    return null;
                }

                List<book_issue> BookIssues = new List<book_issue>();
                foreach (string Id in BookRecIds)
                {
                    string[] IdAndReffId = Id.Split('-');
                    if (IdAndReffId.Length < 1)
                        continue;
                    book_record BR = (book_record)bookRecordService.GetById(IdAndReffId[0]);
                    book_issue ReffBookIssue = (book_issue)BookIssueService.GetById(IdAndReffId[1]);
                    if (ReffBookIssue.book_return == 1)
                    {
                        continue;
                    }
                    book_issue BookIssue = new book_issue();
                    BookIssue.id = StringUtil.GenerateRandomChar(10);
                    BookIssue.issue_id = IssueID;
                    BookIssue.book_issue_id = IdAndReffId[1];
                    BookIssue.book_record_id = IdAndReffId[0];
                    BookIssue.qty = 1;
                    BookIssues.Add(BookIssue);
                    BookIssueService.Add(BookIssue);

                    BR.available = 1;
                    ReffBookIssue.book_return = 1;
                    BookIssueService.Update(ReffBookIssue);
                    bookRecordService.Update(BR);

                }
                Issue.book_issue = BookIssues;

                return Issue;
            }
            return null;
        }

        private string GetBookStudentIssue()
        {
            if (StringUtil.NotNullAndNotBlank(Request.Form["rec_id"])
                && StringUtil.NotNullAndNotBlank(Request.Form["student_id"]))
            {
                BookIssueService = new book_issueService();
                bookRecordService = new Book_recordService();
                studentService = new StudentService();

                student Std = studentService.GetByIdFull(Request.Form["student_id"].ToString());
                if (Std == null)
                {
                    return "0";
                }
                List<object> BookIssuesOBJ = BookIssueService.SearchAdvanced(new Dictionary<string, object>()
                {
                        {"student_id",Request.Form["student_id"].ToString() },
                        {"book_record_id",Request.Form["rec_id"].ToString() },
                        {"book_return","0" },
                        {"issue_type","issue" },
                });
                if (BookIssuesOBJ == null || BookIssuesOBJ.Count == 0)
                {
                    return "0";
                }
                book_issue StudentBookIssue = (book_issue)BookIssuesOBJ.ElementAt(0);
                if (StudentBookIssue == null)
                {
                    return "0";
                }
                book_record BookRecord = (book_record)bookRecordService.GetById(Request.Form["rec_id"]);
                if (BookRecord == null)
                {
                    return "0";
                }


                book_record BookRecordToSend = (book_record)ObjectUtil.GetObjectValues(new string[]{
                            "id","book_id","available"
                        }, BookRecord);
                if (BookRecord.book != null)
                {
                    BookRecordToSend.book = (book)ObjectUtil.GetObjectValues(new string[]{
                            "id","title" }, BookRecord.book);
                }
                book_issue BookIssueToSend = (book_issue)ObjectUtil.GetObjectValues(new string[]
                {
                    "id","book_record_id","book_return"
                }, StudentBookIssue);

                BookIssueToSend.book_record = BookRecordToSend;

                return JsonConvert.SerializeObject(BookIssueToSend);
            }
            return "0";
        }

        private string GetBookRecordById()
        {
            if (StringUtil.NotNullAndNotBlank(Request.Form["Id"]))
            {
                bookRecordService = new Book_recordService();
                book_record BookRecord = (book_record)bookRecordService.GetById(Request.Form["Id"]);
                if (BookRecord == null)
                {
                    return "0";
                }
                book_record toSend = (book_record)ObjectUtil.GetObjectValues(new string[]{
                            "id","book_id","available"
                        }, BookRecord);
                if (BookRecord.book != null)
                {
                    toSend.book = (book)ObjectUtil.GetObjectValues(new string[]{
                            "id","title" }, BookRecord.book);
                }

                return JsonConvert.SerializeObject(toSend);
            }
            return "0";
        }

        private issue IssueBook()
        {
            if (StringUtil.NotNullAndNotBlank(Request.Form["student_id"]) &&
                 StringUtil.NotNullAndNotBlank(Request.Form["book_recs"]))
            {
                issueService = new IssueService();
                studentService = new StudentService();
                bookRecordService = new Book_recordService();
                BookIssueService = new book_issueService();

                string StudentId = Request.Form["student_id"];
                student Student = (student)studentService.GetById(StudentId);
                if (Student == null)
                {
                    return null;
                }

                string IssueID = StringUtil.GenerateRandomNumber(9);
                issue Issue = new issue();
                Issue.user_id = User.id;
                Issue.id = IssueID;
                Issue.type = "issue";
                Issue.date = DateTime.Now;
                Issue.student_id = Student.id;
                Issue.addtional_info = "test";

                string[] BookRecIds = Request.Form["book_recs"].Split(';');

                if (BookRecIds.Length < 1)
                {
                    return null;
                }

                if (null == issueService.Add(Issue))
                {
                    return null;
                }

                List<book_issue> BookIssues = new List<book_issue>();
                foreach (string Id in BookRecIds)
                {
                    book_record BR = (book_record)bookRecordService.GetById(Id);
                    if (BR == null || BR.available == 0)
                    {
                        continue;
                    }
                    book_issue BookIssue = new book_issue();
                    BookIssue.id = StringUtil.GenerateRandomChar(10);
                    BookIssue.issue_id = IssueID;
                    BookIssue.book_record_id = Id;
                    BookIssue.qty = 1;
                    BookIssues.Add(BookIssue);
                    BookIssueService.Add(BookIssue);

                    BR.available = 0;
                    bookRecordService.Update(BR);

                }
                Issue.book_issue = BookIssues;

                return Issue;
            }
            return null;
        }

        private void CheckUser()
        {
            string Username = Request.Form["u"];
            string Password = Request.Form["p"];

            user LoggedUser = UserService.GetUser(Username, Password);
            if (LoggedUser != null)
            {
                User = LoggedUser;
                UserValid = true;
            }
        }

        private string GetBookIssueList()
        {
            BookIssueService = new book_issueService();
            List<object> book_issuesObj = BaseService.GetObjectList(BookIssueService, Request);
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
            VisitService = new VisitService();
            List<object> VisitsObj = BaseService.GetObjectList(VisitService, Request);
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
            BookIssueService = new book_issueService();
            List<object> BookIssues = BaseService.GetObjectList(BookIssueService, Request);
            if (BookIssues != null && BookIssues.Count > 0)
            {
                List<book_issue> BookIssuesList = (List<book_issue>)ObjectUtil.ConvertList(BookIssues, typeof(List<book_issue>));
                return BookIssuesList.ElementAt(0);
            }
            return null;
        }

        private string GetStudentList()
        {
            studentService = new StudentService();
            List<object> StudentObj = BaseService.GetObjectList(studentService, Request);
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

        private string GetClassList()
        {
            ClassService = new ClassService();
            List<object> ClassObj = BaseService.GetObjectList(ClassService, Request);
            ClassCoount = ClassService.ObjectCount();
            if (ClassObj != null)
            {
                List<@class> Classes = (List<@class>)ObjectUtil.ConvertList(ClassObj, typeof(List<@class>));
                List<@class> ClassesToSerialize = new List<@class>();
                if (Classes.Count > 0)
                {
                    foreach (@class CLass in Classes)
                    {


                        @class NewClass = (@class)ObjectUtil.GetObjectValues(new string[]{
                            "id","class_name"
                        }, CLass);
                        ClassesToSerialize.Add(NewClass);
                    }

                    return JsonConvert.SerializeObject(ClassesToSerialize);
                }
                return "0";
            }
            return "0";
        }

        private string GetAuthorList()
        {
            AuthorService = new AuthorService();
            List<object> AuthorObj = BaseService.GetObjectList(AuthorService, Request);
            AuthorCount = AuthorService.ObjectCount();
            if (AuthorObj != null)
            {
                List<author> Authors = (List<author>)ObjectUtil.ConvertList(AuthorObj, typeof(List<author>));
                List<author> AuthorsToSerialize = new List<author>();
                if (Authors.Count > 0)
                {
                    foreach (author Auhtor in Authors)
                    {


                        author NewClass = (author)ObjectUtil.GetObjectValues(new string[]{
                            "id","name","address","email","phone"
                        }, Auhtor);
                        AuthorsToSerialize.Add(NewClass);
                    }

                    return JsonConvert.SerializeObject(AuthorsToSerialize);
                }
                return "0";
            }
            return "0";
        }

        private string GetPublisherList()
        {
            PublisherService = new PublisherService();
            List<object> PublisherObj = BaseService.GetObjectList(PublisherService, Request);
            PublisherCount = PublisherService.ObjectCount();
            if (PublisherObj != null)
            {
                List<publisher> Publishers = (List<publisher>)ObjectUtil.ConvertList(PublisherObj, typeof(List<publisher>));
                List<publisher> PublishersToSerialize = new List<publisher>();
                if (Publishers.Count > 0)
                {
                    foreach (publisher Publisher in Publishers)
                    {


                        publisher NewClass = (publisher)ObjectUtil.GetObjectValues(new string[]{
                            "id","name","address","contact"
                        }, Publisher);
                        PublishersToSerialize.Add(NewClass);
                    }

                    return JsonConvert.SerializeObject(PublishersToSerialize);
                }
                return "0";
            }
            return "0";
        }

        private string GetCategoryList()
        {
            CategoryService = new CategoryService();
            List<object> CategoryObj = BaseService.GetObjectList(CategoryService, Request);
            CategoryCount = CategoryService.ObjectCount();
            if (CategoryObj != null)
            {
                List<category> Categories = (List<category>)ObjectUtil.ConvertList(CategoryObj, typeof(List<category>));
                List<category> CategoriesToSerialize = new List<category>();
                if (Categories.Count > 0)
                {
                    foreach (category Category in Categories)
                    {


                        category NewClass = (category)ObjectUtil.GetObjectValues(new string[]{
                            "id","category_name"
                        }, Category);
                        CategoriesToSerialize.Add(NewClass);
                    }

                    return JsonConvert.SerializeObject(CategoriesToSerialize);
                }
                return "0";
            }
            return "0";
        }



        private string GetBookList()
        {
            bookService = new BookService();
            List<object> BooksObj = BaseService.GetObjectList(bookService, Request);
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
            issueService = new IssueService();
            List<object> Issues = BaseService.GetObjectList(issueService, Request);
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        private string StudentVisit(string StdId)
        {
            VisitService = new VisitService();
            visit Visit = new visit()
            {
                student_id = StdId,
                date = DateTime.Now,
                info = "visit"
            };
            visit VisitDB = (visit)VisitService.Add(Visit);

            if (VisitDB != null)
            {
                VisitDB.student = null;
                return JsonConvert.SerializeObject(VisitDB);
            }
            return "0";
        }

        private string GetStudentById()
        {
            if (StringUtil.NotNullAndNotBlank(Request.Form["Id"]))
            {
                studentService = new StudentService();
                student Std = studentService.GetByIdFull(Request.Form["Id"].ToString());
                if (Std == null)
                {
                    return "0";
                }
                student toSend = (student)ObjectUtil.GetObjectValues(new string[]{
                            "id","name","bod","class_id","address","email"
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