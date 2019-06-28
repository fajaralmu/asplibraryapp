using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OurLibrary.Parameter;
using System.Web.UI.WebControls;
using System.Diagnostics;
using OurLibrary.Models;
using OurLibrary.Service;

namespace OurLibrary
{
    public class BasePage : System.Web.UI.Page
    {
        protected static UserService userService = new UserService();
        protected user LoggedUser;
        protected int State = ModelParameter.ADD; //1 add, 2 edit
        protected int Limit = 5, Offset = 0, Total = 0;

        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
        }

        protected override void OnInitComplete(EventArgs e)
        {
            
            base.OnInitComplete(e);

        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
        }

        protected override void OnPreRenderComplete(EventArgs e)
        {
            base.OnPreRenderComplete(e);
        }

        /**NAVIGATION**/
        protected void BtnNavClick(object sender, EventArgs e)
        {
            Button Btn = (Button)sender;
            if (Btn != null)
            {
                string OffsetLimitStr = Btn.CommandArgument;
                string[] OffsetLimit = OffsetLimitStr.Split('~');
                Debug.WriteLine("Offset~Limit:" + OffsetLimitStr);
                if (OffsetLimit.Length == 2)
                {
                    Session[PageParameter.PagingOffset] = Convert.ToInt32(OffsetLimit[0]);
                    Session[PageParameter.PagingLimit] = Convert.ToInt32(OffsetLimit[1]);
                    Limit = (int)Session[PageParameter.PagingLimit];
                    Offset = (int)Session[PageParameter.PagingOffset];
                    UpdateList();
                }
            }
        }

        protected virtual void UpdateList()
        {

        }

        protected void ClearPagingSession(string ObjectName = "")
        {
            Session[PageParameter.PagingLimit] = null;
            Session[PageParameter.PagingOffset] = null;
            if (!ObjectName.Equals(""))
            {
                Session["OrderBy_MNG_" + ObjectName] = null;
                Session["OrderType_MNG_" + ObjectName] = null;
            }
            System.Collections.Specialized.NameObjectCollectionBase.KeysCollection Keys = Session.Keys;
            foreach (string Key in Keys)
            {
               
                if (Key.ToString().Contains("OrderBy_MNG_") ||
                    Key.ToString().Contains("OrderType_MNG_"))
                {
                  //  Session[Key.ToString()] = null;
                }
            }
        }

    }
}