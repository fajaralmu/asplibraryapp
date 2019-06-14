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
        private book_issueService BookIssueService=new book_issueService();
        private Book_recordService BookRecordService = new Book_recordService();
        private IssueService IssueService = new IssueService();
        private List<issue> Issues = new List<issue>();

        protected void Page_Load(object sender, EventArgs e)
        {

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
                PanelIssue.Controls.Add(ControlUtil.GenerateLabel("<b>Issue ID: "+Issue.id+"</b>, date: "+Issue.date));
                Panel PanelIssueItem = new Panel();
                foreach(book_issue Bs in Issue.book_issue)
                {
                    PanelIssueItem.Controls.Add(ControlUtil.GenerateLabel(Bs.id+" rec: "+Bs.book_record_id+"<br/>"));
                }
                PanelIssue.Controls.Add(PanelIssueItem);
                DateTime MaxReturn = DateUtil.PlusDay(Issue.date, int.Parse(TextBoxDuration.Text));
                bool Late = false;
                if (Issue.date.CompareTo(MaxReturn) > 0)
                {
                    Late = true;
                }
                PanelIssue.Controls.Add(ControlUtil.GenerateLabel("Max return: " + MaxReturn+", Late:"+Late+"<hr/>")) ;
                PanelIssueLis.Controls.Add(PanelIssue);
            }
        }

        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            string ID = TextBoxStudentID.Text;
            Dictionary<string, object> Params = new Dictionary<string, object>();
            Params.Add("student_id", ID);
           List<object> ObjList = IssueService.SearchAdvanced(Params);
            Issues = (List<issue>)ObjectUtil.ConvertList(ObjList, typeof(List<issue>));

            PopulateIssueList();
        }
    }
}