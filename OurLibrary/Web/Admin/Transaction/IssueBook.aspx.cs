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
    public partial class IssueBook : BasePage
    {
        private BookService bookService = new BookService();
        private Book_recordService bookRecordService = new Book_recordService();
        private List<book> BookList = new List<book>();
        private book Book;
        private book_record BookRecord;
        private List<string> strings = new List<string>();
        private List<book_issue> BookIssues = new List<book_issue>();

        protected void Page_Load(object sender, EventArgs e)
        {
            info.InnerText = "Postback: " + Page.IsPostBack;
            InitPage();
            if (Page.IsPostBack)
            {
                if (ViewState["GenerateListBook"] != null&& ((bool)ViewState["GenerateListBook"] )== true)
                {
                    generateListBook();
                    ViewState["GenerateListBook"] = false;
                }
            }
        }

        private void InitPage()
        {
            if(ViewState["listString"] != null)
            {
                strings = (List < string > )ViewState["listString"];
            }

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
                LabelCurrentBook.Text = "NULL";
            }
            if (ViewState["CurrentBookRecord"] != null)
            {
                BookRecord = (book_record)ViewState["CurrentBookRecord"];
            }
        }

        private void PopulateBookIssues()
        {
            PanelBookIssues.Controls.Clear();
                       
            foreach (book_issue b in BookIssues)
            {
                Panel PanelItem = new Panel();
                PanelItem.Controls.Add(ControlUtil.GenerateLabel(b.id + " | " + b.book_record.book.title+"("+ b.book_record_id+")"+" "));
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
                    foreach(book_issue BS in BookIssues)
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
                    LabelCurrentBook.Text = Book.title +" by "+Book.author.name;
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
                LabelRecordList.Text += BR.id + " | ";
            }
        }

        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            book_issue BS = new book_issue();
            BS.id = StringUtil.GenerateRandom(11);
            BS.book_record_id=(TextBoxRecordId.Text.Trim());
            if (!ExistBookRecord(BS.book_record_id))
            {
                book_record DBRecord = bookRecordService.FindByIdFull(BS.book_record_id);
                if(null != DBRecord)
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
            ViewState["BookIssues"] = BookIssues;
            PopulateBookIssues();
        }

        private bool ExistBookRecord(string RecId)
        {
            foreach (book_issue bs in BookIssues) {
                if (bs.book_record_id.Equals(RecId))
                {
                    return true;
                }
            }
            return false;
        }
    }
}