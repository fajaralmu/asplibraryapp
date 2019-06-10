using OurLibrary.Models;
using OurLibrary.Parameter;
using OurLibrary.Service;
using OurLibrary.Util.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using OurLibrary.Parameter;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OurLibrary.Web.Admin.Management
{
    public partial class PublisherManagement : BasePage
    {

        private string URL = HttpContext.Current.Request.Url.AbsolutePath;
        private PublisherServiceOLD publisherService = new PublisherServiceOLD();
        private publisher Publisher;
       
        private List<publisher> PublisherList = new List<publisher>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] == null || !userService.IsValid((user)Session["user"]))
            {
                Response.Redirect("~/Web/Admin/");
            }
            else
            {
                LoggedUser = (user)Session["user"];
                Total = publisherService.PublisherCount();
                if (Session[ModelParameter.PublisherId] != null)
                {
                    string Id = Session[ModelParameter.PublisherId].ToString();
                    publisher DBPublisher = publisherService.GetPublisherById(Id);
                    if (DBPublisher != null)
                    {
                        Publisher = DBPublisher;
                        State = ModelParameter.EDIT;
                    }
                }


                InitElement();
            }
        }

        private void InitElement()
        {
            //Session.Clear();
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
            PopulateListTable();

            LabelState.Text = "REQ: " + Request.HttpMethod + " state: " + (State == 1 ? "ADD" : "EDIT") + " publisher name:" + (Publisher == null ? "NULL" : Publisher.name);

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

        private void BtnEditClick(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("BtnEdit");
            Button Btn = (Button)sender;
            if (Btn != null)
            {
                string Id = Btn.CommandArgument;
                publisher DBPublisher = publisherService.GetPublisherById(Id);
                if (DBPublisher != null)
                {
                    Session[ModelParameter.PublisherId] = DBPublisher.id;
                    State = ModelParameter.EDIT;
                    PopulateField(DBPublisher);
                    //  InitElement();
                    FormTitle.InnerText = "Edit Form: "+ DBPublisher.name;
                    LabelState.Text = "REQ: " + Request.HttpMethod + " state: " + (State == 1 ? "ADD" : "EDIT") + " publisher name:" + (Publisher == null ? "NULL" : Publisher.name);

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
                publisher DBPublisher = publisherService.GetPublisherById(Id);
                if (DBPublisher != null)
                {
                    Session[ModelParameter.PublisherId] = DBPublisher.id;
                    State = ModelParameter.EDIT;
                    LabelNameToDelete.Text = DBPublisher.name;
                    PanelConfirmDelete.Visible = true;
                    PanelForm.Visible = false;
                }
            }
        }

        private void PopulateField(publisher Publisher)
        {
            TextBoxName.Text = Publisher.name;
            TextBoxContact.Text = Publisher.contact;
            TextBoxAddress.Text = Publisher.address;
        }

        protected override void UpdateList()
        {
            nav_info.InnerText = "Offset:" + Offset + ", Limit:" + Limit+", from updateList";
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

        private void PopulateListTable()
        {
            PublisherList.Clear();
            PublisherList = publisherService.PublisherList(Offset, Limit);
            TableRow HeaderRow = TableList.Rows[0];
            TableList.Rows.Clear();
            TableList.Rows.Add(HeaderRow);
            int No = Offset * Limit;
            foreach (publisher p in PublisherList)
            {
                No++;
                string NoStr = Convert.ToString(No);
                TableRow TRow = new TableRow();
                TableCell TCellNo = new TableCell();
                TableCell TCellId = new TableCell();
                TableCell TCellName = new TableCell();
                TableCell TCellAddress = new TableCell();
                TableCell TCellContact = new TableCell();
                TableCell TCellOption = new TableCell();
                Button EditButton = new Button();
                Button DeleteButton = new Button();

                EditButton.Text = "edit";
                EditButton.CssClass = "btn btn-warning";
                EditButton.CausesValidation = false;
                EditButton.UseSubmitBehavior = false;
                //  EditButton.PostBackUrl = URL + "?id=" + p.id;
                EditButton.CommandArgument = p.id;
                EditButton.Click += new EventHandler(BtnEditClick);

                DeleteButton.Text = "delete";
                DeleteButton.CssClass = "btn btn-danger";
                DeleteButton.CausesValidation = false;
                DeleteButton.UseSubmitBehavior = false;
                DeleteButton.CommandArgument = p.id;
                DeleteButton.Click += new EventHandler(BtnDeleteClick);

                TCellNo.Controls.Add(ControlUtil.GenerateLabel(NoStr));
                TCellId.Controls.Add(ControlUtil.GenerateLabel(p.id));
                TCellName.Controls.Add(ControlUtil.GenerateLabel(p.name));
                TCellAddress.Controls.Add(ControlUtil.GenerateLabel(p.address));
                TCellContact.Controls.Add(ControlUtil.GenerateLabel(p.contact));
                //option
                TCellOption.Controls.Add(EditButton);
                TCellOption.Controls.Add(DeleteButton);

                TRow.Controls.Add(TCellNo);
                TRow.Controls.Add(TCellId);
                TRow.Controls.Add(TCellName);
                TRow.Controls.Add(TCellAddress);
                TRow.Controls.Add(TCellContact);
                TRow.Controls.Add(TCellAddress);
                TRow.Controls.Add(TCellOption);

                TableList.Controls.Add(TRow);

            }
        }

        private void ClearField()
        {
            State = ModelParameter.ADD;
            TextBoxName.Text = "";
            TextBoxContact.Text = "";
            TextBoxAddress.Text = "";
            LabelStatus.Text = "";
        }

        protected void ButtonReset_Click(object sender, EventArgs e)
        {
            ClearField();
            State = ModelParameter.ADD;
            Session[ModelParameter.PublisherId] = null;
            FormTitle.InnerText = "Form Add New";
            //  InitElement();
            //    LabelState.Text = "ADD";
        }

        protected void ButtonCancelDelete_Click(object sender, EventArgs e)
        {
            PanelForm.Visible = true;
            PanelConfirmDelete.Visible = false;
            Session[ModelParameter.PublisherId] = null;
            ButtonReset_Click(sender, e);

        }

        protected void ButtonConfirmDelete_Click(object sender, EventArgs e)
        {
            if (State == ModelParameter.EDIT && Publisher != null)
            {
                publisherService.DeletePublisher(Publisher);
                ButtonCancelDelete_Click(sender, e);
            }
        }

        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            if (State == ModelParameter.ADD && ModelState.IsValid && userService.IsValid(LoggedUser))
            {
                Publisher = new publisher();
                Publisher.name = TextBoxName.Text.TrimEnd();
                Publisher.contact = TextBoxContact.Text.TrimEnd();
                Publisher.address = TextBoxAddress.Text.TrimEnd();
                publisher NewPublisher = publisherService.AddPublisher(Publisher);
                string status = "";
                if (NewPublisher != null)
                {
                    status = "Success Adding New Publisher: " + NewPublisher.name + "(" + NewPublisher.id + ")";
                    ClearField();
                    InitElement();

                }
                else
                {
                    status = "Failed Adding New Publisher";

                }
                LabelStatus.Text = status;

            }
            else
            if (Publisher != null && State == ModelParameter.EDIT && ModelState.IsValid && userService.IsValid(LoggedUser))
            {
                Publisher.name = TextBoxName.Text.TrimEnd();
                Publisher.contact = TextBoxContact.Text.TrimEnd();
                Publisher.address = TextBoxAddress.Text.TrimEnd();
                publisher UpdatePublisher = publisherService.UpdatePublisher(Publisher);
                string status = "";
                if (UpdatePublisher != null)
                {
                    status = "Success Updating Publisher: " + UpdatePublisher.name + "(" + UpdatePublisher.id + ")";
                    ClearField();
                    InitElement();
                }
                else
                {
                    status = "Failed Updating Publisher";
                }
                LabelStatus.Text = status;

            }
            ButtonReset_Click(sender, e);
            Session[ModelParameter.PublisherId] = null;
            Session.Remove(ModelParameter.PublisherId);
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

        protected void Button1_Click(object sender, EventArgs e)
        {
            PanelForm.Visible = !PanelForm.Visible;
        }
    }
}