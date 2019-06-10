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

namespace OurLibrary.Web.Catalog
{
    public partial class Default : BasePage
    {
        private BookService BookService = new BookService();
        private List<book> BookList = new List<book>();

        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Add("CatalogVisit", true);

            InitElement();
        }

        private void InitElement()
        {
            DoSearch();
            InitNavigation();

        }

        private void InitNavigation()
        {
            Total = BookService.getCountSearch();
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

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            DoSearch();
        }

        private void DoSearch()
        {
            string SearchKeyWordVal = SearchUniversalInput.Value;
            LabelMessage.Text = "Search for: " + SearchKeyWordVal;
            if (SearchPanelUniversal.Visible)
            {
                SearchUniversal(SearchKeyWordVal);
            }
            else if (SearchPanelAdvanced.Visible)
            {
                Dictionary<string, object> Params = new Dictionary<string, object>();
                string searchTitle = SearchTitle.Value;
                string searchAuthor = SearchAuthor.Value;
                string searchPublisher = SearchPublisher.Value;
                string searchISBN = SearchISBN.Value;
                string searchCategory = SearchCategory.Value;

                Params.Add("title", searchTitle);
                Params.Add("author", searchAuthor);
                Params.Add("publisher", searchPublisher);
                Params.Add("isbn", searchISBN);
                Params.Add("category", searchCategory);

                SearchAdvanced(Params);
            }
        }

        private void SearchUniversal(string key)
        {
            BookList.Clear();
            List<object> ObjList = BookService.SearchBookUniversal(key, Limit, Offset);
            BookList = (List<book>)ObjectUtil.ConvertList(ObjList, typeof(List<book>));
            UpdateCatalogList();
        }

        private void SearchAdvanced(Dictionary<string, object> Params)
        {
            BookList.Clear();
            List<object> ObjList = BookService.SearchAdvanced(Params, Limit, Offset);
            BookList = (List<book>)ObjectUtil.ConvertList(ObjList, typeof(List<book>));
            UpdateCatalogList();
        }

        private void UpdateCatalogList()
        {
            
        
            CatalogPanel.InnerHtml = "";
            foreach (book b in this.BookList)
            {
                string Title = b.title;
                string Author = "", Publisher = "", Category = "";
                if (b.author != null)
                    Author = b.author.name;
                if (b.publisher != null)
                    Publisher = b.publisher.name;
                if (b.category != null)
                    Category = b.category.category_name;
                string ISBN = b.isbn;
                string Page = b.page.ToString();
                string Review = b.review;
                string ImageURL = "/Assets/Image/App/bookCover.png";
                if (b.img != null && !b.img.Equals(""))

                {
                    ImageURL = "/Assets/Image/Book/" + b.img;

                }

                string HTML = "<div class=\"col-md-3\">" +
                "<div><img style=\" width:150px; height:200px\" src=\"" + ImageURL+"\" class=\"img-thumbnail\" /></div>"+
                "<h3>" + Title + "<small>"+Category+"</small></h3>" +
                "<p>Author: " +
                      Author + "</p>" +
                "<p>ISBN: " + ISBN + "</p>" +
                    "<p>" +
                "<a class=\"btn btn-default\" href=\"/Web/Catalog/BookDetail.aspx?ID=" + b.id + "\">More &raquo;</a>" +
                "</p></div>";
                CatalogPanel.InnerHtml += HTML;
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

        protected override void UpdateList()
        {
            nav_info.InnerText = "Offset:" + Offset + ", Limit:" + Limit + ", from updateList";
            DoSearch();
            UpdateNavigation();

        }

        protected void ShowAdvanced_Click(object sender, EventArgs e)
        {
            ClearPagingSession();
            SearchPanelAdvanced.Visible = true;
            SearchPanelUniversal.Visible = false;
        }

        protected void UpdateNavigation()
        {
            Total = BookService.getCountSearch();
            double ButtonCount = Math.Ceiling((double)Total / (double)Limit);
            for (int i = 0; i < Convert.ToInt32(ButtonCount); i++)
            {
                Button NavButton = (Button)PanelNavigation.FindControl("NAV_" + i);
                if (NavButton != null)
                {
                    NavButton.CssClass = i == Offset ? "btn btn-primary" : "btn btn-info";
                    PanelNavigation.Controls.RemoveAt(i);
                    PanelNavigation.Controls.AddAt(i, NavButton);
                }

            }
        }


        protected void HideAdvanced_Click(object sender, EventArgs e)
        {
            ClearPagingSession();
            SearchPanelAdvanced.Visible = false;
            SearchPanelUniversal.Visible = true;
        }
    }
}