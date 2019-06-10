using OurLibrary.Models;
using OurLibrary.Parameter;
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
    public partial class AddBookStorage : BasePage
    {
        private BookService bookService = new BookService();
        private Book_recordService bookRecordService = new Book_recordService();
        private List<book> BookList = new List<book>();
        private book Book;
        private book_record BookRecord;
        private List<book_record> BookRecordList = new List<book_record>();

        protected void Page_Load(object sender, EventArgs e)
        {
            init();
            generateListBook();
            checkBookAvailability();
        }

        private void init()
        {
            Total = bookRecordService.ObjectCount();
            LabelMessage.Text = null;
            InitState();
            InitNavigation();
            PopulateListTable();
        }

        private void PopulateListTable()
        {
            BookRecordList.Clear();

            List<object> objList = bookRecordService.ObjectList(Offset, Limit);
            BookRecordList = (List<book_record>)ObjectUtil.ConvertList(objList, typeof(List<book_record>));
            TableRow HeaderRow = TableList.Rows[0];
            TableList.Rows.Clear();
            TableList.Rows.Add(HeaderRow);
            int No = Offset * Limit;
            foreach (book_record br in BookRecordList)
            {
                No++;
                string NoStr = Convert.ToString(No);
                TableRow TRow = new TableRow();
                TableCell TCellNo = new TableCell();
                TableCell TCellRecordId = new TableCell();
                TableCell TCellBookCode = new TableCell();
                TableCell TCellBookTitle = new TableCell();
                TableCell TCellInfo = new TableCell();
                TableCell TCellOption = new TableCell();
                Button EditButton = new Button();
                Button DeleteButton = new Button();

                EditButton.Text = "edit";
                EditButton.CssClass = "btn btn-warning";
                EditButton.CausesValidation = false;
                EditButton.UseSubmitBehavior = false;
                //  EditButton.PostBackUrl = URL + "?id=" + p.id;
                EditButton.CommandArgument = br.id;
                //   EditButton.Click += new EventHandler(BtnEditClick);

                DeleteButton.Text = "delete";
                DeleteButton.CssClass = "btn btn-danger";
                DeleteButton.CausesValidation = false;
                DeleteButton.UseSubmitBehavior = false;
                DeleteButton.CommandArgument = br.id;
                //    DeleteButton.Click += new EventHandler(BtnDeleteClick);

                TCellNo.Controls.Add(ControlUtil.GenerateLabel(NoStr));
                TCellRecordId.Controls.Add(ControlUtil.GenerateLabel(br.id));
                TCellBookCode.Controls.Add(ControlUtil.GenerateLabel(br.book_code));
                TCellBookTitle.Controls.Add(ControlUtil.GenerateLabel(br.book.title));
                TCellInfo.Controls.Add(ControlUtil.GenerateLabel(br.additional_info));
                //option
                TCellOption.Controls.Add(EditButton);
                TCellOption.Controls.Add(DeleteButton);

                TRow.Controls.Add(TCellNo);
                TRow.Controls.Add(TCellRecordId);
                TRow.Controls.Add(TCellBookCode);
                TRow.Controls.Add(TCellBookTitle);
                TRow.Controls.Add(TCellInfo);
                TRow.Controls.Add(TCellOption);

                TableList.Controls.Add(TRow);

            }
        }

        private void InitNavigation()
        {
            if (Session[PageParameter.PagingLimit] != null && Session[PageParameter.PagingOffset] != null)
            {
                if (TextBoxLimit.Text != null && TextBoxLimit.Text != "" && Convert.ToInt32(TextBoxLimit.Text) > 0)
                {

                    Limit = Convert.ToInt32(TextBoxLimit.Text);
                    Session[PageParameter.PagingLimit] = Limit;


                }
                else
                {
                    Limit = (int)Session[PageParameter.PagingLimit];
                }

                Offset = (int)Session[PageParameter.PagingOffset];
            }
            PopulateNavigation();
        }

        private void InitState()
        {
            if (Session[ModelParameter.BookRecordId] != null)
            {
                string Id = Session[ModelParameter.BookRecordId].ToString();
                object[] Params = { Id };
                BookRecord = (book_record)bookRecordService.GetById(Id);
                if (BookRecord != null)
                {

                    State = ModelParameter.EDIT;
                }
            }
        }
        private void PopulateNavigation()
        {

            PanelNavigation.Controls.Clear();
            double ButtonCount = Math.Ceiling((double)Total / (double)Limit);
            for (int i = 0; i < Convert.ToInt32(ButtonCount); i++)
            {
                Button NavButton = new Button();
                NavButton.Text = (i + 1).ToString();
                NavButton.ID = "NAV_" + i;
                NavButton.CausesValidation = false;
                NavButton.UseSubmitBehavior = false;
                NavButton.CommandArgument = i.ToString() + "~" + Limit;
                NavButton.CssClass = i == Offset ? "btn btn-primary" : "btn btn-info";
                NavButton.Click += new EventHandler(BtnNavClick);
                PanelNavigation.Controls.Add(NavButton);
            }
        }


        private void checkBookAvailability()
        {
            if (Session[ModelParameter.MasterBookId] != null && !Session[ModelParameter.MasterBookId].ToString().Equals(""))
            {
                Book = (book)bookService.GetById(Session[ModelParameter.MasterBookId].ToString());
                LabelCurrentBook.Text = Book.title;

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
                    Session[ModelParameter.MasterBookId] = id;
                    Book = (book)bookService.GetById(id);
                    InputMasterBook.Text = Book.title;
                    LabelCurrentBook.Text = Book.title;
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

        protected void ButtonSearchMasterBook_Click(object sender, EventArgs e)
        {
            Book = null;
            //   generateListBook();
            Session[ModelParameter.MasterBookId] = null;

            if (BookList == null || BookList.Count <= 0)
            {
                PanelBookList.Controls.Add(ControlUtil.GenerateLabel("Book Not Found", System.Drawing.Color.Orange));
                LabelCurrentBook.Text = "<b>NULL</b>";
            }

        }

        protected void TextBoxLimit_TextChanged(object sender, EventArgs e)
        {
            Limit = Convert.ToInt32(TextBoxLimit.Text);
            if (Limit > 0)
            {
                Session[PageParameter.PagingLimit] = Limit;
                Session[PageParameter.PagingOffset] = 0;
                Offset = 0;
                UpdateList();
            }
        }

        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            if (Book == null)
            {
                PanelBookList.Controls.Add(ControlUtil.GenerateLabel("Book Required", System.Drawing.Color.Red));
                return;
            }
            else if (inputBookCode.Value != null && !inputBookCode.Value.Equals("") && State == ModelParameter.ADD)
            {
                book_record BookRecord = new book_record();
                BookRecord.id = StringUtil.GenerateRandom(9);
                BookRecord.book_id = Book.id;
                BookRecord.book_code = inputBookCode.Value;
                BookRecord.additional_info = inputAdditionalInfo.InnerText;
                book_record NewBookRecord = (book_record)bookRecordService.Add(BookRecord);
                if (NewBookRecord != null)
                {
                    clearField();
                    LabelMessage.Text = "Success Adding New Record";
                }
                else
                {
                    LabelMessage.Text = "Failed Adding New Record";
                }
            }
        }

        private void clearField()
        {
            Session[ModelParameter.MasterBookId] = null;
            Book = null;
            PanelBookList.Controls.Clear();
            LabelCurrentBook.Text = null;
            BookList.Clear();
            inputAdditionalInfo.InnerText = "";
            InputMasterBook.Text = "";
            inputBookCode.Value = "";

        }
    }
}