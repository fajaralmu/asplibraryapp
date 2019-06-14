﻿using OurLibrary.Models;
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
        private book_issueService BookIssueService=new book_issueService();
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
                PopulateBookIssues();
            }
        }

        private void PopulateIssueList()
        {
            PanelIssueLis.Controls.Clear();
            foreach (issue Issue in Issues) {
                if(null == Issue.book_issue|| Issue.book_issue.Count==0)
                {
                    continue;
                }
                Panel PanelIssue = new Panel();
                PanelIssue.Controls.Add(ControlUtil.GenerateLabel("<b>Issue ID: "+Issue.id+ " (" + Issue.type + ")</b><br/>date: "+Issue.date+"<ul>"));
                Panel PanelIssueItem = new Panel();
                foreach(book_issue Bs in Issue.book_issue)
                {
                    PanelIssueItem.Controls.Add(ControlUtil.GenerateLabel("<li>"+Bs.id+" rec: "+Bs.book_record_id+"<br/>"+Bs.book_record.book.title+" - returned: "+(Bs.book_record.available == 1).ToString().ToUpper()+"</li>"));
                }
                PanelIssue.Controls.Add(PanelIssueItem);
                DateTime MaxReturn = DateUtil.PlusDay(Issue.date, int.Parse(TextBoxDuration.Text));
                bool Late = false;
                if (Issue.date.CompareTo(MaxReturn) > 0)
                {
                    Late = true;
                }
                PanelIssue.Controls.Add(ControlUtil.GenerateLabel("</ul>Max return: " + MaxReturn+", Late:"+Late+"<hr/>")) ;
                PanelIssueLis.Controls.Add(PanelIssue);
            }

            //BOOK RETURNED

            PanelIssueReturn.Controls.Clear();
            foreach (issue Issue in IssuesReturn)
            {
                if (null == Issue.book_issue || Issue.book_issue.Count == 0)
                {
                    continue;
                }
                Panel PanelIssue = new Panel();
                PanelIssue.Controls.Add(ControlUtil.GenerateLabel("<b>Issue ID: " + Issue.id + " ("+Issue.type+")</b><br/>date: " + Issue.date + "<ul>"));
                Panel PanelIssueItem = new Panel();
                foreach (book_issue Bs in Issue.book_issue)
                {
                    
                    Bs.book_issue2 = (book_issue)BookIssueService.GetById(Bs.book_issue_id);
                    if (null != Bs.book_issue2)
                    {
                        DateTime MaxReturn = DateUtil.PlusDay(Bs.book_issue2.issue.date, int.Parse(TextBoxDuration.Text));
                        bool Late = false;
                        if (Issue.date.CompareTo(MaxReturn) > 0)
                        {
                            Late = true;
                        }
                        Bs.book_issue2.issue = (issue)IssueService.GetById(Bs.book_issue2.issue_id);
                        PanelIssueItem.Controls.Add(ControlUtil.GenerateLabel("<li>" + Bs.id + " book rec: "
                            + Bs.book_record_id + " - issue_rec: "+Bs.book_issue_id+
                            "<br/>" + Bs.book_record.book.title
                            + " - returned: " + (Bs.book_record.available == 1).ToString().ToUpper()
                            + " <br/> issued: " + Bs.book_issue2.issue.date + " | max return: " + MaxReturn + " - late: " + Late
                            + "</li>"));
                    }
                                
                }
                PanelIssue.Controls.Add(PanelIssueItem);
                PanelIssue.Controls.Add(ControlUtil.GenerateLabel("</ul><hr/>"));
                PanelIssueReturn.Controls.Add(PanelIssue);
            }
        }

        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            string ID = TextBoxStudentID.Text;
            Dictionary<string, object> Params = new Dictionary<string, object>();
            Params.Add("student_id", ID);
            Params.Add("type", "issue");
            List<object> ObjList = IssueService.SearchAdvanced(Params);
            Issues = (List<issue>)ObjectUtil.ConvertList(ObjList, typeof(List<issue>));

            Dictionary<string, object> Params2 = new Dictionary<string, object>();
            Params2.Add("student_id", ID);
            Params2.Add("type", "return");
            List<object> ObjList2 = IssueService.SearchAdvanced(Params2);
            IssuesReturn = (List<issue>)ObjectUtil.ConvertList(ObjList2, typeof(List<issue>));

            PopulateIssueList();
        }
    
        protected void ButtonReturn_Click(object sender, EventArgs e)
        {
            book_issue BS = new book_issue();
            BS.id = StringUtil.GenerateRandom(10);
            BS.book_issue_id = (TextBoxIssueRecordId.Text.Trim());
            book_issue reffBookIssue = (book_issue)BookIssueService.GetById(BS.book_issue_id);
           
            if (!BookIssue.ExistBookRecord(BS.book_record_id, BookIssuesReturn) && null != reffBookIssue)
            {
                BS.book_record_id = reffBookIssue.book_record.id;
                BS.book_record = reffBookIssue.book_record;
                //Check book_isue where book_rec_id = rec book_return != null || 0 and issue.student_id = std_id
                string StudentId = TextBoxStudentID.Text;
               
                
                if (null != reffBookIssue && reffBookIssue.book_record.available == (0))
                {
                    BS.book_issue2 = reffBookIssue;
                    BS.book_record = reffBookIssue.book_record;
                    BookIssuesReturn.Add(BS);
                }

            }

            ViewState["BookIssuesReturn"] = BookIssuesReturn;
            PopulateBookIssues();
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
                string IssueID = StringUtil.GenerateRandom(9);
                issue Issue = new issue();
                Issue.user_id = LoggedUser.id;
                Issue.id = IssueID;
                Issue.type = "return";
                Issue.date = DateTime.Now;
                Issue.student_id = Student.id;
                Issue.addtional_info = "test";

                if (null == IssueService.Add(Issue))
                {
                    //info.InnerHtml = "Gagal tambah issue";
                    return;
                }
                foreach (book_issue BS in BookIssuesReturn)
                {
                    BS.issue_id = Issue.id;
                    if (null == BookIssueService.Add(BS))
                    {
                        //info.InnerHtml = "Gagal tambah book_issue";
                        break;
                    }
                    book_record BR = (book_record)bookRecordService.GetById(BS.book_record_id);
                    BR.available = 1;
                    BS.book_issue2 =(book_issue)BookIssueService.GetById(BS.book_issue_id);
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
                PopulateBookIssues();
                //info.InnerHtml = "Sukses tambah issue";
                //ButtonClearList_Click(sender, e);

            }
            else
            {

                //info.InnerHtml = "Siswa tdk ada";
            }
        }

        private void PopulateBookIssues()
        {
            PanelBookIssues.Controls.Clear();

            foreach (book_issue b in BookIssuesReturn)
            {
                Panel PanelItem = new Panel();
                PanelItem.Controls.Add(ControlUtil.GenerateLabel(b.id + " | " + b.book_record.book.title + "(" + b.book_record.id + ")" + " "));
                Button DeleteButton = new Button()
                ;
                DeleteButton.CssClass = "btn btn-danger";
                DeleteButton.Text = "Remove";
                DeleteButton.CausesValidation = false;
                DeleteButton.CommandArgument = b.id;
                DeleteButton.Click += new EventHandler(DeleteReturnItem);
                PanelItem.Controls.Add(DeleteButton);
                PanelBookIssues.Controls.Add(PanelItem);
            }

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
            PopulateBookIssues();
        }
    }
}