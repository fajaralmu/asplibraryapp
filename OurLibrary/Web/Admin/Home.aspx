<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryWeb.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="OurLibrary.Web.Admin.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p style="text-align: center">
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
    </p>
    <hr />
    <div class="row" style="text-align: center">
        <p>Transaction</p>
        <div class="admin-menu col-md-2">
            <h2>Book Record</h2>
            <a class="btn btn-default" runat="server" href="~/Web/Admin/Management/Management.aspx?object=book_record">Go</a>

        </div>
        <div class="admin-menu col-md-2">
            <h2>Issue Book</h2>
            <a class="btn btn-default" runat="server" href="~/Web/Admin/Transaction/BookIssue.aspx">Go</a>
        </div>
        <div class="admin-menu col-md-2">
            <h2>Student Issue</h2>
            <a class="btn btn-default" runat="server" href="~/Web/Admin/Transaction/CheckStudentIssue.aspx">Go</a>
        </div>
    </div>
    <hr />
    <div class="row" style="text-align: center">
        <p>Student Data</p>

        <div class="admin-menu col-md-2">
            <h2>Class</h2>
            <a class="btn btn-default" runat="server" href="~/Web/Admin/Management/Management.aspx?object=class">Go</a>
        </div>
        <div class="admin-menu col-md-2">
            <h2>Student</h2>
            <a class="btn btn-default" runat="server" href="~/Web/Admin/Management/Management.aspx?object=student">Go</a>
        </div>
    </div>
    <hr />
    <div class="row" style="text-align: center">
        <p>Master Data</p>
        <div class="admin-menu col-md-2">
            <h2>Home</h2>
            <a class="btn btn-default" runat="server" href="~/Web/Admin">Go</a>
        </div>
        <div class="admin-menu col-md-2">
            <h2>Publisher</h2>
            <a class="btn btn-default" runat="server" href="~/Web/Admin/Management/Management.aspx?object=publisher">Go</a>
        </div>
        <div class="admin-menu col-md-2">
            <h2>Author</h2>
            <a class="btn btn-default" runat="server" href="~/Web/Admin/Management/Management.aspx?object=author">Go</a>
        </div>
        <div class="admin-menu col-md-2">
            <h2>Category</h2>
            <a class="btn btn-default" runat="server" href="~/Web/Admin/Management/Management.aspx?object=category">Go</a>
        </div>
        <div class="admin-menu col-md-2">
            <h2>Book</h2>
            <a class="btn btn-default" runat="server" href="~/Web/Admin/Management/Management.aspx?object=book">Go</a>
        </div>
    </div>





</asp:Content>
