<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryWeb.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="OurLibrary.Web.Admin.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
    </p>
    <div class="navbar-collapse collapse">
        <ul class="nav navbar-nav">
            <li><a runat="server" href="~/Web/Admin">Home</a></li>
            <li><a runat="server" href="~/Web/Admin/Management/Management.aspx?object=publisher">Publisher Management</a></li>
            <li><a runat="server" href="~/Web/Admin/Management/Management.aspx?object=author">Author Management</a></li>
            <li><a runat="server" href="~/Web/Admin/Management/Management.aspx?object=category">Category Management</a></li>
            <li><a runat="server" href="~/Web/Admin/Management/Management.aspx?object=book">Book Master Data Management</a></li>
            <li><a runat="server" href="~/Web/Admin/Management/Management.aspx?object=class">Class Management</a></li>
            <li><a runat="server" href="~/Web/Admin/Management/Management.aspx?object=student">Student Management</a></li>
            
            <li><a runat="server" href="~/Web/Admin/Management/Management.aspx?object=book_record">Book Record</a></li>
            
            <li><a runat="server" href="~/Web/Admin/Transaction/IssueBook.aspx">Issue Book</a></li>
        </ul>

    </div>
    <br />

</asp:Content>
