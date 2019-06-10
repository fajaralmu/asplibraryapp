<%@ Page Title="Management:Publisher" Language="C#" MasterPageFile="~/LibraryWeb.Master" AutoEventWireup="true" CodeBehind="PublisherManagement.aspx.cs" Inherits="OurLibrary.Web.Admin.Management.PublisherManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <h2>Publisher Management</h2>
        <p>
            <asp:Button ID="Button1" runat="server" CausesValidation="false" CssClass="btn btn-info" OnClick="Button1_Click" Text="Toggle Form" />
        </p>
    </div>
    <asp:Panel ID="PanelForm" runat="server" Width="682px" style="margin-top: 12px">
        
        <table id="MainContent_TableAdd" width="544px">
            <tr>
                <td colspan="3">
                    <h2 id="FormTitle" runat="server">Form Add New</h2>
                </td>
            </tr>
            <tr>
                <td>Publisher Name*</td>
                <td>
                    <asp:TextBox ID="TextBoxName" runat="server" Width="170px"></asp:TextBox>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBoxName" ErrorMessage="Publisher Name Required" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>Contact*</td>
                <td>
                    <asp:TextBox ID="TextBoxContact" runat="server" Width="170px"></asp:TextBox>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TextBoxContact" ErrorMessage="Publisher Contact Required" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>Address*</td>
                <td>
                    <asp:TextBox ID="TextBoxAddress" runat="server" TextMode="MultiLine" Width="170px"></asp:TextBox>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TextBoxAddress" ErrorMessage="Publisher Address Required" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>
                    <asp:Button ID="ButtonSave" runat="server" CssClass="btn btn-primary" OnClick="ButtonSave_Click" Text="Save" />
                    <asp:Button ID="ButtonReset" runat="server" CausesValidation="false" CssClass="btn btn-warning" OnClick="ButtonReset_Click" Text="Reset" />
                </td>
                <td>
                    <asp:Label ID="LabelState" runat="server"></asp:Label>
                    <br />
                    <asp:Label ID="LabelStatus" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
    </asp:Panel>
    <asp:Panel ID="PanelConfirmDelete" runat="server" Visible="False" Width="681px">
        <table class="nav-justified" style="width: 72%">
            <tr>
                <td colspan="2">
                    <h2>Delete Confirmation</h2>
                </td>
            </tr>
            <tr>
                <td style="width: 330px">Are You Sure Want To Delete:</td>
                <td style="width: 291px">
                    <asp:Label ID="LabelNameToDelete" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 330px">
                    <asp:Button ID="ButtonConfirmDelete" runat="server" OnClick="ButtonConfirmDelete_Click" Text="Delete" CssClass="btn btn-danger" />
                </td>
                <td style="width: 291px">
                    <asp:Button ID="ButtonCancelDelete" runat="server" OnClick="ButtonCancelDelete_Click" Text="No" CssClass="btn btn-default" />
                </td>
            </tr>
        </table>
        <br />
        <br />
    </asp:Panel>
    <asp:Panel runat="server" ID="ListPanel" Width="679px">
        <p style="width: 200px">
            <strong>List Publisher </strong>
        </p>
        <p style="width: 200px">
            Limit:
            <asp:TextBox ID="TextBoxLimit" runat="server" CausesValidation="false" OnTextChanged="TextBoxLimit_TextChanged" TextMode="Number" AutoPostBack="True">5</asp:TextBox>

        </p>
        <p id="nav_info" runat="server" style="width: 200px">
            &nbsp;
        </p>
        <asp:Panel ID="PanelNavigation" runat="server">
        </asp:Panel>
        <asp:Table ID="TableList" runat="server" Width="638px">
            <asp:TableRow runat="server" AccessKey="1" TableSection="TableHeader" BackColor="#66CCFF" Font-Overline="False" Font-Strikeout="False" ForeColor="#0000CC">
                <asp:TableCell runat="server" Font-Bold="True">No</asp:TableCell>
                <asp:TableCell runat="server" Font-Bold="True">Publisher Id</asp:TableCell>
                <asp:TableCell runat="server" Font-Bold="True">Name</asp:TableCell>
                <asp:TableCell runat="server" Font-Bold="True">Contact</asp:TableCell>
                <asp:TableCell runat="server" Font-Bold="True">Address</asp:TableCell>
                <asp:TableCell runat="server" Font-Bold="True">Option</asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </asp:Panel>


</asp:Content>
