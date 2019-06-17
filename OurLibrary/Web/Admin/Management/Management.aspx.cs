using OurLibrary.Annotation;
using OurLibrary.Models;
using OurLibrary.Parameter;
using OurLibrary.Service;
using OurLibrary.Util.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OurLibrary.Web.Admin.Management
{
    public partial class CategoryManagement : BasePage
    {
        protected object TheObject;
        protected object Service;
        private List<object> ObjectList = new List<object>();
        private Dictionary<string, object> ServicesMap = new Dictionary<string, object>();
        private Dictionary<string, object> ObjectMap = new Dictionary<string, object>();
        private string ObjectName;

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] == null || !userService.IsValid((user)Session["user"]) || Request.QueryString["object"] == null)
            {
                Response.Redirect("~/Web/Admin/");
            }
            else
            {
                ObjectName = Request.QueryString["object"].ToString();
                if (!ObjectName.Equals(Session[ModelParameter.ObjectName]))
                {
                    ClearPagingSession();
                }
                Session[ModelParameter.ObjectName] = ObjectName;
                LoggedUser = (user)Session["user"];
                InitElement();
            }
        }



        private void InitElement()
        {
            Page.Title = StringUtil.ToUpperCase(0, ObjectName.ToLower()) + " Management";
            LabelObjectName.Text = StringUtil.ToUpperCase(0, ObjectName.ToLower());
            InitServiceClass();
            InitState();
            InitNavigation();
            GenerateForm();
            PopulateListTable();
        }

        private void InitNavigation()
        {
            if (Session[PageParameter.PagingLimit] != null && Session[PageParameter.PagingOffset] != null)
            {
                if (TextBoxLimit.Text != null && TextBoxLimit.Text != "" && Convert.ToInt32(TextBoxLimit.Text) > 0)
                {
                    Debug.WriteLine("LIMIT: " + TextBoxLimit.Text);
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

        private void InitServiceClass()
        {
            Type t = Type.GetType(ModelParameter.ServiceNameSpace + StringUtil.ToUpperCase(0, ObjectName.ToLower()) + "Service");
            if (t == null)
            {
                Response.Redirect("~/Web/Admin/Default.aspx?error=ServiceIsNull");
            }
            Service = Activator.CreateInstance(t);
            Total = (int)Service.GetType().GetMethod("ObjectCount").Invoke(Service, null);
            //

        }

        private void InitState()
        {
            if (Session[ModelParameter.ObjectId] != null)
            {
                string Id = Session[ModelParameter.ObjectId].ToString();
                object[] Params = { Id };
                object DBObject = Service.GetType().GetMethod("GetById").Invoke(Service, Params);
                if (DBObject != null)
                {
                    TheObject = DBObject;
                    State = ModelParameter.EDIT;
                }
            }
        }

        private void GenerateForm()
        {
            TableForm.Rows.Clear();
            Type t = Type.GetType(ModelParameter.NameSpace + ObjectName);
            PropertyInfo[] Props = t.GetProperties();

            for (int i = 0; i < Props.Length; i++)
            {
                PropertyInfo PropsInfo = Props[i];
                object[] Attributes = PropsInfo.GetCustomAttributes(typeof(FieldAttribute), true);
                if (Attributes.Length > 0)
                {
                    FieldAttribute Attribute = (FieldAttribute)Attributes[0];
                    if (Attribute.FieldType != null)
                    {
                        string FieldName = Attribute.FieldName != null && Attribute.FieldName != "" ? Attribute.FieldName : PropsInfo.Name;

                        //CREATE ROW
                        TableRow TRow = new TableRow();
                        TableCell LabelCell = new TableCell();
                        TableCell FieldCell = new TableCell();
                        TableCell ValidatorCell = new TableCell();

                        //LABEL
                        Label FieldNameLabel = new Label();
                        FieldNameLabel.Text = FieldName;
                        LabelCell.ID = "Label_Cell" + PropsInfo.Name;
                        LabelCell.Controls.Add(FieldNameLabel);

                        //INPUT CONTROL
                        Control FieldControl = new Control();

                        switch (Attribute.FieldType)
                        {
                            case AttributeConstant.TYPE_ID_STR_NUMB:
                            case AttributeConstant.TYPE_ID_NUMB:
                                if (Attribute.AutoGenerated)
                                {
                                    FieldControl = new TextBox();
                                    FieldControl.ID = PropsInfo.Name;
                                    ((TextBox)FieldControl).TextMode = TextBoxMode.SingleLine;
                                    ((TextBox)FieldControl).Text = "AUTO-GENERATED";
                                    ((TextBox)FieldControl).CssClass = "form-control";
                                    ((TextBox)FieldControl).Enabled = false;
                                }
                                FieldCell.Controls.Add(FieldControl);
                                break;
                            case AttributeConstant.TYPE_NUMBER:
                                FieldControl = new TextBox();
                                FieldControl.ID = PropsInfo.Name;
                                ((TextBox)FieldControl).TextMode = TextBoxMode.Number;
                                ((TextBox)FieldControl).CssClass = "form-control";
                                FieldCell.Controls.Add(FieldControl);
                                break;
                            case AttributeConstant.TYPE_TEXTBOX:
                                FieldControl = new TextBox();
                                FieldControl.ID = PropsInfo.Name;
                                ((TextBox)FieldControl).TextMode = TextBoxMode.SingleLine;
                                ((TextBox)FieldControl).CssClass = "form-control";
                                FieldCell.Controls.Add(FieldControl);
                                break;
                            case AttributeConstant.TYPE_TEXTAREA:
                                FieldControl = new TextBox();
                                FieldControl.ID = PropsInfo.Name;
                                ((TextBox)FieldControl).TextMode = TextBoxMode.MultiLine;
                                ((TextBox)FieldControl).CssClass = "form-control";
                                FieldCell.Controls.Add(FieldControl);
                                break;
                            case AttributeConstant.TYPE_DATE:
                                FieldControl = new TextBox();
                                FieldControl.ID = PropsInfo.Name;
                                ((TextBox)FieldControl).TextMode = TextBoxMode.Date;
                                ((TextBox)FieldControl).CssClass = "form-control";
                                FieldCell.Controls.Add(FieldControl);
                                break;
                            case AttributeConstant.TYPE_FILE_IMAGE:
                                FieldControl = new FileUpload();
                                FieldControl.ID = PropsInfo.Name;
                                Label LabelForImage = new Label();
                                LabelForImage.Text = "Current Image: ";
                                Image ImageControl = new Image();
                                ImageControl.Visible = false;
                                ImageControl.Width = 150;
                                ImageControl.Height = 200;
                                ImageControl.ID = PropsInfo.Name + "_IMG_VIEW";
                                LabelForImage.Controls.Add(ImageControl);

                                ((FileUpload)FieldControl).Controls.Add(LabelForImage);
                                ((FileUpload)FieldControl).CssClass = "form-control";
                                FieldCell.Controls.Add(FieldControl);
                                break;
                            case AttributeConstant.TYPE_SEARCHLIST:
                                FieldControl = new TextBox();
                                FieldControl.ID = PropsInfo.Name;

                                Label LabelForCurrentObj = new Label();
                                LabelForCurrentObj.Text = "<br/>Current " + FieldName + " : ";
                                Label LabelValueObj = new Label();
                                LabelValueObj.ID = "LABEL_VALUE_" + PropsInfo.Name;
                                LabelCell.Controls.Add(LabelForCurrentObj);
                                LabelCell.Controls.Add(LabelValueObj);

                                Panel PanelForList = new Panel();
                                PanelForList.ID = "PANEL_LIST_" + PropsInfo.Name;
                                Button ButtonSearch = new Button();
                                ButtonSearch.Text = "Search by " + Attribute.ClassAttributeConverter;
                                ButtonSearch.CausesValidation = false;
                                ButtonSearch.CssClass = "btn btn-primary";
                                string attrs = "";
                                for (int y = 0; y < Attribute.AttrToDisplay.Length; y++)
                                {
                                    attrs += Attribute.AttrToDisplay[y];
                                    if (y < Attribute.AttrToDisplay.Length - 1)
                                    {
                                        attrs += ",";
                                    }
                                }
                                ButtonSearch.CommandArgument = PropsInfo.Name + "~" + Attribute.ClassReference + "~" + Attribute.ClassAttributeConverter + "|" + attrs;
                                ButtonSearch.Click += new EventHandler(SearchObjectList);

                                //CHECK CURRENT OBJ FROM QUERY STRING
                                string objIdFromReq = Request.QueryString["_OBJ_" + Attribute.ClassReference + "_PROP_" + PropsInfo.Name];
                                if (objIdFromReq != null && objIdFromReq != "")
                                {
                                    if (objIdFromReq.Contains(","))
                                    {
                                        objIdFromReq = objIdFromReq.Split(',')[0];
                                    }
                                    object ObjClassService = null;
                                    string objClassReferenceAttribute = Attribute.ClassAttributeConverter;
                                    if (!ServicesMap.ContainsKey(Attribute.ClassReference))
                                    {
                                        Type serviceClassRefference = Type.GetType(ModelParameter.ServiceNameSpace + StringUtil.ToUpperCase(0, Attribute.ClassReference.ToLower()) + "Service");
                                        ObjClassService = Activator.CreateInstance(serviceClassRefference);
                                        ServicesMap.Add(Attribute.ClassReference, ObjClassService);
                                    }
                                    else
                                    {
                                        ServicesMap.TryGetValue(Attribute.ClassReference, out ObjClassService);
                                    }
                                    object[] objParams = { objIdFromReq };
                                    object obj = ObjClassService.GetType().GetMethod("GetById").Invoke(ObjClassService, objParams);
                                    if (obj != null)
                                    {
                                        string objPropVal = (string)obj.GetType().GetProperty(Attribute.ClassAttributeConverter).GetValue(obj);
                                        ((TextBox)FieldControl).Text = objPropVal;
                                        LabelValueObj.Text = ((TextBox)FieldControl).Text;
                                    }

                                }

                                FieldCell.Controls.Add(FieldControl);
                                FieldCell.Controls.Add(ButtonSearch);
                                FieldCell.Controls.Add(PanelForList);
                                break;
                            case AttributeConstant.TYPE_DROPDOWN:
                                FieldControl = new DropDownList();
                                FieldControl.ID = PropsInfo.Name;
                                ((DropDownList)FieldControl).CssClass = "form-control";

                                if (Attribute.DropDownValues != null && Attribute.DropDownItemName != null)
                                {
                                    for (int v = 0;v< Attribute.DropDownValues.Length; v++)
                                    {
                                        ListItem Item = new ListItem(Attribute.DropDownItemName[v].ToString(), Attribute.DropDownValues[v].ToString());
                                        ((DropDownList)FieldControl).Items.Add(Item);
                                    }
                                    FieldCell.Controls.Add(FieldControl);
                                    break;
                                }
                                object ClassService = null;
                                string classReferenceAttribute = Attribute.ClassAttributeConverter;
                                if (!ServicesMap.ContainsKey(Attribute.ClassReference))
                                {
                                    Type serviceClassRefference = Type.GetType(ModelParameter.ServiceNameSpace + StringUtil.ToUpperCase(0, Attribute.ClassReference.ToLower()) + "Service");
                                    ClassService = Activator.CreateInstance(serviceClassRefference);
                                    ServicesMap.Add(Attribute.ClassReference, ClassService);
                                }
                                else
                                {
                                    ServicesMap.TryGetValue(Attribute.ClassReference, out ClassService);
                                }
                                object[] Params = { 0, 100 };
                                List<object> ObjList = (List<object>)ClassService.GetType().GetMethod("ObjectList").Invoke(ClassService, Params);
                               
                                if (ObjectList != null)
                                    foreach (object obj in ObjList)
                                    {
                                        string IdField = ObjectUtil.GetIDProps(ModelParameter.NameSpace + Attribute.ClassReference);
                                        string Value = (string)obj.GetType().GetProperty(IdField).GetValue(obj);
                                        string Text = (string)obj.GetType().GetProperty(classReferenceAttribute).GetValue(obj);
                                        ListItem Item = new ListItem(Text.TrimEnd(), Value.TrimEnd());
                                        ((DropDownList)FieldControl).Items.Add(Item);
                                    }
                                FieldCell.Controls.Add(FieldControl);
                                break;

                        }



                        if (Attribute.Required == true)
                        {
                            RequiredFieldValidator Validator = new RequiredFieldValidator();
                            Validator.ControlToValidate = PropsInfo.Name;
                            Validator.ForeColor = System.Drawing.Color.Red;
                            Validator.ErrorMessage = FieldName + " Required";
                            ValidatorCell.Controls.Add(Validator);
                        }
                        TRow.Controls.Add(LabelCell);
                        TRow.Controls.Add(FieldCell);
                        TRow.Controls.Add(ValidatorCell);

                        TableForm.Controls.Add(TRow);
                    }

                    if (Attribute.ClassReference != null)
                    {
                        Type refType = Type.GetType(ModelParameter.NameSpace + Attribute.ClassReference);
                        string refConverter = Attribute.ClassAttributeConverter;
                        PropertyInfo AttributeConverter = refType.GetProperty(refConverter);
                    }
                }
            }

        }

        private void SearchObjectList(object sender, EventArgs e)
        {
            Button Btn = (Button)sender;
            if (Btn != null)
            {
                string RawArgs = Btn.CommandArgument;
                string[] Args = RawArgs.Split('|');
                string[] PropAgrs = Args[0].Split('~');
                string[] FieldsToShow = Args[1].Split(',');
                string PropName = PropAgrs[0];
                string ClassName = PropAgrs[1];
                string ClassKey = PropAgrs[2];

                //SERVICE
                string classReferenceAttribute = ClassName;
                object ClassService = null;
                if (!ServicesMap.ContainsKey(ClassName))
                {
                    Type serviceClassRefference = Type.GetType(ModelParameter.ServiceNameSpace + StringUtil.ToUpperCase(0, ClassName.ToLower()) + "Service");
                    ClassService = Activator.CreateInstance(serviceClassRefference);
                    ServicesMap.Add(ClassName, ClassService);
                }
                else
                {
                    ServicesMap.TryGetValue(ClassName, out ClassService);
                }
                // object[] Params = { 0, 100 };
                Dictionary<string, object> MapParam = new Dictionary<string, object>();
                string Value = ((TextBox)PanelForm.FindControl(PropName)).Text;
                MapParam.Add(ClassKey, Value);
                object[] Params = { MapParam, 0, 100 };
                List<object> ObjList = (List<object>)ClassService.GetType().GetMethod("SearchAdvanced").Invoke(ClassService, Params);

                ((Panel)PanelForm.FindControl("PANEL_LIST_" + PropName)).Controls.Clear();

                foreach (object o in ObjList)
                {
                    Button btnObj = new Button();
                    btnObj.UseSubmitBehavior = false;
                    btnObj.CausesValidation = false;
                    string BtnText = "";
                    for (int i = 0; i < FieldsToShow.Length; i++)
                    {
                        string label = FieldsToShow[i];

                        if (label.Contains(">"))
                        {
                            string[] props = label.Split('>');
                            string propName = props[0];
                            string fieldFromProp = props[1];
                            object propObj = o.GetType().GetProperty(propName).GetValue(o);
                            object propVal = propObj.GetType().GetProperty(fieldFromProp).GetValue(propObj);
                            BtnText += propName + ":" + propVal;
                        }
                        else
                        {
                            object val = o.GetType().GetProperty(label).GetValue(o);
                            BtnText += label + ":" + val;
                        }

                        if (i < FieldsToShow.Length - 1)
                        {
                            BtnText += "|";
                        }
                    }

                    btnObj.Text = BtnText;
                    string idField = ObjectUtil.GetIDProps(ModelParameter.NameSpace + ClassName);
                    object idVal = o.GetType().GetProperty(idField).GetValue(o);
                    btnObj.CommandArgument = (string)idVal + "~" + PropName;
                    btnObj.PostBackUrl = StringUtil.AddURLParam(Request.Url.AbsoluteUri, "_OBJ_" + ClassName + "_PROP_" + PropName, idVal);
                    //"~/Web/Admin/Management/Management.aspx?object="+this.ObjectName+"&Obj_" + ClassName+"_PROP_"+PropName + "=" + idVal;
                    btnObj.Click += new EventHandler(BtnObjClick);
                    btnObj.CssClass = "btn btn-success";
                    btnObj.ID = "BTN_OBJLIST_" + ClassName + idVal;
                    ((Panel)PanelForm.FindControl("PANEL_LIST_" + PropName)).Controls.Add(btnObj);
                }

                if (ObjList == null || ObjList.Count == 0)
                {
                    ((Panel)PanelForm.FindControl("PANEL_LIST_" + PropName)).Controls.Add(ControlUtil.GenerateLabel(ClassName + " not found", System.Drawing.Color.Orange));
                }
            }
        }

        private void BtnObjClick(object sender, EventArgs e)
        {
            Button Btn = (Button)sender;
            if (Btn != null)
            {
                string[] args = Btn.CommandArgument.Split('~');
                ((Label)PanelForm.FindControl("LABEL_VALUE_")).Text = args[0] + " " + args[1];
            }
        }

        private string UploadFile(FileUpload fileUploadControl)
        {
            if (fileUploadControl.HasFile)
            {
                try
                {
                    string filename = DateTime.Now.ToLongDateString().Replace(" ", "") + DateTime.Now.ToLongTimeString().Replace(":", "") + Path.GetFileName(fileUploadControl.FileName);
                    fileUploadControl.SaveAs(Server.MapPath("~/Assets/Image/" + StringUtil.ToUpperCase(0, ObjectName.ToLower())) + "/" + filename);
                    // StatusLabel.Text = "Upload status: File uploaded!";
                    return filename;
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                    return null;
                    // StatusLabel.Text = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message;
                }
            }
            return null;
        }

        private void PopulateListTable()
        {
            TableList.Rows.Clear();

            //HEADER//
            TableRow HeaderRow = new TableRow();
            TableCell NoCell = new TableCell();
            NoCell.Text = "No";
            HeaderRow.TableSection = TableRowSection.TableHeader;
            HeaderRow.Controls.Add(NoCell);
            Type t = Type.GetType(ModelParameter.NameSpace + ObjectName);
            PropertyInfo[] Props = t.GetProperties();
            for (int i = 0; i < Props.Length; i++)
            {
                PropertyInfo PropsInfo = Props[i];
                object[] Attributes = PropsInfo.GetCustomAttributes(typeof(FieldAttribute), true);
                if (Attributes.Length > 0)
                {
                    FieldAttribute Attribute = (FieldAttribute)Attributes[0];
                    if (Attribute.FieldType != null)
                    {
                        string FieldName = Attribute.FieldName != null && Attribute.FieldName != "" ? Attribute.FieldName : PropsInfo.Name;

                        TableCell HeaderCell = new TableCell();
                        HeaderCell.Text = StringUtil.ToUpperCase(0, FieldName.ToLower());
                        HeaderRow.Controls.Add(HeaderCell);
                    }
                }
            }
            TableCell OptionCell = new TableCell();
            OptionCell.Text = "Option";
            HeaderRow.Controls.Add(OptionCell);
            HeaderRow.Font.Bold = true;
            HeaderRow.Font.Size = FontUnit.Larger;
            //   HeaderRow.ForeColor = System.Drawing.Color.BlanchedAlmond;
            TableList.Controls.Add(HeaderRow);
            //BODY//
            ObjectList.Clear();
            object[] Params = { Offset, Limit };
            ObjectList = (List<object>)Service.GetType().GetMethod("ObjectList").Invoke(Service, Params);
            int No = Offset * Limit;
            foreach (object obj in ObjectList)
            {
                No++;
                string NoStr = Convert.ToString(No);
                TableRow TRow = new TableRow();
                TRow.TableSection = TableRowSection.TableBody;
                TableCell TCellNo = new TableCell();
                TableCell TCellOption = new TableCell();
                Button EditButton = new Button();
                Button DeleteButton = new Button();

                EditButton.Text = "edit";
                EditButton.CssClass = "btn btn-warning";
                
                EditButton.CausesValidation = false;
                EditButton.UseSubmitBehavior = false;
                //  EditButton.PostBackUrl = URL + "?id=" + p.id;
                //EditButton.CommandArgument = obj.id;


                DeleteButton.Text = "delete";
                DeleteButton.CssClass = "btn btn-danger";
                DeleteButton.CausesValidation = false;
                DeleteButton.UseSubmitBehavior = false;
                //DeleteButton.CommandArgument = obj.id;


                TCellNo.Controls.Add(ControlUtil.GenerateLabel(NoStr));
                //option

                TRow.Controls.Add(TCellNo);

                for (int i = 0; i < Props.Length; i++)
                {
                    PropertyInfo PropsInfo = Props[i];
                    object[] Attributes = PropsInfo.GetCustomAttributes(typeof(FieldAttribute), true);
                    if (Attributes.Length > 0)
                    {
                        FieldAttribute Attribute = (FieldAttribute)Attributes[0];
                        if (Attribute.FieldType != null)
                        {
                            //CREATE ROW
                            TableCell FieldCell = new TableCell();

                            //LABEL
                            Label FieldLabel = new Label();
                            object PropValue = obj.GetType().GetProperty(PropsInfo.Name).GetValue(obj);
                            
                            if (Attribute.ClassReference != null && Attribute.ClassAttributeConverter != null)
                            {
                                object ClassService = null;
                                string classReferenceAttribute = Attribute.ClassAttributeConverter;
                                if (!ServicesMap.ContainsKey(Attribute.ClassReference))
                                {
                                    Type serviceClassRefference = Type.GetType(ModelParameter.ServiceNameSpace + StringUtil.ToUpperCase(0, Attribute.ClassReference.ToLower()) + "Service");
                                    ClassService = Activator.CreateInstance(serviceClassRefference);
                                    ServicesMap.Add(Attribute.ClassReference, ClassService);
                                }
                                else
                                {
                                    ServicesMap.TryGetValue(Attribute.ClassReference, out ClassService);
                                }
                                object[] Params2 = { PropValue };
                                object ClassReff = ClassService.GetType().GetMethod("GetById").Invoke(ClassService, Params2);
                                object ClassRefConverterValue = ClassReff.GetType().GetProperty(Attribute.ClassAttributeConverter).GetValue(ClassReff);
                                FieldLabel.Text = ClassRefConverterValue.ToString();

                            }else
                            if (Attribute.DropDownItemName != null && Attribute.DropDownValues != null && PropValue != null
                                && Attribute.DropDownItemName.Length > 0 && Attribute.DropDownValues.Length > 0)
                            {
                                string index = PropValue.ToString();
                                int intindex = 0;
                                int.TryParse(index, out intindex);
                                FieldLabel.Text = Attribute.DropDownItemName[intindex].ToString();
                            }
                            else
                            {
                                if (PropValue != null)
                                    FieldLabel.Text = PropValue.ToString();
                            }
                            TRow.Controls.Add(FieldCell);
                            if (Attribute.FieldType.Contains("id_"))
                            {
                                EditButton.CommandArgument = PropValue.ToString();
                                DeleteButton.CommandArgument = PropValue.ToString();
                                EditButton.Click += new EventHandler(BtnEditClick);
                                EditButton.Text = "Edit";
                                DeleteButton.Click += new EventHandler(BtnDeleteClick);
                            }
                            else if (Attribute.FieldType.Equals(AttributeConstant.TYPE_SEARCHLIST))
                            {
                                string currentURI = Request.Url.AbsoluteUri;
                                string param = "_OBJ_" + Attribute.ClassReference + "_PROP_" + PropsInfo.Name;
                                object value = obj.GetType().GetProperty(PropsInfo.Name).GetValue(obj);
                                EditButton.PostBackUrl = StringUtil.AddURLParam(currentURI, param, value);
                            }

                            FieldCell.Controls.Add(FieldLabel);
                            TRow.Controls.Add(FieldCell);

                        }

                        /*if (Attribute.ClassReference != null)
                        {
                            Type refType = Type.GetType(ModelParameter.NameSpace + Attribute.ClassReference);
                            string refConverter = Attribute.ClassAttributeConverter;
                            PropertyInfo AttributeConverter = refType.GetProperty(refConverter);

                        }*/
                    }

                }
                TCellOption.Controls.Add(EditButton);
                TCellOption.Controls.Add(DeleteButton);

                TRow.Controls.Add(TCellOption);
                TableList.Controls.Add(TRow);

            }
        }

        protected override void UpdateList()
        {
            nav_info.InnerText = "Offset:" + Offset + ", Limit:" + Limit + ", from updateList";
            UpdateNavigation();
            PopulateListTable();
        }

        protected void UpdateNavigation()
        {
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

        private object UpdateObjectBasedOnInput(bool Update)
        {

            Type t = Type.GetType(ModelParameter.NameSpace + ObjectName);
            PropertyInfo[] Props = t.GetProperties();
            if (TheObject == null)
            {
                TheObject = Activator.CreateInstance(t);
            }


            for (int i = 0; i < Props.Length; i++)
            {
                bool updateValue = true;
                PropertyInfo PropsInfo = Props[i];
                object[] Attributes = PropsInfo.GetCustomAttributes(typeof(FieldAttribute), true);
                if (Attributes.Length > 0)
                {
                    FieldAttribute Attribute = (FieldAttribute)Attributes[0];
                    bool isNumber = false;
                    if (Attribute.FieldType != null)
                    {
                        if (Attribute.FieldType.Contains("id_") && Update)
                        {
                            continue;
                        }

                        object InputValue = null;
                        if (Attribute.AutoGenerated)
                        {
                            int size = Attribute.FixSize != null && Attribute.FixSize > 0 ? Attribute.FixSize : 5;
                            if (Attribute.FieldType.Equals(AttributeConstant.TYPE_ID_NUMB))
                            {
                                InputValue = StringUtil.GenerateRandomNumber(size);
                            }
                            else
                            {
                                InputValue = StringUtil.GenerateRandomChar(size);
                            }
                           
                        }
                        else
                        {
                            if (Attribute.FieldType.Equals(AttributeConstant.TYPE_DROPDOWN))
                            {
                                DropDownList DropDown = (DropDownList)TableForm.FindControl(PropsInfo.Name);
                                InputValue = DropDown.SelectedValue.TrimEnd();
                               if( PropsInfo.PropertyType == typeof(Nullable<short>))
                                {
                                    string strInt = InputValue.ToString();
                                    short intVal = 0;
                                    short.TryParse(strInt, out intVal);
                                    InputValue = intVal;
                                }
                            }
                            else if (Attribute.FieldType.Equals(AttributeConstant.TYPE_NUMBER))
                            {
                                TextBox InputControl = (TextBox)TableForm.FindControl(PropsInfo.Name);
                                if (InputControl.Text != null && !InputControl.Text.Equals(""))
                                    InputValue = Convert.ToInt32(InputControl.Text.TrimEnd());
                            }
                            else if (Attribute.FieldType.Equals(AttributeConstant.TYPE_FILE_IMAGE))
                            {
                                FileUpload InputControl = (FileUpload)TableForm.FindControl(PropsInfo.Name);
                                InputValue = UploadFile(InputControl);
                                if (Update && InputValue == null)
                                {
                                    updateValue = false;
                                }

                            }
                            else if (Attribute.FieldType.Equals(AttributeConstant.TYPE_SEARCHLIST))
                            {
                                string objIdFromReq = Request.QueryString["_OBJ_" + Attribute.ClassReference + "_PROP_" + PropsInfo.Name];
                                if (objIdFromReq != null && objIdFromReq != "")
                                {
                                    if (objIdFromReq.Contains(","))
                                    {
                                        InputValue = objIdFromReq.Split(',')[0];
                                    }else
                                    {
                                        InputValue = objIdFromReq;
                                    }
                                }
                                else
                                {
                                    return null;
                                }
                            }
                            else
                            {
                                TextBox InputControl = (TextBox)TableForm.FindControl(PropsInfo.Name);
                                InputValue = InputControl.Text.TrimEnd();
                            }
                        }
                        if (updateValue)
                            TheObject.GetType().GetProperty(PropsInfo.Name).SetValue(TheObject, InputValue);


                    }
                }
            }
            return TheObject;
        }

        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            string status = "";
            object OBJ = null;
            if (State == ModelParameter.ADD && ModelState.IsValid && userService.IsValid(LoggedUser))
            {
                OBJ = UpdateObjectBasedOnInput(false);
                if (OBJ == null)
                    goto Fail;
                object[] Params = { TheObject };
                object NewObj = Service.GetType().GetMethod("Add").Invoke(Service, Params);

                
                if (NewObj != null)
                {
                    // status = "Success Adding New Publisher: " + NewPublisher.name + "(" + NewPublisher.id + ")";
                    ClearField();
                    InitElement();
                    status = "Success Adding New Object";
                }
                else
                {
                    status = "Failed Adding New Object";
                }

            }
            else if (TheObject != null && State == ModelParameter.EDIT && ModelState.IsValid && userService.IsValid(LoggedUser))
            {
                OBJ = UpdateObjectBasedOnInput(true);
                if (OBJ == null)
                    goto Fail;
                object[] Params = { TheObject };
                object UpdateObject = Service.GetType().GetMethod("Update").Invoke(Service, Params);
                
                if (UpdateObject != null)
                {
                    ClearField();
                    InitElement();
                    status = "Success Updating Object";
                }
                else
                {
                    status = "Failed Updating Object";
                }
            }
            Fail:
            if(OBJ == null)
                status = "Failed Adding New Object";
            LabelStatus.Text = status;
            ButtonReset_Click(sender, e);
            Session[ModelParameter.ObjectId] = null;
            Session.Remove(ModelParameter.ObjectId);
        }

        protected void ButtonReset_Click(object sender, EventArgs e)
        {
            ClearField();
            State = ModelParameter.ADD;
            Session[ModelParameter.ObjectId] = null;
            UpdateList();
        }

        private void BtnEditClick(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("BtnEdit");
            Button Btn = (Button)sender;
            if (Btn != null)
            {
                string Id = Btn.CommandArgument;
                object[] Params = { Id };
                object DBObject = Service.GetType().GetMethod("GetById").Invoke(Service, Params);
                if (DBObject != null)
                {
                    string IdField = ObjectUtil.GetIDProps(ModelParameter.NameSpace + ObjectName);
                    Session[ModelParameter.ObjectId] = DBObject.GetType().GetProperty(IdField).GetValue(DBObject);
                    State = ModelParameter.EDIT;
                    PopulateField(DBObject);
                    //  InitElement();

                }
            }
        }

        private void BtnDeleteClick(object sender, EventArgs e)
        {
            //  Request.Params["id"]
            Button Btn = (Button)sender;
            if (Btn != null)
            {
                string Id = Btn.CommandArgument;
                object[] Params = { Id };
                object DBObject = Service.GetType().GetMethod("GetById").Invoke(Service, Params);
                if (DBObject != null)
                {
                    string IdField = ObjectUtil.GetIDProps(ModelParameter.NameSpace + ObjectName);
                    Session[ModelParameter.ObjectId] = DBObject.GetType().GetProperty(IdField).GetValue(DBObject);
                    State = ModelParameter.EDIT;
                    LabelNameToDelete.Text = Session[ModelParameter.ObjectId].ToString();
                    PanelConfirmDelete.Visible = true;
                    PanelForm.Visible = false;
                }
            }
        }

        protected void ButtonConfirmDelete_Click(object sender, EventArgs e)
        {
            if (State == ModelParameter.EDIT && TheObject != null)
            {
                object[] Params = { TheObject };
                Service.GetType().GetMethod("Delete").Invoke(Service, Params);
                // publisherService.DeletePublisher(Publisher);
                ButtonCancelDelete_Click(sender, e);
            }
        }

        protected void ButtonCancelDelete_Click(object sender, EventArgs e)
        {
            PanelForm.Visible = true;
            PanelConfirmDelete.Visible = false;
            Session[ModelParameter.ObjectId] = null;
            ButtonReset_Click(sender, e);

        }

        private void PopulateField(object Object)
        {
            ClearField();
            PropertyInfo[] Props = Object.GetType().GetProperties();

            for (int i = 0; i < Props.Length; i++)
            {
                PropertyInfo PropsInfo = Props[i];
                object[] Attributes = PropsInfo.GetCustomAttributes(typeof(FieldAttribute), true);
                if (Attributes.Length > 0)
                {
                    FieldAttribute Attribute = (FieldAttribute)Attributes[0];
                    if (Attribute.FieldType != null)
                    {
                        // string InputValue;
                        if (Attribute.AutoGenerated)
                        {
                            int size = Attribute.FixSize != null && Attribute.FixSize > 0 ? Attribute.FixSize : 5;
                            // InputValue = StringUtil.GenerateRandom(size);
                        }
                        else
                        {
                            if (Attribute.FieldType.Equals(AttributeConstant.TYPE_DROPDOWN))
                            {
                                DropDownList DropDown = (DropDownList)TableForm.FindControl(PropsInfo.Name);
                                object Value = ObjectUtil.GetValueFromProp(PropsInfo.Name, Object);
                                DropDown.SelectedValue = Value.ToString().TrimEnd();
                                DropDownList d2 = DropDown;

                            }
                            else if (Attribute.FieldType.Equals(AttributeConstant.TYPE_FILE_IMAGE))
                            {
                                object Value = ObjectUtil.GetValueFromProp(PropsInfo.Name, Object);

                                Image ImageView = (Image)TableForm.FindControl(PropsInfo.Name + "_IMG_VIEW");
                                ImageView.ImageUrl = "~/Assets/Image/" + StringUtil.ToUpperCase(0, ObjectName) + "/" + Value;
                                ImageView.Visible = Value != null && Value.GetType() == typeof(string) && !Value.Equals("");
                            }else if (Attribute.FieldType.Equals(AttributeConstant.TYPE_SEARCHLIST))
                            {
                                string objIdFromReq = Request.QueryString["_OBJ_" + Attribute.ClassReference + "_PROP_" + PropsInfo.Name];
                                if (objIdFromReq != null && objIdFromReq != "")
                                {
                                    if (objIdFromReq.Contains(","))
                                    {
                                        objIdFromReq = objIdFromReq.Split(',')[0];
                                    }
                                    object ObjClassService = null;
                                    string objClassReferenceAttribute = Attribute.ClassAttributeConverter;
                                    if (!ServicesMap.ContainsKey(Attribute.ClassReference))
                                    {
                                        Type serviceClassRefference = Type.GetType(ModelParameter.ServiceNameSpace + StringUtil.ToUpperCase(0, Attribute.ClassReference.ToLower()) + "Service");
                                        ObjClassService = Activator.CreateInstance(serviceClassRefference);
                                        ServicesMap.Add(Attribute.ClassReference, ObjClassService);
                                    }
                                    else
                                    {
                                        ServicesMap.TryGetValue(Attribute.ClassReference, out ObjClassService);
                                    }
                                    object[] objParams = { objIdFromReq };
                                    object obj = ObjClassService.GetType().GetMethod("GetById").Invoke(ObjClassService, objParams);
                                    if (obj != null)
                                    {
                                        string objPropVal = (string)obj.GetType().GetProperty(Attribute.ClassAttributeConverter).GetValue(obj);
                                        ((TextBox)PanelForm.FindControl(PropsInfo.Name)).Text = objPropVal;
                                        ((Label)PanelForm.FindControl("LABEL_VALUE_"+PropsInfo.Name)).Text = objPropVal;
                                    }

                                }
                            }
                            else
                            {
                                TextBox InputControl = (TextBox)TableForm.FindControl(PropsInfo.Name);
                                object PropValue = ObjectUtil.GetValueFromProp(PropsInfo.Name, Object);
                                if (PropValue != null)
                                    InputControl.Text = PropValue.ToString().TrimEnd();
                            }
                        }


                    }
                }
            }
        }

        private void ClearField()
        {
            Type t = Type.GetType(ModelParameter.NameSpace + ObjectName);
            TheObject = Activator.CreateInstance(t);
            PropertyInfo[] Props = t.GetProperties();

            for (int i = 0; i < Props.Length; i++)
            {
                PropertyInfo PropsInfo = Props[i];
                object[] Attributes = PropsInfo.GetCustomAttributes(typeof(FieldAttribute), true);
                if (Attributes.Length > 0)
                {
                    FieldAttribute Attribute = (FieldAttribute)Attributes[0];
                    if (Attribute.FieldType != null)
                    {

                        {
                            if (!Attribute.FieldType.Equals(AttributeConstant.TYPE_DROPDOWN) && !Attribute.FieldType.Equals(AttributeConstant.TYPE_FILE_IMAGE))

                            {
                                TextBox InputControl = (TextBox)TableForm.FindControl(PropsInfo.Name);
                                if (Attribute.AutoGenerated)
                                {
                                    InputControl.Text = "AUTO-GENERATED";
                                }
                                else
                                {
                                    InputControl.Text = "";
                                }

                                InputControl.Controls.Clear();
                            }
                            else if (Attribute.FieldType.Equals(AttributeConstant.TYPE_FILE_IMAGE))
                            {
                                Image InputControl = (Image)TableForm.FindControl(PropsInfo.Name + "_IMG_VIEW");
                                InputControl.Visible = false;

                            }
                        }

                    }
                }
            }
        }

        protected void ButtonToggleForm_Click(object sender, EventArgs e)
        {
            PanelForm.Visible = !PanelForm.Visible;
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
    }
}