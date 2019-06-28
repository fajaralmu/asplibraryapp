using OurLibrary.Util.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OurLibrary.Web.API
{
    public partial class VisitInfo : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string json = "";
            Dictionary<string, object> ResponseMap = new Dictionary<string, object>();
            string raw_req = "";

            using (StreamReader reader = new StreamReader(HttpContext.Current.Request.InputStream))
            {
                raw_req = reader.ReadToEnd();
            }
            ResponseMap.Add("RequestBody", raw_req);
            json = JSONUtil.MapToJsonString(ResponseMap);
            Response.Clear();
            Response.ContentType = "application/json; charset=utf-8";
            Response.Write(json);
            Response.End();

        }
    }
}