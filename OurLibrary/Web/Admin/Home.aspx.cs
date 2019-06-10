using OurLibrary.Models;
using OurLibrary.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OurLibrary.Web.Admin
{
    public partial class Home : System.Web.UI.Page
    {
        private UserService userService = new UserService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] != null)
            {
                user User = (user)Session["user"];
                Label1.Text = "WELCOME, " + User.name;
            }else
            {
                Response.Redirect("~/Web/Admin/");
            }
        }
    }
}