using OurLibrary.Models;
using OurLibrary.Service;
using OurLibrary.Util.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OurLibrary.Web.Catalog
{
    public partial class BookDetail : BasePage
    {
        private Book_recordService BookRecordService = new Book_recordService();
        private BookService BookService = new BookService();
        private string BookId = "";
        private book Book;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["ID"] == null || Request.QueryString["ID"].ToString().Equals(""))
            {
                Response.Redirect("~/Web/Catalog/");
            }
            BookId = Request.QueryString["ID"];
            InitBook();
            PopulateBookDetail();
            PopulateAvailabilityDetail();
        }

        private void InitBook()
        {
            this.Book = BookService.GetCompleteBook(BookId);
            if (Book == null)
            {
                Response.Redirect("~/Web/Catalog/");
            }
        }

        private void PopulateBookDetail()
        {
            BannerTitle.InnerText = Book.title;
            LabelTitle.Text = Book.title;
            if (Book.author != null)
                LabelAuthor.Text = Book.author.name;
            if (Book.publisher != null)
                LabelPublisher.Text = Book.publisher.name;
            if (Book.category != null)
                LabelCategory.Text = Book.category.category_name;
            LabelISBN.Text = Book.isbn;
            LabelPage.Text = Book.page.ToString();
            LabelReview.Text = Book.review;
            if (Book.img == null || Book.img.Equals(""))
            {
                BookCoverImage.ImageUrl = "~/Assets/Image/App/bookCover.png";

            }
            else
            {
                BookCoverImage.ImageUrl = "~/Assets/Image/Book/" + Book.img;

            }
        }

        private void PopulateAvailabilityDetail()
        {
            if (Book == null)
                return;

            PanelAvailabilityDetail.Controls.Clear();
            List<book_record> BookRecords = BookRecordService.FindByBookId(Book.id);
            if (BookRecords == null || BookRecords.Count == 0)
            {
                LabelAvailabilityStatus.Text = "Book Is Not Present In This Library";
                LabelAvailabilityStatus.ForeColor = System.Drawing.Color.Red;
                return;
            }
            foreach (book_record BR in BookRecords)
            {
                LabelAvailabilityStatus.Text = "Available";
                LabelAvailabilityStatus.ForeColor = System.Drawing.Color.Green;
                System.Drawing.Color LabelColor = System.Drawing.Color.Blue;
                string text;
                if (BR.available == 1)
                {
                    
                    text = "Record Id: " + BR.id + " | Book Code: " + BR.book_code + " <br/>";
                }
                else
                {
                    LabelColor = System.Drawing.Color.Black;
                    text = "Record Id: " + BR.id + " | Book Code: " + BR.book_code + " (unavailable) <br/>";
                }
                
                Label RecordDetail = ControlUtil.GenerateLabel(text, LabelColor);
                PanelAvailabilityDetail.Controls.Add(RecordDetail);
            }
        }
    }
}