using OurLibrary.Models;
using OurLibrary.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OurLibrary.Web.API
{
    public partial class CheckUser : System.Web.UI.Page
    {
        private UserService User_Service = new UserService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.RequestType.Equals("POST"))
            {
                string Username = Request.Form["username"];
                string Password = Request.Form["password"];

                user User = User_Service.GetUser(Username, Password);
                bool Login = false;
                if (User != null)
                {
                    Login = true;
                }
                Response.Clear();
                Response.ContentType = "application/json; charset=utf-8";
                Response.Write("{ " +
                   (Login ?
                    "\"id\":\"" + User.id + "\"," +
                    "\"username\":\"" + User.username + "\"," +
                    "\"name\":\"" + User.name + "\"," +
                     "\"password\":\"" + User.password + "\","
                    : "")+
                    " \"login\":" + Login.ToString().ToLower() + "}");
                Response.End();
            }
            else
            {
                return;
            }
        }
    }
}