<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/LibraryWeb.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="OurLibrary._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Library</h1>
        <p class="lead">A Library Management System is a software built to handle the primary housekeeping functions of a library</p>
        <p><a  href="~/Web/Doc/Home.aspx"class="btn btn-primary btn-lg">Learn more &raquo;</a></p>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>Adding new Book Record</h2>
            <p>
                Record new book to library </p>
            <p>
                <a class="btn btn-default" href="~/Web/Admin/Management/Management.aspx?object=book_record">Go &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Issue Book</h2>
            <p>
                Issing Book Page
            </p>
            <p>
                <a class="btn btn-default" href="/Web/Admin/Transaction/BookIssue.aspx">Enter Page &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Return Book</h2>
            <p>
                Returning Book Issued
            </p>
            <p>
                <a class="btn btn-default" href="/Web/Admin/Transaction/CheckStudentIssue.aspx">Enter Page &raquo;</a>
            </p>
        </div>

    </div>

</asp:Content>
