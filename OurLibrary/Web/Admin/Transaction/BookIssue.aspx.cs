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
        private List<book_issue> BookIssues = new List<book_issue>();

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
                if (Page.IsPostBack)
                {
                    if (ViewState["GenerateListBook"] != null && ((bool)ViewState["GenerateListBook"]) == true)
                    {
                        generateListBook();
                        ViewState["GenerateListBook"] = false;
                    }
                }
            }
        }

        private void InitPage()
        {
            if (ViewState["BookIssues"] != null)
            {
                BookIssues = (List<book_issue>)ViewState["BookIssues"];
                PopulateBookIssues();

            }
            if (ViewState["CurrentBook"] != null)
            {
                Book = (book)ViewState["CurrentBook"];
                InputMasterBook.Text = Book.title;
                LabelCurrentBook.Text = Book.title + " by " + Book.author.name;
            }
            else
            {
                LabelCurrentBook.Text = "";
            }
            if (ViewState["CurrentBookRecord"] != null)
            {
                BookRecord = (book_record)ViewState["CurrentBookRecord"];
            }
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
            ViewState["BookIssues"] = BookIssues;
            PopulateBookIssues();
        }

        protected void ButtonSearchMasterBook_Click(object sender, EventArgs e)
        {
            ViewState["CurrentBook"] = null;
            Book = null;
            ViewState["GenerateListBook"] = true;

            if (BookList == null || BookList.Count <= 0)
            {
                PanelBookList.Controls.Add(ControlUtil.GenerateLabel("Book Not Found", System.Drawing.Color.Orange));
                LabelCurrentBook.Text = "<b>NULL</b>";
            }

        }

        protected void BtnBookClick(object sender, EventArgs e)
        {
            PanelBookList.Controls.Clear();
            Button Btn = (Button)sender;
            if (Btn != null)
            {
                string id = Btn.CommandArgument;
                if (!id.Equals(""))
                {

                    Book = (book)bookService.GetById(id);
                    InputMasterBook.Text = Book.title;
                    LabelCurrentBook.Text = Book.title + " by " + Book.author.name;
                    ViewState["CurrentBook"] = Book;
                    CheckAvailability();
                }
                else
                {

                }

            }
        }

        private void generateListBook()
        {
            Dictionary<string, object> Params = new Dictionary<string, object>();
            if (InputMasterBook.Text.Equals(""))
            {
                return;
            }
            Params.Add("title", InputMasterBook.Text);
            List<object> ObjList = bookService.SearchAdvanced(Params, 10);
            BookList = (List<book>)ObjectUtil.ConvertList(ObjList, typeof(List<book>));

            foreach (book b in BookList)
            {
                Button btnBook = new Button();
                btnBook.UseSubmitBehavior = false;
                btnBook.CausesValidation = false;
                btnBook.Text = b.title + " | author: " + b.author.name;
                btnBook.CommandArgument = b.id;
                btnBook.Click += new EventHandler(BtnBookClick);
                btnBook.CssClass = "btn btn-success";
                btnBook.ID = "BOOK_" + b.id;
                PanelBookList.Controls.Add(btnBook);
            }
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

            ViewState["BookIssues"] = BookIssues;
            PopulateBookIssues();
        }

        protected void ButtonClearList_Click(object sender, EventArgs e)
        {
            BookIssues = new List<book_issue>();
            ViewState["BookIssues"] = null;
            Book = null;
            ViewState["CurrentBook"] = null;
            BookRecord = null;
            ViewState["CurrentBookRecord"] = null;
            TextBoxStudentID.Text = "";
            PanelBookIssues.Controls.Clear();
            PanelBookList.Controls.Clear();
            InitPage();
        }

        public static bool ExistBookRecord(string RecId, List<book_issue> BookIssues)
        {
            foreach (book_issue bs in BookIssues)
            {
                if (bs.book_record_id.Equals(RecId))
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
                AlertMessage("Sukses tambah issue");

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

    }
}