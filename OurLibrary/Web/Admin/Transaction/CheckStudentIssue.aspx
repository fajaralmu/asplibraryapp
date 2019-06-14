<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryWeb.Master" AutoEventWireup="true" CodeBehind="CheckStudentIssue.aspx.cs" Inherits="OurLibrary.Web.Admin.Transaction.CheckStudentIssue" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <h3>Check Student Issue</h3>
    </div>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Panel ID="PanelInput" runat="server">
                <p>Input Student ID:</p>
                <asp:TextBox ID="TextBoxStudentID" runat="server" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="TextBoxStudentID"
                    ForeColor="Red" ErrorMessage="ID required" />
                <p>Input Duration:</p>
                <asp:TextBox ID="TextBoxDuration" TextMode="Number" runat="server" />
                <br />
                <asp:Button ID="ButtonSearch" runat="server" Text="Search" CssClass="btn" OnClick="ButtonSearch_Click" />
                <hr />
            </asp:Panel>
            <asp:Panel ID="PanelIssueLis" runat="server">
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
