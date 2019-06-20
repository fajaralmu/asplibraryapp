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
            
            List<object[]> BookInfos = new List<object[]>();
            string[] RecordIds = new string[BookRecords.Count+1];
            string[] BookCodes = new string[BookRecords.Count+1];
            RecordIds[0] = "Record Id";
            BookCodes[0] = "Book Codes";
            int Index = 1;
            foreach (book_record BR in BookRecords)
            {
                LabelAvailabilityStatus.Text = "Available";
                LabelAvailabilityStatus.ForeColor = System.Drawing.Color.Green;
                System.Drawing.Color LabelColor = System.Drawing.Color.Blue;
                string text;
                RecordIds[Index] = BR.id;
                BookCodes[Index] = BR.book_code;
                if (BR.available != 1)
                {
                    BookCodes[Index] = BR.book_code + " (unavailable)";
                }
                Index++;
            }
            BookInfos.Add(RecordIds);
            BookInfos.Add(BookCodes);
            Table BookDetailInfo = ControlUtil.GenerateTableFromArray(BookInfos);
            PanelAvailabilityDetail.Controls.Add(BookDetailInfo);
        }
    }
}