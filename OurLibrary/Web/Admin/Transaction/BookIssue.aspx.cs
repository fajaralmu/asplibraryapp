using OurLibrary.Models;
using OurLibrary.Service;
using OurLibrary.Util.Common;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OurLibrary.Web.Admin.Transaction
{
    public partial class BookIssue : BasePage
    {
        private StudentService StudentService = new StudentService();
        private book_issueService BookIssueService = new book_issueService();
        private IssueService IssueService = new IssueService();
        private BookService bookService = new BookService();
        private Book_recordService bookRecordService = new Book_recordService();
        private List<book> BookList = new List<book>();
        private book Book;
        private book_record BookRecord;
        private List<string> strings = new List<string>();
        private string TitleKeyWord = "";
        private List<book_issue> BookIssues = new List<book_issue>();
        private student CurrentStudent;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] == null || !userService.IsValid((user)Session["user"]))
            {
                Response.Redirect("~/Web/Admin/");
            }
            else
            {
                LoggedUser = (user)Session["user"];
                info.InnerText = "Postback: " + Page.IsPostBack;
                 InitPage();
               
               

                if (Request.QueryString["book_master_id"] != null && !((string)Request.QueryString["book_master_id"]).Equals(""))
                {
                    ShowCurrentBook((string)Request.QueryString["book_master_id"]);
                }
                //if (Session["GenerateListBook"] != null && ((bool)Session["GenerateListBook"]) == true)
                //{
                //    generateListBook();
                //    Session["GenerateListBook"] = false;
                //}

            }
        }

        private bool IsStudentValid()
        {
            return Session["Student_Id"] != null && !Session["Student_Id"].GetType().Equals(typeof(student));
        }

        private void InitPage()
        {
            if (!IsStudentValid())
            {
                return;
            }
            CurrentStudent = ((student)Session["Student_Id"]);
            if (Session["BookIssues_"+CurrentStudent.id] != null)
            {
                BookIssues = (List<book_issue>)Session["BookIssues_"+CurrentStudent.id];
                PopulateBookIssues();

            }
            //if (Session["CurrentBook_"+CurrentStudent.id]!= null)
            //{
            //    Book = (book)Session["CurrentBook"];
            //    InputMasterBook.Text = Book.title;
            //    LabelCurrentBook.Text = Book.title + " by " + Book.author.name;
            //}
            //else
            //{
            //    LabelCurrentBook.Text = "";
            //}
            if (Session["CurrentBookRecord_"+CurrentStudent.id] != null)
            {
                BookRecord = (book_record)Session["CurrentBookRecord_"+CurrentStudent.id];
            }
          //  TextBoxStudentID.Text = CurrentStudent.id;
            idStd.InnerHtml = "id: " + CurrentStudent.id + " name: " + CurrentStudent.name;
            AlertMessage(null);
        }

        private void PopulateBookIssues()
        {
            PanelBookIssues.Controls.Clear();

            foreach (book_issue b in BookIssues)
            {
                Panel PanelItem = new Panel();
                PanelItem.Controls.Add(ControlUtil.GenerateLabel(b.id + " | " + b.book_record.book.title + "(" + b.book_record_id + ")" + " "));
                Button DeleteButton = new Button()
                ;
                DeleteButton.CssClass = "btn btn-danger";
                DeleteButton.Text = "Remove";
                DeleteButton.CausesValidation = false;
                DeleteButton.CommandArgument = b.id;
                DeleteButton.Click += new EventHandler(DeleteIssueItem);
                PanelItem.Controls.Add(DeleteButton);
                PanelBookIssues.Controls.Add(PanelItem);
            }

        }

        protected void DeleteIssueItem(object sender, EventArgs e)
        {
            if(!IsStudentValid()){
                return;
            }
            Button Btn = (Button)sender;
            if (Btn != null)
            {
                string id = Btn.CommandArgument;
                if (!id.Equals(""))
                {
                    foreach (book_issue BS in BookIssues)
                    {
                        if (BS.id.Equals(id))
                        {
                            BookIssues.Remove(BS);
                            break;
                        }
                    }
                }
            }
            Session["BookIssues_"+CurrentStudent.id] = BookIssues;
            PopulateBookIssues();
        }

        protected void ButtonSearchMasterBook_Click(object sender, EventArgs e)
        {
            if (!IsStudentValid())
            {
                return;
            }
            Session["CurrentBook_"+CurrentStudent.id]= null;
            Book = null;
            Session["GenerateListBook"] = true;
            TitleKeyWord = InputMasterBook.Text;
            GenerateListBook();
            if (BookList == null || BookList.Count <= 0)
            {
                PanelBookList.Controls.Add(ControlUtil.GenerateLabel("Book Not Found", System.Drawing.Color.Orange));
                LabelCurrentBook.Text = "<b>NULL</b>";
            }

        }

        protected void ShowCurrentBook(string id)
        {
            if (!IsStudentValid())
            {
                return;
            }
            PanelBookList.Controls.Clear();
            if (id != null && !id.Equals(""))
            {

                Book = (book)bookService.GetCompleteBook(id);
                if (Book == null)
                {
                    return;
                }
                //  InputMasterBook.Text = Book.title;
                LabelCurrentBook.Text = Book.title + " by " + Book.author.name;
                Session["CurrentBook_"+CurrentStudent.id]= Book;
                CheckAvailability();
            }
            Book = null;


        }

        private void GenerateListBook()
        {
            Dictionary<string, object> Params = new Dictionary<string, object>();
            if (TitleKeyWord.Equals(""))
            {
                return;
            }
            Params.Add("title", TitleKeyWord);
            List<object> ObjList = bookService.SearchAdvanced(Params, 10);
            BookList = (List<book>)ObjectUtil.ConvertList(ObjList, typeof(List<book>));

            foreach (book b in BookList)
            {
                Button btnBook = new Button();
                btnBook.UseSubmitBehavior = false;
                btnBook.CausesValidation = false;
                btnBook.Text = b.title + " | author: " + b.author.name;
                btnBook.CommandArgument = b.id;
                btnBook.PostBackUrl = "/Web/Admin/Transaction/BookIssue.aspx?book_master_id=" + b.id;
                //   btnBook.Click += new EventHandler(BtnBookClick);
                btnBook.CssClass = "btn btn-success";
                btnBook.ID = "BOOK_" + b.id;
                PanelBookList.Controls.Add(btnBook);
            }
            //    InputMasterBook.Text = TitleKeyWord;
            TitleKeyWord = "";
        }

        private void CheckAvailability()
        {
            if (Book == null)
                return;

            LabelRecordList.Text = "";
            List<book_record> BookRecords = bookRecordService.FindByBookId(Book.id);
            if (BookRecords == null || BookRecords.Count == 0)
            {
                LabelRecordList.Text = "Unavailable";
                return;
            }
            foreach (book_record BR in BookRecords)
            {
                if (BR.available == 1)
                {
                    string[] Attrs = new string[] {
                        "href=\"#\"",
                        "onclick=SetTextInput('"+BR.id+"','MainContent_TextBoxRecordId')"
                    };
                    string RecButton = ControlUtil.GenerateHtmlTag("a", Attrs, BR.id);
                    LabelRecordList.Text += RecButton + " | ";
                }
            }
        }

        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            if (!IsStudentValid())
            {
                return;
            }
            book_issue BS = new book_issue();
            BS.id = StringUtil.GenerateRandomChar(10);
            BS.book_record_id = (TextBoxRecordId.Text.Trim());
            if (!ExistBookRecord(BS.book_record_id, BookIssues))
            {
                book_record DBRecord = bookRecordService.FindByIdFull(BS.book_record_id);
                if (null != DBRecord && DBRecord.available == (1))
                {
                    BS.book_record = DBRecord;
                    BookIssues.Add(BS);
                }

            }

            Session["BookIssues_"+CurrentStudent.id] = BookIssues;
            PopulateBookIssues();
        }

        protected void ButtonClearList_Click(object sender, EventArgs e)
        {
            Session["Student_Id"] = null;
            BookIssues = new List<book_issue>();
            NameObjectCollectionBase.KeysCollection Keys = Session.Keys;
            for (int i = 0;i<  Keys.Count;i++)
            {
                string key = Keys[i];
                if (key.Contains("BookIssues_"))
                {
                    Session[key] = null;
                    Book = null;
                }

                if (key.Contains("CurrentBook_"))
                {
                    Session[key] = null;
                    BookRecord = null;
                }

                if (key.Contains("CurrentBookRecord_"))
                {
                    Session[key] = null;
                   
                }
               
            }
            /*Session["BookIssues_"+CurrentStudent.id] = null;
            Book = null;
            Session["CurrentBook_"+CurrentStudent.id]= null;
            BookRecord = null;
            Session["CurrentBookRecord_"+CurrentStudent.id] = null;*/
            TextBoxStudentID.Text = "";
            PanelBookIssues.Controls.Clear();
            PanelBookList.Controls.Clear();
            InitPage();
            ShowCurrentBook("");
            LabelRecordList.Text = "";

        }

        public static bool ExistBookRecord(string IssId, List<book_issue> BookIssues)
        {
            foreach (book_issue bs in BookIssues)
            {
                if (bs.book_issue_id!=null && bs.book_issue_id.Equals(IssId))
                {
                    return true;
                }
            }
            return false;
        }
        protected void ButtonSaveIssue_Click(object sender, EventArgs e)
        {
            if (BookIssues == null || BookIssues.Count == 0)
            {
                info.InnerHtml = "Please choose book to issue";
                return;
            }
            student Student = (student)StudentService.GetById(TextBoxStudentID.Text);
            if (Student != null)
            {
                string IssueID = StringUtil.GenerateRandomNumber(9);
                issue Issue = new issue();
                Issue.user_id = LoggedUser.id;
                Issue.id = IssueID;
                Issue.type = "issue";
                Issue.date = DateTime.Now;
                Issue.student_id = Student.id;
                Issue.addtional_info = "test";

                if (null == IssueService.Add(Issue))
                {
                    AlertMessage("Gagal tambah issue");
                    return;
                }
                foreach (book_issue BS in BookIssues)
                {

                    BS.issue_id = Issue.id;
                    if (null == BookIssueService.Add(BS))
                    {
                        AlertMessage("Gagal tambah book_issue");
                        break;
                    }
                    book_record BR = (book_record)bookRecordService.GetById(BS.book_record_id);
                    BR.available = 0;

                    if (null == bookRecordService.Update(BR))
                    {
                        AlertMessage("Gagal update book_record ");
                        break;
                    }

                }

                ButtonClearList_Click(sender, e);
                AlertMessage("Sukses tambah issue "+ IssueID);

            }
            else
            {

                AlertMessage("Siswa tdk ada");
            }

        }

        private void AlertMessage(string Message)
        {
            info.InnerHtml = ControlUtil.GenerateHtmlTag("p", null, Message);
        }

        protected void ButtonSearchStd_Click(object sender, EventArgs e)
        {
            string StudentID = TextBoxStudentID.Text;
            if (!StudentID.Equals(""))
            {
                student STD =(student) StudentService.GetById(StudentID);
                if(STD != null)
                {
                    Session["Student_Id"] = STD;
                    CurrentStudent = STD;
                    idStd.InnerHtml = "id: "+STD.id + " name: " + STD.name;
                }
            }
        }
    }
}