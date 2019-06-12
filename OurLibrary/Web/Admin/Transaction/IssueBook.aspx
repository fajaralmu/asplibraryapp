<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryWeb.Master" AutoEventWireup="true" CodeBehind="IssueBook.aspx.cs" Inherits="OurLibrary.Web.Admin.Transaction.IssueBook" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <h3>Issue Book Page</h3>
    </div>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Panel runat="server">
                <p id="info" runat="server">
                </p>
                <hr />
                <asp:Label ID="Label1" runat="server" Text="Current Book: "></asp:Label>
                            <asp:Label ID="LabelCurrentBook" runat="server"></asp:Label>
                <br />
                <asp:Label ID="Label2" runat="server" Text="Available Record(s):"></asp:Label>
                <asp:Label ID="LabelRecordList" runat="server"></asp:Label>
                <hr />
                <asp:Label ID="Label4" runat="server" Text="Serach By Title " />
                <br />
                <asp:TextBox ID="InputMasterBook" runat="server" />
                <asp:Button CausesValidation="false" runat="server" ID="ButtonSearchMasterBook"
                    Text="Search Book" OnClick="ButtonSearchMasterBook_Click" CssClass="btn btn-primary" />
                <!--   <input onkeyup="cek()" type="text" runat="server" class="form-control" id="inputMasterBookId" />

                             -->
                <asp:Panel runat="server" ID="PanelBookList">
                </asp:Panel>
                <hr />
                
                <asp:Label ID="Label3" runat="server" Text="Record Id"></asp:Label>
                <br />
                <asp:TextBox ID="TextBoxRecordId" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBoxRecordId" ErrorMessage="Record Id Required" ForeColor="Red"></asp:RequiredFieldValidator>
                <br />
                <asp:Button ID="ButtonSave" CssClass="btn btn-primary" runat="server" Text="Add" OnClick="ButtonSave_Click" />
                <asp:Button ID="ButtonClearList" runat="server" CssClass="btn btn-danger" OnClick="ButtonClearList_Click" Text="Clear" />
                <hr />

                <asp:Label ID="LabelListIssueBook" runat="server"></asp:Label>

                 <hr />
                <asp:Panel ID="PanelBookIssues" runat="server">

                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
