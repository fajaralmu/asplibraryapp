<%@ Page Title="Management" Language="C#" MasterPageFile="~/LibraryWeb.Master" AutoEventWireup="true" CodeBehind="Management.aspx.cs" Inherits="OurLibrary.Web.Admin.Management.CategoryManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <h2>
            <asp:Label ID="LabelObjectName" runat="server" />
            Management</h2>
        <p>
            <asp:Button ID="ButtonToggleForm" runat="server" CausesValidation="false" CssClass="btn btn-info" Text="Toggle Form" OnClick="ButtonToggleForm_Click" />
        </p>
    </div>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Panel ID="PanelForm" runat="server" Width="682px" Style="margin-top: 12px">
                <asp:Table ID="TableForm" runat="server" Width="638px">
                </asp:Table>
                <br />
                <asp:Button ID="ButtonSave" runat="server" CssClass="btn btn-primary" OnClick="ButtonSave_Click" Text="Save" />
                <asp:Button ID="ButtonReset" runat="server" CausesValidation="false" CssClass="btn btn-warning" OnClick="ButtonReset_Click" Text="Reset" />

            </asp:Panel>


            <asp:Panel ID="PanelConfirmDelete" runat="server" Visible="False" Width="681px">
                <table id="tableConfirmDelete" runat="server" class="nav-justified" style="width: 72%">
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
                            <asp:Button ID="ButtonConfirmDelete" runat="server" Text="Delete" CssClass="btn btn-danger" OnClick="ButtonConfirmDelete_Click" />
                        </td>
                        <td style="width: 291px">
                            <asp:Button ID="ButtonCancelDelete" runat="server" Text="No" OnClick="ButtonCancelDelete_Click" CssClass="btn btn-default" />
                        </td>
                    </tr>
                </table>

            </asp:Panel>
            <br />
            <asp:Label ID="LabelStatus" runat="server"></asp:Label>
            <br />
            <asp:Panel runat="server" ID="ListPanel" Width="800px">
                <p class="form-control-static" style="width: 326px">
                    Item to show:
            <asp:TextBox ID="TextBoxLimit" CssClass="form-control" OnTextChanged="TextBoxLimit_TextChanged" runat="server" CausesValidation="false" TextMode="Number" AutoPostBack="True">5</asp:TextBox>

                </p>
                <p id="nav_info" runat="server" style="width: 200px">
                    &nbsp;
                </p>
                <asp:Panel ID="PanelNavigation" runat="server" CssClass="btn-group btn-group-sm">
                </asp:Panel>
                <asp:Table ID="TableList" CssClass="table table-striped" runat="server" Width="800px">
                    <asp:TableRow runat="server" AccessKey="1" TableSection="TableHeader" BackColor="#66CCFF" Font-Overline="False" Font-Strikeout="False" ForeColor="#0000CC">
                    </asp:TableRow>
                </asp:Table>
            </asp:Panel>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ButtonSave" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
