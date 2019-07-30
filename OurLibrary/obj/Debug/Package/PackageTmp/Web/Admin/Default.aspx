<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryWeb.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="OurLibrary.Web.Admin.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Login To Continue</h1>
    <div role="form">
        <div class="form-group">
            <label>Username</label>
            <asp:TextBox CssClass="form-control" ID="TextBoxUsername" runat="server"></asp:TextBox>
        </div>
        <div class="form-group">
            <label>Password</label>
            <asp:TextBox CssClass="form-control" ID="TextBoxPassword" runat="server" TextMode="Password"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Button ID="Button1" CssClass="btn" runat="server" OnClick="Button1_Click" Text="Login" />

        </div>
    </div>
</asp:Content>
