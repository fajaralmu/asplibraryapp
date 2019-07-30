<%@ Page Title="Home" Language="C#" MasterPageFile="~/LibraryWeb.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="OurLibrary.Web.Admin.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p style="text-align: center">
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
    </p>
    <hr />
    <div class="row" style="text-align: center">
        <p>Transaction</p>
        <div class="col-md-2">
            <h2><span class="glyphicon glyphicon-file"></span>
                <br />
                <small>Book Record</small></h2>
            <a class="btn btn-default" runat="server" href="~/Web/Admin/Management/Management.aspx?object=book_record">Go</a>

        </div>
        <div class=" col-md-2">
            <h2><span class="glyphicon glyphicon-folder-close"></span>
                <br />
                <small>Issue Book</small></h2>
            <a class="btn btn-default" runat="server" href="~/Web/Admin/Transaction/BookIssue.aspx">Go</a>
        </div>
        <div class=" col-md-2">
            <h2><span class="glyphicon glyphicon-folder-open"></span>
                <br />
                <small>Student Issue</small></h2>
            <a class="btn btn-default" runat="server" href="~/Web/Admin/Transaction/CheckStudentIssue.aspx">Go</a>
        </div>
    </div>
    <hr />
    <div class="row" style="text-align: center">
        <p>Stat</p>

        <div class=" col-md-2">
            <h2>&#128221;<br />
                <small>Visit Recorder</small></h2>
            <a class="btn btn-default" runat="server" href="~/Web/Stat/Visit.aspx">Go</a>
        </div>
        <div class=" col-md-2">
            <h2>&#128202;<br />
                <small>Visit Graph</small></h2>
            <a class="btn btn-default" runat="server" href="~/Web/Stat/Visit.aspx">Go</a>
        </div>
        <div class=" col-md-2">
            <h2>&#128211;<br />
                <small>Visit Management</small></h2>
            <a class="btn btn-default" runat="server" href="~/Web/Admin/Management/Management.aspx?object=visit">Go</a>
        </div>
    </div>
    <hr />
    <div class="row" style="text-align: center">
        <p>Student Data</p>

        <div class=" col-md-2">
            <h2>&#128682;<br />
                <small>Class</small></h2>
            <a class="btn btn-default" runat="server" href="~/Web/Admin/Management/Management.aspx?object=class">Go</a>
        </div>
        <div class=" col-md-2">
            <h2>&#128104;<br />
                <small>Student</small></h2>
            <a class="btn btn-default" runat="server" href="~/Web/Admin/Management/Management.aspx?object=student">Go</a>
        </div>
        <div class=" col-md-2">
            <h2>&#128221;<br />
                <small>Transactions</small></h2>
            <a class="btn btn-default" runat="server" href="~/Web/Admin/Management/Management.aspx?object=issue">Go</a>
        </div>
    </div>
    <hr />
    <div class="row" style="text-align: center">
        <p>Master Data</p>
        <div class="col-md-2">
            <h2><span class="glyphicon glyphicon-home"></span>
                <br />
                <small>Home</small></h2>
            <a class="btn btn-default" runat="server" href="~/Web/Admin">Go</a>
        </div>
        <div class=" col-md-2">
            <h2>&#127981;</span>
                <br />
                <small>Publisher</small></h2>
            <a class="btn btn-default" runat="server" href="~/Web/Admin/Management/Management.aspx?object=publisher">Go</a>
        </div>
        <div class="col-md-2">
            <h2><span class="glyphicon glyphicon-user"></span>
                <br />
                <small>Author</small></h2>
            <a class="btn btn-default" runat="server" href="~/Web/Admin/Management/Management.aspx?object=author">Go</a>
        </div>
        <div class=" col-md-2">
            <h2><span class="glyphicon glyphicon-bookmark"></span>
                <br />
                <small>Category</small></h2>
            <a class="btn btn-default" runat="server" href="~/Web/Admin/Management/Management.aspx?object=category">Go</a>
        </div>
        <div class=" col-md-2">
            <h2><span class="glyphicon glyphicon-book"></span>
                <br />
                <small>Book</small></h2>
            <a class="btn btn-default" runat="server" href="~/Web/Admin/Management/Management.aspx?object=book">Go</a>
        </div>
    </div>





</asp:Content>
