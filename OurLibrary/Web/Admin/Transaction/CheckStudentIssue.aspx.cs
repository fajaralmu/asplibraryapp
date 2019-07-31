using OurLibrary.Models;
using OurLibrary.Service;
using OurLibrary.Util.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OurLibrary.Web.Admin.Transaction
{
    public partial class CheckStudentIssue : BasePage
    {
        private book_issueService BookIssueService = new book_issueService();
        private Book_recordService BookRecordService = new Book_recordService();
        private Book_recordService bookRecordService = new Book_recordService();
        private StudentService StudentService = new StudentService();
        private IssueService IssueService = new IssueService();
        private List<issue> Issues = new List<issue>();
        private List<issue> IssuesReturn = new List<issue>();
        private List<book_issue> BookIssuesReturn = new List<book_issue>();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] == null || !userService.IsValid((user)Session["user"]))
            {
                Response.Redirect("~/Web/Admin/");
            }
            else
            {
                LoggedUser = (user)Session["user"];
                InitPage();
            }
        }

        private void InitPage()
        {
            if (ViewState["BookIssuesReturn"] != null)
            {
                BookIssuesReturn = (List<book_issue>)ViewState["BookIssuesReturn"];
                PopulateBooksToReturn();

            }
            AlertMessage(null);
        }

        private void PopulateIssueList()
        {
            int No = 0;
            PanelIssueLis.Controls.Clear();
            int BookStillIssued = 0;
            foreach (issue Issue in Issues)
            {
                if (null == Issue.book_issue || Issue.book_issue.Count == 0)
                {
                    continue;
                }
                No++;
                Panel PanelIssue = new Panel();
                PanelIssue.Controls.Add(ControlUtil.GenerateLabel("<b>" + No + ". Issue ID: " + Issue.id + " (" + Issue.type + ")</b><br/>date: " + Issue.date + "<ol>"));
                Panel PanelIssueItem = new Panel();
                foreach (book_issue Bs in Issue.book_issue)
                {
                    List<book_issue> BooksReturned = BookIssueService.GetByBookIssueIdReturned(Bs.id);
                    bool BookReturned = BooksReturned.Count == 1;
                    string labelHtml = "<li class=\"book_issue-item\" id=\"Book_Issue_Rec_" + Bs.id
                        + "\"> Issue Rec Id: " + Bs.id + "<br/>Book Rec Id: "
                        + Bs.book_record_id + "<br/>"
                        + Bs.book_record.book.title + " - returned: <code>" + BookReturned+"</code>";// + DateTime.Now.ToString();

                    if (!BookReturned)
                    {
                        string[] Attrs = new string[]
                        {
                            "href=\"#\"",
                            "onclick=SetTextInput('"+Bs.book_record_id+"','MainContent_TextBoxIssueRecordId')"
                        };
                        string anchor = ControlUtil.GenerateHtmlTag("a", Attrs, " return now ");
                        labelHtml +=  "<br/>"+anchor;
                        BookStillIssued++;
                    }
                    else
                    {
                        string[] Attrs = new string[] {
                            "class=\"pointerable\"",
                            "onclick=\"ScrollToElement('Book_Return_Rec_" + BooksReturned[0].id + "')\""
                        };
                        string anchor = ControlUtil.GenerateHtmlTag("span", Attrs, BooksReturned[0].id);
                        labelHtml += "<br/> Return ref: " + anchor;
                    }
                    labelHtml += "</li>";
                    PanelIssueItem.Controls.Add(ControlUtil.GenerateLabel(labelHtml));

                }
                PanelIssue.Controls.Add(PanelIssueItem);
                DateTime MaxReturn = DateUtil.PlusDay(Issue.date, int.Parse(TextBoxDuration.Text));
                bool Late = false;
                if (Issue.date.CompareTo(MaxReturn) > 0)
                {
                    Late = true;
                }
                PanelIssue.Controls.Add(ControlUtil.GenerateLabel("</ol>Max return: " + MaxReturn + ", Late:" + Late + "<hr/>"));
                PanelIssueLis.Controls.Add(PanelIssue);
            }

            issueCount.InnerHtml = BookStillIssued.ToString();
           
            //BOOK RETURNED
            No = 0;
            PanelIssueReturn.Controls.Clear();

            foreach (issue Issue in IssuesReturn)
            {
                if (null == Issue.book_issue || Issue.book_issue.Count == 0)
                {
                    continue;
                }
                No++;
                Panel PanelIssue = new Panel();
                PanelIssue.Controls.Add(ControlUtil.GenerateLabel("<b>" + No + ". Issue ID: " + Issue.id + " (" + Issue.type + ")</b><br/>date: " + Issue.date + "<ol>"));
                Panel PanelIssueItem = new Panel();
                
                foreach (book_issue Bs in Issue.book_issue)
                {
                   
                    Bs.book_issue2 = (book_issue)BookIssueService.GetById(Bs.book_issue_id);
                    if (null != Bs.book_issue2)
                    {


                        book_issue BookIssueRef = Bs.book_issue2;
                        issue IssueRef = (issue)IssueService.GetById(BookIssueRef.issue_id);
                        DateTime IssuedDate = IssueRef.date;
                        DateTime MaxReturn = DateUtil.PlusDay(IssuedDate, int.Parse(TextBoxDuration.Text));
                      
                        bool Late = false;
                        if (Issue.date.CompareTo(MaxReturn) > 0)
                        {
                            Late = true;
                        }
                        
                        string[] Attrs = new string[] {
                            "class=\"pointerable\"",
                            "onclick=\"ScrollToElement('Book_Issue_Rec_" + Bs.book_issue_id + "')\""
                        };
                        string anchor = ControlUtil.GenerateHtmlTag("span", Attrs, Bs.book_issue_id);
                      
                        PanelIssueItem.Controls.Add(ControlUtil.GenerateLabel("<li class=\"book_issue-item\" id=\"Book_Return_Rec_" + Bs.id
                        + "\"> Return Rec Id: " + Bs.id + "<br/>Book Rec Id: "
                            + Bs.book_record_id + " - Returned From Issue Rec Id: " + anchor +
                            "<br/>" + Bs.book_record.book.title
                            + "<br/>Issued: " + IssuedDate + "<br/>Max return: " + MaxReturn + " - late: <code>" + Late
                            + "</code></li>"));


                    }

                }


                PanelIssue.Controls.Add(PanelIssueItem);
                PanelIssue.Controls.Add(ControlUtil.GenerateLabel("</ol><hr/>"));
                PanelIssueReturn.Controls.Add(PanelIssue);
            }
        }

        protected void ButtonSearch_Click(object sender, EventArgs e)
        {

            string ID = TextBoxStudentID.Text;
            Dictionary<string, object> ParamsIssue = new Dictionary<string, object>();
            ParamsIssue.Add("student_id", ID);
            ParamsIssue.Add("exact", true);
            ParamsIssue.Add("type", "issue");
            List<object> ObjList = IssueService.SearchAdvanced(ParamsIssue);
            Issues = (List<issue>)ObjectUtil.ConvertList(ObjList, typeof(List<issue>));

            Dictionary<string, object> ParamsReturn = new Dictionary<string, object>();
            ParamsReturn.Add("student_id", ID);
            ParamsReturn.Add("exact", true);
            ParamsReturn.Add("type", "return");
            List<object> ObjList2 = IssueService.SearchAdvanced(ParamsReturn);
            IssuesReturn = (List<issue>)ObjectUtil.ConvertList(ObjList2, typeof(List<issue>));
            PopulateStudentDetail(ID);
            PopulateIssueList();
        }

        private void PopulateStudentDetail(string Id)
        {

            studentDetail.InnerHtml = "";
            student Std = StudentService.GetByIdFull(Id);
            if (Std != null)
            {
                Dictionary<string, object> StudentInfo = new Dictionary<string, object>();
                StudentInfo.Add("Student ID", Std.id);
                StudentInfo.Add("Student Name", Std.name);

                if (Std.@class != null)
                {
                    StudentInfo.Add("Student Class", Std.@class.class_name);
                }
                studentDetail.Controls.Add(ControlUtil.GenerateTableFromMap(StudentInfo));
            }
            else
            {
                studentDetail.InnerHtml += "~<i>Student Not Found</i>";
            }
        }

        protected void ButtonReturn_Click(object sender, EventArgs e)
        {
            try
            {
                book_issue BS = new book_issue();
                BS.id = StringUtil.GenerateRandomChar(10);
                //BS.book_issue_id = (TextBoxIssueRecordId.Text.Trim());
                //add
                student Student = (student)StudentService.GetById(TextBoxStudentID.Text);
                if(Student == null)
                {
                    return;
                }
                string Student_ID = Student.id;
                string RecId = (TextBoxIssueRecordId.Text.Trim());
                List<object> BookIssuesOBJ = BookIssueService.SearchAdvanced(new Dictionary<string, object>()
                {
                        {"student_id",Student_ID },
                        {"book_record_id",RecId },
                        {"issue_type","issue" },
                        {"book_return","0" }
                });
                if (BookIssuesOBJ == null || BookIssuesOBJ.Count == 0)
                {
                    return;
                }
                book_issue reffBookIssue = (book_issue)BookIssuesOBJ.ElementAt(0);
                if(reffBookIssue.book_return != 0)
                {
                    return;
                }
                BS.book_issue_id = reffBookIssue.id;
                //   book_issue reffBookIssue = (book_issue)BookIssueService.GetById(BS.book_issue_id);

                if (!BookIssue.ExistBookRecord(BS.book_issue_id, BookIssuesReturn) && null != reffBookIssue)
                {
                    
                    //Check book_isue where book_rec_id = rec book_return != null || 0 and issue.student_id = std_id
                    string StudentId = TextBoxStudentID.Text;


                    if ( BookIssueService.GetByBookIssueIdReturned(reffBookIssue.id).Count == 0)
                    {
                        book_record BookRecord =(book_record) bookRecordService.GetById(reffBookIssue.book_record_id);

                        //BS.book_issue2 = reffBookIssue;
                        BS.book_record_id = BookRecord.id;

                        book Book =(book) ObjectUtil.GetObjectValues(new string[]
                        {
                            "id","title"
                        },BookRecord.book);

                        book_record BookRec = (book_record)ObjectUtil.GetObjectValues(new string[]
                        {
                            "id","book_id", "available"
                        }, BookRecord);
                        BookRec.book = Book;
                        BS.book_record = BookRec;
                        BookIssuesReturn.Add(BS);
                    }

                }
            }catch(HttpUnhandledException ex)
            {
                throw ex;
            }

            ButtonSearch_Click(sender, e);
            ViewState["BookIssuesReturn"] = BookIssuesReturn;
            PopulateBooksToReturn();

        }

        protected void ButtonSubmitReturn_Click(object sender, EventArgs e)
        {
            if (BookIssuesReturn == null || BookIssuesReturn.Count == 0)
            {
                //info.InnerHtml = "Please choose book to issue";
                return;
            }
            student Student = (student)StudentService.GetById(TextBoxStudentID.Text);
            if (Student != null)
            {
                string IssueID = StringUtil.GenerateRandomNumber(9);
                issue Issue = new issue();
                Issue.user_id = LoggedUser.id;
                Issue.id = IssueID;
                Issue.type = "return";
                Issue.date = DateTime.Now;
                Issue.student_id = Student.id;
                Issue.addtional_info = "RETURN INFO";

                if (null == IssueService.Add(Issue))
                {
                    AlertMessage("Gagal tambah issue");
                    return;
                }
                foreach (book_issue BS in BookIssuesReturn)
                {
                    BS.issue_id = Issue.id;
                    if (null == BookIssueService.Add(BS))
                    {
                        AlertMessage("Gagal tambah book_issue");
                        break;
                    }
                    book_record BR = (book_record)bookRecordService.GetById(BS.book_record_id);
                    BR.available = 1;
                    BS.book_issue2 = (book_issue)BookIssueService.GetById(BS.book_issue_id);
                    BS.book_issue2.book_return = 1;

                    if (null == bookRecordService.Update(BR) || null == BookIssueService.Update(BS.book_issue2))
                    {
                        //info.InnerHtml = "Gagal update book_record ";
                        break;
                    }

                }
                ViewState["BookIssuesReturn"] = null;
                Issues = new List<issue>();
                IssuesReturn = new List<issue>();
                BookIssuesReturn = new List<book_issue>();
                PopulateBooksToReturn();
                AlertMessage("Sukses mengembalikan buku " + IssueID);
                //ButtonClearList_Click(sender, e);

            }
            else
            {

                AlertMessage("Siswa tdk ada");
            }
        }

        private void PopulateBooksToReturn()
        {
            PanelBookIssues.Controls.Clear();
            // PanelBookIssues.Controls.Add(ControlUtil.GenerateLabel("test"));
            foreach (book_issue b in BookIssuesReturn)
            {
                Panel PanelItem = new Panel();
                PanelItem.Controls.Add(ControlUtil.GenerateLabel(b.id + (b.book_record != null? " | " + b.book_record.book.title + "(" + b.book_record.id + ")" + " " : "")));
                Button DeleteButton = new Button()
                ;
                DeleteButton.CssClass = "btn btn-danger";
                DeleteButton.Text = "Remove";
                DeleteButton.CausesValidation = false;
                DeleteButton.CommandArgument = b.id;
                DeleteButton.Click += new EventHandler(DeleteReturnItem);
                PanelItem.Controls.Add(DeleteButton);
                //  PanelBookIssues.Controls.Add(PanelItem);
                PanelBookIssues.Controls.Add(PanelItem);
            }
            //PanelTest.Controls.Clear();
           
            //    PanelTest.Controls.Add(new Label() { Text = BookIssuesReturn.Count+ DateTime.Now.ToLongTimeString() });
           

        }

        protected void DeleteReturnItem(object sender, EventArgs e)
        {
            Button Btn = (Button)sender;
            if (Btn != null)
            {
                string id = Btn.CommandArgument;
                if (!id.Equals(""))
                {
                    foreach (book_issue BS in BookIssuesReturn)
                    {
                        if (BS.id.Equals(id))
                        {
                            BookIssuesReturn.Remove(BS);
                            break;
                        }
                    }
                }
            }

            ViewState["BookIssuesReturn"] = BookIssuesReturn;
            ButtonSearch_Click(sender, e);

            PopulateBooksToReturn();
        }

        private void AlertMessage(string Message)
        {
            info.InnerHtml = ControlUtil.GenerateHtmlTag("p", null, Message);
        }
    }
}