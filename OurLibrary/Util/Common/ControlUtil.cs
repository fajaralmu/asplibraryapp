using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace OurLibrary.Util.Common
{
    public class ControlUtil
    {
        public static Label GenerateLabel(string Text, Color foreColor )
        {
            Label GenLabel = new Label();
            GenLabel.ForeColor = foreColor;
            GenLabel.Text = Text;
            return GenLabel;
        }
        public static Label GenerateLabel(string Text)
        {
            return GenerateLabel(Text, Color.Black);
        }
    }
}