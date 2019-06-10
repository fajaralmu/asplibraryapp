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
    public partial class Default : BasePage
    {

        private UserService userService = new UserService();
        private user LoggedUser;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] != null && userService.IsValid((user)Session["user"]))
            {
                LoggedUser = (user)Session["user"];
                Response.Redirect("~/Web/Admin/Home.aspx");
            }
            
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string Username = TextBoxUsername.Text;
            string Password = TextBoxPassword.Text;
            LoggedUser = userService.GetUser(Username, Password);
            if (LoggedUser != null)
            {
               Session["user"] = LoggedUser;
               Response.Redirect("~/Web/Admin/Home.aspx");
            }
            
        }
    }
}