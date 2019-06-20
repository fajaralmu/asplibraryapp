using OurLibrary.Models;
using OurLibrary.Service;
using OurLibrary.Util.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OurLibrary.Web.Stat
{
    public partial class Visit : BasePage
    {
        private StudentService StudentService = new StudentService();
        private VisitService VisitService = new VisitService();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            string ID = TextBoxStudentId.Text;
            PopulateStudentDetail(ID);
        }

        private void PopulateStudentDetail(string Id)
        {

            PanelStudentInfo.Controls.Clear();
            student Std = StudentService.GetByIdFull(Id);
            if (Std != null)
            {
                visit Visit = new visit();
                Visit.student_id = Std.id;
                Visit.date = DateTime.Now;
                Visit.info = "visit";
               visit VisitDB =(visit) VisitService.Add(Visit);
                if (VisitDB != null)
                {
                    Dictionary<string, object> StudentInfo = new Dictionary<string, object>();
                    StudentInfo.Add("Student ID", Std.id);
                    StudentInfo.Add("Student Name", Std.name);

                    if (Std.@class != null)
                    {
                        StudentInfo.Add("Student Class", Std.@class.class_name);
                    }
                    StudentInfo.Add("Visit_No", VisitDB.id.ToString());
                    StudentInfo.Add("Date", VisitDB.date.ToString());
                    PanelStudentInfo.Controls.Add(ControlUtil.GenerateTableFromMap(StudentInfo));
                }
            }
            else
            {
                PanelStudentInfo.Controls.Add(ControlUtil.GenerateLabel( "~<i>Student Not Found</i>"));
            }
        }


    }
}